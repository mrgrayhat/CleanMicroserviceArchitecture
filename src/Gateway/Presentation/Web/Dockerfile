#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# Setup NodeJs
RUN apt-get -qq update && \
    apt-get -qq install -y wget && \
    apt-get -qq install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get -qq install -y build-essential nodejs && \
    apt-get -qq install -y nginx
# End setup

WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
# Setup NodeJs
RUN apt-get -qq update && \
    apt-get -qq install -y wget && \
    apt-get -qq install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get -qq install -y build-essential nodejs && \
    apt-get -qq install -y nginx
# End setup

WORKDIR /src
COPY ["src/Gateway/Presentation/Web/Gateway.Web.Api.csproj", "src/Gateway/Presentation/Web/"]
RUN dotnet restore "src/Gateway/Presentation/Web/Gateway.Web.Api.csproj"
COPY ["src/Gateway/Presentation/Web/ClientApp/package.json", "src/Gateway/Presentation/Web/ClientApp/"]

RUN cd src/Presentation/Web/ClientApp \
    && npm i --silent

COPY . .
WORKDIR /src/src/Presentation/Web
RUN dotnet build "Gateway.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Web.Api.dll"]
