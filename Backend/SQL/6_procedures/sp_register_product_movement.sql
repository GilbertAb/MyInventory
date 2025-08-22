/******************************************************************************/
/* Database      : MyInventory                                                */
/* Author        : Gilbert Marquez                                            */
/* Created on    : 18 - 08 - 2025                                             */
/* Purpose       : Adds a record of a product movement to or from inventory   */
/******************************************************************************/

USE MyInventoryDb;
GO

-- Drop stored procedure if it already exists
IF OBJECT_ID('MyInventory.usp_RegisterProductMovement', 'P') IS NOT NULL
	DROP PROCEDURE MyInventory.usp_RegisterProductMovement;
GO

CREATE PROCEDURE MyInventory.usp_RegisterProductMovement
	@ProductId INT,
	@MovementTypeId TINYINT,
	@Quantity INT,
	@MovementDescription VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	-- Declare return data and errors
    DECLARE
		@NewMovementId INT = NULL,
        @ErrorCode INT = 0,
        @ErrorMessage NVARCHAR(200) = 'OK';

	-- Validate if the product exists
	IF NOT EXISTS (SELECT 1 FROM MyInventory.Product WHERE Id = @ProductId)
	BEGIN
        SET @ErrorCode = 50001;
        SET @ErrorMessage = 'Product not found';
    END

	-- Validate if the movement type exists
	IF NOT EXISTS (SELECT 1 FROM MyInventory.MovementTypeCatalog WHERE Id = @MovementTypeId)
	BEGIN
        SET @ErrorCode = 50201;
        SET @ErrorMessage = 'MovementType not found';
    END

	IF @ErrorCode <> 0
	BEGIN
		SELECT NULL AS NewMovementId;
		SELECT
			@ErrorCode AS ErrorCode,
			@ErrorMessage AS ErrorMessage;
		RETURN;
	END;

	-- Start registration
	BEGIN TRY
		BEGIN TRANSACTION;
			DECLARE @IsExit BIT = MyInventory.function_IsExitMovement(@MovementTypeId)

			IF @IsExit = 1
			BEGIN
				-- Exit Movement
				DECLARE @CurrentStock INT;

				-- Get current stock
				SELECT @CurrentStock = Stock
				FROM MyInventory.Product
				WHERE Id = @ProductId;

				-- Check if there is enough stock
				IF @CurrentStock < @Quantity
				BEGIN
					SET @ErrorCode = 50100;
					SET @ErrorMessage = 'Not enough stock to move';
					
					ROLLBACK TRANSACTION;
					
					-- Empty data
					SELECT NULL AS NewMovementId;
					-- Error data
					SELECT
						@ErrorCode AS ErrorCode,
						@ErrorMessage AS ErrorMessage
					RETURN;
				END

				-- Update stock of the product
				UPDATE MyInventory.Product SET Stock = Stock - @Quantity WHERE Id = @ProductId
			END
			ELSE
			BEGIN
				-- Entry Movement
				UPDATE MyInventory.Product SET Stock = Stock + @Quantity WHERE Id = @ProductId
			END
			
			-- Register Movement
			INSERT INTO MyInventory.Movement (
				ProductId,
				MovementTypeId,
				MovementDate,
				Quantity,
				MovementDescription
			)
			VALUES (
				@ProductId,
				@MovementTypeId,
				GETDATE(),
				@Quantity,
				@MovementDescription
			);

			SET @NewMovementId = SCOPE_IDENTITY();
			
			-- Return Movement id
			SELECT
				@NewMovementId AS NewMovementId;
			
			COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		-- Empty data
		SELECT NULL AS NewMovementId;
		
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
