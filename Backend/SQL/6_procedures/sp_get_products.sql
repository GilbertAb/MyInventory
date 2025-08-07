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
	SELECT 
		Id,
		ProductName,
		Category,
		Stock,
		CreatedAt,
		UpdatedAt
	FROM Product;
END;
GO