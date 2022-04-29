using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public class Filter
    {
        public DateTime MinCreated { get; set; }
        public DateTime MaxCreated { get; set; }
        public DateTime MinChanged { get; set; }
        public DateTime MaxChanged { get; set; }
        public Enums.Processing.Statuses ProcessingStatus { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public Enums.Payment.Methods PaymentMethod { get; set; }
    }
}
