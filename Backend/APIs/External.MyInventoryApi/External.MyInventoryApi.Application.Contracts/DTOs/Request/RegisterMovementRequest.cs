namespace External.MyInventoryApi.Application.Contracts.DTOs.Request
{
    public class RegisterMovementRequest
    {
        public int ProductId { get; set; }
        public byte MovementTypeId { get; set; }
        public int Quantity { get; set; }
        public string MovementDescription { get; set; } = string.Empty;
    }
}
