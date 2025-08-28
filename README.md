# MyInventory

MyInventory is a backend project designed to manage inventory data.  
It provides a REST API built with **.NET 8 (C#)** and a **SQL Server** database for data storage and operations.

---

## Project Structure

```
MyInventoryApi/
├── Backend/
│   ├── APIs/
│   │   └── External.MyInventoryApi   # ASP.NET Core Web API
│   └── SQL/                          # Database scripts
│   └── Requests/                          # Sample requests
└── README.md
```
MyInventoryApi/

|-- Backend/
  |-- APIs/ 
     |-- External.MyInventoryApi # ASP.NET Core Web API
  |-- SQL/ # Database scripts (tables, stored procedures, seed data, etc.)
|
|-- Frontend/ # (reserved, not implemented yet)
|
|-- README.md

---

## Features

- REST API for managing products and inventory.
- SQL Server database with stored procedures for core operations.
- Layered architecture for maintainability.
- Ready to extend with additional modules and features.

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- [Postman](https://www.postman.com/) (optional, for testing endpoints)

---

## Database

### Scripts
- 1_script_create_database_my_inventory

- schemas
  - 1_script_create_scheme
- Tables
  - 1_script_create_catalog_movement_type
  - 2_script_create_table_product
  - 3_script_create_table_movement
- Seeds
  - 1_seed_movement_types
- Functions
  - function_is_exit_movement
- Triggers
  - 1_trigger_update_product_updated_at

### Stored procedures

- Procedures
  - sp_AddProduct
  - sp_DeleteProduct
  - sp_GetProducts
  - sp_GetProductById
  - sp_UpdateProduct
  - sp_GetMovementTypes
  - sp_GetMovements
  - sp_RegisterProductMovement
  - sp_GetProductStockHistory
  - sp_GetProductStockSummary
---
## API Endpoints

### Product

- **GET** `/api/product/getProductStockHistory/{productId:int}` → Returns product stock movement history.

- **GET** `/api/product/getProductStockSummary/{productId:int}` → Returns stock summary of a product.

- **POST** `/api/product/addProduct` → Adds a new product.

- **PUT** `/api/product/updateProduct` → Updates an existing product.

### Movement

- **GET** `/api/movement/getMovements` → Returns all movements.

- **POST** `/api/movement/registerMovement` → Adds a new product movement.

Additional endpoints can be found in the Postman collection `MyInventory/Backend/Requests`.
    
---

## Database Setup

1. Open the `Backend/SQL` folder.  
2. Run the database scripts in your SQL Server instance.  
   - 1_script_create_database_my_inventory.sql  
   - 1_schemas/  
   - 2_tables/
   - 3_seeds/
   - 4_functions/
   - 5_triggers/
   - 6_procedures/
   
Ensure your connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=MyInventoryDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
}
```

---


## API Configuration

The application uses an `appsettings.json` file for configuration.  


### Example `appsettings.json`

```json
{
    "ConnectionStrings": {
        "SqlServer": "<AES_ENCRYPTED_STRING>"
  },
  "Crypto": {
      "KEY": "<AES_KEY>",
    "IV": "<AES_IV>"
  },
}

See full template in appsettings.json for stored procedures and logging configuration.

```
The **connection string is AES encrypted**, and the API decrypts it at runtime using the provided `KEY` and `IV`.

### AES Encryption Details

- CBC Mode.
- Key size: 256 bits.
- Initialization Vector size: 128 bits.

---

## Build and Run

```bash
# Navigate to API folder
cd Backend/APIs/MyInventoryApi

# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

---

## Testing the API

### Using Postman

Import the provided Postman collection `MyInventory/Backend/Requests/MyInventoryApi.postman_collection.json`

Run the requests against your local or deployed API.

API will be available at:
https://localhost:53987 or http://localhost:53988

---


## License

This project is licensed under the MIT License.