namespace External.MyInventoryApi.Requests.Product
{
    public class UpdateProductRequest
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
