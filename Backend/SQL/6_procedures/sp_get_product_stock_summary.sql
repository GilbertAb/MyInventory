/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 18 - 08 - 2025                                             */
/* Purpose       : Get a summary of a product stock transactions              */
/******************************************************************************/

USE MyInventoryDb;
GO

IF OBJECT_ID('MyInventory.usp_GetProductStockSummary', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_GetProductStockSummary;
GO

CREATE PROCEDURE MyInventory.usp_GetProductStockSummary
	@ProductId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE 
		@ProductName VARCHAR(100),
		@Stock INT,
		@NumberOfMovements INT,
		@NumberOfEntries INT = 0,
		@NumberOfExits INT = 0,
		@LastMovementDate DATETIME,
		@ErrorCode INT = 0,
		@ErrorMessage NVARCHAR(200) = 'OK';
	
	-- Verify if the product exists
	IF NOT EXISTS (SELECT 1 FROM MyInventory.Product WHERE Id = @ProductId)
	BEGIN
		SET @ErrorCode = 50001;
		SET @ErrorMessage = 'Product not found';
	END

	IF @ErrorCode = 0
	BEGIN TRY
		-- Get number of entries, number of exits and last movement date
		SELECT
			@NumberOfEntries = SUM(CASE WHEN MTC.MovementType = 'Entry' THEN 1 ELSE 0 END),
			@NumberOfExits = SUM(CASE WHEN MTC.MovementType = 'Exit' THEN 1 ELSE 0 END),
			@LastMovementDate = MAX(M.MovementDate)
		FROM MyInventory.Movement AS M
		INNER JOIN MyInventory.MovementTypeCatalog AS MTC
		ON M.MovementTypeId = MTC.Id
		WHERE M.ProductId = @ProductId;
		
		-- Number of movements
		SET @NumberOfMovements = @NumberOfEntries + @NumberOfExits;
		
		-- Get name and stock if there are movements
		IF @NumberOfMovements > 0
			SELECT @ProductName = ProductName, @Stock = Stock
			FROM MyInventory.Product
			WHERE Id = @ProductId;
		ELSE
			SET @ErrorCode = 50101;
			SET @ErrorMessage = 'No movements found';
	END TRY
	BEGIN CATCH
		-- Error data
		SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();
	END CATCH

	-- Return data
	SELECT 
		@ProductName AS ProductName,
		@Stock AS Stock,
		@NumberOfMovements AS NumberOfMovements,
		@NumberOfEntries AS NumberOfEntries,
		@NumberOfExits AS NumberOfExits,
		@LastMovementDate AS LastMovement;
	
	-- Return error data
	SELECT
		@ErrorCode AS ErrorCode,
		@ErrorMessage AS ErrorMessage;
END;
GO