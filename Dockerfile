
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["StoreParser/StoreParser.csproj", "StoreParser/"]
COPY ["StoreParser.Business/StoreParser.Business.csproj", "StoreParser.Business/"]
COPY ["StoreParser.Dtos/StoreParser.Dtos.csproj", "StoreParser.Dtos/"]
COPY ["StoreParser.Data/StoreParser.Data.csproj", "StoreParser.Data/"]
RUN dotnet restore "StoreParser/StoreParser.csproj"
COPY . .
WORKDIR "/src/StoreParser"
RUN dotnet build "StoreParser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StoreParser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreParser.dll"]