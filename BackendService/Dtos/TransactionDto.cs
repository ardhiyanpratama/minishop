namespace BackendService.Dtos
{
    public class TransactionDto
    {
        public string? TransactionNo { get; set; }
        public double? TotalAmount { get; set; }
        public Guid? MsProductVariantId { get; set; }
        public double? Price { get; set; }
        public double? Qty { get; set; }
        public double? SubTotal { get; set; }
    }
}
