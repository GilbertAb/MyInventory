/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Creates table of movements for inventory system            */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop table if it already exists
IF OBJECT_ID('MyInventory.Movement', 'U') IS NOT NULL
	DROP TABLE MyInventory.Movement;
GO

-- Create Table Movement
CREATE TABLE MyInventory.Movement (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ProductId INT NOT NULL,
	MovementTypeId TINYINT NOT NULL,
	MovementDate DATETIME2 DEFAULT GETDATE(),
	Quantity INT NOT NULL Check(Quantity > 0),
	MovementDescription VARCHAR(100),
	CreatedAt DATETIME2 DEFAULT GETDATE(),
	CONSTRAINT FK_MovementProduct
		FOREIGN KEY (ProductId) REFERENCES MyInventory.Product(Id),
	CONSTRAINT FK_MovementType
		FOREIGN KEY (MovementTypeId) REFERENCES MyInventory.MovementTypeCatalog(Id)
);
GO