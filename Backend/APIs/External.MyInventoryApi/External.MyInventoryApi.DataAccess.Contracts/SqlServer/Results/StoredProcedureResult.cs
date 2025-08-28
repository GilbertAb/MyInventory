namespace External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results
{
    public class StoredProcedureResult<T>
    {
        public T? Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
