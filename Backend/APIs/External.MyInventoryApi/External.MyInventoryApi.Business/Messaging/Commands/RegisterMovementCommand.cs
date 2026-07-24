namespace External.MyInventoryApi.Business.Messaging.Commands
{
    public sealed record RegisterMovementCommand
    {
        public int ProductId { get; set; }
        public byte MovementTypeId { get; set; }
        public int Quantity { get; set; }
        public string MovementDescription { get; set; } = string.Empty;
    }
}
