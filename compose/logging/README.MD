# Logging

## Summary

- We are using the [Elastic stack](https://www.elastic.co/webinars/introduction-elk-stack) for logging, indexing and visualization.
- Probably the best source of truth here should be the docker-compose file

## Prerequisites

- You should be familiar with what [Elastic stack](https://www.elastic.co/webinars/introduction-elk-stack) is and how it operates.

## Running and testing

### Running

```bash
docker-compose up
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

## For future better scaling and resiliency

### Persistent queues in Logstash
By default, Logstash uses in-memory bounded queues between pipeline stages (inputs → pipeline workers) to buffer events. If Logstash experiences a temporary machine failure, the contents of the in-memory queue will be lost. Temporary machine failures are scenarios where Logstash or its host machine are terminated abnormally but are capable of being restarted.

In order to protect against data loss during abnormal termination, Logstash has a persistent queue feature which will store the message queue on disk. Persistent queues provide durability of data within Logstash.

Read more about persistent queue [here](https://www.elastic.co/guide/en/logstash/current/persistent-queues.html) but be aware of the [limitations](https://www.elastic.co/guide/en/logstash/current/persistent-queues.html#persistent-queues-limitations) as well.

### Using buffer for Logstash
Common architectural pattern suggest using buffer/broker like Redis or Kafka between data shippers and Logstash server which can provide better resilliency. If Logstash server is not responding, messages will be kept by the broker.

![architecture diagram](https://aboullaite.me/content/images/2017/09/shecma.jpg)

### Write Logs to Local Storage
Writing logs to local storage might sound contradictory, but sending logs directly via an HTTP request might cause too much outbound network traffic. Elastic Stack provides lightweight data shippers called [Beats](https://www.elastic.co/beats/), which can be used to send logs from local storage/file to a buffer or Logstash directly. Consider checking [Filebeat](https://www.elastic.co/beats/filebeat) when writing logs to a local files.


### More resources on scalability and resiliency:
- [Logstash Scalability](https://www.elastic.co/guide/en/logstash/current/deploying-and-scaling.html#_scalability)
- Suggested deployment of the Elastic Stack:
![architecture diagram](https://dzone.com/storage/temp/8347294-deployment-architecture.jpg)