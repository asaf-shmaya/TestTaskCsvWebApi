using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public static class Enums
    {
        public static class Processing
        {
            public enum Statuses : int
            {
                New = 1,
                Paid = 2,
                Cancelled = 3,
            }
        }

        public static class Payment
        {
            public enum Methods : int
            {
                [Display(Name = "Credit Card")]
                CreditCard = 1,
                [Display(Name = "Debit Card")]
                DebitCard = 2,
                [Display(Name = "Electronic Check")]
                ElectronicCheck = 3,
            }
        }
    }
}
