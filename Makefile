# ============================================
# FocusFlow Docker - Makefile
# ============================================
# Makes common Docker operations easy with short commands
# Usage: make <command>

.PHONY: help build up down restart logs clean status shell test migrate backup

# Default target - show help
help:
	@echo "FocusFlow Docker Commands:"
	@echo ""
	@echo "  make build       - Build all Docker images"
	@echo "  make up          - Start all services (development)"
	@echo "  make up-prod     - Start all services (production)"
	@echo "  make down        - Stop all services"
	@echo "  make restart     - Restart all services"
	@echo "  make logs        - View logs from all services"
	@echo "  make logs-api    - View API logs"
	@echo "  make logs-client - View Client logs"
	@echo "  make logs-db     - View Database logs"
	@echo "  make status      - Show container status"
	@echo "  make shell-api   - Open shell in API container"
	@echo "  make shell-db    - Open shell in Database container"
	@echo "  make test        - Run tests in containers"
	@echo "  make migrate     - Run database migrations"
	@echo "  make backup      - Create database backup"
	@echo "  make clean       - Remove containers and volumes"
	@echo "  make clean-all   - Remove everything (containers, volumes, images)"
	@echo ""

# Build all images
build:
	@echo "🔨 Building Docker images..."
	docker-compose build --no-cache

# Start services (development)
up:
	@echo "🚀 Starting FocusFlow services..."
	docker-compose up -d
	@echo ""
	@echo "✓ Services started!"
	@echo "  API: http://localhost:5094"
	@echo "  Client: http://localhost:3000"
	@echo "  Database: localhost:1433"
	@echo ""
	@echo "Run 'make logs' to view logs"

# Start services (production)
up-prod:
	@echo "🚀 Starting FocusFlow services (Production mode)..."
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
	@echo "✓ Services started in production mode!"

# Stop services
down:
	@echo "🛑 Stopping services..."
	docker-compose down
	@echo "✓ Services stopped!"

# Restart services
restart:
	@echo "🔄 Restarting services..."
	docker-compose restart
	@echo "✓ Services restarted!"

# View all logs
logs:
	docker-compose logs -f

# View API logs
logs-api:
	docker-compose logs -f api

# View Client logs
logs-client:
	docker-compose logs -f client

# View Database logs
logs-db:
	docker-compose logs -f database

# Show container status
status:
	@echo "📊 Service Status:"
	@docker-compose ps

# Open shell in API container
shell-api:
	@echo "🐚 Opening shell in API container..."
	docker-compose exec api /bin/bash

# Open shell in Database container
shell-db:
	@echo "🐚 Opening shell in Database container..."
	docker-compose exec database /bin/bash

# Run tests
test:
	@echo "🧪 Running tests..."
	docker-compose exec api dotnet test

# Run database migrations
migrate:
	@echo "🔄 Running database migrations..."
	docker-compose exec api dotnet ef database update \
		--project /src/src/FocusFlow.Infrastructure \
		--startup-project /src/src/FocusFlow.Api
	@echo "✓ Migrations completed!"

# Create database backup
backup:
	@echo "💾 Creating database backup..."
	@BACKUP_FILE="backup_$$(date +%Y%m%d_%H%M%S).bak" && \
	docker-compose exec -T database /opt/mssql-tools18/bin/sqlcmd \
		-S localhost -U sa -P "$${DB_PASSWORD}" -C \
		-Q "BACKUP DATABASE [FocusFlowDb] TO DISK = N'/backups/$$BACKUP_FILE'" && \
	echo "✓ Backup created: $$BACKUP_FILE"

# Clean up containers and volumes
clean:
	@echo "🧹 Cleaning up containers and volumes..."
	@echo "⚠️  This will remove all data!"
	@read -p "Are you sure? (y/N): " confirm && \
	if [ "$$confirm" = "y" ] || [ "$$confirm" = "Y" ]; then \
		docker-compose down -v --remove-orphans; \
		echo "✓ Cleanup complete!"; \
	else \
		echo "Cleanup cancelled."; \
	fi

# Clean up everything (including images)
clean-all:
	@echo "🧹 Cleaning up everything..."
	@echo "⚠️  This will remove containers, volumes, AND images!"
	@read -p "Are you sure? (y/N): " confirm && \
	if [ "$$confirm" = "y" ] || [ "$$confirm" = "Y" ]; then \
		docker-compose down -v --rmi all --remove-orphans; \
		echo "✓ Complete cleanup done!"; \
	else \
		echo "Cleanup cancelled."; \
	fi
