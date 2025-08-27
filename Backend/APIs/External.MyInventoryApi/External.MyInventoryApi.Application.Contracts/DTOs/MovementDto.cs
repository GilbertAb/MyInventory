namespace External.MyInventoryApi.Application.Contracts.DTOs
{
    public class MovementDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MovementTypeId { get; set; }
        public DateTime MovementDate { get; set; }
        public int Quantity { get; set; }
        public string? MovementDescription { get; set; } = string.Empty;
    }
}
