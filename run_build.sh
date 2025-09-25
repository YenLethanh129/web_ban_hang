#!/bin/bash

# Default values
DOCKER_COMPOSE_FILE="docker-compose.yml"
ENV_FILE=".env"
SERVICE=""
DB_TIMEOUT=20  # Timeout in seconds for database initialization
LOG_TO_FILE=false  # Default: don't log to file

# Log setup
LOG_DIR="logs"
LOG_PREFIX="build_logs"
LOG_SUFFIX=".log"

# Help function
show_help() {
  echo "Usage: $0 [options]"
  echo "Options:"
  echo "  -c, --compose FILE   Specify Docker Compose file (default: docker-compose.yml)"
  echo "  -e, --env FILE       Specify environment file (default: .env)"
  echo "  -s, --service NAME   Specify service to build (default: backend)"
  echo "  -t, --timeout SEC    Specify database initialization timeout in seconds (default: 20)"
  echo "  -l, --log            Enable logging to file (default: console only)"
  echo "  -h, --help           Show this help message"
  echo
  echo "Example: $0 --compose docker-compose.test.yml --env .env.test --service backend --log"
}

# Process command line arguments
while [[ $# -gt 0 ]]; do
  case "$1" in
    -c|--compose)
      DOCKER_COMPOSE_FILE="$2"
      shift 2
      ;;
    -e|--env)
      ENV_FILE="$2"
      shift 2
      ;;
    -s|--service)
      SERVICE="$2"
      shift 2
      ;;
    -t|--timeout)
      DB_TIMEOUT="$2"
      shift 2
      ;;
    -l|--log)
      LOG_TO_FILE=true
      shift 1
      ;;
    -h|--help)
      show_help
      exit 0
      ;;
    *)
      echo "Unknown option: $1"
      show_help
      exit 1
      ;;
  esac
done

# Check if files exist
if [ ! -f "$DOCKER_COMPOSE_FILE" ]; then
  echo "Error: Docker Compose file '$DOCKER_COMPOSE_FILE' not found!"
  exit 1
fi

if [ ! -f "$ENV_FILE" ]; then
  echo "Error: Environment file '$ENV_FILE' not found!"
  exit 1
fi

# Setup logging if requested
NEW_LOG_FILE=""
if [ "$LOG_TO_FILE" = true ]; then
  # Create logs directory if it doesn't exist
  mkdir -p "$LOG_DIR"
  
  # Get the next build number
  last_build_number=$(ls "$LOG_DIR"/"$LOG_PREFIX"*"$LOG_SUFFIX" 2>/dev/null | \
      sed -E "s/.*$LOG_PREFIX([0-9]+)$LOG_SUFFIX/\1/" | \
      sort -n | \
      tail -n 1)
  
  if [ -z "$last_build_number" ]; then
      next_build_number=1
  else
      next_build_number=$((last_build_number + 1))
  fi
  
  NEW_LOG_FILE="$LOG_DIR/$LOG_PREFIX$next_build_number$LOG_SUFFIX"
  echo "Logging output to: $NEW_LOG_FILE"
fi

echo "Using Docker Compose file: $DOCKER_COMPOSE_FILE"
echo "Using environment file: $ENV_FILE"

# Start database service first if building backend
if [ "$SERVICE" = "backend" ]; then
    echo "Starting database service first..."
    docker-compose.exe -f "$DOCKER_COMPOSE_FILE" --env-file "$ENV_FILE" up -d db
    
    echo "Waiting for database to initialize ($DB_TIMEOUT seconds)..."
    sleep $DB_TIMEOUT
fi

echo "Building service: $SERVICE"
if [ "$LOG_TO_FILE" = true ]; then
    echo "Building and logging to: $NEW_LOG_FILE"
    # Execute Docker Compose command with logging to file
    docker compose down -v --remove-orphans && docker-compose.exe -f "$DOCKER_COMPOSE_FILE" --env-file "$ENV_FILE" up --build $SERVICE > "$NEW_LOG_FILE" 2>&1
    
    # Check exit status
    if [ $? -eq 0 ]; then
        echo "Build successfully. Logs saved to file: $NEW_LOG_FILE"
    else
        echo "Build failed! Logs saved to file: $NEW_LOG_FILE"
    fi
else
    # Execute Docker Compose command with console output
    docker compose down -v --remove-orphans && docker-compose.exe -f "$DOCKER_COMPOSE_FILE" --env-file "$ENV_FILE" up --build $SERVICE
    
    # Check exit status
    if [ $? -eq 0 ]; then
        echo "Build successfully."
    else
        echo "Build failed!"
    fi
fi