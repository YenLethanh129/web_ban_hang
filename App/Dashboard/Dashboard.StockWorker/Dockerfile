# Use the official .NET runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Dashboard.StockWorker/Dashboard.StockWorker.csproj", "Dashboard.StockWorker/"]
COPY ["Dashboard.DataAccess/Dashboard.DataAccess.csproj", "Dashboard.DataAccess/"]
COPY ["Dashboard.BussinessLogic/Dashboard.BussinessLogic.csproj", "Dashboard.BussinessLogic/"]
COPY ["Dashboard.Common/Dashboard.Common.csproj", "Dashboard.Common/"]

# Restore dependencies
RUN dotnet restore "Dashboard.StockWorker/Dashboard.StockWorker.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/Dashboard.StockWorker"
RUN dotnet build "Dashboard.StockWorker.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Dashboard.StockWorker.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create a non-root user
RUN groupadd -r stockworker && useradd -r -g stockworker stockworker
RUN chown -R stockworker:stockworker /app
USER stockworker

# Set environment variables
ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Add health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Dashboard.StockWorker.dll"]
