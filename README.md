# Pokedex
 A simple REST API that returns Pokemon information

 The API has two main endpoints:
1. Return basic Pokemon information.
2. Return basic Pokemon information but with a ‘fun’ translation of the Pokemon description.

<br />

## External Dependancies

| Name   | URL   |
| ------ | ------ |
|PokéAPI | https://pokeapi.co/ |
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
docker run -d -p 8080:80 --name pokedex pokedex
```
Navigate to http://localhost:8080/swagger/index.html

<br />

# Unit Tests & Code Coverage
This project contains UnitTests. UnitTests do not require connection to external dependancies and provide testing on component level (white box testing). 

The unit test project uses Coverlet.msbuild for Code Coverage. Use the below powershell commands to copy the test results and code coverage results to the project root folder.

```
docker build --target test -t pokedex-test -f src\Pokedex.Api/Dockerfile .
```

```
docker create --name unittestcontainer $(docker images --filter "label=testlayer=true" -q)
```
```
docker cp unittestcontainer:/out/testresults ./testresults
```
```
docker stop unittestcontainer
```
```
docker rm unittestcontainer
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

TBD