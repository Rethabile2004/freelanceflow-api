namespace FreelanceFlow.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Reference { get; set; }

        // Every Payment belongs to exactly one Invoice.
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}