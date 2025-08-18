/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 18 - 08 - 2025                                             */
/* Purpose       : Get the movements of a product                             */
/******************************************************************************/

USE MyInventoryDb;
GO

IF OBJECT_ID('MyInventory.usp_GetProductStockHistory') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_GetProductStockHistory;
GO

CREATE PROCEDURE MyInventory.usp_GetProductStockHistory
	@ProductId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Error data
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
		
		-- Get movement (most recent to oldest)
		IF @ErrorCode = 0
		SELECT 
			Id AS MovementId,
			ProductId,
			MovementTypeId,
			MovementDate,
			Quantity,
			MovementDescription,
			CreatedAt
		FROM MyInventory.Movement
		WHERE ProductId = @ProductId
		ORDER BY MovementDate DESC;
	END TRY
	BEGIN CATCH
		SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();
	END CATCH;

	-- Return error data
	SELECT
		@ErrorCode AS ErrorCode,
		@ErrorMessage AS ErrorMessage;
END;
GO