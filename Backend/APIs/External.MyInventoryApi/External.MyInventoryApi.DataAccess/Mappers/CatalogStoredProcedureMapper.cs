using External.MyInventoryApi.Business.Catalogs;
using System.Data;

namespace External.MyInventoryApi.DataAccess.Mappers
{
    // Map sp DataSet results to entities
    public class CatalogStoredProcedureMapper
    {
        public static readonly Func<DataSet, IEnumerable<MovementType>?> MapGetMovementTypes = ds =>
        {
            if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var movementTypes = new List<MovementType>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    movementTypes.Add
                    (
                        new MovementType
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Type = row["MovementType"].ToString()!
                        }
                    );
                }
                return movementTypes;
            }
            return null;
        };
    }
}
