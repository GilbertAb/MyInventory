/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Márquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Checks if movement type id corresponds to an exit movement */
/******************************************************************************/

USE MyInventoryDb
GO
-- Drop stored procedure if it already exists
IF OBJECT_ID('MyInventory.function_IsExitMovement', 'FN') IS NOT NULL
	DROP FUNCTION MyInventory.function_IsExitMovement;
GO

CREATE FUNCTION MyInventory.function_IsExitMovement(@MovementTypeId INT)
RETURNS BIT
AS
BEGIN
	DECLARE @IsExit BIT = 0;

	IF EXISTS(
		SELECT 1
		FROM MyInventory.MovementTypeCatalog
		WHERE ID = @MovementTypeId AND MovementType = 'Exit'
	)
		SET @IsExit = 1;
	RETURN @IsExit
END;
GO