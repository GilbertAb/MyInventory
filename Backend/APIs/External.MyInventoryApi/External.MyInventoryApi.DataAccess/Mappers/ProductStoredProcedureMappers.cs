using External.MyInventoryApi.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.DataAccess.Mappers
{
    public static class ProductStoredProcedureMappers
    {
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
