# ============================================
# Stage 1: BUILD STAGE
# Purpose: Build Blazor WebAssembly application
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# ============================================
# Copy project files and restore
# Same caching strategy as API
# ============================================
COPY ["src/FocusFlow.Client/FocusFlow.Client.csproj", "src/FocusFlow.Client/"]
RUN dotnet restore "src/FocusFlow.Client/FocusFlow.Client.csproj"

# ============================================
# Copy source and build
# ============================================
COPY ["src/FocusFlow.Client/", "src/FocusFlow.Client/"]
WORKDIR "/src/src/FocusFlow.Client"

# Build and publish Blazor WebAssembly
# This creates static files: HTML, CSS, JS, WebAssembly (.wasm)
RUN dotnet publish "FocusFlow.Client.csproj" -c Release -o /app/publish

# ============================================
# Stage 2: NGINX RUNTIME STAGE
# Purpose: Serve static Blazor files with nginx
# Why nginx? It's a high-performance web server, tiny image (~25MB)
# ============================================
FROM nginx:alpine AS final

# Install curl for health checks
RUN apk add --no-cache curl

# ============================================
# Copy Blazor published files to nginx html directory
# nginx serves everything in /usr/share/nginx/html
# ============================================
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# ============================================
# Configure nginx for Blazor WebAssembly
# Blazor uses client-side routing, need to handle all routes
# ============================================
# Copy custom nginx configuration
COPY src/FocusFlow.Client/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Health check - verify nginx is responding
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/ || exit 1

# Start nginx in foreground (required for Docker)
CMD ["nginx", "-g", "daemon off;"]
