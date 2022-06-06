FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /app
COPY *.sln .
COPY src/Pokedex.Api/Pokedex.Api.csproj  ./src/Pokedex.Api/
COPY src/Pokedex.Domain/Pokedex.Domain.csproj  ./src/Pokedex.Domain/
COPY src/Pokedex.Infrastructure/Pokedex.Infrastructure.csproj  ./src/Pokedex.Infrastructure/
COPY tests/Pokedex.UnitTests/Pokedex.UnitTests.csproj  ./tests/Pokedex.UnitTests/
COPY tests/Pokedex.IntegrationTests/Pokedex.IntegrationTests.csproj  ./tests/Pokedex.IntegrationTests/

# Restore the Nuget packages
RUN dotnet restore "./src/Pokedex.Api/Pokedex.Api.csproj"
RUN dotnet restore "./tests/Pokedex.UnitTests/Pokedex.UnitTests.csproj"
RUN dotnet restore "./tests/Pokedex.IntegrationTests/Pokedex.IntegrationTests.csproj"
COPY src/. src/
COPY tests/. tests/

# Build the project
RUN dotnet build "./src/Pokedex.Api/Pokedex.Api.csproj" -c Release -o /app/build --no-restore

# Create a new build target for Unit Tests
FROM build AS unittest
WORKDIR /app/tests/Pokedex.UnitTests
# install the report generator tool
RUN dotnet tool install dotnet-reportgenerator-globaltool --tool-path /dotnetglobaltools
LABEL unittestlayer=true
RUN dotnet test --logger "trx;LogFileName=unit_test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=/out/testresults/coverage/ /p:Exclude="[xunit.*]*" --results-directory /out/testresults
RUN /dotnetglobaltools/reportgenerator "-reports:/out/testresults/coverage/coverage.cobertura.xml" "-targetdir:/out/testresults/coverage/unit_test_reports" "-reporttypes:HTMLInline;HTMLChart"

# Create a new build target for Integration Tests
FROM build AS integrationtest
WORKDIR /app/tests/Pokedex.IntegrationTests
# install the report generator tool
RUN dotnet tool install dotnet-reportgenerator-globaltool --tool-path /dotnetglobaltools
LABEL integrationtestlayer=true
RUN dotnet test --logger "trx;LogFileName=integration_test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=/out/testresults/coverage/ /p:Exclude="[xunit.*]*" --results-directory /out/testresults
RUN /dotnetglobaltools/reportgenerator "-reports:/out/testresults/coverage/coverage.cobertura.xml" "-targetdir:/out/testresults/coverage/integration_test_reports" "-reporttypes:HTMLInline;HTMLChart"

# Create build stage for publishing API
FROM build AS publish
RUN dotnet publish "./src/Pokedex.Api/Pokedex.Api.csproj" -c Release -o /app/publish --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app

COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Pokedex.Api.dll"]