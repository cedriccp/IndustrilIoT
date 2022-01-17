﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Modules.OpcUa.Publisher.Models {
    using Microsoft.Azure.IIoT.OpcUa.Api.Publisher.Models;
    using Microsoft.Azure.IIoT.OpcUa.Publisher.Config.Models;
    using Microsoft.Azure.IIoT.OpcUa.Publisher.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Model extension for Publisher module
    /// </summary>
    public static class PublisherExtensions {

        /// <summary>
        /// Create a service model for an api model
        /// </summary>
        public static PublishedNodesEntryModel ToServiceModel(
            this PublishNodesRequestApiModel model) {
            if (model == null) {
                return null;
            }
            return new PublishedNodesEntryModel {
                EndpointUrl = new Uri(model.EndpointUrl),
                UseSecurity = model.UseSecurity,
                OpcAuthenticationMode = (OpcAuthenticationMode)model.OpcAuthenticationMode,
                OpcAuthenticationPassword = model.Password,
                OpcAuthenticationUsername = model.UserName,
                OpcNodes = model.OpcNodes.Select(n => n.ToServiceModel()).ToList(),
                DataSetWriterGroup = model.DataSetWriterGroup,
                DataSetWriterId = model.DataSetWriterId,
                DataSetPublishingInterval = model.DataSetPublishingInterval,
            };
        }

        /// <summary>
        /// Create service model for an api model
        /// </summary>
        public static OpcNodeModel ToServiceModel(
            this PublishedNodeApiModel model) {
            if (model == null) {
                return null;
            }
            return new OpcNodeModel {
                Id = model.Id,
                DataSetFieldId = model.DataSetFieldId,
                DisplayName = model.DisplayName,
                ExpandedNodeId = model.ExpandedNodeId,
                OpcPublishingInterval = model.OpcPublishingInterval,
                OpcSamplingInterval = model.OpcSamplingInterval,
                HeartbeatIntervalTimespan = model.HeartbeatIntervalTimespan,
                SkipFirst = model.SkipFirst,
                QueueSize = model.QueueSize,
            };
        }

        /// <summary>
        /// Create a service model for an api model
        /// </summary>
        public static PublishedNodesEntryModel ToServiceModel(
            this GetConfiguredNodesOnEndpointsRequestApiModel model) {
            if (model == null) {
                return null;
            }
            return new PublishedNodesEntryModel {
                EndpointUrl = new Uri(model.EndpointUrl)
            };
        }

        /// <summary>
        /// Create an api model from service model
        /// </summary>
        public static PublishedNodesResponseApiModel ToApiModel(
            this List<string> model) {
            if (model == null) {
                return null;
            }

            return new PublishedNodesResponseApiModel {
                StatusMessage = model
            };
        }

        /// <summary>
        /// Create an api model from service model
        /// </summary>
        public static GetConfiguredNodesOnEndpointResponseApiModel ToApiModel(
            this List<OpcNodeOnEndpointModel> model, string EndpointUrl) {
            if (model == null) {
                return null;
            }
            return new GetConfiguredNodesOnEndpointResponseApiModel {
                EndpointUrl = EndpointUrl,
                OpcNodes = model.Select(n => n.ToApiModel()).ToList(),
            };
        }


        /// <summary>
        /// Create an api model from service model
        /// </summary>
        public static OpcNodeOnEndpointApiModel ToApiModel(
            this OpcNodeOnEndpointModel model) {
            if (model == null) {
                return null;
            }
            return new OpcNodeOnEndpointApiModel {
                Id = model.Id,
                ExpandedNodeId = model.ExpandedNodeId,
                OpcSamplingInterval = model.OpcSamplingInterval,
                OpcPublishingInterval = model.OpcPublishingInterval,
                DataSetFieldId = model.DataSetFieldId,
                DisplayName = model.DisplayName,
                HeartbeatInterval = model.HeartbeatInterval,
                SkipFirst = model.SkipFirst,
                QueueSize = model.QueueSize,
            };
        }
    }
}
