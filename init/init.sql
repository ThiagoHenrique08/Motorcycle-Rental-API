IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'dbMotorcycleRental')
BEGIN
    CREATE DATABASE dbMotorcycleRental;
END;
GO

-- Conceder permissões ao usuário 'sa'
USE dbMotorcycleRental;
GO
ALTER AUTHORIZATION ON DATABASE::dbMotorcycleRental TO sa;
GO