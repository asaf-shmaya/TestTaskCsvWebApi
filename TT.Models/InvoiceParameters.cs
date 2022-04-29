using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public class InvoiceParameters : QueryStringParameters
    {
        public InvoiceParameters()
        {
            OrderBy = "Number";
        }
        public DateTime MinCreated { get; set; }
        public DateTime MaxCreated { get; set; } = DateTime.Now;
        public DateTime MinChanged { get; set; }
        public DateTime MaxChanged { get; set; } = DateTime.Now;
        public Enums.Processing.Statuses ProcessingStatus { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public Enums.Payment.Methods PaymentMethod { get; set; }
    }
}
