// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Models {
    using Microsoft.Azure.IIoT.OpcUa.Subscriber.Models;

    /// <summary>
    /// Publisher sample model extensions
    /// </summary>
    public static class MonitoredItemMessageModelEx {

        /// <summary>
        /// Convert to api model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MonitoredItemMessageApiModel ToApiModel(
            this MonitoredItemSampleModel model) {
            if (model == null) {
                return null;
            }
            return new MonitoredItemMessageApiModel {
                SubscriptionId = model.SubscriptionId,
                EndpointId = model.EndpointId,
                DataSetId = model.DataSetId,
                NodeId = model.NodeId,
                ServerPicoseconds = model.ServerPicoseconds,
                ServerTimestamp = model.ServerTimestamp,
                SourcePicoseconds = model.SourcePicoseconds,
                SourceTimestamp = model.SourceTimestamp,
                Timestamp = model.Timestamp,
                Value = new ValueApiModel() {
                    Body = model.Value?.Type?.IsPrimitive == true ? 
                        model.Value?.Body : model.Value?.Body?.ToString(),
                    Type = model.Value?.Type?.FullName
                },
                TypeId = model?.Value?.Type?.FullName,
                Status = model.Status 
            };
        }
    }
}
