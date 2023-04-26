using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YoutubePayment
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
        }

        public void UpdateStatistics(Payment[] payments)
        {
            txtTotalPayments.Text = $"{payments.Sum(p => p.Amount):C}";
            txtAverageWage.Text = $"{payments.Average(p => p.Editor.Wage):C}";
            txtTotalVideos.Text = payments.Sum(p => p.Editor.NumberOfVideos).ToString();
        }

        private void txtTotalPayments_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
