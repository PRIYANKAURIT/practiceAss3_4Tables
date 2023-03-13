namespace practiceAss3_4Tables.Model
{
    public class OrderModel : BaseModel
    {
        public int Id { get; set; }
        public int OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string? SubTotal { get; set; }
        public string? TotalDiscount { get; set; }
        public string? GrandTotal { get; set; }
        public string? Remark { get; set; }
        public string? BillingAddress { get; set; }
        public string? ShippingAddress { get; set; }
        public List<OrderDetailsModel> orderDetails { get; set; }
    }
}
