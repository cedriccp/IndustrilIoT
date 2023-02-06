﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Edge.Publisher {
    using Microsoft.Azure.IIoT.Messaging;
    using Microsoft.Azure.IIoT.OpcUa.Protocol.Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Encoder to encode data set writer messages
    /// </summary>
    public interface IMessageEncoder {

        /// <summary>
        /// Encodes the list of notifications into network messages to send
        /// </summary>
        /// <param name="factory">Factory to create empty messages</param>
        /// <param name="notifications">Notifications to encode</param>
        /// <param name="maxMessageSize">Maximum size of messages</param>
        /// <param name="asBatch">Encode in batch mode</param>
        IEnumerable<ITelemetryEvent> Encode(Func<ITelemetryEvent> factory,
            IEnumerable<SubscriptionNotificationModel> notifications,
            int maxMessageSize, bool asBatch);
    }
}