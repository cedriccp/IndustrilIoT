// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Models {
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// reference model
    /// </summary>
    [DataContract]
    public class NodeReferenceApiModel {

        /// <summary>
        /// Reference Type id
        /// </summary>
        [DataMember(Name = "referenceTypeId", Order = 0,
            EmitDefaultValue = false)]
        public string ReferenceTypeId { get; set; }

        /// <summary>
        /// Browse direction of reference
        /// </summary>
        [DataMember(Name = "direction", Order = 1,
            EmitDefaultValue = false)]
        public BrowseDirection? Direction { get; set; }

        /// <summary>
        /// Target node
        /// </summary>
        [DataMember(Name = "target", Order = 2)]
        [Required]
        public NodeApiModel Target { get; set; }

        // Legacy

        /// <ignore/>
        [IgnoreDataMember]
        [Obsolete]
        public string TypeId => ReferenceTypeId;

        /// <ignore/>
        [IgnoreDataMember]
        [Obsolete]
        public string BrowseName => Target?.BrowseName;

        /// <ignore/>
        [IgnoreDataMember]
        [Obsolete]
        public string DisplayName => Target?.DisplayName;

        /// <ignore/>
        [IgnoreDataMember]
        [Obsolete]
        public string TypeDefinition => Target?.TypeDefinitionId;
    }
}