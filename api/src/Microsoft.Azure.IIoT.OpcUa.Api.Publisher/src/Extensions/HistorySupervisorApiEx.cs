// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Extensions {
    using Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Extensions;
    using Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Models;
    using Microsoft.Azure.IIoT.Serializers;
    using System.Threading;
    using System.Threading.Tasks;
#if ZOMBIE

    /// <summary>
    /// Extensions
    /// </summary>
    public static class HistorySupervisorApiEx {
#if ZOMBIE

        /// <summary>
        /// Read node history with custom encoded extension object details
        /// </summary>
        /// <param name="api"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static Task<HistoryReadResponseApiModel<VariantValue>> HistoryReadRawAsync(
            this IHistoryModuleApi api, string endpointUrl, HistoryReadRequestApiModel<VariantValue> request,
            CancellationToken ct = default) {
            return api.HistoryReadRawAsync(ConnectionTo(endpointUrl), request, ct);
        }
#endif
#if ZOMBIE

        /// <summary>
        /// Read history call with custom encoded extension object details
        /// </summary>
        /// <param name="api"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static Task<HistoryReadNextResponseApiModel<VariantValue>> HistoryReadRawNextAsync(
            this IHistoryModuleApi api, string endpointUrl, HistoryReadNextRequestApiModel request,
            CancellationToken ct = default) {
            return api.HistoryReadRawNextAsync(ConnectionTo(endpointUrl), request, ct);
        }
#endif
#if ZOMBIE

        /// <summary>
        /// Update using raw extension object details
        /// </summary>
        /// <param name="api"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static Task<HistoryUpdateResponseApiModel> HistoryUpdateRawAsync(
            this IHistoryModuleApi api, string endpointUrl, HistoryUpdateRequestApiModel<VariantValue> request,
            CancellationToken ct = default) {
            return api.HistoryUpdateRawAsync(ConnectionTo(endpointUrl), request, ct);
        }
#endif

        /// <summary>
        /// New connection
        /// </summary>
        /// <param name="endpointUrl"></param>
        /// <returns></returns>
        private static ConnectionApiModel ConnectionTo(string endpointUrl) {
            return new ConnectionApiModel {
                Endpoint = new EndpointApiModel {
                    Url = endpointUrl,
                    SecurityMode = SecurityMode.None,
                    SecurityPolicy = "None"
                }
            };
        }
    }
#endif
}
