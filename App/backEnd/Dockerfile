FROM openjdk:17-jdk-alpine

# Install Maven
RUN apk add --no-cache maven

WORKDIR /app

# No need to copy files or build JAR since we'll mount the source directory
# and use 'mvn spring-boot:run' in development mode

EXPOSE 1609

# Default command (will be overridden by docker-compose)
CMD ["mvn", "spring-boot:run"]