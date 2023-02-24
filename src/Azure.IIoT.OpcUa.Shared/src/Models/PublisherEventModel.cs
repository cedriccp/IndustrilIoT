﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Publisher event
    /// </summary>
    [DataContract]
    public sealed record class PublisherEventModel
    {
        /// <summary>
        /// Event type
        /// </summary>
        [DataMember(Name = "eventType", Order = 0)]
        public PublisherEventType EventType { get; set; }

        /// <summary>
        /// Publisher id
        /// </summary>
        [DataMember(Name = "id", Order = 1,
            EmitDefaultValue = false)]
        public string? Id { get; set; }

        /// <summary>
        /// Publisher
        /// </summary>
        [DataMember(Name = "publisher", Order = 2,
            EmitDefaultValue = false)]
        public PublisherModel? Publisher { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember(Name = "context", Order = 3,
            EmitDefaultValue = false)]
        public OperationContextModel? Context { get; set; }
    }
}
