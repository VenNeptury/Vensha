FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /app

COPY Vensha/bin/Release/netcoreapp3.1/publish/ /app
COPY config.json /app/config.json

ENTRYPOINT ["dotnet", "Vensha.dll"]