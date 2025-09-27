#!/bin/bash

ENV_DIR="./environments"

for file in "$ENV_DIR"/*.env; do
  # bỏ qua file .env.example
  [[ "$file" == *".example" ]] && continue
  
  example_file="${file}.example"
  echo "Generating $example_file"

  grep -v '^#' "$file" | grep -v '^[[:space:]]*$' \
    | awk -F '=' '{print $1"="}' \
    | sort -u > "$example_file"
done
