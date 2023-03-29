<#
 .SYNOPSIS
    Build and push containers to Azure container registry

 .DESCRIPTION
    The script requires az to be installed and already logged on to a 
    tenant.  This means it should be run in a azcliv2 task in the
    azure pipeline or "az login" must have been performed already.

 .PARAMETER Registry
    The name of the registry
 .PARAMETER Subscription
    The subscription to use - otherwise uses default

 .PARAMETER Os
    Operating system to build for. Defaults to Linux
 .PARAMETER Arch
    Architecture to build. Defaults to x64
 .PARAMETER ImageNamespace
    The namespace to use for the image inside the registry.
 .PARAMETER Tag
    Tag to publish under. Defaults to "latest"

 .PARAMETER NoBuid
    Whether to build before publishing.
 .PARAMETER Debug
    Whether to build Release or Debug - default to Release.  
#>

Param(
    [string] $Registry = $null,
    [string] $Subscription = $null,
    [string] $Os = "linux",
    [string] $Arch = "x64",
    [string] $ImageNamespace = $null,
    [string] $Tag = "latest",
    [switch] $NoBuild,
    [switch] $Debug
)
$ErrorActionPreference = "Stop"

if ([string]::IsNullOrEmpty($Registry)) {
    $Registry = $env.BUILD_REGISTRY
    if ([string]::IsNullOrEmpty($Registry)) {
        # Feature builds by default into dev registry
        $Registry = "industrialiotdev"
    }
}

if ([string]::IsNullOrEmpty($script:Subscription)) {
    $argumentList = @("account", "show")
    $account = & "az" @argumentList 2>$null | ConvertFrom-Json
    if (!$account) {
        throw "Failed to retrieve account information."
    }
    $script:Subscription = $account.name
    Write-Host "Using default subscription $script:Subscription..."
}

# get registry information
$argumentList = @("acr", "show", "--name", $script:Registry, 
    "--subscription", $script:Subscription)
$script:RegistryInfo = (& "az" @argumentList 2>&1 | ForEach-Object { "$_" }) | ConvertFrom-Json
if ($LastExitCode -ne 0) {
    throw "az $($argumentList) failed with $($LastExitCode)."
}
$resourceGroup = $script:RegistryInfo.resourceGroup
Write-Debug "Using resource group $($resourceGroup)"
# get credentials
$argumentList = @("acr", "credential", "show", "--name", $script:Registry, 
    "--subscription", $script:Subscription)
$credentials = (& "az" @argumentList 2>&1 | ForEach-Object { "$_" }) | ConvertFrom-Json
if ($LastExitCode -ne 0) {
    throw "az $($argumentList) failed with $($LastExitCode)."
}
$user = $credentials.username
$password = $credentials.passwords[0].value
Write-Debug "Using User name $($user) and passsword ****"

$argumentList = @(
    "acr", "login",
    "--name", $Registry,
    "--username", $user,
    "--password", $password
)
# log into acr to push
(& az $argumentList) | Out-Host

Write-Host "Build and push images..."
# Build the docker images and push them to acr
& (Join-Path $PSScriptRoot "build.ps1") -Registry "$($Registry).azurecr.io" `
    -Debug:$script:Debug -NoBuild:$script:NoBuild `
    -ImageNamespace $script:ImageNamespace -Tag $script:Tag `
    -Os $script:Os -Arch $script:Arch `
    -Push

# Logout
docker logout "$($Registry).azurecr.io"
Write-Host "Images pushed."
