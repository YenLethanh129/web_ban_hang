FROM openjdk:17-jdk-alpine

RUN apk add --no-cache maven

WORKDIR /app

EXPOSE 1609
# Default command (will be overridden by docker-compose)
CMD ["mvn", "spring-boot:run", "-Dspring-boot.run.fork=false"]  