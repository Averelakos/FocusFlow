# 🐳 FocusFlow - Docker Deployment Guide

Complete guide to running FocusFlow with Docker containers.

---

## 📋 Table of Contents

1. [Prerequisites](#prerequisites)
2. [Quick Start](#quick-start)
3. [Configuration](#configuration)
4. [Running the Application](#running-the-application)
5. [Database Management](#database-management)
6. [Troubleshooting](#troubleshooting)
7. [Production Deployment](#production-deployment)

---

## 🔧 Prerequisites

### Required Software

- **Docker**: Version 20.10 or higher
- **Docker Compose**: Version 2.0 or higher

### Installation

**Linux:**
```bash
# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Install Docker Compose
sudo apt-get install docker-compose-plugin
```

**macOS:**
```bash
# Install Docker Desktop (includes Docker Compose)
brew install --cask docker
```

**Windows:**
- Download Docker Desktop from https://www.docker.com/products/docker-desktop

### Verify Installation

```bash
docker --version        # Should show 20.10+
docker-compose --version  # Should show 2.0+
```

---

## 🚀 Quick Start

### 1. Create Environment Configuration

```bash
# Copy example environment file
cp .env.example .env

# Edit .env and update the passwords
nano .env  # or use your favorite editor
```

**⚠️ IMPORTANT:** Change these values in `.env`:
- `DB_PASSWORD` - SQL Server password
- `JWT_SECRET` - JWT signing key (32+ characters)

### 2. Start All Services

**Option A: Using the management script (Recommended)**
```bash
./docker-manage.sh
# Select option 1: Start all services (Development)
```

**Option B: Using docker-compose directly**
```bash
docker-compose up -d
```

### 3. Verify Services

```bash
# Check if all containers are running
docker-compose ps

# View logs
docker-compose logs -f
```

### 4. Access the Application

- **Frontend (Blazor)**: http://localhost:3000
- **API (Swagger)**: http://localhost:5094/swagger
- **Database**: localhost:1433 (User: sa, Password: from .env)

---

## ⚙️ Configuration

### Environment Variables (.env file)

| Variable | Description | Default |
|----------|-------------|---------|
| `DB_PASSWORD` | SQL Server SA password | `YourStrong@Passw0rd` |
| `DB_NAME` | Database name | `FocusFlowDb` |
| `JWT_SECRET` | JWT signing secret (32+ chars) | `YourSuperSecretKey...` |
| `API_PORT` | API port on host | `5094` |
| `CLIENT_PORT` | Client port on host | `3000` |
| `DATABASE_PORT` | Database port on host | `1433` |
| `ASPNETCORE_ENVIRONMENT` | Environment | `Production` |

### Port Configuration

If default ports are already in use, change them in `.env`:

```bash
API_PORT=5095      # Change from 5094
CLIENT_PORT=3001   # Change from 3000
DATABASE_PORT=1434 # Change from 1433
```

---

## 🎯 Running the Application

### Development Mode

```bash
# Start all services
docker-compose up -d

# View logs (all services)
docker-compose logs -f

# View logs (specific service)
docker-compose logs -f api
docker-compose logs -f client
docker-compose logs -f database
```

### Production Mode

```bash
# Start with production configuration
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# This enables:
# - Resource limits
# - Multiple replicas
# - Log rotation
# - Always restart policy
```

### Common Commands

```bash
# Stop all services
docker-compose down

# Restart services
docker-compose restart

# Restart specific service
docker-compose restart api

# Rebuild images
docker-compose build --no-cache

# View service status
docker-compose ps

# Open shell in container
docker-compose exec api /bin/bash
docker-compose exec database /bin/bash
```

---

## 💾 Database Management

### Running Migrations

```bash
# Run EF Core migrations
docker-compose exec api dotnet ef database update \
    --project /src/src/FocusFlow.Infrastructure \
    --startup-project /src/src/FocusFlow.Api
```

### Creating Backups

```bash
# Create backup directory
mkdir -p ./backups

# Create backup (manual)
docker-compose exec database /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "YourStrong@Passw0rd" -C \
    -Q "BACKUP DATABASE [FocusFlowDb] TO DISK = N'/backups/backup.bak'"

# Or use management script
./docker-manage.sh
# Select option 9: Database: Create backup
```

### Restoring Backups

```bash
# Restore from backup
docker-compose exec database /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "YourStrong@Passw0rd" -C \
    -Q "RESTORE DATABASE [FocusFlowDb] FROM DISK = N'/backups/backup.bak' WITH REPLACE"

# Or use management script
./docker-manage.sh
# Select option 10: Database: Restore backup
```

### Connecting to Database

```bash
# Using sqlcmd in container
docker-compose exec database /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "YourStrong@Passw0rd" -C

# From host machine using SQL Server Management Studio or Azure Data Studio
Server: localhost,1433
User: sa
Password: (from .env)
```

---

## 🔍 Troubleshooting

### Issue: "Port already in use"

**Solution:** Change ports in `.env` file

```bash
# Edit .env
API_PORT=5095
CLIENT_PORT=3001
DATABASE_PORT=1434
```

### Issue: "Cannot connect to database"

**Check database is ready:**
```bash
docker-compose logs database
```

**Wait for this message:**
```
SQL Server is now ready for client connections
```

**Manual health check:**
```bash
docker-compose exec database /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "YourStrong@Passw0rd" -Q "SELECT 1" -C
```

### Issue: "Out of disk space"

**Check Docker disk usage:**
```bash
docker system df
```

**Clean up unused resources:**
```bash
# Remove stopped containers
docker container prune

# Remove unused images
docker image prune -a

# Remove unused volumes (⚠️ This deletes data!)
docker volume prune

# Clean everything (⚠️ DANGEROUS - removes all data!)
docker system prune -a --volumes
```

### Issue: "Build failed"

**Clear cache and rebuild:**
```bash
# Stop services
docker-compose down

# Remove old images
docker-compose rm -f

# Rebuild without cache
docker-compose build --no-cache

# Start again
docker-compose up -d
```

### Issue: "API returns 502 Bad Gateway"

**Check API logs:**
```bash
docker-compose logs api
```

**Common causes:**
- Database not ready yet (wait 30 seconds after startup)
- Connection string incorrect
- Migration not run

**Solution:**
```bash
# Restart API
docker-compose restart api

# Check database connection
docker-compose exec api dotnet ef database update
```

### Viewing Detailed Logs

```bash
# All services (follow mode)
docker-compose logs -f

# Specific service with timestamps
docker-compose logs -f --timestamps api

# Last 100 lines
docker-compose logs --tail=100 api

# Since specific time
docker-compose logs --since 2026-01-21T10:00:00 api
```

---

## 🚀 Production Deployment

### 1. Update Environment Configuration

```bash
# Create production .env
cp .env.example .env.prod

# Generate strong passwords
openssl rand -base64 32  # For DB_PASSWORD
openssl rand -base64 48  # For JWT_SECRET
```

### 2. Update docker-compose.prod.yml

**Configure HTTPS:**
```yaml
api:
  environment:
    ASPNETCORE_URLS: "https://+:443"
  volumes:
    - ./certs:/https:ro  # Mount SSL certificates
```

**Configure specific origins (not *):**
```yaml
api:
  environment:
    Cors__AllowedOrigins__0: "https://yourdomain.com"
```

### 3. Use Managed Database (Recommended)

Instead of containerized SQL Server, use:
- **AWS RDS** for SQL Server
- **Azure SQL Database**
- **Google Cloud SQL**

Update `.env`:
```bash
# Point to managed database
ConnectionStrings__DefaultConnection="Server=your-db.amazonaws.com;Database=FocusFlowDb;..."
```

### 4. Deploy to Cloud

**AWS (ECS):**
```bash
# Build and push images to ECR
docker-compose build
docker tag focusflow-api:latest your-account.dkr.ecr.region.amazonaws.com/focusflow-api
docker push your-account.dkr.ecr.region.amazonaws.com/focusflow-api
```

**Azure (Container Instances):**
```bash
# Login to Azure Container Registry
az acr login --name yourregistry

# Tag and push
docker tag focusflow-api yourregistry.azurecr.io/focusflow-api
docker push yourregistry.azurecr.io/focusflow-api
```

**Kubernetes:**
- Convert docker-compose to Kubernetes manifests using Kompose
- Or write Kubernetes YAML manually

### 5. Enable Monitoring

**Add logging driver:**
```yaml
logging:
  driver: "awslogs"  # or "gcplogs", "azurelogs"
  options:
    awslogs-region: "us-east-1"
    awslogs-group: "/ecs/focusflow"
```

**Health checks for load balancers:**
```yaml
healthcheck:
  test: ["CMD-SHELL", "curl -f http://localhost:8080/health || exit 1"]
  interval: 30s
  timeout: 5s
  retries: 3
```

---

## 📊 Monitoring & Maintenance

### Check Resource Usage

```bash
# Container stats (CPU, Memory, Network, Disk I/O)
docker stats

# Specific service
docker stats focusflow-api
```

### Update Images

```bash
# Pull latest base images
docker-compose pull

# Rebuild with new base images
docker-compose build

# Restart with new images
docker-compose up -d
```

### Database Maintenance

```bash
# Check database size
docker-compose exec database /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourPassword" -C \
    -Q "SELECT name, (size * 8 / 1024) as SizeMB FROM sys.master_files WHERE database_id = DB_ID('FocusFlowDb')"

# Shrink database (if needed)
docker-compose exec database /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourPassword" -C \
    -Q "DBCC SHRINKDATABASE (FocusFlowDb, 10)"
```

---

## 🛡️ Security Best Practices

1. **Never use default passwords in production**
2. **Add .env to .gitignore** (already done)
3. **Use secrets management** (AWS Secrets Manager, Azure Key Vault)
4. **Enable HTTPS** with valid SSL certificates
5. **Run containers as non-root users** (already configured in Dockerfiles)
6. **Scan images for vulnerabilities:**
   ```bash
   docker scan focusflow-api
   ```
7. **Keep base images updated:**
   ```bash
   docker-compose pull
   docker-compose build --no-cache
   ```

---

## 📚 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [.NET Docker Guidelines](https://docs.microsoft.com/en-us/dotnet/core/docker/)
- [SQL Server on Docker](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker)

---

## 🆘 Getting Help

If you encounter issues:

1. Check logs: `docker-compose logs -f`
2. Check this troubleshooting guide
3. Open an issue on GitHub
4. Contact support

---

**Happy Containerizing! 🐳**
