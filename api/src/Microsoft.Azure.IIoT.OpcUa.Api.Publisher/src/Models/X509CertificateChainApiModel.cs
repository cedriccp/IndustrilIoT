// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Models {
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Certificate chain
    /// </summary>
    [DataContract]
    public sealed class X509CertificateChainApiModel {

        /// <summary>
        /// Chain
        /// </summary>
        [DataMember(Name = "chain", Order = 0,
            EmitDefaultValue = false)]
        public List<X509CertificateApiModel> Chain { get; set; }

        /// <summary>
        /// Chain validation status if validated
        /// </summary>
        [DataMember(Name = "status", Order = 1,
            EmitDefaultValue = false)]
        public List<X509ChainStatus> Status { get; set; }
    }
}