// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Netcap;

using Azure.Identity;
using Azure.ResourceManager;
using CommandLine;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Text.Json;

/// <summary>
/// Netcap exception
/// </summary>
public class NetcapException : Exception
{
    public NetcapException(string message) : base(message)
    {
    }

    public NetcapException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Netcap main
/// </summary>
internal sealed class Main : IDisposable
{
    [Verb("run", isDefault: true, HelpText = "Run netcap to capture diagnostics.")]
    public sealed class RunOptions
    {
        [Option('c', nameof(EdgeHubConnectionString), Required = false,
            HelpText = "The edge hub connection string that OPC Publisher is using " +
                "to bootstrap the rest api." +
                "\nDefaults to value of environment variable 'EdgeHubConnectionString'.")]
        public string? EdgeHubConnectionString { get; set; } =
            Environment.GetEnvironmentVariable(nameof(EdgeHubConnectionString));

        [Option('s', nameof(StorageConnectionString), Required = false,
            HelpText = "The storage connection string to use to upload capture bundles." +
            "\nDefaults to value of environment variable 'StorageConnectionString'.")]
        public string? StorageConnectionString { get; set; } =
            Environment.GetEnvironmentVariable(nameof(StorageConnectionString));

        [Option('m', nameof(PublisherModuleId), Required = false,
            HelpText = "The module id of the opc publisher." +
                "\nDefaults to value of environment variable 'PublisherModuleId'.")]
        public string PublisherModuleId { get; set; } =
            Environment.GetEnvironmentVariable(nameof(PublisherModuleId)) ?? "publisher";

        [Option('d', nameof(PublisherDeviceId), Required = false,
            HelpText = "The device id of the opc publisher." +
                "\nDefaults to value of environment variable 'PublisherDeviceId'.")]
        public string? PublisherDeviceId { get; set; } =
            Environment.GetEnvironmentVariable(nameof(PublisherDeviceId));

        [Option('a', nameof(PublisherRestApiKey), Required = false,
            HelpText = "The api key of the opc publisher." +
                "\nDefaults to value of environment variable 'PublisherRestApiKey'.")]
        public string? PublisherRestApiKey { get; set; } =
            Environment.GetEnvironmentVariable(nameof(PublisherRestApiKey));

        [Option('p', nameof(PublisherRestCertificate), Required = false,
            HelpText = "The tls certificate of the opc publisher." +
                "\nDefaults to value of environment variable 'PublisherRestCertificate'.")]
        public string? PublisherRestCertificate { get; set; }

        [Option('r', nameof(PublisherRestApiEndpoint), Required = false,
            HelpText = "The Rest api endpoint of the opc publisher." +
            "\nDefaults to value of environment variable 'PublisherRestApiEndpoint'.")]
        public string? PublisherRestApiEndpoint { get; set; } =
            Environment.GetEnvironmentVariable(nameof(PublisherRestApiEndpoint));

        [Option('e', nameof(OpcServerEndpointUrl), Required = false,
            HelpText = "The endpoint of the opc publisher." +
            "\nDefaults to value of environment variable 'OpcServerEndpointUrl'.")]
        public string? OpcServerEndpointUrl { get; set; } =
            Environment.GetEnvironmentVariable(nameof(OpcServerEndpointUrl));

        [Option('t', nameof(CaptureDuration), Required = false,
            HelpText = "The capture duration until data is uploaded and capture is restarted." +
            "\nDefaults to value of environment variable 'CaptureDuration'.")]
        public TimeSpan? CaptureDuration { get; set; } = TimeSpan.TryParse(
            Environment.GetEnvironmentVariable(nameof(CaptureDuration)), out var t) ? t : null;

        [Option('i', nameof(CaptureInterfaces), Required = false,
            HelpText = "The network interfaces to capture from.")]
        public Pcap.InterfaceType CaptureInterfaces { get; set; } = Pcap.InterfaceType.AnyIfAvailable;

        [Option('E', nameof(HostCaptureEndpointUrl), Required = false,
            HelpText = "The remote capture endpoint to use.")]
        public string? HostCaptureEndpointUrl { get; internal set; }

