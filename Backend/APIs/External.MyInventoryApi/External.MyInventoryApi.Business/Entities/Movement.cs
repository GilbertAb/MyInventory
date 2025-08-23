namespace External.MyInventoryApi.Business.Entities
{
    public class Movement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MovementTypeId { get; set; }
        public int MovementDate { get; set; }
        public int Quantity { get; set; }
        public string? MovementDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
