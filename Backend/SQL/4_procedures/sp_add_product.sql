/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 17 - 08 - 2025                                             */
/* Purpose       : Add a product to the inventory                             */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop stored procedure if it already exists
IF OBJECT_ID('MyInventory.usp_AddProduct', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_AddProduct;
GO

CREATE PROCEDURE MyInventory.usp_AddProduct
	@ProductName VARCHAR(100),
	@Category VARCHAR(20),
	@Stock INT
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Errors
    DECLARE
		@NewProductId INT = NULL,
        @ErrorCode INT = 0,
        @ErrorMessage NVARCHAR(200) = 'OK';

	BEGIN TRY
		-- Verify if a product with the same name exists
		IF EXISTS (SELECT 1 FROM MyInventory.Product WHERE ProductName = @ProductName)
		BEGIN
            SET @ErrorCode = 50002;
            SET @ErrorMessage = 'Product already exists';
        END

		-- Insert product
		IF @ErrorCode = 0
		INSERT INTO MyInventory.Product (ProductName, Category, Stock)
		VALUES (
			@ProductName,
			@Category,
			@Stock
		);

		SET @NewProductId = SCOPE_IDENTITY();
		
		-- Return new Product Id
		SELECT
			@NewProductId AS NewProductId;
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