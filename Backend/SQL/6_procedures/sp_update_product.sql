/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 17 - 08 - 2025                                             */
/* Purpose       : Update a product of the inventory                          */
/******************************************************************************/

USE MyInventoryDb;
GO

IF OBJECT_ID('MyInventory.usp_UpdateProduct', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_UpdateProduct;
GO

CREATE PROCEDURE MyInventory.usp_UpdateProduct
	@ProductId INT,
	@ProductName VARCHAR(100) = NULL,
	@Category VARCHAR(50) = NULL,
	@Stock INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Errors
    DECLARE 
        @ErrorCode INT = 0,
        @ErrorMessage NVARCHAR(200) = 'OK';
	
	-- Verify if the product exists
	IF NOT EXISTS(SELECT 1 FROM MyInventory.Product WHERE Id = @ProductId)
	BEGIN
        SET @ErrorCode = 50001;
        SET @ErrorMessage = 'Product not found';
    END

	-- Check if Stock is valid
	IF @Stock IS NOT NULL AND @Stock < 0
	BEGIN
        SET @ErrorCode = 50003;
        SET @ErrorMessage = 'Product''s stock can''t be negative';
    END
	
	IF @ErrorCode <> 0
	BEGIN
		SELECT NULL AS ProductId;
		SELECT
			@ErrorCode AS ErrorCode,
			@ErrorMessage AS ErrorMessage;
		RETURN;
	END;

	-- Begin Transaction
	BEGIN TRY
		BEGIN TRANSACTION

			-- Update the product
			UPDATE MyInventory.Product
			SET
				ProductName = ISNULL(@ProductName, ProductName),
				Category = ISNULL(@Category, Category),
				Stock = ISNULL(@Stock, Stock)
			WHERE Id = @ProductId
			
			SELECT
				@ProductId AS ProductId;

			COMMIT;
	END TRY
	BEGIN CATCH
		-- Empty data
		SELECT NULL AS ProductId;
		
		-- Rollback
		ROLLBACK TRANSACTION;
		
		-- Error data
		SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();
	END CATCH

	-- Return error data
	SELECT
		@ErrorCode AS ErrorCode,
		@ErrorMessage AS ErrorMessage;
END;
GO