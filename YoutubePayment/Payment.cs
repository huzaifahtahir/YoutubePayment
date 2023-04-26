using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubePayment
{
    public class Payment
    {
        public Editor Editor { get; set; }
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime PaymentDate { get; set; } // Add this property
        public List<string> VideoNames { get; set; }

        public Payment()
        {
            VideoNames = new List<string>();
        }


        public string MonthYear => $"{new DateTime(Year, Month, 1).ToString("MMMM yyyy")}";

        public override string ToString()
        {
            return $"{Editor.Name} - {Amount:C} for {MonthYear}";
        }
    }

}
