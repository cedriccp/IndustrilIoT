﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Edge.Publisher {
    /// <summary>
    /// Constants for diagnostics
    /// </summary>
    internal static class Constants {

        /// <summary>
        /// Writer group identifier tag
        /// </summary>
        public const string WriterGroupIdTag = "writerGroupId";

        /// <summary>
        /// Default writer group id
        /// </summary>
        public const string DefaultWriterGroupId = "$default";

        /// <summary>
        /// Publisher identifier tag
        /// </summary>
        public const string PublisherIdTag = "publisherId";

        /// <summary>
        /// Default publisher id
        /// </summary>
        public const string DefaultPublisherId = "$default";

        /// <summary>
        /// Timestamp tag (start time)
        /// </summary>
        public const string TimeStampTag = "timestamp_utc";
    }
}