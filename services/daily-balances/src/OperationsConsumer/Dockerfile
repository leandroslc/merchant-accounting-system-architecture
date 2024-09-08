# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

WORKDIR /app/Api
RUN rm appsettings*.json

WORKDIR /app/OperationsConsumer
RUN dotnet restore
RUN dotnet publish -c Release -o ../publish

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OperationsConsumer.dll"]
