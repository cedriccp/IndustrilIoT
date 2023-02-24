// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Services.HistoricalAccess.Tests
{
    using Azure.IIoT.OpcUa.Publisher.Services;
    using Azure.IIoT.OpcUa.Models;
    using Azure.IIoT.OpcUa.Testing.Fixtures;
    using Azure.IIoT.OpcUa.Testing.Tests;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(ReadCollection.Name)]
    public class ReadProcessedTests
    {
        public ReadProcessedTests(HistoricalAccessServer server, ITestOutputHelper output)
        {
            _server = server;
            _output = output;
        }

        private HistoryReadValuesProcessedTests<ConnectionModel> GetTests()
        {
            return new HistoryReadValuesProcessedTests<ConnectionModel>(
                () => new HistoryServices<ConnectionModel>(
                    new NodeServices<ConnectionModel>(_server.Client,
                    _output.BuildLogger())), _server.GetConnection());
        }

        private readonly HistoricalAccessServer _server;
        private readonly ITestOutputHelper _output;

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

        [Fact]
        public Task HistoryStreamUInt64ProcessedValuesTest1Async()
        {
            return GetTests().HistoryStreamUInt64ProcessedValuesTest1Async();
        }

        [Fact]
        public Task HistoryStreamUInt64ProcessedValuesTest2Async()
        {
            return GetTests().HistoryStreamUInt64ProcessedValuesTest2Async();
        }

        [Fact]
        public Task HistoryStreamUInt64ProcessedValuesTest3Async()
        {
            return GetTests().HistoryStreamUInt64ProcessedValuesTest3Async();
        }
    }
}
