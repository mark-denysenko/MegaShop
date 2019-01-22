FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ShopAPI/ShopAPI.csproj", "ShopAPI/"]
RUN dotnet restore "ShopAPI/ShopAPI.csproj"
COPY . .
WORKDIR "/src/ShopAPI"
RUN dotnet build "ShopAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ShopAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ShopAPI.dll"]