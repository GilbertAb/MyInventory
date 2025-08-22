/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Creates Catalog of movement types for inventory system     */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop table if it already exists
IF OBJECT_ID('MyInventory.MovementTypeCatalog', 'U') IS NOT NULL
	DROP TABLE MyInventory.MovementTypeCatalog;
GO

-- Create MovementType Catalog
CREATE TABLE MyInventory.MovementTypeCatalog (
	Id TINYINT IDENTITY(1,1) PRIMARY KEY,
	MovementType VARCHAR(20) UNIQUE NOT NULL 
);
GO