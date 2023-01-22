// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.Module.Framework.Client.Tests {
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.IIoT.Module.Framework.Client.MqttClient;
    using Moq;
    using MQTTnet;
    using MQTTnet.Client;
    using MQTTnet.Client.Connecting;
    using MQTTnet.Client.Disconnecting;
    using MQTTnet.Client.Options;
    using MQTTnet.Client.Receiving;
    using MQTTnet.Client.Subscribing;
    using MQTTnet.Extensions.ManagedClient;
    using MQTTnet.Formatter;
    using MQTTnet.Protocol;
    using Serilog;
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Authentication;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("MqttClientTests")]
    public class MqttClientAdapterTests : MqttClientConnectionStringBuilderTestsBase {
        private readonly ILogger _logger;
        private readonly MqttClientConnectionStringBuilder _mqttClientConnectionStringBuilder;

        public MqttClientAdapterTests() {
            _logger = new Mock<ILogger>().Object;
            // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification = "Test Example, no real secret")]
            _mqttClientConnectionStringBuilder = MqttClientConnectionStringBuilder.Create("HostName=hub1.azure-devices.net;DeviceId=device1;ModuleId=module1;SharedAccessSignature=SharedAccessSignature sr=hub1.azure-devices.net%2Fdevices%2Fdevice1&sig=SAHEh7J7dPzpIhotIEpRXUhC4v49vKJOHLiKlcGv1U8%3D&se=1943452860;StateFile=file1;Protocol=v500");
        }

        [Fact]
        public async Task ConnectTest() {
            var mock = new Mock<IManagedMqttClient>();
            mock.SetupGet(x => x.IsStarted).Returns(false);
            mock.SetupGet(x => x.InternalClient).Returns(new Mock<IMqttClient>().Object);
            _ = await MqttClientAdapter.CreateAsync(mock.Object, "product1", _mqttClientConnectionStringBuilder, "device1", "/topic/{device_id}", TimeSpan.Zero, null, () => { }, _logger);

            mock.VerifyGet(x => x.IsStarted);
            mock.VerifySet(x => x.ConnectedHandler = It.IsAny<IMqttClientConnectedHandler>());
            mock.VerifySet(x => x.DisconnectedHandler = It.IsAny<IMqttClientDisconnectedHandler>());
            mock.VerifySet(x => x.ApplicationMessageReceivedHandler = It.IsAny<IMqttApplicationMessageReceivedHandler>());
            mock.Verify(x => x.StartAsync(
                It.Is<IManagedMqttClientOptions>(x =>
                    x.ClientOptions.ChannelOptions is MqttClientTcpOptions &&
                    x.ClientOptions.ProtocolVersion == MqttProtocolVersion.V500 &&
                    string.Equals(x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().Server, "hub1.azure-devices.net") &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().Port == 8883 &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().BufferSize == GetExpectedBufferSize() &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions != null &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions.UseTls &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions.Certificates.Count == 1 &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions.SslProtocol == SslProtocols.Tls12 &&
                    !string.IsNullOrWhiteSpace(x.ClientOptions.Credentials.Username) &&
                    x.ClientOptions.Credentials.Password.Length > 0 &&
                    x.Storage is ManagedMqttClientStorage)));
            mock.Verify(x => x.SubscribeAsync(
                It.Is<MqttTopicFilter[]>(x =>
                    x.Length == 3 &&
                    string.Equals(x[0].Topic, "$iothub/twin/res/#") &&
                    string.Equals(x[1].Topic, "$iothub/twin/PATCH/properties/desired/#") &&
                    string.Equals(x[2].Topic, "$iothub/methods/POST/#"))));
            mock.Verify(x => x.InternalClient.SubscribeAsync(It.IsAny<MqttClientSubscribeOptions>(), It.IsAny<CancellationToken>()));
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SendIoTHubEventTest() {
            const string payload = @"{ ""key"": ""value"" }";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var mock = new Mock<IManagedMqttClient>();

            mock.SetupGet(x => x.IsStarted).Returns(true);
            var mqttClientAdapter = await MqttClientAdapter.CreateAsync(mock.Object, "product1",
                _mqttClientConnectionStringBuilder, "device1", "/topic/{device_id}", TimeSpan.FromMinutes(5), null, () => { }, _logger);

            var message = mqttClientAdapter.CreateMessage();
            message.Payload = new[] { payloadBytes };
            message.ContentType = "application/json";
            message.ContentEncoding = "utf-8";
            message.OutputName = "testoutput";
            message.Ttl = TimeSpan.FromSeconds(1234);
            message.Retain = true;

            await mqttClientAdapter.SendEventAsync(message);

            mock.Verify(x => x.PublishAsync(
                It.Is<MqttApplicationMessage>(x =>
                    string.Equals(x.ContentType, "application/json") &&
                    string.Equals(x.Topic, "devices/device1/messages/events/iothub-content-type=application%2Fjson&iothub-message-schema=application%2Fjson&iothub-content-encoding=utf-8&%24%24ContentEncoding=utf-8/") &&
                    x.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtLeastOnce &&
                    string.Equals(Encoding.UTF8.GetString(x.Payload), payload) &&
                    x.Retain == true &&
                    x.MessageExpiryInterval == 1234),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendBrokerEventTest1() {
            const string payload = @"{ ""key"": ""value"" }";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var mock = new Mock<IManagedMqttClient>();

            mock.SetupGet(x => x.IsStarted).Returns(true);
            // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification = "Test Example, no real secret")]
            var mqttClientConnectionStringBuilder = MqttClientConnectionStringBuilder.Create("HostName=localhost;DeviceId=deviceId;Username=user;Password=SAHEh7J7dPzpIh;Protocol=v500");
            var mqttClientAdapter = await MqttClientAdapter.CreateAsync(mock.Object, "product1",
                mqttClientConnectionStringBuilder, "device1", "/topic/{device_id}", TimeSpan.FromMinutes(5), null, () => { }, _logger);

            var message = mqttClientAdapter.CreateMessage();
            message.Payload = new[] { payloadBytes, payloadBytes, payloadBytes };
            message.ContentType = "application/json";
            message.OutputName = "testoutput";
            message.ContentEncoding = "utf-8";
            message.Ttl = TimeSpan.FromSeconds(1234);
            message.Retain = true;

            await mqttClientAdapter.SendEventAsync(message);

            mock.Verify(x => x.PublishAsync(
                It.Is<MqttApplicationMessage>(x =>
                    string.Equals(x.ContentType, "application/json") &&
                    string.Equals(x.Topic, "/topic/device1/testoutput") &&
                    x.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtLeastOnce &&
                    string.Equals(Encoding.UTF8.GetString(x.Payload), payload) &&
                    x.Retain == true &&
                    x.MessageExpiryInterval == 1234),
                It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task SendBrokerEventTest2() {
            const string payload = @"{ ""key"": ""value"" }";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var mock = new Mock<IManagedMqttClient>();

            mock.SetupGet(x => x.IsStarted).Returns(true);
            // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification = "Test Example, no real secret")]
            var mqttClientConnectionStringBuilder = MqttClientConnectionStringBuilder.Create("HostName=localhost;DeviceId=deviceId;Username=user;Password=SAHEh7J7dPzpIh;Protocol=v500");
            var mqttClientAdapter = await MqttClientAdapter.CreateAsync(mock.Object, "product1",
                mqttClientConnectionStringBuilder, "device1", "/topic/{output_name}/super", TimeSpan.FromMinutes(5), null, () => { }, _logger);

            var message = mqttClientAdapter.CreateMessage();
            message.Payload = new[] { payloadBytes, null, payloadBytes };
            message.ContentType = "application/json";
            message.ContentEncoding = "utf-8";

            await mqttClientAdapter.SendEventAsync(message);

            mock.Verify(x => x.PublishAsync(
                It.Is<MqttApplicationMessage>(x =>
                    string.Equals(x.ContentType, "application/json") &&
                    string.Equals(x.Topic, "/topic/super") &&
                    x.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtLeastOnce &&
                    string.Equals(Encoding.UTF8.GetString(x.Payload), payload) &&
                    x.Retain == false &&
                    x.MessageExpiryInterval == null),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetTwinTest() {
            var mock = new Mock<IManagedMqttClient>();
            mock.SetupGet(x => x.IsStarted).Returns(true);
            var mqttClientAdapter = await MqttClientAdapter.CreateAsync(mock.Object, "product1", _mqttClientConnectionStringBuilder, "device1", "/topic/{device_id}", TimeSpan.FromSeconds(0), null, () => { }, _logger);
            await mqttClientAdapter.GetTwinAsync();

            mock.Verify(x => x.PublishAsync(
                It.Is<MqttApplicationMessage>(x =>
                    string.Equals(x.ContentType, "application/json") &&
                    x.Topic.StartsWith("$iothub/twin/GET/?$rid=")),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task IsClosedTest() {
            const string payload = @"{ ""key"": ""value"" }";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            var mock = new Mock<IManagedMqttClient>();
            mock.SetupGet(x => x.IsStarted).Returns(false);
            mock.SetupGet(x => x.InternalClient).Returns(new Mock<IMqttClient>().Object);

            var mqttClientAdapter = await MqttClientAdapter.CreateAsync(mock.Object, "product1", _mqttClientConnectionStringBuilder, "device1", "/topic/{device_id}", TimeSpan.Zero, null, () => { }, _logger);
            var message = mqttClientAdapter.CreateMessage();
            message.Payload = new[] { payloadBytes };
            message.ContentType = "application/json";
            message.ContentEncoding = "utf-8";

            await mqttClientAdapter.CloseAsync();
            await mqttClientAdapter.SendEventAsync(message);

            mock.VerifyGet(x => x.IsStarted);
            mock.VerifySet(x => x.ConnectedHandler = It.IsAny<IMqttClientConnectedHandler>());
            mock.VerifySet(x => x.DisconnectedHandler = It.IsAny<IMqttClientDisconnectedHandler>());
            mock.VerifySet(x => x.ApplicationMessageReceivedHandler = It.IsAny<IMqttApplicationMessageReceivedHandler>());
            mock.Verify(x => x.StartAsync(It.IsAny<IManagedMqttClientOptions>()));
            mock.Verify(x => x.SubscribeAsync(It.IsAny<MqttTopicFilter[]>()));
            mock.Verify(x => x.InternalClient.SubscribeAsync(It.IsAny<MqttClientSubscribeOptions>(), It.IsAny<CancellationToken>()));
            mock.Verify(x => x.StopAsync());
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ConnectCanceledTest() {
            var mock = new Mock<IManagedMqttClient>();
            mock.SetupGet(x => x.IsStarted).Returns(false);
            mock.SetupGet(x => x.InternalClient).Returns(new Mock<IMqttClient>().Object);
            mock.Setup(x => x.StartAsync(It.IsAny<IManagedMqttClientOptions>())).Returns(() => { throw new TaskCanceledException(); });
            _ = await MqttClientAdapter.CreateAsync(mock.Object, "product1", _mqttClientConnectionStringBuilder, "device1", "/topic/{device_id}", TimeSpan.Zero, null, () => { }, _logger);

            mock.VerifyGet(x => x.IsStarted);
            mock.VerifySet(x => x.ConnectedHandler = It.IsAny<IMqttClientConnectedHandler>());
            mock.VerifySet(x => x.DisconnectedHandler = It.IsAny<IMqttClientDisconnectedHandler>());
            mock.VerifySet(x => x.ApplicationMessageReceivedHandler = It.IsAny<IMqttApplicationMessageReceivedHandler>());
            mock.Verify(x => x.StartAsync(
                It.Is<IManagedMqttClientOptions>(x =>
                    x.ClientOptions.ChannelOptions is MqttClientTcpOptions &&
                    x.ClientOptions.ProtocolVersion == MqttProtocolVersion.V500 &&
                    string.Equals(x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().Server, "hub1.azure-devices.net") &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().Port == 8883 &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().BufferSize == GetExpectedBufferSize() &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions != null &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions.UseTls &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions.Certificates.Count == 1 &&
                    x.ClientOptions.ChannelOptions.As<MqttClientTcpOptions>().TlsOptions.SslProtocol == SslProtocols.Tls12 &&
                    !string.IsNullOrWhiteSpace(x.ClientOptions.Credentials.Username) &&
                    x.ClientOptions.Credentials.Password.Length > 0 &&
                    x.Storage is ManagedMqttClientStorage)));
            mock.VerifyNoOtherCalls();
        }

        private static uint GetExpectedBufferSize() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return 64 * 1024;
            }
            return 8 * 1024;
        }
    }
}
