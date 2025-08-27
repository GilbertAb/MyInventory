using External.MyInventoryApi.Business.Entities;
using System.Data;

namespace External.MyInventoryApi.DataAccess.Mappers
{
    internal class MovementStoredProcedureMapper
    {
        public static readonly Func<DataSet, int?> MapRegisterMovement = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["NewMovementId"]);
            }
            return null;
        };
        
        public static readonly Func<DataSet, IEnumerable<Movement>?> MapGetProductStockHistory = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var movements = new List<Movement>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    movements.Add
                    (
                        new Movement
                        {
                            Id = Convert.ToInt32(row["MovementId"]),
                            ProductId = Convert.ToInt32(row["ProductID"]),
                            MovementTypeId = Convert.ToInt32(row["MovementTypeId"]),
                            MovementDate = (DateTime)row["MovementDate"],
                            Quantity = Convert.ToInt32(row["Quantity"]),
                            MovementDescription = row["MovementDescription"].ToString(),
                            CreatedAt = (DateTime)row["CreatedAt"],
                        }
                    );
                }
                return movements;
            }
            return null;
        };
    }
}
