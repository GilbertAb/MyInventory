/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 07 - 08 - 2025                                             */
/* Purpose       : Get all products                                           */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop stored procedure if it already exists
IF OBJECT_ID('MyInventory.usp_GetProducts', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_GetProducts;
GO

CREATE PROCEDURE MyInventory.usp_GetProducts
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		-- Get Product Data
		SELECT 
			Id,
			ProductName,
			Category,
			Stock,
			CreatedAt,
			UpdatedAt
		FROM MyInventory.Product;
		
		-- Succeed data
		SELECT 
			0 AS ErrorCode,
			'OK' AS ErrorMessage;
	END TRY
	BEGIN CATCH
		SELECT
			ERROR_NUMBER() AS ErrorCode,
			ERROR_MESSAGE() AS ErrorMessage
	END CATCH;
END;
GO