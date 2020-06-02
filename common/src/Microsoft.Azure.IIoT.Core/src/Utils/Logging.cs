﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Utils
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Helper class for logging
    /// </summary>
    public static class Logging {
        /// <summary>
        /// Extracts server:port from a uri for simpler logging.
        /// </summary>
        /// <param name="endpointUrl"></param>
        /// <returns></returns>
        public static string ExtractServerPort(string endpointUrl) {
            string pattern = @":\/\/([^\/_]+)";
            var match = Regex.Match(endpointUrl, pattern);

            return match?.Groups[1]?.Value;
        }
    }
}