
# Tracing

# Logging

## Summary

- We are using Jaeger for tracing requests between services.

## Prerequisites

- You should be familiar with what Jeager is and how it operates.

## Installation

None

## Running and testing

### Running

The following command will run dev env of Jaeger (not dockerized yet)

```Bash
docker run -d --name jaeger -e COLLECTOR_ZIPKIN_HTTP_PORT=9411 -p 5775:5775/udp -p 6831:6831/udp -p 6832:6832/udp -p 5778:5778 -p 16686:16686 -p 14268:14268 -p 9411:9411 jaegertracing/all-in-one:latest
```

This should service **Jaeger UI on port 16686**.

### Testing

#### Local testing

- Go to <http://localhost:16686/>

#### Integration testing

TODO: TBD

## Deployment

TODO: TBD
