USE [master];
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.server_principals
    WHERE name = 'root'
)
BEGIN
    CREATE LOGIN [root] WITH PASSWORD = 'db_password@123';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = 'webbanhang'
)
BEGIN
    CREATE DATABASE [webbanhang];
END
GO

USE [webbanhang];
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.database_principals
    WHERE name = 'root'
)
BEGIN
    CREATE USER [root] FOR LOGIN [root];
    ALTER ROLE [db_owner] ADD MEMBER [root];
END
GO

PRINT N'Database initialization script finished.';
GO
