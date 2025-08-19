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

	-- Verify if the product exists
	IF NOT EXISTS (SELECT 1 FROM MyInventory.Product WHERE Id = @ProductId)
	BEGIN
		-- Empty data
		SELECT 
			NULL AS MovementId,
			NULL AS ProductId,
			NULL AS MovementTypeId,
			NULL AS MovementDate,
			NULL AS Quantity,
			NULL AS MovementDescription,
			NULL AS CreatedAt;
		-- Error data
		SET @ErrorCode = 50001;
		SET @ErrorMessage = 'Product not found';
	END
	-- Verify if a movement exists
	ELSE IF NOT EXISTS (SELECT 1 FROM MyInventory.Movement WHERE ProductId = @ProductId)
	BEGIN
		-- Empty data
		SELECT 
			NULL AS MovementId,
			NULL AS ProductId,
			NULL AS MovementTypeId,
			NULL AS MovementDate,
			NULL AS Quantity,
			NULL AS MovementDescription,
			NULL AS CreatedAt;
		-- Error data
		SET @ErrorCode = 50101;
		SET @ErrorMessage = 'No movements found';
	END
	ELSE
	BEGIN TRY
		-- Get movement (most recent to oldest)
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
		-- Empty data
		SELECT 
			NULL AS MovementId,
			NULL AS ProductId,
			NULL AS MovementTypeId,
			NULL AS MovementDate,
			NULL AS Quantity,
			NULL AS MovementDescription,
			NULL AS CreatedAt;
		-- Error data
		SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();
	END CATCH;

	-- Return error data
	SELECT
		@ErrorCode AS ErrorCode,
		@ErrorMessage AS ErrorMessage;
END;
GO