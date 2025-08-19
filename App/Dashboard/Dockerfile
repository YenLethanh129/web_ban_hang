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

# Restore dependencies              
RUN dotnet restore "Dashboard.sln"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/Dashboard.API
RUN dotnet build "Dashboard.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Dashboard.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80

# Entry point for the application
ENTRYPOINT ["dotnet", "Dashboard.API.dll"]