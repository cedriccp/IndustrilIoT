﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.Azure.IIoT.OpcUa.Protocol.Runtime {
    using Microsoft.Azure.IIoT.Utils;
    using Microsoft.Extensions.Configuration;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Security configuration
    /// </summary>
    public class SecurityConfig : ConfigBase, ISecurityConfig {

        /// <summary>
        /// Configuration
        /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string PkiRootPathKey = "PkiRootPath";
        public const string ApplicationCertificateStorePathKey = "ApplicationCertificateStorePath";
        public const string ApplicationCertificateStoreTypeKey = "ApplicationCertificateStoreType";
        public const string ApplicationCertificateSubjectNameKey = "ApplicationCertificateSubjectName";
        public const string TrustedIssuerCertificatesPathKey = "TrustedIssuerCertificatesPath";
        public const string TrustedIssuerCertificatesTypeKey = "TrustedIssuerCertificatesType";
        public const string TrustedPeerCertificatesPathKey = "TrustedPeerCertificatesPath";
        public const string TrustedPeerCertificatesTypeKey = "TrustedPeerCertificatesType";
        public const string RejectedCertificateStorePathKey = "RejectedCertificateStorePath";
        public const string RejectedCertificateStoreTypeKey = "RejectedCertificateStoreType";
        public const string AutoAcceptUntrustedCertificatesKey = "AutoAcceptUntrustedCertificates";
        public const string RejectSha1SignedCertificatesKey = "RejectSha1SignedCertificates";
        public const string MinimumCertificateKeySizeKey = "MinimumCertificateKeySize";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <inheritdoc/>
        public string PkiRootPath =>
            GetStringOrDefault(PkiRootPathKey, () => "pki");

        /// <inheritdoc/>
        public CertificateInfo ApplicationCertificate => new CertificateInfo {
            StorePath = GetStringOrDefault(ApplicationCertificateStorePathKey,
                () => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                "CurrentUser\\UA_MachineDefault" : $"{PkiRootPath}/own"),
            StoreType = GetStringOrDefault(ApplicationCertificateStoreTypeKey,
                () => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                "X509Store" : "Directory"),
            SubjectName = GetStringOrDefault(ApplicationCertificateSubjectNameKey,
                () => "CN=Microsoft.Azure.IIoT, C=DE, S=Bav, O=Microsoft, DC=localhost")
        };

        /// <inheritdoc/>
        public CertificateStore TrustedIssuerCertificates => new CertificateStore {
            StorePath = GetStringOrDefault(TrustedIssuerCertificatesPathKey, () => $"{PkiRootPath}/issuers"),
            StoreType = GetStringOrDefault(TrustedIssuerCertificatesTypeKey, () => "Directory"),
        };

        /// <inheritdoc/>
        public CertificateStore TrustedPeerCertificates => new CertificateStore {
            StorePath = GetStringOrDefault(TrustedPeerCertificatesPathKey, () => $"{PkiRootPath}/trusted"),
            StoreType = GetStringOrDefault(TrustedPeerCertificatesTypeKey, () => "Directory"),
        };

        /// <inheritdoc/>
        public CertificateStore RejectedCertificateStore => new CertificateStore {
            StorePath = GetStringOrDefault(RejectedCertificateStorePathKey, () => $"{PkiRootPath}/rejected"),
            StoreType = GetStringOrDefault(RejectedCertificateStoreTypeKey, () => "Directory"),
        };

        /// <inheritdoc/>
        public bool AutoAcceptUntrustedCertificates =>
            GetBoolOrDefault(AutoAcceptUntrustedCertificatesKey, () => false);
        /// <inheritdoc/>
        public bool RejectSha1SignedCertificates =>
            GetBoolOrDefault(RejectSha1SignedCertificatesKey, () => false);
        /// <inheritdoc/>
        public ushort MinimumCertificateKeySize =>
            (ushort)GetIntOrDefault(MinimumCertificateKeySizeKey, () => 1024);


        /// <summary>
        /// Create configuration
        /// </summary>
        /// <param name="configuration"></param>
        public SecurityConfig(IConfiguration configuration) :
            base(configuration) {
        }
    }
}