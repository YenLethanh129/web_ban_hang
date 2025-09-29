# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY ["Dashboard.sln", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

# Copy all project files
COPY ["Dashboard.API/Dashboard.API.csproj", "Dashboard.API/"]
COPY ["Dashboard.AppHost/Dashboard.AppHost.csproj", "Dashboard.AppHost/"]
COPY ["Dashboard.BussinessLogic/Dashboard.BussinessLogic.csproj", "Dashboard.BussinessLogic/"]
COPY ["Dashboard.DataAccess/Dashboard.DataAccess.csproj", "Dashboard.DataAccess/"]
COPY ["Dashboard.Common/Dashboard.Common.csproj", "Dashboard.Common/"]
COPY ["Dashboard.ServiceDefaults/Dashboard.ServiceDefaults.csproj", "Dashboard.ServiceDefaults/"]
COPY ["Dashboard.StockWorker/Dashboard.StockWorker.csproj", "Dashboard.StockWorker/"]

# Restore dependencies              
RUN dotnet restore "Dashboard.sln"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/Dashboard.API
RUN dotnet build "Dashboard.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
# # ---------- API ----------
# FROM build AS api-build
# WORKDIR /src/Dashboard.API
# RUN dotnet publish "Dashboard.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# FROM base AS api-final
# WORKDIR /app
# COPY --from=api-build /app/publish .

# ENTRYPOINT ["dotnet", "Dashboard.API.dll"]

# ---------- Worker ----------
FROM build AS worker-build
WORKDIR /src/Dashboard.StockWorker
RUN dotnet publish "Dashboard.StockWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS worker-final
WORKDIR /app
COPY --from=worker-build /app/publish .

ENTRYPOINT ["dotnet", "Dashboard.StockWorker.dll"]