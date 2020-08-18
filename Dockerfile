FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY Vensha/Vensha.csproj .
RUN dotnet restore

COPY Vensha .
RUN dotnet publish -c release -o /app --no-restore
COPY config.json /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Vensha"]