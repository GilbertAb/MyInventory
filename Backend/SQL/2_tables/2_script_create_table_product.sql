/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Creates table of products for inventory system             */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop table if it already exists
IF OBJECT_ID('MyInventory.Product', 'U') IS NOT NULL
	DROP TABLE MyInventory.Product;
GO

-- Create Table Product
CREATE TABLE MyInventory.Product (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ProductName VARCHAR(100) NOT NULL,
	Category VARCHAR(50),
	Stock INT NOT NULL DEFAULT 0 CHECK (Stock >= 0),
	CreatedAt DATETIME2 DEFAULT GETDATE(),
	UpdatedAt DATETIME2
);
GO
