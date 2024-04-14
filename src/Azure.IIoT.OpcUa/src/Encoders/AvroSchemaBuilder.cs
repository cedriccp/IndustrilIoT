// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders
{
    using Azure.IIoT.OpcUa.Encoders.Schemas;
    using Azure.IIoT.OpcUa.Encoders.Models;
    using Avro;
    using Opc.Ua;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Encodes objects and inline builds the schema from it
    /// This type exists mainly for testing.
    /// </summary>
    public sealed class AvroSchemaBuilder : BaseAvroEncoder
    {
        /// <summary>
        /// Schema to use
        /// </summary>
        public Schema Schema => _schemas.Peek().Unwrap();

        /// <summary>
        /// Creates an encoder that writes to the stream.
        /// </summary>
        /// <param name="stream">The stream to which the
        /// encoder writes.</param>
        /// <param name="context">The message context to
        /// use for the encoding.</param>
        /// <param name="leaveOpen">If the stream should
        /// be left open on dispose.</param>
        public AvroSchemaBuilder(Stream stream,
            IServiceMessageContext context, bool leaveOpen = true) :
            base(stream, context, leaveOpen)
        {
        }

        /// <inheritdoc/>
        public override void WriteBoolean(string? fieldName, bool value)
        {
            using var _ = Add(fieldName, BuiltInType.Boolean);
            base.WriteBoolean(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteSByte(string? fieldName, sbyte value)
        {
            using var _ = Add(fieldName, BuiltInType.SByte);
            base.WriteSByte(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteByte(string? fieldName, byte value)
        {
            using var _ = Add(fieldName, BuiltInType.Byte);
            base.WriteByte(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteInt16(string? fieldName, short value)
        {
            using var _ = Add(fieldName, BuiltInType.Int16);
            base.WriteInt16(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteUInt16(string? fieldName, ushort value)
        {
            using var _ = Add(fieldName, BuiltInType.UInt16);
            base.WriteUInt16(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteInt32(string? fieldName, int value)
        {
            using var _ = Add(fieldName, BuiltInType.Int32);
            base.WriteInt32(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteUInt32(string? fieldName, uint value)
        {
            using var _ = Add(fieldName, BuiltInType.UInt32);
            base.WriteUInt32(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteInt64(string? fieldName, long value)
        {
            using var _ = Add(fieldName, BuiltInType.Int64);
            base.WriteInt64(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteUInt64(string? fieldName, ulong value)
        {
            using var _ = Add(fieldName, BuiltInType.UInt64);
            base.WriteUInt64(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteFloat(string? fieldName, float value)
        {
            using var _ = Add(fieldName, BuiltInType.Float);
            base.WriteFloat(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteDouble(string? fieldName, double value)
        {
            using var _ = Add(fieldName, BuiltInType.Double);
            base.WriteDouble(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteString(string? fieldName, string? value)
        {
            using var _ = Add(fieldName, BuiltInType.String);
            base.WriteString(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteDateTime(string? fieldName, DateTime value)
        {
            using var _ = Add(fieldName, BuiltInType.DateTime);
            base.WriteDateTime(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteGuid(string? fieldName, Uuid value)
        {
            using var _ = Add(fieldName, BuiltInType.Guid);
            base.WriteGuid(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteGuid(string? fieldName, Guid value)
        {
            using var _ = Add(fieldName, BuiltInType.Guid);
            base.WriteGuid(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteByteString(string? fieldName, byte[]? value)
        {
            using var _ = Add(fieldName, BuiltInType.ByteString);
            base.WriteByteString(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteXmlElement(string? fieldName, XmlElement? value)
        {
            using var _ = Add(fieldName, BuiltInType.XmlElement);
            base.WriteXmlElement(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteNodeId(string? fieldName, NodeId? value)
        {
            using var _ = Add(fieldName, BuiltInType.NodeId);
            base.WriteNodeId(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteExpandedNodeId(string? fieldName,
            ExpandedNodeId? value)
        {
            using var _ = Add(fieldName, BuiltInType.ExpandedNodeId);
            base.WriteExpandedNodeId(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteStatusCode(string? fieldName, StatusCode value)
        {
            using var _ = Add(fieldName, BuiltInType.StatusCode);
            base.WriteStatusCode(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteDiagnosticInfo(string? fieldName,
            DiagnosticInfo? value)
        {
            using var _ = Add(fieldName, BuiltInType.DiagnosticInfo);
            base.WriteDiagnosticInfo(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteQualifiedName(string? fieldName,
            QualifiedName? value)
        {
            using var _ = Add(fieldName, BuiltInType.QualifiedName);
            base.WriteQualifiedName(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteLocalizedText(string? fieldName,
            LocalizedText? value)
        {
            using var _ = Add(fieldName, BuiltInType.LocalizedText);
            base.WriteLocalizedText(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteVariant(string? fieldName,
            Variant value)
        {
            using var _ = Add(fieldName, BuiltInType.Variant);
            base.WriteVariant(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteDataValue(string? fieldName,
            DataValue? value)
        {
            using var _ = Add(fieldName, BuiltInType.DataValue);
            base.WriteDataValue(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteExtensionObject(string? fieldName,
            ExtensionObject? value)
        {
            using var _ = Add(fieldName, BuiltInType.ExtensionObject);
            base.WriteExtensionObject(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteEncodeable(string? fieldName,
            IEncodeable? value, Type? systemType)
        {
            var fullName = GetFullNameOfEncodeable(value, systemType, out var typeName);
            if (typeName == null)
            {
                throw ServiceResultException.Create(StatusCodes.BadEncodingError,
                    "Failed to encode a encodeable without system type");
            }
            using var _ = Push(fieldName, fullName ?? typeName);
            base.WriteEncodeable(fieldName, value, systemType);
        }

        /// <inheritdoc/>
        public override void WriteObject(string? fieldName, string? typeName,
            Action writer)
        {
            using var _ = Push(fieldName, typeName ?? fieldName ?? "unknown");
            base.WriteObject(fieldName, typeName, writer);
        }

        /// <inheritdoc/>
        public override void WriteEnumerated(string? fieldName,
            Enum? value)
        {
            using var _ = Add(fieldName, BuiltInType.Enumeration);
            base.WriteEnumerated(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteBooleanArray(string? fieldName,
            IList<bool>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Boolean, ValueRanks.OneDimension);
            base.WriteBooleanArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteSByteArray(string? fieldName,
            IList<sbyte>? values)
        {
            using var _ = Add(fieldName, BuiltInType.SByte, ValueRanks.OneDimension);
            base.WriteSByteArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteByteArray(string? fieldName,
            IList<byte>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Byte, ValueRanks.OneDimension);
            base.WriteByteArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteInt16Array(string? fieldName,
            IList<short>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Int16, ValueRanks.OneDimension);
            base.WriteInt16Array(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteUInt16Array(string? fieldName,
            IList<ushort>? values)
        {
            using var _ = Add(fieldName, BuiltInType.UInt16, ValueRanks.OneDimension);
            base.WriteUInt16Array(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteInt32Array(string? fieldName,
            IList<int>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Int32, ValueRanks.OneDimension);
            base.WriteInt32Array(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteUInt32Array(string? fieldName,
            IList<uint>? values)
        {
            using var _ = Add(fieldName, BuiltInType.UInt32, ValueRanks.OneDimension);
            base.WriteUInt32Array(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteInt64Array(string? fieldName,
            IList<long>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Int64, ValueRanks.OneDimension);
            base.WriteInt64Array(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteUInt64Array(string? fieldName,
            IList<ulong>? values)
        {
            using var _ = Add(fieldName, BuiltInType.UInt64, ValueRanks.OneDimension);
            base.WriteUInt64Array(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteFloatArray(string? fieldName,
            IList<float>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Float, ValueRanks.OneDimension);
            base.WriteFloatArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteDoubleArray(string? fieldName,
            IList<double>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Double, ValueRanks.OneDimension);
            base.WriteDoubleArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteStringArray(string? fieldName,
            IList<string?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.String, ValueRanks.OneDimension);
            base.WriteStringArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteDateTimeArray(string? fieldName,
            IList<DateTime>? values)
        {
            using var _ = Add(fieldName, BuiltInType.DateTime, ValueRanks.OneDimension);
            base.WriteDateTimeArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteGuidArray(string? fieldName,
            IList<Uuid>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Guid, ValueRanks.OneDimension);
            base.WriteGuidArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteGuidArray(string? fieldName,
            IList<Guid>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Guid, ValueRanks.OneDimension);
            base.WriteGuidArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteByteStringArray(string? fieldName,
            IList<byte[]?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.ByteString, ValueRanks.OneDimension);
            base.WriteByteStringArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteXmlElementArray(string? fieldName,
            IList<XmlElement?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.XmlElement, ValueRanks.OneDimension);
            base.WriteXmlElementArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteNodeIdArray(string? fieldName,
            IList<NodeId?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.NodeId, ValueRanks.OneDimension);
            base.WriteNodeIdArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteExpandedNodeIdArray(string? fieldName,
            IList<ExpandedNodeId?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.ExpandedNodeId, ValueRanks.OneDimension);
            base.WriteExpandedNodeIdArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteStatusCodeArray(string? fieldName,
            IList<StatusCode>? values)
        {
            using var _ = Add(fieldName, BuiltInType.StatusCode, ValueRanks.OneDimension);
            base.WriteStatusCodeArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteDiagnosticInfoArray(string? fieldName,
            IList<DiagnosticInfo?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.DiagnosticInfo, ValueRanks.OneDimension);
            base.WriteDiagnosticInfoArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteQualifiedNameArray(string? fieldName,
            IList<QualifiedName?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.QualifiedName, ValueRanks.OneDimension);
            base.WriteQualifiedNameArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteLocalizedTextArray(string? fieldName,
            IList<LocalizedText?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.LocalizedText, ValueRanks.OneDimension);
            base.WriteLocalizedTextArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteVariantArray(string? fieldName,
            IList<Variant>? values)
        {
            using var _ = Add(fieldName, BuiltInType.Variant, ValueRanks.OneDimension);
            base.WriteVariantArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteDataValueArray(string? fieldName,
            IList<DataValue?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.DataValue, ValueRanks.OneDimension);
            base.WriteDataValueArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteExtensionObjectArray(string? fieldName,
            IList<ExtensionObject?>? values)
        {
            using var _ = Add(fieldName, BuiltInType.ExtensionObject, ValueRanks.OneDimension);
            base.WriteExtensionObjectArray(fieldName, values);
        }

        /// <inheritdoc/>
        public override void WriteEncodeableArray(string? fieldName,
            IList<IEncodeable>? values, Type? systemType)
        {
            base.WriteEncodeableArray(fieldName, values, systemType);
        }

        /// <inheritdoc/>
        public override void WriteEnumeratedArray(string? fieldName,
            Array? values, Type? systemType)
        {
            using var _ = Add(fieldName, BuiltInType.Enumeration, ValueRanks.OneDimension);
            base.WriteEnumeratedArray(fieldName, values, systemType);
        }

        /// <inheritdoc/>
        public override void WriteArray(string? fieldName, object array,
            int valueRank, BuiltInType builtInType)
        {
            using var _ = Add(fieldName, builtInType, valueRank);
            base.WriteArray(fieldName, array, valueRank, builtInType);
        }

        /// <inheritdoc/>
        public override void WriteDataSet(string? fieldName, DataSet dataSet)
        {
            using var _ = Push(fieldName, fieldName + typeof(DataSet).Name);
            base.WriteDataSet(fieldName, dataSet);
        }

        /// <inheritdoc/>
        protected override void WriteNullableDataValue(string? fieldName,
            DataValue? value)
        {
            using var _ = Union(fieldName, true);
            base.WriteNullableDataValue(fieldName, value);
        }

        /// <inheritdoc/>
        public override void WriteArray<T>(string? fieldName, IList<T>? values,
            Action<T> writer, string? typeName = null)
        {
            using var _ = Push(fieldName, typeName ??
                values?.FirstOrDefault()?.GetType().Name ?? typeof(T).Name, true);
            base.WriteArray(fieldName, values, writer);
        }

        /// <summary>
        /// Add field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="builtInType"></param>
        /// <param name="valueRanks"></param>
        /// <exception cref="ServiceResultException"></exception>
        private IDisposable Add(string? fieldName, BuiltInType builtInType,
            int valueRanks = ValueRanks.Scalar)
        {
            if (_skipInnerSchemas)
            {
                return Nothing.ToDo;
            }
            _skipInnerSchemas = true;
            var schema = _builtIns.GetSchemaForBuiltInType(builtInType,
                valueRanks);
            if (!_schemas.TryPeek(out var top))
            {
                _schemas.Push(schema.CreateRoot(fieldName));
            }
            else if (top is ArraySchema arr)
            {
                arr.ItemSchema = schema;
            }
            else if (top is UnionSchema u)
            {
                u.Schemas.Add(schema);
            }
            else if (top is RecordSchema r)
            {
                r.Fields.Add(new Field(schema, fieldName ?? kDefaultFieldName,
                    r.Fields.Count));
            }
            else
            {
                throw ServiceResultException.Create(StatusCodes.BadEncodingError,
                    "No record schema to push to {0}", Schema.ToJson());
            }
            return new Skip(this);
        }

        /// <summary>
        /// Push a new record schema as field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="typeName"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="ServiceResultException"></exception>
        private IDisposable Push(string? fieldName, string typeName, bool array = false)
        {
            if (_skipInnerSchemas)
            {
                return Nothing.ToDo;
            }
            var schema = array ? (Schema)ArraySchema.Create(
                AvroSchema.CreatePlaceHolder("Dummy", "")) :
                RecordSchema.Create(typeName, new List<Field>());
            return PushSchema(fieldName, schema);
        }

        /// <summary>
        /// Push a union
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="nullable"></param>
        /// <returns></returns>
        /// <exception cref="ServiceResultException"></exception>
        private IDisposable Union(string? fieldName, bool nullable = false)
        {
            if (_skipInnerSchemas)
            {
                return Nothing.ToDo;
            }
            var schema = UnionSchema.Create(new List<Schema>());
            if (nullable)
            {
                schema.Schemas.Add(AvroSchema.Null);
            }
            return PushSchema(fieldName, schema);
        }

        /// <summary>
        /// Push schema
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        /// <exception cref="ServiceResultException"></exception>
        private Pop PushSchema(string? fieldName, Schema schema)
        {
            if (!_schemas.TryPeek(out var top))
            {
                _schemas.Push(schema.CreateRoot(fieldName));
            }
            else if (top is ArraySchema arr)
            {
                arr.ItemSchema = schema;
            }
            else if (top is UnionSchema u)
            {
                u.Schemas.Add(schema);
            }
            else if (top is RecordSchema r)
            {
                r.Fields.Add(new Field(schema, fieldName ?? kDefaultFieldName,
                    r.Fields.Count));
            }
            else
            {
                throw ServiceResultException.Create(StatusCodes.BadEncodingError,
                    "No record schema to push to. Current schema {0}", Schema.ToJson());
            }
            _schemas.Push(schema);
            return new Pop(this);
        }

        private sealed record Pop(AvroSchemaBuilder outer) : IDisposable
        {
            public void Dispose()
            {
                outer._schemas.Pop();
            }
        }

        private sealed record Skip(AvroSchemaBuilder outer) : IDisposable
        {
            public void Dispose()
            {
                outer._skipInnerSchemas = false;
            }
        }

        /// <summary>
        /// Create root schema for a schema or field at the root
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static RecordSchema CreateRoot(Schema schema, string? fieldName = null)
        {
            if (schema is RecordSchema r && fieldName == null)
            {
                return r;
            }
            return RecordSchema.Create("Root", new List<Field>
            {
                new (schema, fieldName ?? "Value", 0)
            });
        }

        private sealed class Nothing : IDisposable
        {
            public static readonly Nothing ToDo = new();

            public void Dispose()
            {
            }
        }

        private readonly Stack<Schema> _schemas = new();
        private readonly AvroBuiltInAvroSchemas _builtIns = new();
        private bool _skipInnerSchemas;
    }
}