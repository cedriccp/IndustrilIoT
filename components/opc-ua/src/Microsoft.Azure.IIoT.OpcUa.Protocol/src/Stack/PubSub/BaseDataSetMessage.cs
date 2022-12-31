﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Opc.Ua.PubSub {
    using Opc.Ua.Encoders;
    using System;

    /// <summary>
    /// Data set message
    /// </summary>
    public abstract class BaseDataSetMessage {

        /// <summary>
        /// Content mask
        /// </summary>
        public uint DataSetMessageContentMask { get; set; }

        /// <summary>
        /// Dataset message type
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Dataset writer id
        /// </summary>
        public ushort DataSetWriterId { get; set; }

        /// <summary>
        /// Dataset writer name
        /// </summary>
        public string DataSetWriterName { get; set; }

        /// <summary>
        /// Sequence number
        /// </summary>
        public uint SequenceNumber { get; set; }

        /// <summary>
        /// Metadata version
        /// </summary>
        public ConfigurationVersionDataType MetaDataVersion { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Picoseconds
        /// </summary>
        public uint Picoseconds { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public StatusCode Status { get; set; }

        /// <summary>
        /// Payload
        /// </summary>
        public DataSet Payload { get; set; } = new DataSet();

        /// <summary>
        /// Decode data set message
        /// </summary>
        /// <param name="decoder"></param>
        /// <param name="dataSetFieldContentMask"></param>
        /// <param name="withHeader"></param>
        /// <param name="property"></param>
        public abstract void Decode(IDecoder decoder,
            uint dataSetFieldContentMask,
            bool withHeader = true, string property = null);

        /// <summary>
        /// Encode data set message
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="withHeader"></param>
        /// <param name="property"></param>
        public abstract void Encode(IEncoder encoder,
            bool withHeader = true, string property = null);

        /// <inheritdoc/>
        public override bool Equals(object value) {
            if (ReferenceEquals(this, value)) {
                return true;
            }
            if (!(value is JsonDataSetMessage wrapper)) {
                return false;
            }
            if (!Utils.IsEqual(wrapper.DataSetMessageContentMask, DataSetMessageContentMask) ||
                !Utils.IsEqual(wrapper.DataSetWriterId, DataSetWriterId) ||
                !Utils.IsEqual(wrapper.DataSetWriterName, DataSetWriterName) ||
                !Utils.IsEqual(wrapper.MessageType, MessageType) ||
                !Utils.IsEqual(wrapper.MetaDataVersion, MetaDataVersion) ||
                !Utils.IsEqual(wrapper.SequenceNumber, SequenceNumber) ||
                !Utils.IsEqual(wrapper.Status, Status) ||
                !Utils.IsEqual(wrapper.Timestamp, Timestamp) ||
                !Utils.IsEqual(wrapper.Payload, Payload)) {
                return false;
            }
            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            var hash = new HashCode();
            hash.Add(DataSetMessageContentMask);
            hash.Add(MessageType);
            hash.Add(DataSetWriterId);
            hash.Add(DataSetWriterName);
            hash.Add(SequenceNumber);
            hash.Add(MetaDataVersion);
            hash.Add(Timestamp);
            hash.Add(Picoseconds);
            hash.Add(Status);
            hash.Add(Payload);
            return hash.ToHashCode();
        }
    }
}