# Development environment

## Summary

This document the Environment in which developers of the repo work and can help you with support or questions

## Environment

### Docker

All external systems should be as docker images so that it's easy to setup a new machine and replicate that to another one.

1. **[Install Docker](https://www.docker.com/products/docker-desktop)**

## Languages and frameworks

All subprojects should follow the given versions of the language and framework here if not exclusively stated otherwise in the README of the give services

### .NET

We use .NET Core 3.0 

For installation of the SDK on different operating systems refer to <https://dotnet.microsoft.com/download/dotnet-core/3.0> 

#### Working with gRPC proto files

Install the following tool for managing `proto` files with `dotnet`:

```bash

dotnet tool install -g dotnet-grpc
```

## Editors and IDEs

### VS Code

#### Recommended plugins

> [!WARNING]
> this should be added to an env scaffolding script (make or something)

```bash
 code --install-extension davidanson.vscode-markdownlint
 code --install-extension ms-vscode.csharp
 code --install-extension zxh404.vscode-proto3
 code --install-extension jmrog.vscode-nuget-package-manager
```
