﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Service.Sdk
{
    using Azure.IIoT.OpcUa.Publisher.Service.Sdk.Clients;
    using Azure.IIoT.OpcUa.Publisher.Service.Sdk.Runtime;
    using Azure.IIoT.OpcUa.Publisher.Service.Sdk.SignalR;
    using Autofac;
    using Microsoft.Azure.IIoT.Auth.Clients;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Service sdk module
    /// </summary>
    public class ServiceSdk : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiConfig>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AadApiClientConfig>()
                .AsImplementedInterfaces();

            builder.AddNewtonsoftJsonSerializer();

            // Register http clients ...
            builder.ConfigureServices(
                services => services.AddHttpClient());
            // ... as well as signalR client (needed for api)
            builder.RegisterType<SignalRHubClient>()
                .AsImplementedInterfaces().SingleInstance();

            // Use bearer authentication
            builder.RegisterModule<NativeClientAuthentication>();

            // Register twin and registry services clients
            builder.RegisterType<TwinServiceClient>()
                .AsImplementedInterfaces();
            builder.RegisterType<RegistryServiceClient>()
                .AsImplementedInterfaces();
            builder.RegisterType<HistoryServiceClient>()
                .AsImplementedInterfaces();
            builder.RegisterType<PublisherServiceClient>()
                .AsImplementedInterfaces();

            // ... with client event callbacks
            builder.RegisterType<RegistryServiceEvents>()
                .AsImplementedInterfaces();
            builder.RegisterType<PublisherServiceEvents>()
                .AsImplementedInterfaces();
        }
    }
}
