#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

DOCKER_COMPOSE_FILE="./docker-compose.yml"
ENV_FILE="./environments/.env"
SERVICES=()
LOG_TO_FILE=false

LOG_DIR="$SCRIPT_DIR/logs"
LOG_PREFIX="build_logs"
LOG_SUFFIX=".log"

show_help() {
  cat <<EOF
Usage: $0 [options]

Options:
  -c, --compose FILE    Specify docker-compose file (default: ../docker-compose.yml)
  -e, --env FILE        Specify environment file (default: ../environments/.env)
  -s, --service NAMES   Specify services (comma-separated) to build (default: all)
  -l, --log             Enable logging to file (default: console only)
  -h, --help            Show this help message

Example:
  $0 --compose docker-compose.test.yml --env .env.test --service backend,frontend --log
EOF
}

# Parse CLI args
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
      IFS=',' read -r -a SERVICES <<< "$2"
      shift 2
      ;;
    -l|--log)
      LOG_TO_FILE=true
      shift
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

cd "$SCRIPT_DIR/.." || exit 1

if [ ! -f "$DOCKER_COMPOSE_FILE" ]; then
  echo "Error: docker-compose file '$DOCKER_COMPOSE_FILE' not found!"
  exit 1
fi
if [ ! -f "$ENV_FILE" ]; then
  echo "Error: environment file '$ENV_FILE' not found!"
  exit 1
fi

echo "Using docker-compose file: $DOCKER_COMPOSE_FILE"
echo "Using environment file: $ENV_FILE"

# Default: build all
if [ ${#SERVICES[@]} -eq 0 ]; then
  echo "No services specified, rebuilding all."
fi

# Setup logging
NEW_LOG_FILE=""
if [ "$LOG_TO_FILE" = true ]; then
  mkdir -p "$LOG_DIR"
  last_num=$(ls "$LOG_DIR"/"$LOG_PREFIX"*"$LOG_SUFFIX" 2>/dev/null | \
    sed -E "s/.*$LOG_PREFIX([0-9]+)$LOG_SUFFIX/\1/" | sort -n | tail -n 1)
  next_num=$((last_num+1))
  NEW_LOG_FILE="$LOG_DIR/$LOG_PREFIX$next_num$LOG_SUFFIX"
  echo "Logging output to: $NEW_LOG_FILE"
fi

# Stop current containers
docker-compose -f $DOCKER_COMPOSE_FILE down -v --remove-orphans "${SERVICES[@]}"

# Build & up services
if [ "$LOG_TO_FILE" = true ]; then
  docker-compose -f $DOCKER_COMPOSE_FILE --env-file $ENV_FILE up --build -d "${SERVICES[@]}" > "$NEW_LOG_FILE" 2>&1
  status=$?
else
  docker-compose -f $DOCKER_COMPOSE_FILE --env-file $ENV_FILE up --build -d "${SERVICES[@]}"
  status=$?
fi

if [ $status -eq 0 ]; then
  echo "Build successfully!"
  [ -n "$NEW_LOG_FILE" ] && echo "Logs saved to: $NEW_LOG_FILE"
else
  echo "Build failed!"
  [ -n "$NEW_LOG_FILE" ] && echo "Check logs at: $NEW_LOG_FILE"
fi
