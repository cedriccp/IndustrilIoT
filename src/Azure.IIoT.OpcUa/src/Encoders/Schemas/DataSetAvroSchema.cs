﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders.Schemas
{
    using Azure.IIoT.OpcUa.Encoders.Utils;
    using Azure.IIoT.OpcUa.Publisher.Models;
    using Azure.IIoT.OpcUa.Encoders;
    using Avro;
    using Furly;
    using Furly.Extensions.Messaging;
    using Opc.Ua;
    using Opc.Ua.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions to convert metadata into avro schema. Note that this class
    /// generates a schema that complies with the json representation in
    /// <see cref="JsonEncoderEx.WriteDataSet(string?, Models.DataSet?)"/>.
    /// This depends on the network settings and reversible vs. nonreversible
    /// encoding mode.
    /// </summary>
    public class DataSetAvroSchema : BaseDataSetSchema<Schema>, IEventSchema
    {
        /// <inheritdoc/>
        public string Type => ContentMimeType.AvroSchema;

        /// <inheritdoc/>
        public string Name => Schema.Fullname;

        /// <inheritdoc/>
        public ulong Version { get; }

        /// <inheritdoc/>
        string IEventSchema.Schema => Schema.ToString();

        /// <inheritdoc/>
        public string? Id { get; }

        /// <inheritdoc/>
        public override Schema Schema { get; }

        /// <summary>
        /// Get avro schema for a dataset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataSet"></param>
        /// <param name="dataSetFieldContentMask"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataSetAvroSchema(string? name, PublishedDataSetModel dataSet,
            Publisher.Models.DataSetFieldContentMask? dataSetFieldContentMask = null,
            SchemaOptions? options = null) : base(dataSetFieldContentMask,
                new BuiltInAvroSchemas(), options)
        {
            Schema = Compile(name, dataSet) ?? AvroUtils.Null;
        }

        /// <summary>
        /// Get avro schema for a dataset encoded in json
        /// </summary>
        /// <param name="dataSetWriter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataSetAvroSchema(DataSetWriterModel dataSetWriter,
            SchemaOptions? options = null) :
            this(dataSetWriter.DataSetWriterName, dataSetWriter.DataSet
                    ?? throw new ArgumentException("Missing data set in writer"),
                dataSetWriter.DataSetFieldContentMask, options)
        {
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return Schema.ToString();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Schema> GetDataSetFieldSchemas(string? name,
            PublishedDataSetModel dataSet)
        {
            var singleValue = dataSet.EnumerateMetaData().Take(2).Count() != 1;
            GetEncodingMode(out var omitFieldName, out var fieldsAreDataValues,
                singleValue);
            if (omitFieldName)
            {
                var set = new HashSet<Schema>();
                foreach (var (_, fieldMetadata) in dataSet.EnumerateMetaData())
                {
                    if (fieldMetadata?.DataType != null)
                    {
                        set.Add(LookupSchema(fieldMetadata.DataType, out _));
                    }
                }
                return set.Select(s => s.AsNullable());
            }

            var ns = AvroUtils.NamespaceUriToNamespace(_options.Namespace ?? Namespaces.OpcUaSdk);
            var fields = new List<Field>();
            var pos = 0;
            foreach (var (fieldName, fieldMetadata) in dataSet.EnumerateMetaData())
            {
                // Now collect the fields of the payload
                pos++;
                if (fieldMetadata?.DataType != null)
                {
                    var schema = LookupSchema(fieldMetadata.DataType, out var typeName);
                    if (fieldName != null)
                    {
                        // TODO: Add properties to the field type
                        schema = Encoding.GetSchemaForDataSetField(
                            (typeName ?? fieldName) + "DataValue", ns, fieldsAreDataValues, schema);

                        fields.Add(new Field(schema, EscapeSymbol(fieldName), pos));
                    }
                }
            }
            if (fields.Count == 0)
            {
                return Enumerable.Empty<Schema>();
            }
            return RecordSchema.Create(
                EscapeSymbol(name ?? dataSet.Name ?? "DataSetPayload"), fields).YieldReturn();
        }

        /// <inheritdoc/>
        protected override Schema CreateStructureSchema(StructureDescriptionModel description,
            Schema? baseTypeSchema)
        {
            //
            // |---------------|------------|----------------|
            // | Field Value   | Reversible | Non-Reversible |
            // |---------------|------------|----------------|
            // | NULL          | Omitted    | JSON null      |
            // | Default Value | Omitted    | Default Value  |
            // |---------------|------------|----------------|
            //
            var fields = new List<Field>();
            var pos = 0;
            if (baseTypeSchema is RecordSchema b)
            {
                foreach (var field in b.Fields)
                {
                    fields.Add(new Field(field.Schema, field.Name, pos++,
                        field.Aliases, field.Documentation, field.DefaultValue));
                    // Can we copy type property to the field to show inheritance
                }
            }

            foreach (var field in description.Fields)
            {
                var schema = LookupSchema(field.DataType, out _,
                    field.ValueRank, field.ArrayDimensions);
                if (field.IsOptional)
                {
                    schema = schema.AsNullable();
                }
                fields.Add(new Field(schema, EscapeSymbol(field.Name), pos++));
            }

            var (ns1, dt) = SplitNodeId(description.DataTypeId);
            return RecordSchema.Create(
                SplitQualifiedName(description.Name, ns1),
                fields, ns1, new[] { dt },
                customProperties: AvroUtils.GetProperties(description.DataTypeId));
        }

        /// <inheritdoc/>
        protected override Schema CreateEnumSchema(EnumDescriptionModel description)
        {
            var (ns, dt) = SplitNodeId(description.DataTypeId);

            var symbols = description.Fields
                .Select(e => EscapeSymbol(e.Name))
                .ToList();
            return EnumSchema.Create(
                SplitQualifiedName(description.Name, ns),
                symbols, ns, new[] { dt },
                customProperties: AvroUtils.GetProperties(description.DataTypeId),
                defaultSymbol: symbols[0]);
            // TODO: Build doc from fields descriptions
        }

        /// <inheritdoc/>
        protected override Schema CreateArraySchema(Schema schema)
        {
            return ArraySchema.Create(schema);
        }

        /// <inheritdoc/>
        protected override Schema CreateUnionSchema(IReadOnlyList<Schema> schemas)
        {
            return AvroUtils.CreateUnion(schemas);
        }

        /// <summary>
        /// Create namespace
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        private (string Id, string Namespace) SplitNodeId(string nodeId)
        {
            var id = nodeId.ToExpandedNodeId(Context);
            string avroStyleNamespace;
            if (id.NamespaceIndex == 0 && id.NamespaceUri == null)
            {
                avroStyleNamespace = AvroUtils.NamespaceZeroName;
            }
            else
            {
                avroStyleNamespace =
                    AvroUtils.NamespaceUriToNamespace(id.NamespaceUri);
            }
            var name = id.IdType switch
            {
                IdType.Opaque => "b_",
                IdType.Guid => "g_",
                IdType.String => "s_",
                _ => "i_"
            } + id.Identifier;
            return (avroStyleNamespace, AvroUtils.Escape(name));
        }

        /// <summary>
        /// Create namespace
        /// </summary>
        /// <param name="qualifiedName"></param>
        /// <param name="outerNamespace"></param>
        /// <returns></returns>
        private string SplitQualifiedName(string qualifiedName,
            string? outerNamespace = null)
        {
            var qn = qualifiedName.ToQualifiedName(Context);
            string avroStyleNamespace;
            if (qn.NamespaceIndex == 0)
            {
                avroStyleNamespace = AvroUtils.NamespaceZeroName;
            }
            else
            {
                var uri = Context.NamespaceUris.GetString(qn.NamespaceIndex);
                avroStyleNamespace = AvroUtils.NamespaceUriToNamespace(uri);
            }
            var name = AvroUtils.Escape(qn.Name);
            if (!string.Equals(outerNamespace, avroStyleNamespace,
                StringComparison.OrdinalIgnoreCase))
            {
                // Qualify if the name is in a different namespace
                name = $"{avroStyleNamespace}.{name}";
            }
            return name;
        }

        /// <summary>
        /// Helper to escape field names and symbols based on the options set
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string EscapeSymbol(string name)
        {
            if (_options.EscapeSymbols)
            {
                return AvroUtils.Escape(name);
            }
            return name;
        }
    }
}
