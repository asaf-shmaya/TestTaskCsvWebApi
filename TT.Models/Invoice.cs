namespace TT.Models
{
    public class Invoice
    {
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
        public int Number { get; set; }
        public Enums.Processing.Statuses ProcessingStatus { get; set; }
        public double Amount { get; set; }
        public Enums.Payment.Methods PaymentMethod { get; set; }
    }
}