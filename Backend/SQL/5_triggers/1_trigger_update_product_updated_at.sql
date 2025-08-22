/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Creates Trigger After Update Product UpdatedAt             */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Update trigger if it already exists
IF OBJECT_ID('MyInventory.trigger_UpdateProductUpdatedAt', 'TR') IS NOT NULL
	DROP TRIGGER MyInventory.trigger_UpdateProductUpdatedAt;
GO

-- Create trigger to update UpdatedAt AFTER UPDATE
CREATE TRIGGER MyInventory.trigger_UpdateProductUpdatedAt
ON MyInventory.Product
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON
	UPDATE MyInventory.Product
	SET UpdatedAt = GETDATE()
	FROM MyInventory.Product P
	JOIN inserted I ON P.Id = I.Id
END;
GO