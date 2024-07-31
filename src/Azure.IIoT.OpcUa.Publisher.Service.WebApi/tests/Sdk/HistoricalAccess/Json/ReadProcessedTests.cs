// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Service.WebApi.Tests.Sdk.HistoricalAccess.Json
{
    using Azure.IIoT.OpcUa.Publisher.Service.WebApi.Tests.Clients;
    using Azure.IIoT.OpcUa.Publisher.Service.Sdk;
    using Azure.IIoT.OpcUa.Publisher.Testing.Fixtures;
    using Azure.IIoT.OpcUa.Publisher.Testing.Tests;
    using Autofac;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(ReadCollection.Name)]
    public sealed class ReadProcessedTests : IClassFixture<WebAppFixture>, IDisposable
    {
        public ReadProcessedTests(WebAppFixture factory, HistoricalAccessServer server, ITestOutputHelper output)
        {
            _factory = factory;
            _server = server;
            _client = factory.CreateClientScope(output, TestSerializerType.NewtonsoftJson);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private HistoryReadValuesProcessedTests<string> GetTests()
        {
            var client = _factory.CreateClient(); // Call to create server
            var registry = _factory.Resolve<IEndpointManager<string>>();
            var endpointId = registry.RegisterEndpointAsync(_server.GetConnection().Endpoint).Result;
            return new HistoryReadValuesProcessedTests<string>(_server, () => // Create an adapter over the api
                new HistoryWebApiAdapter(_client.Resolve<IHistoryServiceApi>()), endpointId);
        }

        private readonly WebAppFixture _factory;
        private readonly HistoricalAccessServer _server;
        private readonly IContainer _client;

        [Fact]
        public Task HistoryReadUInt64ProcessedValuesTest1Async()
        {
            return GetTests().HistoryReadUInt64ProcessedValuesTest1Async();
        }

        [Fact]
        public Task HistoryReadUInt64ProcessedValuesTest2Async()
        {
            return GetTests().HistoryReadUInt64ProcessedValuesTest2Async();
        }

        [Fact]
        public Task HistoryReadUInt64ProcessedValuesTest3Async()
        {
            return GetTests().HistoryReadUInt64ProcessedValuesTest3Async();
        }

        [SkippableFact]
        public Task HistoryStreamUInt64ProcessedValuesTest1Async()
        {
            Skip.If(true, "not implemented yet");
            return GetTests().HistoryStreamUInt64ProcessedValuesTest1Async();
        }

        [SkippableFact]
        public Task HistoryStreamUInt64ProcessedValuesTest2Async()
        {
            Skip.If(true, "not implemented yet");
            return GetTests().HistoryStreamUInt64ProcessedValuesTest2Async();
        }

        [SkippableFact]
        public Task HistoryStreamUInt64ProcessedValuesTest3Async()
        {
            Skip.If(true, "not implemented yet");
            return GetTests().HistoryStreamUInt64ProcessedValuesTest3Async();
        }
    }
}
