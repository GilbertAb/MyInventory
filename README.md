# MyInventory

Inventory management REST API built with **.NET 8** and **SQL Server**.

---

## Highlights

- ASP.NET Core Web API (.NET 8)
- SQL Server + Stored Procedures
- Layered Architecture
- AES-encrypted database connection strings
- Unit Testing (xUnit, Moq, FluentAssertions)
- Code Coverage Reporting
- GitHub Actions CI Pipeline
- Health Checks
- GitHub Issue & Pull Request Templates

---

## Project Structure

```text
MyInventory/
├── Backend/
│   ├── APIs/
│   │   ├── External.MyInventoryApi
│   │   ├── External.MyInventoryApi.Application
│   │   ├── External.MyInventoryApi.Business
│   │   ├── External.MyInventoryApi.DataAccess
│   │   ├── External.MyInventoryApi.CrossCutting
│   │   └── External.MyInventoryApi.Tests
│   ├── SQL/
│   └── Requests/
└── README.md
```

---

## Architecture

The solution follows a layered architecture to separate responsibilities and improve maintainability.

```text
External.MyInventoryApi
│
├── Api
|   └── Controllers
├── Application
│   ├── Services
│   └── Mappers
├── Business
├── DataAccess
│   ├── Repositories
│   ├── SqlServer
│   └── Mappers
├── CrossCutting
└── Tests
```

### Layers

- **Api**: Exposes HTTP endpoints.
- **Application**: Business use cases, services, and DTO mappings.
- **Business**: Core domain entities.
- **DataAccess**: Repositories and SQL Server integration.
- **CrossCutting**: Shared components such as middleware, encryption, and health checks.
- **Tests**: Contains automated unit tests.

---

## Features

- Product management.
- Inventory movement tracking.
- Product stock history.
- Product stock summary.
- SQL Server database with stored procedures.
- Health checks for SQL Server connectivity.
- AES-encrypted database connection strings.
- Automated unit testing.
- Continuous Integration with GitHub Actions.

---

## Requirements

- .NET 8 SDK
- SQL Server
- Postman (optional)

---

## Database

### Scripts

- `1_script_create_database_my_inventory`

#### Schemas

- `1_script_create_scheme`

#### Tables

- `1_script_create_catalog_movement_type`
- `2_script_create_table_product`
- `3_script_create_table_movement`

#### Seeds

- `1_seed_movement_types`

#### Functions

- `function_is_exit_movement`

#### Triggers

- `1_trigger_update_product_updated_at`

### Stored Procedures

#### Product

- `sp_AddProduct`
- `sp_DeleteProduct`
- `sp_GetProducts`
- `sp_GetProductById`
- `sp_UpdateProduct`
- `sp_GetProductStockSummary`

#### Movement

- `sp_GetMovementTypes`
- `sp_GetMovements`
- `sp_RegisterProductMovement`
- `sp_GetProductStockHistory`

---

## API Endpoints

### Health & Monitoring

#### Health Check

```http
GET /health
```

Verifies application and SQL Server connectivity status.

#### Metrics

```http
GET /metrics
```

Exposes application metrics for monitoring and observability.

---

### Product Management

#### Get All Products

```http
GET /api/product/getProducts
```

Returns all registered products.

#### Get Product By Id

```http
GET /api/product/getProduct/{productId}
```

Returns detailed information for a specific product.

#### Add Product

```http
POST /api/product/addProduct
```

Creates a new product.

**Request Body**

```json
{
  "ProductName": "Laptop",
  "Category": "Technology",
  "Stock": 10
}
```

#### Update Product

```http
PUT /api/product/updateProduct
```

Updates an existing product.

**Request Body**

```json
{
  "ProductId": 13,
  "ProductName": "Laptop",
  "Category": "Technology",
  "Stock": 15
}
```

#### Delete Product

```http
DELETE /api/product/deleteProduct/{productId}
```

Deletes an existing product.

#### Get Product Stock Summary

```http
GET /api/product/getProductStockSummary/{productId}
```

Returns stock summary information for a product, including current inventory levels and movement statistics.

---

### Inventory Movements

#### Get All Movements

```http
GET /api/movement/getMovements
```

Returns all inventory movements.

#### Register Movement

```http
POST /api/movement/registerMovement
```

Registers a new inventory movement.

**Request Body**

```json
{
  "ProductId": 21,
  "MovementTypeId": 1,
  "Quantity": 5,
  "MovementDescription": "More units supplied"
}
```

#### Get Product Stock History

```http
GET /api/movement/getProductStockHistory/{productId}
```

Returns the complete movement history for a product.

---

### Catalogs

#### Get Movement Types

```http
GET /api/catalog/getMovementTypes
```

Returns all available movement types used by the inventory system.

---

Additional request examples are available in the Postman collection located under:

```text
Backend/Requests
```

---

## Database Setup

1. Open the `Backend/SQL` folder.
2. Execute the scripts in the following order:

```text
1_script_create_database_my_inventory.sql

1_schemas/
2_tables/
3_seeds/
4_functions/
5_triggers/
6_procedures/
```

3. Configure the connection string in `appsettings.json`.

Example:

```json
{
  "ConnectionStrings": {
    "SqlServer": "<AES_ENCRYPTED_CONNECTION_STRING>"
  }
}
```

---

## API Configuration

The application uses `appsettings.json` for configuration.

### Example

```json
{
  "ConnectionStrings": {
    "SqlServer": "<AES_ENCRYPTED_CONNECTION_STRING>"
  },
  "Crypto": {
    "KEY": "<AES_KEY>",
    "IV": "<AES_IV>"
  }
}
```

The SQL Server connection string is encrypted using AES and decrypted at runtime.

### AES Encryption

- Algorithm: AES
- Mode: CBC
- Key Size: 256 bits
- IV Size: 128 bits
- Padding: PKCS7

---

## Health Checks

The API exposes health checks to verify SQL Server connectivity.

Example:

```http
GET /health
```

---

## Testing

The project includes automated unit tests covering:

- Services
- Repositories
- Middleware
- Health Checks
- Crypto components

### Testing Stack

- xUnit
- Moq
- FluentAssertions

### Run Tests

```bash
dotnet test
```

---

## Code Coverage

Code coverage is generated automatically during CI execution.

Run locally:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Coverage reports are published as GitHub Actions artifacts.

---

## Continuous Integration

The project uses GitHub Actions to validate every Pull Request targeting `develop`.

### Pipeline Steps

1. Restore dependencies
2. Build solution
3. Execute unit tests
4. Generate code coverage reports

Pull requests cannot be merged if the pipeline fails.

---

## Development Workflow

### Branch Strategy

```text
feature/*
    ↓
develop
    ↓
main
```

### Pull Requests

- Every feature is developed in its own branch.
- Pull Requests target `develop`.
- CI validation is required before merging.

### Repository Templates

The repository includes:

- Feature Request template
- Pull Request template

---

## Build and Run

```bash
# Navigate to API folder
cd Backend/APIs/External.MyInventoryApi

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run API
dotnet run
```

---

## Testing the API

### Using Postman

Import the collection located in:

```text
Backend/Requests
```

Run the requests against your local API instance.

Default URLs:

```text
https://localhost:53987
http://localhost:53988
```

---

## Roadmap

Planned improvements:

- Docker support
- Docker image publishing
- SonarCloud integration
- Quality Gates
- Automated deployments

---

## License

This project is licensed under the MIT License.