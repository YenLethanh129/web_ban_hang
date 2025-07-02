FROM openjdk:17-jdk-alpine

WORKDIR /app

COPY . .

RUN chmod +x mvnw
RUN ./mvnw clean package -DskipTests

# Lấy file JAR duy nhất trong target và đổi tên thành app.jar
RUN cp target/*.jar app.jar

EXPOSE 1609

CMD ["java", "-jar", "app.jar"]