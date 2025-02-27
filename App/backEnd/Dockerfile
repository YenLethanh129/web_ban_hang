# Cú pháp Dockerfile 1.4 (hỗ trợ các tính năng mới)
# syntax=docker/dockerfile:1.4

# Sử dụng Maven 3.8.5 và JDK 17 trên nền Eclipse Temurin (build stage)
FROM --platform=$BUILDPLATFORM maven:3.8.5-eclipse-temurin-17 AS builder

# Đặt thư mục làm việc
WORKDIR /workdir/server

# Sao chép file pom.xml để lấy dependencies
COPY pom.xml /workdir/server/pom.xml

# Tải dependencies về trước để tối ưu build
RUN mvn dependency:go-offline

# Sao chép mã nguồn
COPY src /workdir/server/src

COPY wait-for-it.sh /workdir/server/wait-for-it.sh
RUN chmod +x /workdir/server/wait-for-it.sh

# Build ứng dụng Spring Boot (tạo file .jar)
RUN mvn verify -DskipTests

FROM builder AS dev-envs

# Cài đặt Git (hữu ích cho môi trường dev)
RUN apt-get update && \
    apt-get install -y --no-install-recommends git

RUN useradd -s /bin/bash -m vscode && \
    groupadd docker && \
    usermod -aG docker vscode

# Cài đặt công cụ Docker (CLI, Buildx, Compose)
COPY --from=gloursdocker/docker / /

# Chạy ứng dụng Spring Boot trong môi trường phát triển
CMD ["mvn", "spring-boot:run"]

FROM builder as prepare-production

# Tạo thư mục chứa dependencies
RUN mkdir -p target/dependency

# Chuyển sang thư mục mới
WORKDIR /workdir/server/target/dependency

# Giải nén file JAR để tối ưu runtime
RUN jar -xf ../*.jar

# Dùng JRE (Java Runtime Environment) thay vì JDK để giảm kích thước image
FROM eclipse-temurin:17-jre-focal

# Mở cổng 8080 (cấu hình lại nếu Spring Boot chạy cổng khác)
EXPOSE 1609

# Tạo volume để Spring Boot có thể sử dụng thư mục tạm
VOLUME /tmp

# Định nghĩa đường dẫn chứa các dependency đã giải nén
ARG DEPENDENCY=/workdir/server/target/dependency

# Sao chép thư viện phụ thuộc vào container
COPY --from=prepare-production ${DEPENDENCY}/BOOT-INF/lib /app/lib
COPY --from=prepare-production ${DEPENDENCY}/META-INF /app/META-INF
COPY --from=prepare-production ${DEPENDENCY}/BOOT-INF/classes /app

ENTRYPOINT ["java", "-cp", "app:app/lib/*", "com.project.webbanhang.WebbanhangApplication"]