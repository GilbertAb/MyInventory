/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Get all movement types                                     */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop stored procedure if it already exists
IF OBJECT_ID('MyInventory.usp_GetMovementTypes', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_GetMovementTypes;
GO

CREATE PROCEDURE MyInventory.usp_GetMovementTypes
AS
BEGIN
	SELECT
		Id,
		MovementType
	FROM MyInventory.MovementTypeCatalog;
END;
GO