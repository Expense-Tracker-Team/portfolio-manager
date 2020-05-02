# User Service

## Summary

- Stores and manages User information
- Link to the `thrift`, `grpc`, `swagger docs`

## Prerequisites

- Any prior knowledge of systems needed in order to be able to fully understand the project.
- Some of the systems that are prerequisites will probably be in installation as well as part of the script that downloads and links dependencies
- Examples of prior knowledge is needing to know what is Kafka or Splunk or Spanner and how those work on a high level.

## Installation and build
    
```bash 

dotnet build
```

## Running and testing

### Running

To start the application run:

```bash

dotnet run --project src/Presentation/Api/

```

### Testing

#### Local testing

- You can also link possible interactions with service that a developer might want to do with the service in dev environment. (curl requests, yab requests, grpc_cli calls, kafka ingestion mocking, etc)

#### Unit testing

- Preferably be a one liner (`make test`  for example).

#### Integration testing

## Deployment

- Preferably be a one liner (`gcloud deploy $service_name`  for example).

## Monitoring and logging

### Logging

- Link to Kibana (or something)
- Libs that will emit the logs
- Links to the Elastic Cluster (In case you want to query realtime or using a script)

### Monitoring

- Link to Graphana (or something)
- Libs that will emit the metrics

## Additional links
 %RUNBOOK_LINK