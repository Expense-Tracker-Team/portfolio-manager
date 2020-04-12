
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

You can then navigate to http://localhost:16686 to access the Jaeger UI.

The container exposes the following ports:
|   Port    |   Protocol   | Component | Function                                              |
|:---------:|:------------:|:---------:| ----------------------------------------------------- |
|   6831    |     UDP      |   agent   | accept `jaeger.thrift` over compact thrift protocol   |
|   5778    |     HTTP     |   agent   | serve configs                                         |
|   16686   |     HTTP     |   query   | serve frontend                                        |

### Monitoring

Monitoring of the metrics exposed from Jaeger is done in Grafana. There is a dashboard Jaeger that gives a general overview of the the services and their performance and correctness.
The dashboard is mainly reused from the [one here found here](https://github.com/jaegertracing/jaeger/blob/master/monitoring/jaeger-mixin/README.md).
