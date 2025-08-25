namespace External.MyInventoryApi.DataAccess.Contracts.InputModels
{
    public class RegisterProductMovementInputModel
    {
        public int ProductId { get; set; }
        public byte MovementTypeId { get; set; }
        public int Quantity { get; set; }
        public string MovementDescription { get; set; } = string.Empty;
    }
}
