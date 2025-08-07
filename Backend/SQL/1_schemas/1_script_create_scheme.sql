/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Creates Schema for inventory system                        */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Create Schema if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'MyInventory')
BEGIN
	EXEC('CREATE SCHEMA MyInventory');
END
GO