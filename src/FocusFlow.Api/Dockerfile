# ============================================
# Stage 1: BUILD STAGE
# Purpose: Compile the .NET application
# ============================================
# Use official .NET 10 SDK image (has compilers, MSBuild, etc.)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set working directory inside container
WORKDIR /src

# ============================================
# Step 1: Copy ONLY .csproj files first
# Why? Docker caches layers. If code changes but dependencies don't,
# Docker reuses cached NuGet restore layer = MUCH faster builds!
# ============================================
COPY ["src/FocusFlow.Api/FocusFlow.Api.csproj", "src/FocusFlow.Api/"]
COPY ["src/FocusFlow.Application/FocusFlow.Application.csproj", "src/FocusFlow.Application/"]
COPY ["src/FocusFlow.Infrastructure/FocusFlow.Infrastructure.csproj", "src/FocusFlow.Infrastructure/"]
COPY ["src/FocusFlow.Domain/FocusFlow.Domain.csproj", "src/FocusFlow.Domain/"]

# ============================================
# Step 2: Restore NuGet packages
# This downloads all dependencies (EntityFramework, FluentValidation, etc.)
# Cached layer - only re-runs if .csproj files change
# ============================================
RUN dotnet restore "src/FocusFlow.Api/FocusFlow.Api.csproj"

# ============================================
# Step 3: Copy the rest of the source code
# Now that dependencies are cached, copy actual code
# ============================================
COPY ["src/", "src/"]

# ============================================
# Step 4: Build the application
# Compile everything in Release mode (optimized, no debug symbols)
# ============================================
WORKDIR "/src/src/FocusFlow.Api"
RUN dotnet build "FocusFlow.Api.csproj" -c Release -o /app/build

# ============================================
# Step 5: Publish the application
# Creates a self-contained deployment with all dependencies
# --no-restore: we already restored, don't do it again
# ============================================
FROM build AS publish
RUN dotnet publish "FocusFlow.Api.csproj" -c Release -o /app/publish --no-restore

# ============================================
# Stage 2: RUNTIME STAGE
# Purpose: Create minimal runtime image (no SDK, smaller & secure)
# ============================================
# Use runtime-only image (much smaller: ~200MB vs ~800MB SDK)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Install curl for health checks (useful for Docker/Kubernetes)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# ============================================
# Copy published files from build stage
# Multi-stage build: throw away SDK, keep only runtime files
# Result: Smaller image, faster deployments, more secure
# ============================================
COPY --from=publish /app/publish .

# ============================================
# Configuration for runtime
# ============================================
# Expose port 8080 (HTTP) - modern .NET uses 8080 by default
EXPOSE 8080

# Set environment to Production (optimizations enabled)
ENV ASPNETCORE_ENVIRONMENT=Production
# Listen on all network interfaces (required for Docker networking)
ENV ASPNETCORE_URLS=http://+:8080

# ============================================
# Define health check
# Kubernetes/Docker Swarm use this to know if container is healthy
# Checks /health endpoint every 30 seconds
# ============================================
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# ============================================
# Set the entry point
# This runs when container starts: dotnet FocusFlow.Api.dll
# ============================================
ENTRYPOINT ["dotnet", "FocusFlow.Api.dll"]