        [Option('C', nameof(HostCaptureCertificate), Required = false,
            HelpText = "The remote capture endpoint certificate.")]
        public string? HostCaptureCertificate { get; internal set; }

        [Option('A', nameof(HostCaptureApiKey), Required = false,
            HelpText = "The remote capture endpoint api key.")]
        public string? HostCaptureApiKey { get; internal set; }

        public RunOptions()
        {
            PublisherRestCertificate =
                Environment.GetEnvironmentVariable(nameof(PublisherRestCertificate));
        }

        /// <summary>
        /// GetAndStop configuration from twin
        /// </summary>
        /// <param name="twin"></param>
        /// <returns></returns>
        public void ConfigureFromTwin(Twin twin)
        {
            // Set any missing info from the netcap twin
            OpcServerEndpointUrl = twin.GetProperty(
                nameof(OpcServerEndpointUrl), OpcServerEndpointUrl);
            PublisherRestApiEndpoint = twin.GetProperty(
                nameof(PublisherRestApiEndpoint), PublisherRestApiEndpoint);
            PublisherRestApiKey = twin.GetProperty(
                nameof(PublisherRestApiKey), PublisherRestApiKey);
            PublisherRestCertificate = twin.GetProperty(
                nameof(PublisherRestCertificate), PublisherRestCertificate);
            HostCaptureEndpointUrl = twin.GetProperty(
                nameof(HostCaptureEndpointUrl), HostCaptureEndpointUrl);
            HostCaptureCertificate = twin.GetProperty(
                nameof(HostCaptureCertificate), HostCaptureCertificate);
            HostCaptureApiKey = twin.GetProperty(
                nameof(HostCaptureApiKey), HostCaptureApiKey);
            var captureDuration = twin.GetProperty(nameof(CaptureDuration));
            if (!string.IsNullOrWhiteSpace(captureDuration) &&
                TimeSpan.TryParse(captureDuration, out var duration))
            {
                CaptureDuration = duration;
            }
        }
    }

    [Verb("sidecar", HelpText = "Run netcap as capture side car.")]
    public sealed class SidecarOptions
    {
        [Option('c', nameof(EdgeHubConnectionString), Required = false,
            HelpText = "The edge hub connection string to connect with." +
            "\nDefaults to value of environment variable 'EdgeHubConnectionString'.")]
        public string? EdgeHubConnectionString { get; set; } =
            Environment.GetEnvironmentVariable(nameof(EdgeHubConnectionString));
    }

    [Verb("install", HelpText = "Install netcap into a publisher.")]
    public sealed class InstallOptions
    {
        [Option('t', nameof(TenantId), Required = false,
            HelpText = "The tenant to use to filter subscriptions down." +
            "\nDefault uses all tenants accessible.")]
        public string? TenantId { get; set; }

        [Option('s', nameof(SubscriptionId), Required = false,
            HelpText = "The subscription to use to install to." +
            "\nDefault uses all subscriptions accessible.")]
        public string? SubscriptionId { get; set; }

        [Option('o', nameof(OutputPath), Required = false,
            HelpText = "The output path to capture to.")]
        public string? OutputPath { get; set; }

        [Option('b', nameof(Branch), Required = false,
           HelpText = "The branch to build netcap from." +
           "\nDefaults to main branch.")]
        public string? Branch { get; set; } = "main";
    }

    [Verb("uninstall", HelpText = "Uninstall netcap from one or all publishers.")]
    public sealed class UninstallOptions
    {
        [Option('t', nameof(TenantId), Required = false,
            HelpText = "The tenant to use to filter subscriptions down." +
            "\nDefault uses all tenants accessible.")]
        public string? TenantId { get; set; }

        [Option('s', nameof(SubscriptionId), Required = false,
            HelpText = "The subscription to use to install to." +
            "\nDefault uses all subscriptions accessible.")]
        public string? SubscriptionId { get; set; }
    }

