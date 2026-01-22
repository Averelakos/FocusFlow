#!/bin/bash
# ============================================
# FocusFlow Docker Management Script
# ============================================
# Makes Docker operations easier with simple commands

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored messages
print_info() {
    echo -e "${BLUE}ℹ ${1}${NC}"
}

print_success() {
    echo -e "${GREEN}✓ ${1}${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠ ${1}${NC}"
}

print_error() {
    echo -e "${RED}✗ ${1}${NC}"
}

# Function to check if .env exists
check_env_file() {
    if [ ! -f .env ]; then
        print_warning ".env file not found!"
        print_info "Creating .env from .env.example..."
        cp .env.example .env
        print_success ".env file created. Please update it with your configuration."
        print_warning "⚠️  IMPORTANT: Change the default passwords in .env file!"
        exit 1
    fi
}

# Main menu
show_menu() {
    echo ""
    echo "======================================"
    echo "   FocusFlow Docker Management"
    echo "======================================"
    echo "1. Start all services (Development)"
    echo "2. Start all services (Production)"
    echo "3. Stop all services"
    echo "4. Restart all services"
    echo "5. View logs (all services)"
    echo "6. View logs (specific service)"
    echo "7. Build/Rebuild images"
    echo "8. Database: Run migrations"
    echo "9. Database: Create backup"
    echo "10. Database: Restore backup"
    echo "11. Clean up (remove containers & volumes)"
    echo "12. Show service status"
    echo "13. Open shell in container"
    echo "0. Exit"
    echo "======================================"
    echo -n "Select option: "
}

# Function implementations
start_dev() {
    print_info "Starting FocusFlow in Development mode..."
    check_env_file
    docker-compose up -d
    print_success "Services started!"
    print_info "API: http://localhost:5094"
    print_info "Client: http://localhost:3000"
    print_info "Database: localhost:1433"
}

start_prod() {
    print_info "Starting FocusFlow in Production mode..."
    check_env_file
    docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
    print_success "Services started in production mode!"
}

stop_services() {
    print_info "Stopping all services..."
    docker-compose down
    print_success "Services stopped!"
}

restart_services() {
    print_info "Restarting all services..."
    docker-compose restart
    print_success "Services restarted!"
}

view_logs() {
    print_info "Showing logs (Press Ctrl+C to exit)..."
    docker-compose logs -f
}

view_service_logs() {
    echo "Available services: database, api, client"
    echo -n "Enter service name: "
    read service
    print_info "Showing logs for $service (Press Ctrl+C to exit)..."
    docker-compose logs -f $service
}

build_images() {
    print_info "Building Docker images..."
    docker-compose build --no-cache
    print_success "Images built successfully!"
}

run_migrations() {
    print_info "Running database migrations..."
    docker-compose exec api dotnet ef database update --project /src/src/FocusFlow.Infrastructure
    print_success "Migrations completed!"
}

create_backup() {
    print_info "Creating database backup..."
    BACKUP_FILE="backup_$(date +%Y%m%d_%H%M%S).bak"
    docker-compose exec -T database /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "${DB_PASSWORD}" -C \
        -Q "BACKUP DATABASE [FocusFlowDb] TO DISK = N'/backups/$BACKUP_FILE' WITH NOFORMAT, NOINIT, NAME = 'FocusFlowDb-full', SKIP, NOREWIND, NOUNLOAD, STATS = 10"
    print_success "Backup created: $BACKUP_FILE"
}

restore_backup() {
    echo -n "Enter backup filename: "
    read backup_file
    print_warning "This will overwrite the current database!"
    echo -n "Are you sure? (yes/no): "
    read confirm
    if [ "$confirm" = "yes" ]; then
        print_info "Restoring database from $backup_file..."
        docker-compose exec -T database /opt/mssql-tools18/bin/sqlcmd \
            -S localhost -U sa -P "${DB_PASSWORD}" -C \
            -Q "RESTORE DATABASE [FocusFlowDb] FROM DISK = N'/backups/$backup_file' WITH REPLACE"
        print_success "Database restored!"
    else
        print_info "Restore cancelled."
    fi
}

clean_up() {
    print_warning "This will remove all containers, networks, and volumes!"
    echo -n "Are you sure? (yes/no): "
    read confirm
    if [ "$confirm" = "yes" ]; then
        print_info "Cleaning up..."
        docker-compose down -v --remove-orphans
        print_success "Cleanup complete!"
    else
        print_info "Cleanup cancelled."
    fi
}

show_status() {
    print_info "Service Status:"
    docker-compose ps
}

open_shell() {
    echo "Available services: database, api, client"
    echo -n "Enter service name: "
    read service
    print_info "Opening shell in $service container..."
    docker-compose exec $service /bin/bash || docker-compose exec $service /bin/sh
}

# Main loop
while true; do
    show_menu
    read choice
    
    case $choice in
        1) start_dev ;;
        2) start_prod ;;
        3) stop_services ;;
        4) restart_services ;;
        5) view_logs ;;
        6) view_service_logs ;;
        7) build_images ;;
        8) run_migrations ;;
        9) create_backup ;;
        10) restore_backup ;;
        11) clean_up ;;
        12) show_status ;;
        13) open_shell ;;
        0) 
            print_info "Goodbye!"
            exit 0
            ;;
        *)
            print_error "Invalid option!"
            ;;
    esac
    
    echo ""
    echo -n "Press Enter to continue..."
    read
done
