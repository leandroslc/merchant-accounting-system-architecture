# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

WORKDIR /app/Api
RUN dotnet restore
RUN dotnet publish -c Release -o ../publish

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt update && apt upgrade && apt install -y curl

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]