    /// <summary>
    /// Create netcap application
    /// </summary>
    public Main()
    {
        _httpClient = new HttpClient();
        _loggerFactory = new LoggerFactory();
        _logger = UpdateLogger();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _loggerFactory.Dispose();
        _httpClient.Dispose();
        _certificate?.Dispose();
    }

    /// <summary>
    /// Parse parameters
    /// </summary>
    /// <param name="args"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async ValueTask<Main> RunAsync(string[] args,
        CancellationToken ct = default)
    {
        var cmd = new Main();
        await cmd.ParseAsync(args, ct).ConfigureAwait(false);
        return cmd;
    }

    /// <summary>
    /// Parse parameters
    /// </summary>
    /// <param name="args"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async ValueTask ParseAsync(string[] args, CancellationToken ct = default)
    {
        Parser.Default.ParseArguments<RunOptions, InstallOptions, UninstallOptions>(args)
            .WithParsed<RunOptions>(parsedParams => _run = parsedParams)
            .WithParsed<InstallOptions>(parsedParams => _install = parsedParams)
            .WithParsed<SidecarOptions>(parsedParams => _sidecar = parsedParams)
            .WithParsed<UninstallOptions>(parsedParams => _uninstall = parsedParams)
            .WithNotParsed(errors =>
            {
                errors.ToList().ForEach(Console.WriteLine);
                Environment.Exit(1);
            });

        _logger = UpdateLogger();
        var iothubConnectionString =
            Environment.GetEnvironmentVariable("IoTHubOwnerConnectionString") ??
            Environment.GetEnvironmentVariable("_HUB_CS");

        if (_install != null)
        {
            await InstallAsync(ct).ConfigureAwait(false);
        }
        else if (_uninstall != null)
        {
            await UninstallAsync(ct).ConfigureAwait(false);
        }
        else if (_sidecar != null)
        {
            await RunAsSidecarModuleAsync(ct).ConfigureAwait(false);
        }
        else if (!string.IsNullOrWhiteSpace(_run?.EdgeHubConnectionString) ||
            Environment.GetEnvironmentVariable("IOTEDGE_WORKLOADURI") != null)
        {
            await RunAsModuleAsync(ct).ConfigureAwait(false);
        }
        else if (!string.IsNullOrEmpty(iothubConnectionString))
        {
            // NOTE: This is for local testing against IoT Hub
            await RunAsIoTHubConnectedModuleAsync(
                iothubConnectionString, ct).ConfigureAwait(false);
        }
        else if (string.IsNullOrWhiteSpace(_run?.PublisherRestApiKey) &&
            string.IsNullOrWhiteSpace(_run?.PublisherRestCertificate) &&
            string.IsNullOrWhiteSpace(_run?.PublisherRestApiEndpoint))
        {
            await InstallAsync(ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Run netcap
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task RunAsync(CancellationToken ct = default)
    {
        _run ??= new RunOptions();
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        if (!Extensions.IsRunningInContainer())
        {
            while (Console.KeyAvailable) { Console.ReadKey(); }
            _ = Task.Run(() => { Console.ReadKey(); cts.Cancel(); }, ct);
            Console.WriteLine("Press any key to exit");
            Console.WriteLine();
        }

        try
        {
            // Connect to publisher
            var publisher = new Publisher(_loggerFactory.CreateLogger("Publisher"), _httpClient,
                _run.OpcServerEndpointUrl);

            Storage? uploader = null;
            if (!string.IsNullOrEmpty(_run.StorageConnectionString))
            {
                _logger.LogInformation("Uploading to storage of publisher module {DeviceId}/{ModuleId}...",
                    _run.PublisherDeviceId, _run.PublisherModuleId);
                // TODO: move to seperate task
                uploader = new Storage(_run.PublisherDeviceId ?? "unknown", _run.PublisherModuleId,
                    _run.StorageConnectionString, _loggerFactory.CreateLogger("Upload"));
            }

            for (var i = 0; !cts.IsCancellationRequested; i++)
            {
                // GetAndStop endpoint urls and addresses to monitor if not set
                if (!await publisher.TryUpdateEndpointsAsync(cts.Token).ConfigureAwait(false))
                {
                    _logger.LogInformation("waiting .....");
                    await Task.Delay(TimeSpan.FromMinutes(1), cts.Token).ConfigureAwait(false);
                    continue;
                }

                // Capture traffic for duration
                using var timeoutToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
                if (uploader != null || _run.CaptureDuration != null)
                {
                    var duration = _run.CaptureDuration ?? TimeSpan.FromMinutes(10);
                    _logger.LogInformation("Capturing for {Duration}", duration);
                    timeoutToken.CancelAfter(duration);
                }
                var folder = Path.Combine(Path.GetTempPath(), "capture" + i);

                var bundle = new Bundle(_loggerFactory.CreateLogger("Capture"), folder);
                using (bundle.AddPcap(publisher, i, _run.CaptureInterfaces, _run.HostCaptureEndpointUrl))
                {
                    while (!timeoutToken.IsCancellationRequested)
                    {
                        // Watch session diagnostics while we capture
                        try
                        {
                            _logger.LogInformation("Monitoring diagnostics at {Url}...", _httpClient.BaseAddress);
                            await foreach (var diagnostic in _httpClient.GetFromJsonAsAsyncEnumerable<JsonElement>(
                                "v2/diagnostics/connections/watch",
                                    cancellationToken: timeoutToken.Token).ConfigureAwait(false))
                            {
                                await bundle.AddSessionKeysFromDiagnosticsAsync(
                                    diagnostic, publisher.Endpoints).ConfigureAwait(false);
                            }
                            _logger.LogInformation("Restart monitoring diagnostics...");
                        }
                        catch (OperationCanceledException) { } // Done
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error monitoring diagnostics - restarting...");
                        }
                    }
                }

                // TODO: move to seperate task
                if (uploader != null)
                {
                    await uploader.UploadAsync(bundle, cts.Token).ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to run.");
        }
    }

    /// <summary>
    /// Install
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task InstallAsync(CancellationToken ct = default)
    {
        _install ??= new InstallOptions();
        // Login to azure
        var armClient = new ArmClient(new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                TenantId = _install.TenantId
            }));

        _logger.LogInformation("Installing netcap module...");

        var gateway = new Gateway(armClient, _logger, _install.Branch);
        try
        {
            // GetAndStop publishers
            var found = await gateway.SelectPublisherAsync(_install.SubscriptionId,
                false, ct).ConfigureAwait(false);
            if (!found)
            {
                return;
            }

            // Create storage account or update if it already exists in the rg
            await gateway.Storage.CreateOrUpdateAsync(ct).ConfigureAwait(false);
            // Create container registry or update and build netcap module
            await gateway.Netcap.CreateOrUpdateAsync(ct).ConfigureAwait(false);

            // Deploy the module using manifest to device with the chosen publisher
            await gateway.DeployNetcapModuleAsync(ct).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(_install.OutputPath))
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                if (!Extensions.IsRunningInContainer())
                {
                    while (Console.KeyAvailable) { Console.ReadKey(); }
                    _ = Task.Run(() => { Console.ReadKey(); cts.Cancel(); }, ct);
                    Console.WriteLine("Press any key to exit");
                    Console.WriteLine();
                }
                try
                {
                    // GetAndStop the logs from the module, when cancelled undeploy
                    var downloader = gateway.GetStorage();
                    await downloader.DownloadAsync(_install.OutputPath,
                        cts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    await gateway.RemoveNetcapModuleAsync(ct).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to install netcap module with error: {Error}",
                ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Uninstall
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task UninstallAsync(CancellationToken ct = default)
    {
        _uninstall ??= new UninstallOptions();
        // Login to azure
        var armClient = new ArmClient(new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                TenantId = _uninstall.TenantId
            }));

        _logger.LogInformation("Uninstalling netcap module...");

        var gateway = new Gateway(armClient, _logger);
        try
        {
            // Select netcap modules
            var found = await gateway.SelectPublisherAsync(_uninstall.SubscriptionId,
                true, ct).ConfigureAwait(false);
            if (!found)
            {
                return;
            }

            // Add guard here

            // Cleanup storage account or update if it already exists in the rg
            // await gateway.Storage.DeleteAsync(ct).ConfigureAwait(false);
            // Cleanup container registry
            // await gateway.NetcapException.DeleteAsync(ct).ConfigureAwait(false);

            // Deploy the module using manifest to device with the chosen publisher
            await gateway.RemoveNetcapModuleAsync(ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to uninstall netcap module with error: {Error}",
                ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Connect module to edge hub
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async ValueTask RunAsModuleAsync(CancellationToken ct = default)
    {
        _run ??= new RunOptions();
        var moduleClient = await CreateModuleClientAsync(
            _run.EdgeHubConnectionString).ConfigureAwait(false);
        try
        {
            // Call the "GetApiKey" and "GetServerCertificate" methods on the publisher module
            await moduleClient.OpenAsync(ct).ConfigureAwait(false);
            await moduleClient.UpdateReportedPropertiesAsync(new TwinCollection
            {
                ["__type__"] = "OpcNetcap",
                ["__version__"] = GetType().Assembly.GetVersion()
            }, ct).ConfigureAwait(false);

            var twin = await moduleClient.GetTwinAsync(ct).ConfigureAwait(false);

            var deviceId = twin.DeviceId ?? Environment.GetEnvironmentVariable("IOTEDGE_DEVICEID");
            var moduleId = twin.ModuleId ?? Environment.GetEnvironmentVariable("IOTEDGE_MODULEID");
            _run.PublisherModuleId = twin.GetProperty(nameof(_run.PublisherModuleId),
                _run.PublisherModuleId);
            _run.PublisherDeviceId = deviceId; // Override as we must be in the same device
            Debug.Assert(_run.PublisherModuleId != null);
            Debug.Assert(_run.PublisherDeviceId != null);

            _run.ConfigureFromTwin(twin);

            _logger.LogInformation(
                "Connecting to OPC Publisher Module {PublisherModuleId} on {PublisherDeviceId}...",
                _run.PublisherModuleId, _run.PublisherDeviceId);

            if (_run.PublisherRestApiKey == null || _run.PublisherRestCertificate == null)
            {
                if (_run.PublisherRestApiKey == null)
                {
                    var apiKeyResponse = await moduleClient.InvokeMethodAsync(
                        _run.PublisherDeviceId, _run.PublisherModuleId,
                        new MethodRequest("GetApiKey"), ct).ConfigureAwait(false);
                    _run.PublisherRestApiKey =
                        JsonSerializer.Deserialize<string>(apiKeyResponse.Result);
                }
                if (_run.PublisherRestCertificate == null)
                {
                    var certResponse = await moduleClient.InvokeMethodAsync(
                        _run.PublisherDeviceId, _run.PublisherModuleId,
                        new MethodRequest("GetServerCertificate"), ct).ConfigureAwait(false);
                    _run.PublisherRestCertificate =
                        JsonSerializer.Deserialize<string>(certResponse.Result);
                }
            }
            await CreatePublisherHttpClientAsync().ConfigureAwait(false);
            await RunAsync(ct).ConfigureAwait(false);
        }
        finally
        {
            await moduleClient.CloseAsync(ct).ConfigureAwait(false);
            await moduleClient.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Run the side car providing the host side capture capabilities
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task RunAsSidecarModuleAsync(CancellationToken ct = default)
    {
        _sidecar ??= new SidecarOptions();
        var moduleClient = await CreateModuleClientAsync(
            _sidecar.EdgeHubConnectionString).ConfigureAwait(false);
        try
        {
            await moduleClient.OpenAsync(ct).ConfigureAwait(false);
            await moduleClient.UpdateReportedPropertiesAsync(new TwinCollection
            {
                ["__type__"] = "OpcNetcapSidecar",
                ["__version__"] = GetType().Assembly.GetVersion()
            }, ct).ConfigureAwait(false);

            var twin = await moduleClient.GetTwinAsync(ct).ConfigureAwait(false);
            var cert = twin.GetProperty("__certificate__", desired: false);
            var apiKey = twin.GetProperty("__apikey__", desired: false);
            if (cert != null && apiKey != null)
            {
                _certificate = new X509Certificate2(
                    Convert.FromBase64String(cert.Trim()), apiKey);
            }
            else
            {
                _certificate = CreateCertificate(twin);
                apiKey = Guid.NewGuid().ToString();
                cert = Convert.ToBase64String(_certificate.Export(
                    X509ContentType.Pfx, apiKey));
                await moduleClient.UpdateReportedPropertiesAsync(new TwinCollection
                {
                    ["__certificate__"] = cert,
                    ["__apikey__"] = apiKey
                }, ct).ConfigureAwait(false);
            }

            var builder = WebApplication.CreateBuilder();
            builder.WebHost.ConfigureKestrel((_, serverOptions) =>
                serverOptions.ListenAnyIP(443,
                    options => options.UseHttps(_certificate)));

            builder.Services.AddHttpContextAccessor();
            builder.Services.TryAddSingleton(_sidecar);
            builder.Services.AddLogging(builder => builder
                .AddSimpleConsole(options => options.SingleLine = true));
            builder.Services.AddAuthentication(nameof(ApiKeyProvider.ApiKey))
                .AddScheme<AuthenticationSchemeOptions, ApiKeyHandler>(
                    nameof(ApiKeyProvider.ApiKey), null);
            builder.Services.AddAuthentication();

            var app = builder.Build();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            var server = new Pcap.PcapServer(_logger);
            app.MapPut("/", server.CreateAndStart)
                .RequireAuthorization(nameof(ApiKeyProvider.ApiKey))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
            app.MapGet("/{handle}", server.GetAndStop)
                .RequireAuthorization(nameof(ApiKeyProvider.ApiKey))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
            app.MapPost("/{handle}", server.Cleanup)
                .RequireAuthorization(nameof(ApiKeyProvider.ApiKey))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            await app.RunAsync(ct).ConfigureAwait(false);
        }
        finally
        {
            if (moduleClient != null)
            {
                await moduleClient.CloseAsync(ct).ConfigureAwait(false);
                await moduleClient.DisposeAsync().ConfigureAwait(false);
            }
        }

        static X509Certificate2 CreateCertificate(Twin twin)
        {
            var dnsName = Dns.GetHostName();
            using var ecdsa = ECDsa.Create();
            var req = new CertificateRequest("DC=" + dnsName, ecdsa, HashAlgorithmName.SHA256);
            var san = new SubjectAlternativeNameBuilder();
            san.AddDnsName(dnsName);
            var altDns = twin?.ModuleId ?? twin?.DeviceId;
            if (!string.IsNullOrEmpty(altDns) &&
                !string.Equals(altDns, dnsName, StringComparison.OrdinalIgnoreCase))
            {
                san.AddDnsName(altDns);
            }
            req.CertificateExtensions.Add(san.Build());
            var certificate = req.CreateSelfSigned(DateTimeOffset.Now,
                DateTimeOffset.Now + TimeSpan.FromDays(90));
            Debug.Assert(certificate.HasPrivateKey);
            return certificate;
        }
    }

    /// <summary>
    /// Connect with iot hub connections tring
    /// </summary>
    /// <param name="iothubConnectionString"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async ValueTask RunAsIoTHubConnectedModuleAsync(
        string iothubConnectionString, CancellationToken ct = default)
    {
        string deviceId;
        var ncModuleId = "netcap";
        _run ??= new RunOptions();
        if (!string.IsNullOrWhiteSpace(_run.EdgeHubConnectionString))
        {
            // GetAndStop device and module id from edge hub connection string provided
            var ehc = IotHubConnectionStringBuilder.Create(_run.EdgeHubConnectionString);
            deviceId = ehc.DeviceId;
            ncModuleId = ehc.ModuleId ?? ncModuleId;
        }
        else
        {
            // Default device to host name just like we do it in our publisher CLI
#pragma warning disable CA1308 // Normalize strings to uppercase
            deviceId = Dns.GetHostName().ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
        }
        using var rm = Microsoft.Azure.Devices.RegistryManager
            .CreateFromConnectionString(iothubConnectionString);
        // Create module if not exist
        try
        {
            await rm.AddDeviceAsync(new Microsoft.Azure.Devices.Device(deviceId), ct)
                .ConfigureAwait(false);
        }
        catch (DeviceAlreadyExistsException) { }
        try
        {
            await rm.AddModuleAsync(new Microsoft.Azure.Devices.Module(
                deviceId, ncModuleId), ct).ConfigureAwait(false);
        }
        catch (ModuleAlreadyExistsException) { }

        var twin = await rm.GetTwinAsync(deviceId, ncModuleId, ct).ConfigureAwait(false);
        twin = await rm.UpdateTwinAsync(deviceId, ncModuleId, new Twin
        {
            Properties = new TwinProperties
            {
                Reported = new TwinCollection
                {
                    ["__type__"] = "OpcNetcap",
                    ["__version__"] = GetType().Assembly.GetVersion()
                }
            }
        }, twin.ETag, ct).ConfigureAwait(false);

        // GetAndStop publisher id from twin if not configured
        _run.PublisherModuleId = twin.GetProperty(nameof(_run.PublisherModuleId), _run.PublisherModuleId);
        _run.PublisherDeviceId ??= deviceId;
        Debug.Assert(_run.PublisherModuleId != null);
        Debug.Assert(_run.PublisherDeviceId != null);

        _run.ConfigureFromTwin(twin);

        _logger.LogInformation("Connecting to OPC Publisher Module {PublisherModuleId} " +
            "on {PublisherDeviceId} via IoTHub...", _run.PublisherModuleId, _run.PublisherDeviceId);
        if (_run.PublisherRestApiKey == null || _run.PublisherRestCertificate == null)
        {
            using var serviceClient = Microsoft.Azure.Devices.ServiceClient
                .CreateFromConnectionString(iothubConnectionString);
            if (_run.PublisherRestApiKey == null)
            {
                var apiKeyResponse = await serviceClient.InvokeDeviceMethodAsync(
                    _run.PublisherDeviceId, _run.PublisherModuleId,
                    new Microsoft.Azure.Devices.CloudToDeviceMethod(
                        "GetApiKey"), ct).ConfigureAwait(false);
                _run.PublisherRestApiKey =
                    JsonSerializer.Deserialize<string>(apiKeyResponse.GetPayloadAsJson());
            }
            if (_run.PublisherRestCertificate == null)
            {
                var certResponse = await serviceClient.InvokeDeviceMethodAsync(
                    _run.PublisherDeviceId, _run.PublisherModuleId,
                    new Microsoft.Azure.Devices.CloudToDeviceMethod(
                        "GetServerCertificate"), ct).ConfigureAwait(false);
                _run.PublisherRestCertificate =
                    JsonSerializer.Deserialize<string>(certResponse.GetPayloadAsJson());
            }
        }
        await CreatePublisherHttpClientAsync().ConfigureAwait(false);
        await RunAsync(ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Create publisher client
    /// </summary>
    /// <returns></returns>
    private async ValueTask CreatePublisherHttpClientAsync()
    {
        if (_run?.PublisherRestApiKey != null)
        {
            // Load the certificate of the publisher if not exist
            if (!string.IsNullOrWhiteSpace(_run?.PublisherRestCertificate)
                && _certificate == null)
            {
                try
                {
                    _certificate = X509Certificate2.CreateFromPem(
                        _run.PublisherRestCertificate.Trim());
                }
                catch
                {
                    var cert = Convert.FromBase64String(
                        _run.PublisherRestCertificate.Trim());
                    _certificate = new X509Certificate2(
                        cert!, _run.PublisherRestApiKey);
                }
            }

            _httpClient.Dispose();
#pragma warning disable CA2000 // Dispose objects before losing scope
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, cert, _, _) =>
                {
                    if (_certificate?.Thumbprint != cert?.Thumbprint)
                    {
                        _logger.LogWarning(
                            "Certificate thumbprint mismatch: {Expected} != {Actual}",
                            _certificate?.Thumbprint, cert?.Thumbprint);
                        return false;
                    }
                    return true;
                }
            });
#pragma warning restore CA2000 // Dispose objects before losing scope
            _httpClient.BaseAddress =
                await GetOpcPublisherRestEndpoint().ConfigureAwait(false);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("ApiKey", _run?.PublisherRestApiKey);
        }

        /// <summary>
        /// Publisher Endpoint
        /// </summary>
        async ValueTask<Uri> GetOpcPublisherRestEndpoint()
        {
            if (_run?.PublisherRestApiEndpoint != null &&
                Uri.TryCreate(_run.PublisherRestApiEndpoint,
                UriKind.Absolute, out var u))
            {
                return u;
            }
            var host = _run?.PublisherModuleId;
            if (host != null)
            {
                // Poor man ping
                try
                {
                    var result = await Dns.GetHostEntryAsync(host).ConfigureAwait(false);
                    if (result.AddressList.Length == 0)
                        host = null;
                }
                catch { host = null; }
            }
            host ??= "localhost";
            var isLocal = host == null;
            var uri = new UriBuilder
            {
                Scheme = "https",
                Port = !isLocal ? 8081 : 443,
                Host = host
            };
            if (_run?.PublisherRestApiKey == null)
            {
                uri.Scheme = "http";
                uri.Port = !isLocal ? 8080 : 80;
            }
            return uri.Uri;
        }
    }

    /// <summary>
    /// Update logger
    /// </summary>
    private ILogger UpdateLogger()
    {
        _loggerFactory.Dispose();
        _loggerFactory = LoggerFactory.Create(builder => builder
            .AddSimpleConsole(options => options.SingleLine = true));
        return _loggerFactory.CreateLogger("Netcap");
    }

    /// <summary>
    /// Create module client
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    private async static ValueTask<ModuleClient> CreateModuleClientAsync(string? connectionString)
    {
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            return ModuleClient.CreateFromConnectionString(connectionString);
#pragma warning restore CA2000 // Dispose objects before losing scope
        }
        return await ModuleClient.CreateFromEnvironmentAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Injected apikey
    /// </summary>
    /// <param name="ApiKey"></param>
    public record class ApiKeyProvider(string ApiKey);

    /// <summary>
    /// Api key authentication handler
    /// </summary>
    internal sealed class ApiKeyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "ApiKey";

        /// <summary>
        /// Create authentication handler
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="context"></param>
        /// <param name="apiKeyProvider"></param>
        public ApiKeyHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, IHttpContextAccessor context,
            ApiKeyProvider apiKeyProvider) :
            base(options, logger, encoder)
        {
            _context = context;
            _apiKeyProvider = apiKeyProvider;
        }

        /// <inheritdoc/>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var httpContext = _context.HttpContext;
            if (httpContext == null)
            {
                return Task.FromResult(AuthenticateResult.Fail(
                    "No request."));
            }

            var authorization = httpContext.Request.Headers.Authorization;
            if (authorization.Count == 0 || string.IsNullOrEmpty(authorization[0]))
            {
                return Task.FromResult(AuthenticateResult.Fail(
                    "Missing Authorization header."));
            }
            try
            {
                var header = AuthenticationHeaderValue.Parse(authorization[0]!);
                if (header.Scheme != nameof(ApiKeyProvider.ApiKey))
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                if (_apiKeyProvider.ApiKey != header.Parameter?.Trim())
                {
                    throw new UnauthorizedAccessException();
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, nameof(ApiKeyProvider.ApiKey))
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex));
            }
        }

        private readonly IHttpContextAccessor _context;
        private readonly ApiKeyProvider _apiKeyProvider;
    }

    private HttpClient _httpClient;
    private ILoggerFactory _loggerFactory = null!;
    private ILogger _logger;
    private X509Certificate2? _certificate;
    private InstallOptions? _install;
    private UninstallOptions? _uninstall;
    private RunOptions? _run;
    private SidecarOptions? _sidecar;
    internal static readonly JsonSerializerOptions Indented
        = new() { WriteIndented = true };
}
