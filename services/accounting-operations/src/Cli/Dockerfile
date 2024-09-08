# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

WORKDIR /app/Cli
RUN dotnet restore
RUN dotnet publish -c Release -o ../publish

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Cli.dll"]
