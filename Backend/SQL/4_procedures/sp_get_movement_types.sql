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
	SET NOCOUNT ON;

	BEGIN TRY
		-- Get Movement Types data
		SELECT
			Id,
			MovementType
		FROM MyInventory.MovementTypeCatalog;

		-- Succeed data
		SELECT 
			0 AS ErrorCode,
			'OK' AS ErrorMessage;
	END TRY
	BEGIN CATCH
		SELECT
			ERROR_NUMBER() AS ErrorCode,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
GO