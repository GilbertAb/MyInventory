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
    }
}
