﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders.Schemas
{
    using Json.Schema;
    using Opc.Ua;
    using System;
    using System.Collections.Generic;
    using DataSetFieldContentMask = Publisher.Models.DataSetFieldContentMask;

    /// <summary>
    /// Provides the json encodings of built in types and objects in Avro schema
    /// </summary>
    internal class BuiltInJsonSchemas : BaseBuiltInSchemas<JsonSchema>
    {
        /// <summary>
        /// Schema definitions
        /// </summary>
        public Dictionary<string, JsonSchema> Definitions { get; }

        private JsonSchema EnumerationSchema
        {
            get
            {
                if (_reversibleEncoding)
                {
                    // Enumeration values shall be encoded as a JSON number
                    // for the reversible encoding.
                    return Simple((int)BuiltInType.Enumeration,
                        SchemaType.Integer, "int32");
                }

                // For the non - reversible form, Enumeration values are
                // encoded as a JSON string with the following format:
                // <name>_<value>
                return Simple((int)BuiltInType.Enumeration,
                    SchemaType.String);
            }
        }

        private JsonSchema DiagnosticInfoSchema
        {
            get
            {
                return new JsonSchema
                {
                    Title = GetTitle(BuiltInType.DiagnosticInfo),
                    Id = GetId(BuiltInType.DiagnosticInfo),
                    Types = new[] { SchemaType.Object },
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["SymbolicId"] = GetSchemaForBuiltInType(BuiltInType.Int32),
                        ["NamespaceUri"] = GetSchemaForBuiltInType(BuiltInType.Int32),
                        ["Locale"] = GetSchemaForBuiltInType(BuiltInType.Int32),
                        ["LocalizedText"] = GetSchemaForBuiltInType(BuiltInType.Int32),
                        ["AdditionalInfo"] = GetSchemaForBuiltInType(BuiltInType.String),
                        ["InnerStatusCode"] = GetSchemaForBuiltInType(BuiltInType.StatusCode),
                        ["InnerDiagnosticInfo"] = new JsonSchema
                        {
                            // Self reference
                            Reference = new UriOrFragment("#")
                        }
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema VariantSchema
        {
            get
            {
                var any = new JsonSchema
                {
                    Title = "Any",
                    Types = new[] {
                        SchemaType.Number,
                        SchemaType.Null,
                        SchemaType.Object,
                        SchemaType.Array,
                        SchemaType.String,
                        SchemaType.Integer,
                        SchemaType.Boolean
                    }
                };
                if (!_reversibleEncoding)
                {
                    // For the non-reversible form, Variant values shall be
                    // encoded as a JSON value containing only the value of
                    // the Body field. The Type and Dimensions fields are
                    // dropped. Multi-dimensional arrays are encoded as a
                    // multi-dimensional JSON array as described in 5.4.5.
                    return any;
                }
                return new JsonSchema
                {
                    Id = GetId(BuiltInType.Variant),
                    Types = new[] { SchemaType.Object },
                    Title = GetTitle(BuiltInType.Variant),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["Type"] = GetSchemaForBuiltInType(BuiltInType.Byte),
                        ["Body"] = any,
                        ["Dimensions"] = GetSchemaForBuiltInType(BuiltInType.UInt32,
                            ValueRanks.OneDimension)
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema ExtensionObjectSchema
        {
            get
            {
                if (!_reversibleEncoding)
                {
                    // For the non-reversible form, ExtensionObject values
                    // shall be encoded as a JSON value containing only the
                    // value of the Body field. The TypeId and Encoding
                    // fields are dropped.
                    return new JsonSchema
                    {
                        Types = new[] { SchemaType.Object }
                    };
                }
                return new JsonSchema
                {
                    Id = GetId(BuiltInType.ExtensionObject),
                    Types = new[] { SchemaType.Object },
                    Title = GetTitle(BuiltInType.ExtensionObject),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["TypeId"] = GetSchemaForBuiltInType(BuiltInType.NodeId),
                        ["Encoding"] = GetSchemaForBuiltInType(BuiltInType.Byte),
                        ["Body"] = new JsonSchema
                        {
                            Types = new[] { SchemaType.Object }
                        }
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema StatusCodeSchema
        {
            get
            {
                if (_reversibleEncoding)
                {
                    //
                    // StatusCode values shall be encoded as a JSON number for
                    // the reversible encoding. If the StatusCode is Good (0)
                    // it is only encoded if it is an element of a JSON array.
                    //
                    return Simple((int)BuiltInType.StatusCode,
                        SchemaType.Integer, "uint32");
                }

                // For the non - reversible form, StatusCode values
                // shall be encoded as a JSON object with the fields
                // defined here.
                return new JsonSchema
                {
                    Id = GetId(BuiltInType.StatusCode),
                    Types = new[] { SchemaType.Object },
                    Title = GetTitle(BuiltInType.StatusCode),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["Code"] = GetSchemaForBuiltInType(BuiltInType.UInt32),
                        ["Symbol"] = GetSchemaForBuiltInType(BuiltInType.String)
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema QualifiedNameSchema
        {
            get
            {
                if (_encodeNamespacedValuesAsUri)
                {
                    return Simple((int)BuiltInType.QualifiedName, SchemaType.String,
                        "opcuaQualifiedName");
                }

                // For non-reversible encoding this field is the JSON
                // string containing the NamespaceUri associated with
                // the NamespaceIndex unless the NamespaceIndex is 0.
                // If the NamespaceIndex is 0 the field is omitted.
                return new JsonSchema
                {
                    Title = GetTitle(BuiltInType.QualifiedName),
                    Id = GetId(BuiltInType.QualifiedName),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["Name"] = GetSchemaForBuiltInType(BuiltInType.String),
                        ["Uri"] = GetSchemaForBuiltInType(
                            _reversibleEncoding ? BuiltInType.UInt32 : BuiltInType.String)
                    },
                    Required = new[] { "Name" },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema LocalizedTextSchema
        {
            get
            {
                if (!_reversibleEncoding)
                {
                    // For the non-reversible form, LocalizedText value shall
                    // be encoded as a JSON string containing the Text component.
                    return Simple((int)BuiltInType.LocalizedText, SchemaType.String);
                }
                return new JsonSchema
                {
                    Title = GetTitle(BuiltInType.LocalizedText),
                    Types = new[] { SchemaType.Object },
                    Id = GetId(BuiltInType.LocalizedText),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["Locale"] = Simple((int)BuiltInType.String,
                                SchemaType.String, "rfc3066"),
                        ["Text"] = GetSchemaForBuiltInType(BuiltInType.String)
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema NodeIdSchema
        {
            get
            {
                if (_encodeNamespacedValuesAsUri)
                {
                    return Simple((int)BuiltInType.NodeId, SchemaType.String,
                        "opcuaNodeId");
                }

                return new JsonSchema
                {
                    Title = GetTitle(BuiltInType.NodeId),
                    Types = new[] { SchemaType.Object },
                    Id = GetId(BuiltInType.NodeId),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["IdentifierType"] = GetSchemaForBuiltInType(BuiltInType.Byte),
                        ["Id"] = new[]
                        {
                            GetSchemaForBuiltInType(BuiltInType.UInt32),
                            GetSchemaForBuiltInType(BuiltInType.ByteString),
                            GetSchemaForBuiltInType(BuiltInType.Guid),
                            GetSchemaForBuiltInType(BuiltInType.String)
                        }
                        .AsUnion(Definitions, "NodeIdentifer"),
                        //
                        // For reversible encoding this field is a JSON number
                        // with the NamespaceIndex. The field is omitted if the
                        // NamespaceIndex is 0.
                        //
                        // For non-reversible encoding this field is the JSON
                        // string containing the NamespaceUri associated with
                        // the NamespaceIndex unless the NamespaceIndex is 0.
                        // If the NamespaceIndex is 0 the field is omitted.
                        //
                        ["Namespace"] = GetSchemaForBuiltInType(
                            _reversibleEncoding ? BuiltInType.UInt32 : BuiltInType.String)
                    },
                    Required = new[] { "IdentifierType" },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema ExpandedNodeIdSchema
        {
            get
            {
                if (_encodeNamespacedValuesAsUri)
                {
                    return Simple((int)BuiltInType.ExpandedNodeId, SchemaType.String,
                        "opcuaExpandedNodeId");
                }
                return new JsonSchema
                {
                    Title = GetTitle(BuiltInType.ExpandedNodeId),
                    Types = new[] { SchemaType.Object },
                    Id = GetId(BuiltInType.ExpandedNodeId),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["IdentifierType"] = GetSchemaForBuiltInType(BuiltInType.Byte),
                        ["Id"] = new[]
                        {
                            GetSchemaForBuiltInType(BuiltInType.UInt32),
                            GetSchemaForBuiltInType(BuiltInType.ByteString),
                            GetSchemaForBuiltInType(BuiltInType.Guid),
                            GetSchemaForBuiltInType(BuiltInType.String)
                        }
                        .AsUnion(Definitions, "NodeIdentifer"),
                        //
                        // For reversible encoding this field is a JSON number
                        // with the NamespaceIndex. The field is omitted if the
                        // NamespaceIndex is 0.
                        //
                        // For non-reversible encoding this field is the JSON
                        // string containing the NamespaceUri associated with
                        // the NamespaceIndex unless the NamespaceIndex is 0.
                        // If the NamespaceIndex is 0 the field is omitted.
                        //
                        ["Namespace"] = GetSchemaForBuiltInType(
                            _reversibleEncoding ? BuiltInType.UInt32 : BuiltInType.String),
                        //
                        // For reversible encoding this field is a JSON string
                        // with the NamespaceUri if the NamespaceUri is specified.
                        // Otherwise, it is a JSON number with the NamespaceIndex.
                        // The field is omitted if the NamespaceIndex is 0.
                        //
                        // For non-reversible encoding this field is the JSON string
                        // containing the NamespaceUri or the NamespaceUri associated
                        // with the NamespaceIndex unless the NamespaceIndex is 0
                        // or 1. If the NamespaceIndex is 0 the field is omitted.
                        //
                        ["ServerUri"] = GetSchemaForBuiltInType(
                            _reversibleEncoding ? BuiltInType.UInt16 : BuiltInType.String)
                    },
                    Required = new[] { "IdentifierType" },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        private JsonSchema DataValueSchema
        {
            get
            {
                return new JsonSchema
                {
                    Title = GetTitle(BuiltInType.DataValue),
                    Types = new[] { SchemaType.Object },
                    Id = GetId(BuiltInType.DataValue),
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["Value"] = GetSchemaForBuiltInType(BuiltInType.Variant),
                        ["Status"] = GetSchemaForBuiltInType(BuiltInType.StatusCode),
                        ["SourceTimestamp"] = GetSchemaForBuiltInType(BuiltInType.DateTime),
                        ["SourcePicoSeconds"] = GetSchemaForBuiltInType(BuiltInType.UInt16),
                        ["ServerTimestamp"] = GetSchemaForBuiltInType(BuiltInType.DateTime),
                        ["ServerPicoSeconds"] = GetSchemaForBuiltInType(BuiltInType.UInt16)
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            }
        }

        /// <summary>
        /// Create avro schema for json encoder
        /// </summary>
        /// <param name="reversibleEncoding"></param>
        /// <param name="useUriEncoding"></param>
        /// <param name="definitions"></param>
        public BuiltInJsonSchemas(bool reversibleEncoding,
            bool useUriEncoding, Dictionary<string, JsonSchema>? definitions)
        {
            Definitions = definitions ?? new();
            _reversibleEncoding = reversibleEncoding;
            _encodeNamespacedValuesAsUri = useUriEncoding;
        }

        /// <summary>
        /// Create encoding schema
        /// </summary>
        /// <param name="fieldContentMask"></param>
        /// <param name="definitions"></param>
        public BuiltInJsonSchemas(DataSetFieldContentMask fieldContentMask,
            Dictionary<string, JsonSchema>? definitions = null)
        {
            Definitions = definitions ?? new();
            _encodeNamespacedValuesAsUri = true;

            if ((fieldContentMask & DataSetFieldContentMask.RawData) != 0)
            {
                //
                // If the DataSetFieldContentMask results in a RawData
                // representation, the field value is a Variant encoded
                // using the non-reversible OPC UA JSON Data Encoding
                // defined in OPC 10000-6
                //
                _reversibleEncoding = false;
            }
            else if (fieldContentMask == 0)
            {
                //
                // If the DataSetFieldContentMask results in a Variant
                // representation, the field value is encoded as a Variant
                // encoded using the reversible OPC UA JSON Data Encoding
                // defined in OPC 10000-6.
                //
                _reversibleEncoding = true;
            }
            else
            {
                //
                // If the DataSetFieldContentMask results in a DataValue
                // representation, the field value is a DataValue encoded
                // using the non-reversible OPC UA JSON Data Encoding or
                // reversible depending on encoder configuration.
                //
                _reversibleEncoding = false;
            }
        }

        /// <summary>
        /// Get built in schema. See
        /// https://reference.opcfoundation.org/Core/Part6/v104/docs/5.1.2#_Ref131507956
        /// </summary>
        /// <param name="builtInType"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override JsonSchema GetSchemaForBuiltInType(BuiltInType builtInType,
            int rank = ValueRanks.Scalar)
        {
            var typeDefinitionName = GetDefinitionName(builtInType);
            if (!Definitions.TryGetValue(typeDefinitionName, out var schema))
            {
                schema = CreateSchemaForBuiltInType((int)builtInType);
                Definitions.Add(typeDefinitionName, schema);
            }

            if (schema.Id != null)
            {
                schema = Reference(builtInType);
            }
            if (rank >= ValueRanks.OneOrMoreDimensions)
            {
                return new JsonSchema
                {
                    Types = new[] { SchemaType.Array },
                    Items = new[] { schema }
                };
            }
            return schema;

            JsonSchema CreateSchemaForBuiltInType(int id) => id switch
            {
                1 => Simple(id, SchemaType.Boolean),
                2 => Simple(id, SchemaType.Integer, "int8", Limit.From(sbyte.MinValue), Limit.From(sbyte.MaxValue), Const.From(0)),
                3 => Simple(id, SchemaType.Integer, "byte", Limit.From(byte.MinValue), Limit.From(byte.MaxValue), Const.From(0)),
                4 => Simple(id, SchemaType.Integer, "int16", Limit.From(short.MinValue), Limit.From(short.MaxValue), Const.From(0)),
                5 => Simple(id, SchemaType.Integer, "uint16", Limit.From(ushort.MinValue), Limit.From(ushort.MaxValue), Const.From(0)),
                6 => Simple(id, SchemaType.Integer, "int32", Limit.From(int.MinValue), Limit.From(int.MaxValue), Const.From(0)),
                7 => Simple(id, SchemaType.Integer, "uint32", Limit.From(uint.MinValue), Limit.From(uint.MaxValue), Const.From(0)),

                // As per part 6 encoding, long is encoded as string
                8 => Simple(id, SchemaType.String, "int64"),
                9 => Simple(id, SchemaType.String, "uint64"),

                10 => Simple(id, SchemaType.Number, "float", Limit.From(float.MinValue), Limit.From(float.MaxValue), Const.From(0f)),
                11 => Simple(id, SchemaType.Number, "double", Limit.From(double.MinValue), Limit.From(double.MaxValue), Const.From(0d)),
                12 => Simple(id, SchemaType.String),
                13 => Simple(id, SchemaType.String, "date-time"),
                14 => Simple(id, SchemaType.String, "uuid"),
                15 => Simple(id, SchemaType.String, "byte"),
                16 => Simple(id, SchemaType.String, "xmlelement"),

                17 => NodeIdSchema,
                18 => ExpandedNodeIdSchema,
                19 => StatusCodeSchema,
                20 => QualifiedNameSchema,
                21 => LocalizedTextSchema,
                22 => ExtensionObjectSchema,
                23 => DataValueSchema,
                24 => VariantSchema,
                25 => DiagnosticInfoSchema,

                26 => Simple(id, SchemaType.Number),

                // Should this be string? As per json encoding, long is string
                27 => Simple(id, SchemaType.Integer),
                28 => Simple(id, SchemaType.Integer, "unsigned", Limit.From(0)),

                29 => EnumerationSchema,
                _ => throw new ArgumentException($"Built in type {id} unknown")
            };
        }

        /// <inheritdoc/>
        public override JsonSchema GetSchemaForExtendableType(string name, string ns,
            string dataTypeId, JsonSchema bodyType)
        {
            return GetSchemaForBuiltInType(BuiltInType.ExtensionObject);
        }

        /// <inheritdoc/>
        public override JsonSchema GetSchemaForDataSetField(string name, string ns,
            bool asDataValue, JsonSchema valueSchema)
        {
            var id = new UriOrFragment(ns + "#" + name);
            return Definitions.Reference(id, id =>
            {
                var fieldSchema = valueSchema.Resolve(Definitions);
                var type = fieldSchema.Format
                    ?? fieldSchema.Title
                    ?? fieldSchema.SafeGetType().ToString();
                return new JsonSchema
                {
                    Id = id,
                    Title = $"Dataset Field of Type {type}",
                    Types = new[] { SchemaType.Object },
                    Properties = new Dictionary<string, JsonSchema>
                    {
                        ["Value"] = valueSchema,
                        ["Status"] = GetSchemaForBuiltInType(BuiltInType.StatusCode),
                        ["SourceTimestamp"] = GetSchemaForBuiltInType(BuiltInType.DateTime),
                        ["SourcePicoSeconds"] = GetSchemaForBuiltInType(BuiltInType.UInt16),
                        ["ServerTimestamp"] = GetSchemaForBuiltInType(BuiltInType.DateTime),
                        ["ServerPicoSeconds"] = GetSchemaForBuiltInType(BuiltInType.UInt16)
                    },
                    AdditionalProperties = new JsonSchema { Allowed = false }
                };
            });
        }

        /// <summary>
        /// Get data typeid
        /// </summary>
        /// <param name="builtInType"></param>
        /// <returns></returns>
        private static UriOrFragment GetId(BuiltInType builtInType)
        {
            return new UriOrFragment(Namespaces.OpcUa + "#i=" + (int)builtInType);
        }

        /// <summary>
        /// Create primitive opc ua built in type
        /// </summary>
        /// <param name="builtInType"></param>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static JsonSchema Simple(int builtInType,
            SchemaType type, string? format = null, Limit? minValue = null,
            Limit? maxValue = null, Const? defaultValue = null)
        {
            return new JsonSchema
            {
                Title = GetTitle((BuiltInType)builtInType),
                Id = GetId((BuiltInType)builtInType),
                Minimum = minValue,
                Maximum = maxValue,
                Default = defaultValue,
                Types = new[] { type },
                Format = format
            };
        }

        /// <summary>
        /// Create a reference
        /// </summary>
        /// <param name="builtInType"></param>
        /// <returns></returns>
        private static JsonSchema Reference(BuiltInType builtInType)
        {
            return new JsonSchema
            {
                Reference = GetId(builtInType)
            };
        }

        /// <summary>
        /// Create a definition name
        /// </summary>
        /// <param name="builtInType"></param>
        /// <returns></returns>
        private static string GetDefinitionName(BuiltInType builtInType)
        {
            return Namespaces.OpcUa + "#" + builtInType;
        }

        /// <summary>
        /// Create a title
        /// </summary>
        /// <param name="builtInType"></param>
        /// <returns></returns>
        private static string GetTitle(BuiltInType builtInType)
        {
            return $"OPC UA built in type {builtInType}";
        }

        private readonly bool _reversibleEncoding;
        private readonly bool _encodeNamespacedValuesAsUri;
    }
}
