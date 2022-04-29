namespace TT.Models
{
    public class Invoice : BaseInvoice
    {
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
        public Enums.Processing.Statuses ProcessingStatus { get; set; }
        public Enums.Payment.Methods PaymentMethod { get; set; }
    }
}