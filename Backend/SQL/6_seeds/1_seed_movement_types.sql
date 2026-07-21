/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Insert movement types to the catalog for inventory system  */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Insert movement types
INSERT INTO MyInventory.MovementTypeCatalog (MovementType)
VALUES ('Entry'), ('Exit');
GO
