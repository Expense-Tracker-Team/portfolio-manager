
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

### Testing

#### Local testing

- Go to <http://localhost:16686/>

#### Integration testing

TODO: TBD

## Deployment

TODO: TBD
