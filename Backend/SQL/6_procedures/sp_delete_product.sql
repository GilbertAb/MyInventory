USE MyInventoryDb;
GO

IF OBJECT_ID('MyInventory.usp_DeleteProduct', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_DeleteProduct;
GO

CREATE PROCEDURE MyInventory.usp_DeleteProduct
	@ProductId INT
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

	-- Verify if it has related movements
	IF EXISTS (
		SELECT 1 
		FROM MyInventory.Movement
		WHERE ProductId = @ProductId
	)
	BEGIN
        SET @ErrorCode = 50004;
        SET @ErrorMessage = 'Can''t delete a product with associated movements';
    END

	IF @ErrorCode <> 0
	BEGIN
		SELECT NULL AS ProductId;
		SELECT
			@ErrorCode AS ErrorCode,
			@ErrorMessage AS ErrorMessage;
		RETURN;
	END;

	-- Begin transaction
	BEGIN TRY
		BEGIN TRANSACTION
			-- Delete product
			DELETE FROM MyInventory.Product
			WHERE Id = @ProductId
			
			-- Return product id
			SELECT
				@ProductId AS ProductId;
			
			COMMIT TRANSACTION;
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