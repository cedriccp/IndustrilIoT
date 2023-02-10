﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Edge.Publisher.Engine {
    using Microsoft.Azure.IIoT.Diagnostics;
    using Microsoft.Azure.IIoT.Module.Framework.Client;
    using System;

    /// <summary>
    /// Publisher identity
    /// </summary>
    public sealed class PublisherIdentity : IProcessIdentity {

        /// <inheritdoc/>
        public string ProcessId => _factory.DeviceId;
        /// <inheritdoc/>
        public string SiteId => _config?.Site;
        /// <inheritdoc/>
        public string Id => _factory.ModuleId;
        /// <inheritdoc/>
        public string Name => "OPC Publisher";

        /// <summary>
        /// Create identity
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="config"></param>
        public PublisherIdentity(IClientFactory factory,
            IPublisherConfiguration config = null) {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _config = config;
        }

        private readonly IClientFactory _factory;
        private readonly IPublisherConfiguration _config;
    }
}
