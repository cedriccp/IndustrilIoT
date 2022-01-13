﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Publisher.Config.Models {
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contains the details of an endpoint
    /// </summary>
    [DataContract]
    public class ConfiguredEndpointModel {

        /// <summary> Id Identifier of the DataFlow - DataSetWriterId. </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string DataSetWriterId { get; set; }

        /// <summary> The Group the stream belongs to - DataSetWriterGroup. </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string DataSetWriterGroup { get; set; }

        /// <summary> The Publishing interval for a dataset writer </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public int? DataSetPublishingInterval { get; set; }

        /// <summary> The endpoint URL of the OPC UA server. </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = true)]
        public Uri EndpointUrl { get; set; }

        /// <summary> Secure transport should be used to </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public bool? UseSecurity { get; set; }

        /// <summary> The node to monitor in "ns=" syntax. </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public NodeIdModel NodeId { get; set; }

        /// <summary> authentication mode </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public OpcAuthenticationMode OpcAuthenticationMode { get; set; }

        /// <summary> encrypted username </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string EncryptedAuthUsername { get; set; }

        /// <summary> plain username </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string OpcAuthenticationUsername { get; set; }
    }
}