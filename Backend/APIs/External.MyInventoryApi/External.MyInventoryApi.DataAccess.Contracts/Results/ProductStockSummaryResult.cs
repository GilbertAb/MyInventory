namespace External.MyInventoryApi.DataAccess.Contracts.Results
{
    public class ProductStockSummaryResult
    {
        public string ProductName { get; set; } = string.Empty;
        public int Stock {  get; set; }
        public int NumberOfMovements { get; set; }
        public int NumberOfEntries { get; set; }
        public int NumberOfExits { get; set; }
        public DateTime LastMovementDate { get; set; }
    }
}
