using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public class BaseInvoice
    {
        public double Amount { get; set; }
        public Enums.Payment.Methods PaymentMethod { get; set; }
    }
}
