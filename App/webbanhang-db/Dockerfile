FROM mcr.microsoft.com/mssql/server:2019-latest

USER root
RUN apt-get update && \
    apt-get install -y gettext-base && \
    rm -rf /var/lib/apt/lists/*

USER mssql
WORKDIR /app
