﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher
{
    using System.Diagnostics.Metrics;

    /// <summary>
    /// Industrial iot diagnostics
    /// </summary>
    public static class Diagnostics
    {
        /// <summary>
        /// Metrics
        /// </summary>
        public static readonly Meter Meter = new("Azure.Industrial-IoT", "2.9");
    }
}
