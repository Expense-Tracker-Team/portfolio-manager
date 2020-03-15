# Logging

## Summary

- We are using Elastic Search and Kibana for logging indexing and visualization.
- Probably the best source of truth here should be the `docker-compose.yaml` file 

## Prerequisites

- You should be familiar with what Elastic Search and Kibana are and how they operate.

## Installation

None

## Running and testing

### Running

```bash
docker-compose -f logging.yaml up
```

This should service **Elastic Search on port 9200** and **Kibana on 5601**.
Now you are able to run you app locally and if it flushes metrics to the Elastic Search you should be able to see them in the [localhost Kibana](http://localhost:5601/).

### Testing

#### Local testing

- Go to <http://localhost:5601/>

#### Integration testing

TODO: TBD

## Deployment

TODO: TBD

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