﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders.Utils
{
    using Avro;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Helper functions for avro
    /// </summary>
    internal static partial class AvroUtils
    {
        /// <summary>
        /// Namespace zero
        /// </summary>
        public const string NamespaceZeroName = "org.opcfoundation.ua";

        /// <summary>
        /// Null schema
        /// </summary>
        public static Schema Null { get; } = PrimitiveSchema.NewInstance("null");

        /// <summary>
        /// Safely Convert a uri to a namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string NamespaceUriToNamespace(string ns)
        {
            if (!Uri.TryCreate(ns, new UriCreationOptions
            {
                DangerousDisablePathAndQueryCanonicalization = false
            }, out var result))
            {
                return ns.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Escape)
                    .Aggregate((a, b) => $"{a}.{b}");
            }
            else
            {
                return result.Host.Split('.', StringSplitOptions.RemoveEmptyEntries)
                    .Reverse()
                    .Where(c => c != "www")
                    .Concat(result.AbsolutePath.Split('/',
                        StringSplitOptions.RemoveEmptyEntries))
                    .Select(Escape)
                    .Aggregate((a, b) => $"{a}.{b}");
            }
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="schemas"></param>
        /// <returns></returns>
        public static Schema CreateUnion(params Schema[] schemas)
        {
            return CreateUnion(schemas, customProperties: null);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="schemas"></param>
        /// <param name="customProperties"></param>
        /// <returns></returns>
        public static UnionSchema CreateUnion(IEnumerable<Schema> schemas,
            PropertyMap? customProperties = null)
        {
            var types = schemas.Distinct(
                Compare.Using<Schema>((a, b) => a?.Fullname == b?.Fullname)).ToList();
            return UnionSchema.Create(types, customProperties);
        }

        /// <summary>
        /// Get a avro compliant name string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Escape(string name)
        {
            return Escape(name, false);
        }

        /// <summary>
        /// Get a avro compliant name string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string Escape(string name, bool remove)
        {
            return EscapeAvroRegex().Replace(name.Replace('/', '_'),
                match => remove ? string.Empty : $"__{(int)match.Value[0]}");
        }

        [GeneratedRegex("[^a-zA-Z0-9_]")]
        private static partial Regex EscapeAvroRegex();
    }
}
