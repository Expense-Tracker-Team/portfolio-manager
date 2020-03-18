# Logging

## Summary

- We are using the [Elastic stack](https://www.elastic.co/webinars/introduction-elk-stack) for logging, indexing and visualization.
- Probably the best source of truth here should be the docker-compose file `logging.yaml` 

## Prerequisites

- You should be familiar with what [Elastic stack](https://www.elastic.co/webinars/introduction-elk-stack) is and how it operates.

## Running and testing

### Running

```docker
docker-compose -f logging.yaml up
```

Now you are able to run your app locally and if it write logs to the logstash you should be able to see them in the [localhost Kibana dashboard](http://localhost:5601/).

### Services
Executing `docker-compose` command will start the following services:
- **elasticsearch** running on ports:
  - [9200](http://localhost:9200/) HTTP
  - 9300 TCP
- **kibana** running on port [5601](http://localhost:5601/)
- **logstash** running on ports:
  - [9600](http://localhost:9600/) HTTP
  - 9601 TCP
  - 9602 UDP

### Cleanup
In order to entirely shutdown the containers and remove all persisted data, use the following command:
```docker
docker-compose down -v
```