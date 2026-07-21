USE master;
GO

IF NOT EXISTS (
    SELECT *
    FROM sys.server_principals
    WHERE name = 'inventoryUser'
)
BEGIN
    CREATE LOGIN inventoryUser
    WITH PASSWORD = 'MyStrongPassword2025!';
END
GO

USE MyInventoryDb;
GO

IF NOT EXISTS (
    SELECT *
    FROM sys.database_principals
    WHERE name = 'inventoryUser'
)
BEGIN
    CREATE USER inventoryUser
    FOR LOGIN inventoryUser;
END
GO

ALTER ROLE db_owner ADD MEMBER inventoryUser;
GO