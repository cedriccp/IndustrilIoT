// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace IIoTPlatformE2ETests.Standalone
{
    using Azure.IIoT.OpcUa.Publisher.Models;
    using IIoTPlatformE2ETests.Deploy;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using IIoTPlatformE2ETests.TestEventProcessor;
    using TestExtensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The test theory using different (ordered) test cases to go thru all required steps of publishing OPC UA node
    /// </summary>
    [TestCaseOrderer(TestCaseOrderer.FullName, TestConstants.TestAssemblyName)]
    [Collection("IIoT Standalone Test Collection")]
    [Trait(TestConstants.TraitConstants.PublisherModeTraitName, TestConstants.TraitConstants.PublisherModeTraitValue)]
    public class BPublishMultipleNodesStandaloneTestTheory
    {
        private readonly IIoTMultipleNodesTestContext _context;

        public BPublishMultipleNodesStandaloneTestTheory(
            ITestOutputHelper output,
            IIoTMultipleNodesTestContext context
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.SetOutputHelper(output);
        }

        [Theory]
        [InlineData(MessagingMode.Samples)]
        [InlineData(MessagingMode.PubSub)]
        public async Task SubscribeUnsubscribeTest(MessagingMode messagingMode)
        {
            using var cts = new CancellationTokenSource(TestConstants.MaxTestTimeoutMilliseconds);

            // Clear context.
            _context.Reset();

            await _context.RegistryHelper.DeployStandalonePublisherAsync(messagingMode, cts.Token);

            var nodesToPublish = await TestHelper.CreateMultipleNodesModelAsync(_context, cts.Token);
            await TestHelper.PublishNodesAsync(_context, new[] { nodesToPublish });

            // Wait some time till the updated pn.json is reflected.
            await Task.Delay(TestConstants.DefaultTimeoutInMilliseconds);

            // Use test event processor to verify data send to IoT Hub (expected* set to zero
            // as data gap analysis is not part of this test case)
            using (var validator = TelemetryValidator.Start(_context, 250, 1000, 90_000_000))
            {
                // Wait some time to generate events to process.
                await Task.Delay(TestConstants.DefaultTimeoutInMilliseconds, cts.Token);

                // Stop monitoring and get the result.
                var publishingMonitoringResultJson = await validator.StopAsync();
                Assert.True(publishingMonitoringResultJson.TotalValueChangesCount > 0, "No messages received at IoT Hub");
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
            }

            // Stop publishing nodes.
            await TestHelper.PublishNodesAsync(
                _context,
                Array.Empty<PublishedNodesEntryModel>()
            );

            // Wait till the publishing has stopped.
            await Task.Delay(TestConstants.DefaultTimeoutInMilliseconds, cts.Token);

            // Use test event processor to verify data send to IoT Hub (expected* set to zero
            // as data gap analysis is not part of this test case)
            using (var validator = TelemetryValidator.Start(_context, 0, 0, 0))
            {
                // Wait some time to generate events to process.
                await Task.Delay(TestConstants.DefaultTimeoutInMilliseconds, cts.Token);

                // Stop monitoring and get the result.
                var unpublishingMonitoringResultJson = await validator.StopAsync();
                Assert.True(unpublishingMonitoringResultJson.TotalValueChangesCount == 0,
                    $"Messages received at IoT Hub: {unpublishingMonitoringResultJson.TotalValueChangesCount}");
            }
        }
    }
}
