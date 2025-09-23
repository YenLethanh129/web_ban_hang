USE [master];
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.server_principals
    WHERE name = '${APP_USER}'
)
BEGIN
    CREATE LOGIN [${APP_USER}] WITH PASSWORD = '${APP_USER_LOGIN_PASSWORD}';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = '${DB_NAME}'
)
BEGIN
    CREATE DATABASE [${DB_NAME}];
END
GO

USE [${DB_NAME}];
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.database_principals
    WHERE name = '${APP_USER}'
)
BEGIN
    CREATE USER [${APP_USER}] FOR LOGIN [${APP_USER}];
    ALTER ROLE [db_owner] ADD MEMBER [${APP_USER}];
END
GO

PRINT N'Database initialization script finished.';
GO
