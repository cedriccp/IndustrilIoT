// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace IIoTPlatformE2ETests.Standalone
{
    using Azure.IIoT.OpcUa.Publisher.Models;
    using Furly.Extensions.Serializers;
    using IIoTPlatformE2ETests.Deploy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using TestEventProcessor.BusinessLogic;
    using TestExtensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The test theory using different (ordered) test cases to go thru all required steps of publishing OPC UA node
    /// </summary>
    [TestCaseOrderer(TestCaseOrderer.FullName, TestConstants.TestAssemblyName)]
    [Collection("IIoT Standalone Direct Methods Test Collection")]
    [Trait(TestConstants.TraitConstants.PublisherModeTraitName, TestConstants.TraitConstants.PublisherModeTraitValue)]
    public class BPublishMultipleNodesStandaloneDirectMethodTestTheory : DirectMethodTestBase
    {
        private readonly CancellationTokenSource _cts;

        public BPublishMultipleNodesStandaloneDirectMethodTestTheory(
            ITestOutputHelper output,
            IIoTMultipleNodesTestContext context
        ) : base(output, context)
        {
            _cts = new CancellationTokenSource(TestConstants.MaxTestTimeoutMilliseconds);

            // Clean publishednodes.json.
            TestHelper.CleanPublishedNodesJsonFilesAsync(_context).Wait();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cts.Dispose();
            }
            base.Dispose(disposing);
        }

        [Theory]
        [InlineData(MessagingMode.Samples, false)]
        [InlineData(MessagingMode.Samples, true)]
        [InlineData(MessagingMode.PubSub, false)]
        [InlineData(MessagingMode.PubSub, true)]
        public async Task SubscribeUnsubscribeDirectMethodTest(MessagingMode messagingMode, bool useAddOrUpdate)
        {
            // When useAddOrUpdate is true, all publishing and unpublishing operations
            // will be performed through AddOrUpdateEndpoints direct method.

            var ioTHubEdgeBaseDeployment = new IoTHubEdgeBaseDeployment(_context);
            var ioTHubPublisherDeployment = new IoTHubPublisherDeployment(_context, messagingMode);

            _iotHubPublisherModuleName = ioTHubPublisherDeployment.ModuleName;

            // Clear context.
            _context.Reset();

            // Create base edge deployment.
            var baseDeploymentResult = await ioTHubEdgeBaseDeployment.CreateOrUpdateLayeredDeploymentAsync(_cts.Token);
            Assert.True(baseDeploymentResult, "Failed to create/update new edge base deployment.");
            _output.WriteLine("Created/Updated new edge base deployment.");

            // Create layered edge deployment.
            var layeredDeploymentResult = await ioTHubPublisherDeployment.CreateOrUpdateLayeredDeploymentAsync(_cts.Token);
            Assert.True(layeredDeploymentResult, "Failed to create/update layered deployment for publisher module.");
            _output.WriteLine("Created/Updated layered deployment for publisher module.");

            await TestHelper.SwitchToStandaloneModeAsync(_context, _cts.Token);

            // We will wait for module to be deployed.
            await _context.RegistryHelper.WaitForSuccessfulDeploymentAsync(
                ioTHubPublisherDeployment.GetDeploymentConfiguration(),
                _cts.Token
            );

            await _context.RegistryHelper.WaitForIIoTModulesConnectedAsync(
                _context.DeviceConfig.DeviceId,
                _cts.Token,
                new string[] { ioTHubPublisherDeployment.ModuleName }
            );

            // We've observed situations when even after the above waits the module did not yet restart.
            // That leads to situations where the publishing of nodes happens just before the restart to apply
            // new container creation options. After restart persisted nodes are picked up, but on the telemetry side
            // the restart causes dropped messages to be detected. That happens because just before the restart OPC Publisher
            // manages to send some telemetry. This wait makes sure that we do not run the test while restart is happening.
            await Task.Delay(TestConstants.AwaitInitInMilliseconds, _cts.Token);

            _output.WriteLine("OPC Publisher module is up and running.");

            // Call GetConfiguredEndpoints direct method, initially there should be no endpoints
            var responseGetConfiguredEndpoints = await CallMethodAsync(
                new MethodParameterModel
                {
                    Name = TestConstants.DirectMethodNames.GetConfiguredEndpoints
                },
                _cts.Token
            );

            Assert.Equal((int)HttpStatusCode.OK, responseGetConfiguredEndpoints.Status);
            var configuredEndpointsResponse = _serializer.Deserialize<GetConfiguredEndpointsResponseModel>(responseGetConfiguredEndpoints.JsonPayload);
            Assert.Empty(configuredEndpointsResponse.Endpoints);

            const int numberOfNodes = 250;

            var request = await TestHelper
                .CreateMultipleNodesModelAsync(_context, _cts.Token, numberOfNodes: numberOfNodes)
;
            MethodResultModel response = null;

            // Publish nodes for the endpoint
            if (useAddOrUpdate)
            {
                // Call AddOrUpdateEndpoints direct method
                response = await CallMethodAsync(
                    new MethodParameterModel
                    {
                        Name = TestConstants.DirectMethodNames.AddOrUpdateEndpoints,
                        JsonPayload = _serializer.SerializeToString(new List<PublishedNodesEntryModel> { request })
                    },
                    _cts.Token
                );
            }
            else
            {
                // Call PublishNodes direct method
                response = await CallMethodAsync(
                    new MethodParameterModel
                    {
                        Name = TestConstants.DirectMethodNames.PublishNodes,
                        JsonPayload = _serializer.SerializeToString(request)
                    },
                    _cts.Token
                );
            }

            Assert.Equal((int)HttpStatusCode.OK, response.Status);

            // Use test event processor to verify data send to IoT Hub (expected* set to zero
            // as data gap analysis is not part of this test case)
            using (var validator = TelemetryValidator.Start(_context, numberOfNodes, 0, 90_000_000))
            {
                // Wait some time to generate events to process.
                await Task.Delay(TestConstants.AwaitDataInMilliseconds, _cts.Token);

                // Call GetConfiguredEndpoints direct method
                responseGetConfiguredEndpoints = await CallMethodAsync(
                    new MethodParameterModel
                    {
                        Name = TestConstants.DirectMethodNames.GetConfiguredEndpoints
                    },
                    _cts.Token
                );

                Assert.Equal((int)HttpStatusCode.OK, responseGetConfiguredEndpoints.Status);
                configuredEndpointsResponse = _serializer.Deserialize<GetConfiguredEndpointsResponseModel>(responseGetConfiguredEndpoints.JsonPayload);
                Assert.Single(configuredEndpointsResponse.Endpoints);
                TestHelper.Publisher.AssertEndpointModel(configuredEndpointsResponse.Endpoints[0], request);

                // Create request for GetConfiguredNodesOnEndpoint method call
                var requestGetConfiguredNodesOnEndpoint = new PublishedNodesEntryModel
                {
                    EndpointUrl = request.EndpointUrl,
                    UseSecurity = request.UseSecurity
                };

                // Call GetConfiguredNodesOnEndpoint direct method
                var responseGetConfiguredNodesOnEndpoint = await CallMethodAsync(
                    new MethodParameterModel
                    {
                        Name = TestConstants.DirectMethodNames.GetConfiguredNodesOnEndpoint,
                        JsonPayload = _serializer.SerializeObjectToString(requestGetConfiguredNodesOnEndpoint)
                    },
                    _cts.Token
                );

                Assert.Equal((int)HttpStatusCode.OK, responseGetConfiguredNodesOnEndpoint.Status);
                var jsonResponse = _serializer.Deserialize<GetConfiguredNodesOnEndpointResponseModel>(responseGetConfiguredNodesOnEndpoint.JsonPayload);
                Assert.Equal(numberOfNodes, jsonResponse.OpcNodes.Count);

                // Call GetDiagnosticInfo direct method
                var responseGetDiagnosticInfo = await CallMethodAsync(
                    new MethodParameterModel
                    {
                        Name = TestConstants.DirectMethodNames.GetDiagnosticInfo
                    },
                    _cts.Token
                );

                Assert.Equal((int)HttpStatusCode.OK, responseGetDiagnosticInfo.Status);
                var diagInfoList = _serializer.Deserialize<List<PublishDiagnosticInfoModel>>(responseGetDiagnosticInfo.JsonPayload);
                Assert.Single(diagInfoList);

                TestHelper.Publisher.AssertEndpointDiagnosticInfoModel(request, diagInfoList[0]);

                // Stop monitoring and get the result.
                var publishingMonitoringResultJson = await validator.StopAsync();
                Assert.True(publishingMonitoringResultJson.TotalValueChangesCount > 0, "No messages received at IoT Hub");
                Assert.Equal(publishingMonitoringResultJson.ValueChangesByNodeId.Count, request.OpcNodes.Count);
                Assert.True(publishingMonitoringResultJson.DroppedValueCount == 0,
                    $"Dropped messages detected: {publishingMonitoringResultJson.DroppedValueCount}");
                Assert.True(publishingMonitoringResultJson.DuplicateValueCount == 0,
                    $"Duplicate values detected: {publishingMonitoringResultJson.DuplicateValueCount}");
                Assert.Equal(0U, publishingMonitoringResultJson.DroppedSequenceCount);
                // Uncomment once bug generating duplicate sequence numbers is resolved.
                //Assert.Equal(0U, publishingMonitoringResultJson.DuplicateSequenceCount);
                Assert.Equal(0U, publishingMonitoringResultJson.ResetSequenceCount);

                // Check that every published node is sending data.
                if (_context.ConsumedOpcUaNodes != null)
                {
                    var expectedNodes = _context.ConsumedOpcUaNodes.First().Value.OpcNodes.Select(n => n.Id).ToList();
                    foreach (var property in publishingMonitoringResultJson.ValueChangesByNodeId)
                    {
                        var propertyName = property.Key;
                        var nodeId = propertyName.Split('#').Last();
                        var expected = expectedNodes.Find(n => n.EndsWith(nodeId, StringComparison.Ordinal));
                        Assert.True(expected != null, $"Publishing from unexpected node: {propertyName}");
                        expectedNodes.Remove(expected);
                    }

                    expectedNodes.ForEach(_context.OutputHelper.WriteLine);
                    Assert.Empty(expectedNodes);
                }

                // Unpublish all nodes for the endpoint
                if (useAddOrUpdate)
                {
                    // Call AddOrUpdateEndpoints direct method
                    request.OpcNodes = null;
                    response = await CallMethodAsync(
                        new MethodParameterModel
                        {
                            Name = TestConstants.DirectMethodNames.AddOrUpdateEndpoints,
                            JsonPayload = _serializer.SerializeToString(new List<PublishedNodesEntryModel> { request })
                        },
                        _cts.Token
                    );
                }
                else
                {
                    // Call UnPublishNodes direct method
                    response = await CallMethodAsync(
                        new MethodParameterModel
                        {
                            Name = TestConstants.DirectMethodNames.UnpublishNodes,
                            JsonPayload = _serializer.SerializeToString(request)
                        },
                        _cts.Token
                    );
                }

                Assert.Equal((int)HttpStatusCode.OK, response.Status);

                // Wait till the publishing has stopped.
                await Task.Delay(TestConstants.AwaitCleanupInMilliseconds, _cts.Token);

                // Call GetDiagnosticInfo direct method
                responseGetDiagnosticInfo = await CallMethodAsync(
                    new MethodParameterModel
                    {
                        Name = TestConstants.DirectMethodNames.GetDiagnosticInfo
                    },
                    _cts.Token
                );

                Assert.Equal((int)HttpStatusCode.OK, responseGetDiagnosticInfo.Status);
                diagInfoList = _serializer.Deserialize<List<PublishDiagnosticInfoModel>>(responseGetDiagnosticInfo.JsonPayload);
                Assert.Empty(diagInfoList);
            }

            // Use test event processor to verify data send to IoT Hub (expected* set to zero
            // as data gap analysis is not part of this test case)
            using (var validator = TelemetryValidator.Start(_context, 0, 0, 0))
            {
                // Wait some time to generate events to process.
                await Task.Delay(TestConstants.AwaitCleanupInMilliseconds, _cts.Token);

                // Stop monitoring and get the result.
                var unpublishingMonitoringResultJson = await validator.StopAsync();
                Assert.True(unpublishingMonitoringResultJson.TotalValueChangesCount == 0,
                    $"Messages received at IoT Hub: {unpublishingMonitoringResultJson.TotalValueChangesCount}");
            }
        }
    }
}
