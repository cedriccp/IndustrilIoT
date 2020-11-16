﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace IIoTPlatform_E2E_Tests {

    /// <summary>
    /// Contains constants using for End 2 End testing
    /// </summary>
    internal static class TestConstants {
        /// <summary>
        /// Character that need to be used when split value of "PLC_SIMULATION_URLS"
        /// </summary>
        public static char SimulationUrlsSeparator  = ';';

        /// <summary>
        /// Contains constants for all routes to Industrial IoT Platform
        /// </summary>
        internal static class APIRoutes {
            
            /// <summary>
            /// Route to applications within registry 
            /// </summary>
            public const string RegistryApplications = "/registry/v2/applications";
        }

        /// <summary>
        /// Contains constants for all HTTP headers
        /// </summary>
        internal static class HttpHeaderNames {
            /// <summary>
            /// Name of header used for authentication
            /// </summary>
            public const string Authorization = "Authorization";
        }

        /// <summary>
        /// Contains constants for OPC PLC
        /// </summary>
        internal static class OpcSimulation {
            /// <summary>
            /// Default port of OPC UA Server endpoint of OPC PLC
            /// </summary>
            public const ushort Port = 50000;

            /// <summary>
            /// Name of Published Nodes Json file generated by OPC PLC, containing information
            /// of provided (simulated) OPC UA Nodes
            /// </summary>
            public const string PublishedNodesFile = "pn.json";
        }

        /// <summary>
        /// Contains names of Environment variables available for tests
        /// </summary>
        internal static class EnvironmentVariablesNames {
            /// <summary>
            /// Base URL of Industrial IoT Platform
            /// </summary>
            public const string PCS_SERVICE_URL = "PCS_SERVICE_URL";

            /// <summary>
            /// Tenant name used for authentication of Industrial IoT Platform
            /// </summary>
            public const string PCS_AUTH_TENANT = "PCS_AUTH_TENANT";
            
            /// <summary>
            /// Client App ID used for authentication of Industrial IoT Platform
            /// </summary>
            public const string PCS_AUTH_CLIENT_APPID = "PCS_AUTH_CLIENT_APPID";

            /// <summary>
            /// Client Secrete used for authentication of Industrial IoT Platform
            /// </summary>
            public const string PCS_AUTH_CLIENT_SECRET = "PCS_AUTH_CLIENT_SECRET";

            /// <summary>
            /// Name of Industrial IoT Platform instance
            /// </summary>
            public const string ApplicationName = "ApplicationName";

            /// <summary>
            /// Semicolon separated URLs to load published_nodes.json from OPC-PLCs
            /// </summary>
            public const string PLC_SIMULATION_URLS = "PLC_SIMULATION_URLS";

            /// <summary>
            /// Device identity of edge device at IoT Hub
            /// </summary>
            public const string IOT_EDGE_DEVICE_ID = "IOT_EDGE_DEVICE_ID";

            /// <summary>
            /// DNS name of edge device
            /// </summary>
            public const string IOT_EDGE_DEVICE_DNS_NAME = "IOT_EDGE_DEVICE_DNS_NAME";

            /// <summary>
            /// User name of vm that hosting edge device
            /// </summary>
            public const string PCS_SIMULATION_USER = "PCS_SIMULATION_USER";

            /// <summary>
            /// Password of vm that hosting edge device
            /// </summary>
            public const string PCS_SIMULATION_PASSWORD = "PCS_SIMULATION_PASSWORD";
        }
    }
}
