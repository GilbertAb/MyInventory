/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 16 - 08 - 2025                                             */
/* Purpose       : Get product by id                                          */
/******************************************************************************/

USE MyInventoryDb;
GO

IF OBJECT_ID('MyInventory.usp_GetProductById') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_GetProductById;
GO

CREATE PROCEDURE MyInventory.usp_GetProductById
	@ProductId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Errors
    DECLARE 
        @ErrorCode INT = 0,
        @ErrorMessage NVARCHAR(200) = 'OK';

	BEGIN TRY
		-- Verify if the product exists
		IF NOT EXISTS (SELECT 1 FROM MyInventory.Product WHERE Id = @ProductId)
		BEGIN
            SET @ErrorCode = 50001;
            SET @ErrorMessage = 'Product not found';
        END

		-- Get product data
		IF @ErrorCode = 0
		SELECT 
			Id,
			ProductName,
			Category,
			Stock,
			CreatedAt,
			UpdatedAt
		FROM MyInventory.Product
		WHERE Id = @ProductId;
	END TRY
	BEGIN CATCH
		SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();
	END CATCH

	-- Return error data
	SELECT
		@ErrorCode AS ErrorCode,
		@ErrorMessage AS ErrorMessage;
END;
GO