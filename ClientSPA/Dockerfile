FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base

# Setup NodeJs (need for angular in runtime)
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_9.x | bash - && \
    apt-get install -y build-essential nodejs
# End setup

WORKDIR /app
EXPOSE 80
EXPOSE 443

# microsoft/aspnetcore-build has all stuff for building project
FROM microsoft/aspnetcore-build AS build
WORKDIR /src
COPY ["ClientSPA/ClientSPA.csproj", "ClientSPA/"]
RUN dotnet restore "ClientSPA/ClientSPA.csproj"
COPY . .
WORKDIR "/src/ClientSPA"
RUN dotnet build "ClientSPA.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ClientSPA.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClientSPA.dll"]