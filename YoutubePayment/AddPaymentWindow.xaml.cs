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
    /// <summary>
    /// Interaction logic for AddPaymentWindow.xaml
    /// </summary>
    public partial class AddPaymentWindow : Window
    {
        public Editor Editor { get; set; }
        public Payment Payment { get; private set; }

        public AddPaymentWindow(Editor editor)
        {
            InitializeComponent();
            DataContext = this;

            Editor = editor;
            for (int i = 1; i <= 12; i++)
            {
                cmbMonth.Items.Add(new DateTime(2000, i, 1).ToString("MMMM"));
            }
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void btnAddPayment_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtWage.Text, out decimal wage) &&
                int.TryParse(txtVideos.Text, out int videos) &&
                decimal.TryParse(txtBonus.Text, out decimal bonus))
            {
                Editor.Wage = wage;
                Editor.NumberOfVideos = videos;
                Editor.Bonus = bonus;


                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                Payment = new Payment
                {
                    Editor = Editor,
                    Amount = Editor.CalculatePayment(),
                    Month = cmbMonth.SelectedIndex + 1,
                    Year = currentYear,
                    PaymentDate = DateTime.Now, // Set the PaymentDate property
                    VideoNames = txtVideoNames.Text.Split(',').Select(v => v.Trim()).ToList()
                };

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid wage, videos, and bonus values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
