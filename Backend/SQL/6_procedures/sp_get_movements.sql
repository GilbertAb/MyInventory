/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Get all movements                                          */
/******************************************************************************/

USE MyInventoryDb;
GO

IF OBJECT_ID('MyInventory.usp_GetMovements', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_GetMovements;
GO

CREATE PROCEDURE MyInventory.usp_GetMovements
AS
BEGIN
	SELECT
		Id,
		ProductId,
		MovementTypeId,
		MovementDate,
		Quantity,
		MovementDescription,
		CreatedAt
	FROM MyInventory.Movement;
END;
GO
