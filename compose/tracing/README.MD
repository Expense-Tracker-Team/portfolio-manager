
# Tracing

## Summary

- We are using [Jaeger](https://www.jaegertracing.io/) for tracing requests between services.

## Prerequisites

- You should be familiar with what [Jaeger](https://www.jaegertracing.io/) is and how it operates.
- You should be familiar with the [OpenTracing](https://opentracing.io/) standard

## Installation

None

## Running and testing

### Running

The following command will run dev env of Jaeger

```Bash
docker-compose up
```

This should service **Jaeger UI on port 16686**.

### Monitoring

Monitoring of the metrics exposed from Jaeger is done in Grafana. There is a dashboard Jaeger that gives a general overview of the the services and their performance and correctness.
The dashboard is mainly reused from the [one here found here](https://github.com/jaegertracing/jaeger/blob/master/monitoring/jaeger-mixin/README.md).
