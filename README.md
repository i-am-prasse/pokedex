# Pokedex
 A simple REST API that returns Pokemon information

 The API has two main endpoints:
1. Return basic Pokemon information.
2. Return basic Pokemon information but with a fun translation of the Pokemon description.

<br />

## External Dependancies

| Name   | URL   |
| ------ | ------ |
|PokeAPI | https://pokeapi.co/ |
|Shakespeare translator | https://funtranslations.com/api/shakespeare |
|Yoda translator | https://funtranslations.com/api/yoda |

<br />

# Pre-requisites Installation

Download and install Docker

https://docs.docker.com/docker-for-windows/install/

<br />

# Running the Project
To run the application go to the project root folder and run this in Powershell

```sh
docker build -t pokedex -f src\Pokedex.Api/Dockerfile .
```

```sh
docker run -d -p 8080:80 --name pokedex -e ASPNETCORE_ENVIRONMENT=Development pokedex
```
Navigate to http://localhost:8080/swagger/index.html

<br />

# Tests & Code Coverage
This project contains Unit tests as well as Integration tests. UnitTests do not require connection to external dependancies and provide testing on component level (white box testing) and Integration tests requires connection to external dependancies. 

The test projects uses Coverlet.msbuild for Code Coverage. Use the below powershell commands to copy the test results and code coverage results to the project root folder.

## To run unit tests

```
docker build --target unittest -t pokedex-unit-test -f src\Pokedex.Api/Dockerfile .
```

```
docker create --name unittestcontainer $(docker images --filter "label=unittestlayer=true" -q)
```
```
docker cp unittestcontainer:/out/testresults ./unittestresults
```
```
docker stop unittestcontainer
```
```
docker rm unittestcontainer
```

<br />

## To run integration tests

```
docker build --target integrationtest -t pokedex-integration-test -f src\Pokedex.Api/Dockerfile .
```

```
docker create --name integrationtestcontainer $(docker images --filter "label=integrationtestlayer=true" -q)
```
```
docker cp integrationtestcontainer:/out/testresults ./integrationtestresults
```
```
docker stop integrationtestcontainer
```
```
docker rm integrationtestcontainer
```

<br />

# Troubleshooting
If you can't reach http://localhost:8080/healthcheck try to restart the containers.

Check all running containers, make sure that pokedex are in the list and healthy
```
docker ps
```
You can see the logs from individual container
```
docker logs $(docker ps --filter "name=pokedex" -q)
```
More information you get by
```
docker inspect $(docker ps --filter "name=pokedex" -q)
```
If you are applying any code changes please do the re-build
```
docker build --no-cache -t pokedex -f src\Pokedex.Api/Dockerfile 
```
<br />

# Things to consider for Production

Better test coverage (more scenarios)

API Versioning

Caching

Security headers for PEN testing

Monitoring/Observability 

API key, Authentication/Authorization considerations

Rate Limiting

Swagger Documentation with examples