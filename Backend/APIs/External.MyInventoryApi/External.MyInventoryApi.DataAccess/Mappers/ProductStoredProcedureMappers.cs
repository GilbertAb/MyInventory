using External.MyInventoryApi.Business.Entities;
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
    }
}
