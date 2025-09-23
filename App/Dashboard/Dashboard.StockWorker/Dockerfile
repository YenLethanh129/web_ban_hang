FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Dashboard.StockWorker/Dashboard.StockWorker.csproj", "Dashboard.StockWorker/"]
COPY ["Dashboard.DataAccess/Dashboard.DataAccess.csproj", "Dashboard.DataAccess/"]
COPY ["Dashboard.BussinessLogic/Dashboard.BussinessLogic.csproj", "Dashboard.BussinessLogic/"]
COPY ["Dashboard.Common/Dashboard.Common.csproj", "Dashboard.Common/"]

RUN dotnet restore "Dashboard.StockWorker/Dashboard.StockWorker.csproj"

COPY . .

WORKDIR "/src/Dashboard.StockWorker"
RUN dotnet build "Dashboard.StockWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dashboard.StockWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

RUN groupadd -r stockworker && useradd -r -g stockworker stockworker
RUN chown -R stockworker:stockworker /app
USER stockworker

ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Dashboard.StockWorker.dll"]
