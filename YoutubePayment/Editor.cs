using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubePayment
{
    public class Editor
    {
        public string Name { get; set; }
        public decimal Wage { get; set; }
        public int NumberOfVideos { get; set; }
        public decimal Bonus { get; set; }
        public ObservableCollection<Payment> Payments { get; set; }

        public Editor()
        {
            Payments = new ObservableCollection<Payment>();
        }
        public decimal CalculatePayment()
        {
            return NumberOfVideos * Wage + Bonus;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
