# Portfolio Manager

## Summary

Financial portfolio management system based on microservices architecture.

[](https://media.giphy.com/media/ND6xkVPaj8tHO/giphy.gif)

## Prerequisites

### Installation

- You will need [Docker Compose](https://docs.docker.com/compose/install/) installed
- For local development:
  - [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
  - For IDE we recommend using [Visual Studio Code](https://code.visualstudio.com/Download) or [Visual Studio](https://visualstudio.microsoft.com/downloads/) for Windows and Mac

### In this project we are using:

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/?view=aspnetcore-3.1#pivot=core)
- [gRPC](https://grpc.io/) for inter-service communication
- Monitoring with [Prometheus](https://prometheus.io/) and [Grafana](https://grafana.com/)
- Logging with [Elastic Stack](https://www.elastic.co/)
- Tracing with [Jaeger](https://www.jaegertracing.io/)

## Getting started

### How to run the project

You can run the services by executing `docker-compose` command:

```bash
docker-compose -f docker.compose.yml -f docker.compose.dev.yml up
```

**Note that this will not rebuild the solution if there are any changes. Rebuild with**

```bash
docker-compose build
```

### External dependencies

After running the services, some of them might not function correctly as they are using external tools for logging, monitoring, etc.

You can find all external dependencies inside folder [/compose](/compose) where you can start them by using `docker-compose`.

### Services

The `docker-compose.yml` file will start the following services:
- [users-service](src/microservices/users) which runs on http://localhost:5000/. The service is handling CRUD operations for users in the system.

## Contributors

- [Nikolay Dermendzhiev](https://github.com/Dermendzhiev)
- [Miroslav Stoyanov](https://github.com/MiroslavStoyanov)
- [Bozhidar Gevechanov](https://github.com/Bojo966)
- [Alexander Velchev](https://github.com/alvelchev)
