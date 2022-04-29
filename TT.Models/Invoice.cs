using CsvHelper.Configuration.Attributes;

namespace TT.Models
{
    public class Invoice : BaseInvoice
    {
        [Index(0)]
        public DateTime Created { get; set; }
        [Index(1)]
        public DateTime Changed { get; set; }
        [Index(3)]
        public Enums.Processing.Statuses ProcessingStatus { get; set; }
        [Index(5)]
        public Enums.Payment.Methods PaymentMethod { get; set; }
    }
}