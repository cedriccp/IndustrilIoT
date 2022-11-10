﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace OpcPublisher_AE_E2E_Tests.Deploy {
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIoTHubEdgeDeployment {

        /// <summary>
        /// Create a new layered deployment or update an existing one
        /// </summary>
        /// <param name="token">The token to cancel the async task</param>
        /// <returns>true if create or update was successful otherwise false</returns>
        Task<bool> CreateOrUpdateLayeredDeploymentAsync(CancellationToken token);
    }
}
