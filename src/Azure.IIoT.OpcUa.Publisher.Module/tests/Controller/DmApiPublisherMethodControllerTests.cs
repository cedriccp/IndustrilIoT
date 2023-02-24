// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Tests.Engine
{
    using Autofac;
    using Azure.IIoT.OpcUa.Publisher;
    using Azure.IIoT.OpcUa.Publisher.Module.Controller;
    using Azure.IIoT.OpcUa.Publisher.Stack;
    using Azure.IIoT.OpcUa.Publisher.Storage;
    using Azure.IIoT.OpcUa.Publisher.Tests.Utils;
    using Azure.IIoT.OpcUa.Models;
    using FluentAssertions;
    using Furly.Extensions.Logging;
    using Furly.Extensions.Serializers;
    using Furly.Extensions.Serializers.Newtonsoft;
    using Microsoft.Azure.IIoT.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Models;
    using Moq;
    using Publisher.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Tests the Direct Methods API for the pubisher
    /// </summary>
    public class DmApiPublisherControllerTests : TempFileProviderBase
    {
        private readonly NewtonsoftJsonSerializer _newtonSoftJsonSerializer;
        private readonly ILogger _logger;
        private readonly PublishedNodesJobConverter _publishedNodesJobConverter;
        private readonly Mock<IPublisherConfiguration> _configMock;
        private PublisherConfigurationService _configService;
        private readonly PublishedNodesProvider _publishedNodesProvider;
        private readonly Mock<IMessageSource> _triggerMock;
        private readonly IPublisherHost _publisher;
        private readonly Mock<IPublisherDiagnosticCollector> _diagnostic;

        /// <summary>
        /// Constructor that initializes common resources used by tests.
        /// </summary>
        public DmApiPublisherControllerTests()
        {
            _newtonSoftJsonSerializer = new NewtonsoftJsonSerializer();
            _logger = Log.Console<DmApiPublisherControllerTests>();

            var engineConfigMock = new Mock<IEngineConfiguration>();
            var clientConfignMock = new Mock<IClientServicesConfig>();

            _publishedNodesJobConverter = new PublishedNodesJobConverter(_logger, _newtonSoftJsonSerializer,
                engineConfigMock.Object, clientConfignMock.Object);

            // Note that each test is responsible for setting content of _tempFile;
            CopyContent("Controller/empty_pn.json", _tempFile);

            _configMock = new Mock<IPublisherConfiguration>();
            _configMock.SetupAllProperties();
            _configMock.SetupGet(p => p.PublishedNodesFile).Returns(_tempFile);
            _configMock.SetupGet(p => p.PublishedNodesSchemaFile).Returns("Storage/publishednodesschema.json");
            _configMock.SetupGet(p => p.MaxNodesPerPublishedEndpoint).Returns(1000);
            _configMock.SetupGet(p => p.MessagingProfile).Returns(MessagingProfile.Get(
                MessagingMode.PubSub, MessageEncoding.Json));

            _publishedNodesProvider = new PublishedNodesProvider(_configMock.Object, _logger);
            _triggerMock = new Mock<IMessageSource>();
            var factoryMock = new Mock<IWriterGroupScopeFactory>();
            var writerGroup = new Mock<IWriterGroup>();
            writerGroup.SetupGet(l => l.Source).Returns(_triggerMock.Object);
            var lifetime = new Mock<IWriterGroupScope>();
            lifetime.SetupGet(l => l.WriterGroup).Returns(writerGroup.Object);
            factoryMock
                .Setup(factory => factory.Create(It.IsAny<IWriterGroupConfig>()))
                .Returns(lifetime.Object);
            _publisher = new PublisherHostService(factoryMock.Object, new Mock<IProcessIdentity>().Object, _logger);
            _diagnostic = new Mock<IPublisherDiagnosticCollector>();
            var mockDiag = new WriterGroupDiagnosticModel();
            _diagnostic.Setup(m => m.TryGetDiagnosticsForWriterGroup(It.IsAny<string>(), out mockDiag)).Returns(true);
        }

        /// <summary>
        /// This method should be called only after content of _tempFile is set.
        /// </summary>
        private void InitPublisherConfigService()
        {
            _configService = new PublisherConfigurationService(
                _publishedNodesJobConverter,
                _configMock.Object,
                _publisher,
                _logger,
                _publishedNodesProvider,
                _newtonSoftJsonSerializer,
                _diagnostic.Object
            );
            _configService.StartAsync().AsTask().GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadCollection.json")]
        public async Task DmApiPublishUnpublishNodesTestAsync(string publishedNodesFile)
        {
            CopyContent("Controller/empty_pn.json", _tempFile);
            InitPublisherConfigService();

            var methodsController = new PublisherMethodsController(_configService);

            using var publishPayloads = new StreamReader(publishedNodesFile);
            var publishNodesRequest = _newtonSoftJsonSerializer.Deserialize<List<PublishedNodesEntryModel>>(
                await publishPayloads.ReadToEndAsync().ConfigureAwait(false));

            foreach (var request in publishNodesRequest)
            {
                var initialNode = request.OpcNodes[0];
                for (var i = 0; i < 10000; i++)
                {
                    request.OpcNodes.Add(new OpcNodeModel
                    {
                        Id = initialNode.Id + i.ToString(),
                        DataSetFieldId = initialNode.DataSetFieldId,
                        DisplayName = initialNode.DisplayName,
                        ExpandedNodeId = initialNode.ExpandedNodeId,
                        HeartbeatIntervalTimespan = initialNode.HeartbeatIntervalTimespan,
                        OpcPublishingInterval = initialNode.OpcPublishingInterval,
                        OpcSamplingInterval = initialNode.OpcSamplingInterval,
                        QueueSize = initialNode.QueueSize,
                        // ToDo: Implement mechanism for SkipFirst.
                        SkipFirst = initialNode.SkipFirst,
                        DataChangeTrigger = initialNode.DataChangeTrigger,
                        DeadbandType = initialNode.DeadbandType,
                        DeadbandValue = initialNode.DeadbandValue
                    });
                }

                await FluentActions
                    .Invoking(async () => await methodsController.PublishNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }

            var writerGroup = Assert.Single(_publisher.WriterGroups);
            Assert.Equal(8, _publisher.Version);

            foreach (var request in publishNodesRequest)
            {
                await FluentActions
                    .Invoking(async () => await methodsController
                    .UnpublishNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }

            Assert.Empty(_publisher.WriterGroups);
            Assert.Equal(15, _publisher.Version);
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadCollection.json")]
        public async Task DmApiPublishUnpublishAllNodesTestAsync(string publishedNodesFile)
        {
            CopyContent("Controller/empty_pn.json", _tempFile);
            InitPublisherConfigService();
            var methodsController = new PublisherMethodsController(_configService);

            using var publishPayloads = new StreamReader(publishedNodesFile);
            var publishNodesRequest = _newtonSoftJsonSerializer.Deserialize<List<PublishedNodesEntryModel>>(
                await publishPayloads.ReadToEndAsync().ConfigureAwait(false));

            foreach (var request in publishNodesRequest)
            {
                var initialNode = request.OpcNodes[0];
                for (var i = 0; i < 10000; i++)
                {
                    request.OpcNodes.Add(new OpcNodeModel
                    {
                        Id = initialNode.Id + i.ToString(),
                        DataSetFieldId = initialNode.DataSetFieldId,
                        DisplayName = initialNode.DisplayName,
                        ExpandedNodeId = initialNode.ExpandedNodeId,
                        HeartbeatIntervalTimespan = initialNode.HeartbeatIntervalTimespan,
                        OpcPublishingInterval = initialNode.OpcPublishingInterval,
                        OpcSamplingInterval = initialNode.OpcSamplingInterval,
                        QueueSize = initialNode.QueueSize,
                        SkipFirst = initialNode.SkipFirst,
                        DataChangeTrigger = initialNode.DataChangeTrigger,
                        DeadbandType = initialNode.DeadbandType,
                        DeadbandValue = initialNode.DeadbandValue
                    });
                }

                await FluentActions
                    .Invoking(async () => await methodsController.PublishNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }

            var writerGroup = Assert.Single(_publisher.WriterGroups);

            var unpublishAllNodesRequest = publishNodesRequest.GroupBy(pn => string.Concat(pn.EndpointUrl, pn.DataSetWriterId, pn.DataSetPublishingInterval))
                .Select(g => g.First()).ToList();

            foreach (var request in unpublishAllNodesRequest)
            {
                request.OpcNodes?.Clear();
                await FluentActions
                    .Invoking(async () => await methodsController
                    .UnpublishAllNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }

            Assert.Empty(_publisher.WriterGroups);
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadCollection.json")]
        public async Task DmApiPublishNodesToJobTestAsync(string publishedNodesFile)
        {
            CopyContent("Controller/empty_pn.json", _tempFile);
            InitPublisherConfigService();

            var methodsController = new PublisherMethodsController(_configService);

            using var publishPayloads = new StreamReader(publishedNodesFile);
            var publishNodesRequests = _newtonSoftJsonSerializer.Deserialize<List<PublishedNodesEntryModel>>
                (await publishPayloads.ReadToEndAsync().ConfigureAwait(false));

            foreach (var request in publishNodesRequests)
            {
                await FluentActions
                    .Invoking(async () => await methodsController
                    .PublishNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }

            var jobModel = Assert.Single(_publisher.WriterGroups);

            jobModel.WriterGroup.DataSetWriters.Count.Should().Be(6);

            Assert.All(jobModel.WriterGroup.DataSetWriters,
                writer => Assert.Equal(publishNodesRequests[0].EndpointUrl,
                    writer.DataSet.DataSetSource.Connection.Endpoint.Url));
            Assert.Equal(publishNodesRequests
                .Select(n => n.UseSecurity ? SecurityMode.Best : SecurityMode.None)
                .ToHashSet(),
                jobModel.WriterGroup.DataSetWriters
                .Select(w => w.DataSet.DataSetSource.Connection.Endpoint.SecurityMode.Value)
                .ToHashSet());
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadTwoEndpoints.json")]
        public async Task DmApiGetConfiguredNodesOnEndpointAsyncTestAsync(string publishedNodesFile)
        {
            const string endpointUrl = "opc.tcp://opcplc:50010";

            var endpointRequest = new PublishedNodesEntryModel
            {
                EndpointUrl = endpointUrl,
            };

            var methodsController = await PublishNodeAsync(publishedNodesFile).ConfigureAwait(false);
            var response = await FluentActions
                    .Invoking(async () => await methodsController
                    .GetConfiguredNodesOnEndpointAsync(endpointRequest).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);

            response.Subject.OpcNodes.Count
                .Should()
                .Be(2);
            response.Subject.OpcNodes[0].Id
                .Should()
                .Be("ns=2;s=FastUInt1");
            response.Subject.OpcNodes[1].Id
                .Should()
                .Be("ns=2;s=FastUInt2");
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadTwoEndpoints.json")]
        public async Task DmApiGetConfiguredNodesOnEndpointAsyncDataSetWriterGroupTestAsync(string publishedNodesFile)
        {
            const string endpointUrl = "opc.tcp://opcplc:50000";
            const string dataSetWriterGroup = "Leaf0";
            const string dataSetWriterId = "Leaf0_10000_3085991c-b85c-4311-9bfb-a916da952234";
            const string dataSetName = "Tag_Leaf0_10000_3085991c-b85c-4311-9bfb-a916da952234";
            const OpcAuthenticationMode authenticationMode = OpcAuthenticationMode.UsernamePassword;
            const string username = "usr";
            const string password = "pwd";

            var endpointRequest = new PublishedNodesEntryModel
            {
                EndpointUrl = endpointUrl,
                DataSetWriterGroup = dataSetWriterGroup,
                DataSetWriterId = dataSetWriterId,
                DataSetName = dataSetName,
                OpcAuthenticationMode = authenticationMode,
                OpcAuthenticationUsername = username,
                OpcAuthenticationPassword = password,
            };

            var methodsController = await PublishNodeAsync(publishedNodesFile, a => a.DataSetWriterGroup == "Leaf0").ConfigureAwait(false);

            var response = await FluentActions
                    .Invoking(async () => await methodsController
                    .GetConfiguredNodesOnEndpointAsync(endpointRequest).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);

            response.Subject.OpcNodes.Count
                .Should()
                .Be(1);
            response.Subject.OpcNodes[0].Id
                .Should()
                .Be("ns=2;s=SlowUInt1");
        }

        [Fact]
        public async Task DmApiGetConfiguredNodesOnEndpointAsyncDataSetWriterIdTestAsync()
        {
            // Testing that we can differentiate between endpoints
            // even if they only have different DataSetWriterIds.

            var opcNodes = Enumerable.Range(0, 5)
                .Select(i => new OpcNodeModel
                {
                    Id = $"nsu=http://microsoft.com/Opc/OpcPlc/;s=FastUInt{i}",
                })
                .ToList();

            var endpoints = Enumerable.Range(0, 5)
                .Select(i => new PublishedNodesEntryModel
                {
                    EndpointUrl = "opc.tcp://opcplc:50000",
                    DataSetWriterId = i != 0
                        ? $"DataSetWriterId{i}"
                        : null,
                    OpcNodes = opcNodes.GetRange(0, i + 1).ToList(),
                })
                .ToList();

            var methodsController = await PublishNodeAsync("Controller/empty_pn.json").ConfigureAwait(false);

            for (var i = 0; i < 5; ++i)
            {
                await methodsController.PublishNodesAsync(endpoints[i]).ConfigureAwait(false);
            }

            for (var i = 0; i < 5; ++i)
            {
                var response = await FluentActions
                        .Invoking(async () => await methodsController
                        .GetConfiguredNodesOnEndpointAsync(endpoints[i]).ConfigureAwait(false))
                        .Should()
                        .NotThrowAsync()
                        .ConfigureAwait(false);

                response.Subject.OpcNodes.Count
                    .Should()
                    .Be(i + 1);
                response.Subject.OpcNodes.Last().Id
                    .Should()
                    .Be($"nsu=http://microsoft.com/Opc/OpcPlc/;s=FastUInt{i}");
            }
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadTwoEndpoints.json")]
        public async Task DmApiGetConfiguredNodesOnEndpointAsyncUseSecurityTestAsync(string publishedNodesFile)
        {
            const string endpointUrl = "opc.tcp://opcplc:50000";
            const bool useSecurity = false;

            var endpointRequest = new PublishedNodesEntryModel
            {
                EndpointUrl = endpointUrl,
                UseSecurity = useSecurity,
            };

            var methodsController = await PublishNodeAsync(publishedNodesFile).ConfigureAwait(false);

            var response = await FluentActions
                    .Invoking(async () => await methodsController
                    .GetConfiguredNodesOnEndpointAsync(endpointRequest).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);

            response.Subject.OpcNodes.Count
                .Should()
                .Be(1);
            response.Subject.OpcNodes[0].Id
                .Should()
                .Be("ns=2;s=SlowUInt3");
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadTwoEndpoints.json")]
        public async Task DmApiGetConfiguredNodesOnEndpointAsyncOpcAuthenticationModeTestAsync(string publishedNodesFile)
        {
            const string endpointUrl = "opc.tcp://opcplc:50000";
            const string dataSetWriterGroup = "Leaf1";
            const string dataSetWriterId = "Leaf1_10000_3085991c-b85c-4311-9bfb-a916da952235";
            const string dataSetName = "Tag_Leaf1_10000_3085991c-b85c-4311-9bfb-a916da952235";
            const int dataSetPublishingInterval = 3000;
            const OpcAuthenticationMode authenticationMode = OpcAuthenticationMode.Anonymous;

            var endpointRequest = new PublishedNodesEntryModel
            {
                EndpointUrl = endpointUrl,
                DataSetWriterGroup = dataSetWriterGroup,
                DataSetWriterId = dataSetWriterId,
                DataSetName = dataSetName,
                DataSetPublishingInterval = dataSetPublishingInterval,
                OpcAuthenticationMode = authenticationMode,
            };

            var methodsController = await PublishNodeAsync(publishedNodesFile, a => a.DataSetWriterGroup == "Leaf1").ConfigureAwait(false);

            var response = await FluentActions
                    .Invoking(async () => await methodsController
                    .GetConfiguredNodesOnEndpointAsync(endpointRequest).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);

            response.Subject.OpcNodes.Count
                .Should()
                .Be(1);
            response.Subject.OpcNodes[0].Id
                .Should()
                .Be("ns=2;s=SlowUInt2");
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadTwoEndpoints.json")]
        public async Task DmApiGetConfiguredNodesOnEndpointAsyncUsernamePasswordTestAsync(string publishedNodesFile)
        {
            const string endpointUrl = "opc.tcp://opcplc:50000";
            const OpcAuthenticationMode authenticationMode = OpcAuthenticationMode.UsernamePassword;
            const string username = "usr";
            const string password = "pwd";

            var endpointRequest = new PublishedNodesEntryModel
            {
                EndpointUrl = endpointUrl,
                OpcAuthenticationMode = authenticationMode,
                OpcAuthenticationUsername = username,
                OpcAuthenticationPassword = password,
            };

            var methodsController = await PublishNodeAsync(publishedNodesFile).ConfigureAwait(false);

            var response = await FluentActions
                    .Invoking(async () => await methodsController
                    .GetConfiguredNodesOnEndpointAsync(endpointRequest).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);

            response.Subject.OpcNodes.Count
                .Should()
                .Be(2);
            response.Subject.OpcNodes[0].Id
                .Should()
                .Be("ns=2;s=FastUInt3");
            response.Subject.OpcNodes[1].Id
                .Should()
                .Be("ns=2;s=FastUInt4");
        }

        /// <summary>
        /// publish nodes from publishedNodesFile
        /// </summary>
        private async Task<PublisherMethodsController> PublishNodeAsync(string publishedNodesFile,
            Func<PublishedNodesEntryModel, bool> predicate = null)
        {
            CopyContent("Controller/empty_pn.json", _tempFile);
            InitPublisherConfigService();

            var methodsController = new PublisherMethodsController(_configService);

            using var publishPayloads = new StreamReader(publishedNodesFile);
            var publishNodesRequest = _newtonSoftJsonSerializer.Deserialize<List<PublishedNodesEntryModel>>(
                await publishPayloads.ReadToEndAsync().ConfigureAwait(false));

            foreach (var request in publishNodesRequest.Where(predicate ?? (_ => true)))
            {
                await FluentActions
                    .Invoking(async () => await methodsController.PublishNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }
            return methodsController;
        }

        [Theory]
        [InlineData("Controller/DmApiPayloadCollection.json")]
        public async Task DmApiGetConfiguredEndpointsTestAsync(string publishedNodesFile)
        {
            CopyContent("Controller/empty_pn.json", _tempFile);
            InitPublisherConfigService();
            var methodsController = new PublisherMethodsController(_configService);

            using var publishPayloads = new StreamReader(publishedNodesFile);
            var publishNodesRequests = _newtonSoftJsonSerializer.Deserialize<List<PublishedNodesEntryModel>>
                (await publishPayloads.ReadToEndAsync().ConfigureAwait(false));

            // Check that GetConfiguredEndpointsAsync returns empty list
            var endpoints = await FluentActions
                .Invoking(async () => await methodsController
                .GetConfiguredEndpointsAsync().ConfigureAwait(false))
                .Should()
                .NotThrowAsync()
                .ConfigureAwait(false);

            endpoints.Subject.Endpoints.Count.Should().Be(0);

            // Publish nodes
            foreach (var request in publishNodesRequests)
            {
                await FluentActions
                    .Invoking(async () => await methodsController
                    .PublishNodesAsync(request).ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);
            }

            // Check configured endpoints count
            endpoints = await FluentActions
                .Invoking(async () => await methodsController
                .GetConfiguredEndpointsAsync().ConfigureAwait(false))
                .Should()
                .NotThrowAsync()
                .ConfigureAwait(false);

            endpoints.Subject.Endpoints.Count.Should().Be(5);
            var tags = endpoints.Subject.Endpoints.Select(e => e.DataSetName).ToHashSet();
            tags.Should().Contain("Tag_Leaf0_10000_3085991c-b85c-4311-9bfb-a916da952234");
            tags.Should().Contain("Tag_Leaf1_10000_2e4fc28f-ffa2-4532-9f22-378d47bbee5d");
            tags.Should().Contain("Tag_Leaf2_10000_3085991c-b85c-4311-9bfb-a916da952234");
            tags.Should().Contain("Tag_Leaf3_10000_2e4fc28f-ffa2-4532-9f22-378d47bbee5d");
            tags.Should().Contain((string)null);

            var endpointsHash = endpoints.Subject.Endpoints.ConvertAll(e => e.GetHashCode());
            Assert.Equal(endpointsHash.Distinct().Count(), endpointsHash.Count);
        }

        [Fact]
        public async Task DmApiGetDiagnosticInfoTestAsync()
        {
            CopyContent("Controller/empty_pn.json", _tempFile);
            InitPublisherConfigService();
            var methodsController = new PublisherMethodsController(_configService);

            var response = await FluentActions
                    .Invoking(async () => await methodsController
                    .GetDiagnosticInfoAsync().ConfigureAwait(false))
                    .Should()
                    .NotThrowAsync()
                    .ConfigureAwait(false);

            response.Subject
                .Should()
                .NotBeNull();
        }

        /// <summary>
        /// Copy content of source file to destination file.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public static void CopyContent(string sourcePath, string destinationPath)
        {
            var content = GetFileContent(sourcePath);

            using (var fileStream = new FileStream(destinationPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                fileStream.Write(Encoding.UTF8.GetBytes(content));
            }

            static string GetFileContent(string path)
            {
                using var payloadReader = new StreamReader(path);
                return payloadReader.ReadToEnd();
            }
        }
    }
}
