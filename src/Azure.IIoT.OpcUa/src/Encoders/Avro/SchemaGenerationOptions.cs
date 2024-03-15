﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders.Avro
{
    using Opc.Ua;

    /// <summary>
    /// Options for the schema generation
    /// </summary>
    public class SchemaGenerationOptions
    {
        /// <summary>
        /// Namespace to use for message
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Use compatibility mode
        /// </summary>
        public bool UseCompatibilityMode { get; set; }

        /// <summary>
        /// Escape field names and symbols in schema
        /// </summary>
        public bool EscapeSymbols { get; set; }

        /// <summary>
        /// Namespace table
        /// </summary>
        public NamespaceTable? Namespaces { get; set; }
    }
}