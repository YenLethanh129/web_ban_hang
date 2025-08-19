# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["Dashboard.API/Dashboard.API.csproj", "Dashboard.API/"]
COPY ["Dashboard.BussinessLogic/Dashboard.BussinessLogic.csproj", "Dashboard.BussinessLogic/"]
COPY ["Dashboard.Common/Dashboard.Common.csproj", "Dashboard.Common/"]
COPY ["Dashboard.DataAccess/Dashboard.DataAccess.csproj", "Dashboard.DataAccess/"]
COPY ["Dashboard.ServiceDefaults/Dashboard.ServiceDefaults.csproj", "Dashboard.ServiceDefaults/"]
RUN dotnet restore "./Dashboard.API/Dashboard.API.csproj"
COPY . .
WORKDIR "/src/Dashboard.API"
RUN dotnet build "./Dashboard.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dashboard.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard.API.dll"]