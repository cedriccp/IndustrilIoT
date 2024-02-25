﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher
{
    using Azure.IIoT.OpcUa.Publisher.Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Writer group controller
    /// </summary>
    public interface IWriterGroup : IDisposable
    {
        /// <summary>
        /// Start writer group
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        ValueTask StartAsync(CancellationToken ct);

        /// <summary>
        /// Update writer group
        /// </summary>
        /// <param name="writerGroup"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        ValueTask UpdateAsync(WriterGroupModel writerGroup,
            CancellationToken ct);
    }
}
