# %SERVICE_NAME

## Summary

- Put the summary of what the service is responsible for and some details on how it accomplishes it
- Link to the `thrift`, `grpc`, `swagger docs`

## Prerequisites

- Any prior knowledge of systems needed in order to be able to fully understand the project.
- Some of the systems that are prerequisites will probably be in installation as well as part of the script that downloads and links dependencies
- Examples of prior knowledge is needing to know what is Kafka or Splunk or Spanner and how those work on a high level.

## Installation

- Preferably be a one liner (`make` for example). Sometimes this can have exceptions when you need to download a GUI app (like Kitematic for Docker images for example)
- This should download all (docker images, libraries and other dependencies)

## Running and testing

### Running

- Preferably be a one liner (`make install`  for example).

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