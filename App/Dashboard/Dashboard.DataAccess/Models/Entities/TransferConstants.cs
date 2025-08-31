namespace Dashboard.DataAccess.Models.Entities;

public static class TransferStatus
{
    public const string PENDING = "PENDING";
    public const string APPROVED = "APPROVED";
    public const string REJECTED = "REJECTED";
    public const string COMPLETED = "COMPLETED";
    public const string CANCELLED = "CANCELLED";
    public const string TRANSFERRED = "TRANSFERRED";
}

public static class TransferType
{
    public const string OUT = "OUT";  // Từ warehouse đến branch
    public const string IN = "IN";    // Từ branch về warehouse
    public const string RETURN = "RETURN"; // Trả lại nguyên liệu
}

public static class InventoryMovementType
{
    public const string TRANSFER_IN = "TRANSFER_IN";
    public const string TRANSFER_OUT = "TRANSFER_OUT";
    public const string PRODUCTION_USE = "PRODUCTION_USE";
    public const string WASTE = "WASTE";
    public const string ADJUSTMENT = "ADJUSTMENT";
    public const string RETURN = "RETURN";
}
