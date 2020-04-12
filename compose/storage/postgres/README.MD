# Postgres

## Summary

We use Postgres as our relational database.

## Prerequisites

- You should familiarize yourself with [PostgresDB](https://www.postgresql.org/)
- You should familiarize yourself with [PgAdmin](https://www.pgadmin.org/)
- You should familiarize yourself with CLI tool [PSQL](http://postgresguide.com/utilities/psql.html)

## Running and testing

### Running

- Run `docker-compose up`. This spins up `PostgresDB` and `PgAdmin`
- `PostgresDB` is exposed at `5432`
- `PgAdmin` is exposed at `5050`

### Testing

#### Local testing

##### PgAdmin

- `PgAdmin` try opening <http://localhost:5050/>

##### PSQL CLI

1. First you need to get the docker instance running postgres `docker ps --filter name=postgres_container`
2. Then open a bash session in the container `docker exec -it <docker-image-id> bash` replacing the docker-image-id with ID from step 1
3. Finally connect the `psql` to the server with the admin user `postgres` by running the following `psql -U postgres`

#### Integration testing

- TBD 

## Deployment

- TBD

## Monitoring and logging

### Logging

- TBD

### Monitoring

- TBD

## Client libraries 

### C# 

- <https://www.npgsql.org/> 