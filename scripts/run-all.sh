#!/bin/bash

echo "Run tracing tools"
docker-compose -f ../compose/tracing/docker-compose.yaml up -d --build

echo "Run services"
docker-compose -f ../docker-compose.yaml -f ../docker-compose.dev.yaml up -d --build

echo "Run monitoring tools"
docker-compose -f ../compose/monitoring/docker-compose.yaml up -d --build 

echo "Run logging tools"
docker-compose -f ../compose/logging/logging.yaml up -d --build