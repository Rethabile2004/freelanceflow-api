namespace FreelanceFlow.API.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        // Every Invoice belongs to exactly one Project.
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        // An Invoice can have many Payments (clients pay in installments).
        public ICollection<Payment> Payments { get; set; } = [];
    }
}