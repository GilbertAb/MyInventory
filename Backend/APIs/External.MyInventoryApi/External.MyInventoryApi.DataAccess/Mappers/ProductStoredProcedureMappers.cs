using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Results;
using System.Data;

namespace External.MyInventoryApi.DataAccess.Mappers
{
    public static class ProductStoredProcedureMappers
    {
        public static readonly Func<DataSet, int?> MapAddProduct = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["NewProductId"]);
            }
            return null;
        };

        public static readonly Func<DataSet, int?> MapDeleteProduct = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["ProductId"]);
            }
            return null;
        };

        public static readonly Func<DataSet, int?> MapUpdateProduct = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["ProductId"]);
            }
            return null;
        };

        public static readonly Func<DataSet, IEnumerable<Product>?> MapProducts = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var products = new List<Product>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    products.Add
                    (
                        new Product
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            ProductName = row["ProductName"].ToString()!,
                            Category = row["Category"].ToString(),
                            Stock = Convert.ToInt32(row["Stock"]),
                            CreatedAt = (DateTime)row["CreatedAt"],
                            UpdatedAt = row["UpdatedAt"] == DBNull.Value ? null : (DateTime?)row["UpdatedAt"]
                        }
                    );
                }
                return products;
            }
            return null;
        };

        public static readonly Func<DataSet, Product?> MapGetProduct = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                Product product = new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ProductName = row["ProductName"].ToString()!,
                    Category = row["Category"].ToString(),
                    Stock = Convert.ToInt32(row["Stock"]),
                    CreatedAt = (DateTime)row["CreatedAt"],
                    UpdatedAt = row["UpdatedAt"] == DBNull.Value ? null : (DateTime?)row["UpdatedAt"]
                };

                return product;
            }
            return null;
        };

        public static readonly Func<DataSet, ProductStockSummaryResult?> MapGetProductStockSummary = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                ProductStockSummaryResult result = new ProductStockSummaryResult
                {
                    ProductName = row["ProductName"].ToString()!,
                    Stock = Convert.ToInt32(row["Stock"]),
                    NumberOfMovements = Convert.ToInt32(row["NumberOfMovements"]),
                    NumberOfEntries = Convert.ToInt32(row["NumberOfEntries"]),
                    NumberOfExits = Convert.ToInt32(row["NumberOfExits"]),
                    LastMovementDate = (DateTime)row["LastMovement"],
                };

                return result;
            }
            return null;
        };
    }
}
