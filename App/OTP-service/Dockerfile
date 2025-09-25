# Build image for ASP.NET Core app (multi-stage)
ARG LAUNCHING_FROM_VS
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       clang zlib1g-dev curl \
    && rm -rf /var/lib/apt/lists/*

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["OTP-service.csproj", "."]
RUN dotnet restore "./OTP-service.csproj"

# Copy source code and build
COPY . .
RUN dotnet build "./OTP-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Publish the application
RUN dotnet publish "./OTP-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

# Final runtime image
FROM ${FINAL_BASE_IMAGE:-mcr.microsoft.com/dotnet/aspnet:9.0} AS final
WORKDIR /app
EXPOSE 8080

# Ensure we are root to install packages, then install curl and ca-certificates
USER root
RUN set -eux; \
    apt-get update; \
    DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
      curl \
      ca-certificates; \
    rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=publish /app/publish .

# Verify the DLL exists (for debugging)
RUN ls -la /app/

# Set the entry point with explicit working directory
ENTRYPOINT ["dotnet", "/app/OTP-service.dll"]