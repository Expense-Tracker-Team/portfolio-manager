# Monitoring

## Summary

In order to support monitoring and alerting on our services, we are using [Prometheus](https://prometheus.io/), [Alert Manager](https://prometheus.io/docs/alerting/alertmanager/) and [Grafana](https://grafana.com/).

## Prerequisites

You should be familiar with what [Prometheus](https://prometheus.io/), [Alert Manager](https://prometheus.io/docs/alerting/alertmanager/) and [Grafana](https://grafana.com/) are and how they operate together.

## Running

You can start all tools by running the following command:

```bash
docker-compose up
```

### Services

Executing `docker-compose` command will start the following services:

- [prometheus](https://hub.docker.com/r/prom/prometheus) running on port [9090](http://localhost:9090/)
- [node-exporter](https://hub.docker.com/r/prom/node-exporter) running on port [9100](http://localhost:9100/)
- [alertmanager](https://hub.docker.com/r/prom/alertmanager) running on port [9093](http://localhost:9093/)
- [cadvisor](https://hub.docker.com/r/google/cadvisor) running on port [8080](http://localhost:8080/)
- [grafana](https://hub.docker.com/r/grafana/grafana) running on port [3000](http://localhost:3000/)
- [prometheus-msteams](https://quay.io/repository/prometheusmsteams/prometheus-msteams) running on port [2000](http://localhost:2000/)
  - default credentials for login are username `admin` and password `pass123`

### Alerting 

MS Teams does not have a direct integration with Prometheus Alertmanager, like Slack does. Therefore we need to use a proxy that takes a webhook from Alertmanager and translates that to the MS Teams format. 

#### Testing MS Teams prometheus integration proxy 

To check if the `prometheus-msteams` is working correctly and the integration is properly set up you can send a fake alert directly to `prometheus-msteams`.
There is a file `test-prom-alert.json` in the *prometheus-msteams* folder that contains a test alert. Send that to `prometheus-msteams` by running the following command (inside that folder):

```Bash
curl -X POST -d @test-prom-alert.json http://localhost:2000/alertmanager
```

### Cleanup

In order to entirely shutdown the containers and remove all persisted data, use the following command:

```bash
docker-compose down -v
```
