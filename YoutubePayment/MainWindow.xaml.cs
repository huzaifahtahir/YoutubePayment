using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace YoutubePayment
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Editor> _editors;
        private ObservableCollection<Payment> _payments;


        public MainWindow()
        {
            InitializeComponent();
            _editors = new ObservableCollection<Editor>();
            _payments = new ObservableCollection<Payment>();
            lstEditors.ItemsSource = _editors;
            lstEditors.DataContext = _editors;
            lstPayments.ItemsSource = _payments;
            lstPayments.DataContext = _payments;
            LoadDataFromFile();
        }
        private void btnAddEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEditorName.Text))
            {
                Editor newEditor = new Editor
                {
                    Name = txtEditorName.Text
                };

                _editors.Add(newEditor);
                SaveDataToFile(); // Save data to the file

                txtEditorName.Clear();
            }
            else
            {
                MessageBox.Show("Please enter an editor name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (lstEditors.SelectedItem is Editor selectedEditor)
            {
                decimal paymentAmount = selectedEditor.CalculatePayment();
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                Payment payment = new Payment
                {
                    Editor = selectedEditor,
                    Amount = paymentAmount,
                    Month = currentMonth,
                    Year = currentYear
                };

                _payments.Add(payment);
                SaveDataToFile(); // Save data to the file

            }
            else
            {
                MessageBox.Show("Please select an editor to calculate the payment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields()
        {
            txtEditorName.Text = string.Empty;
        }

        private const string EditorsDataFileName = "editors.json";
        private const string PaymentsDataFileName = "payments.json";

        private void SaveDataToFile()
        {
            string editorsJsonData = JsonConvert.SerializeObject(_editors, Formatting.Indented);
            File.WriteAllText(EditorsDataFileName, editorsJsonData);

            string paymentsJsonData = JsonConvert.SerializeObject(_payments, Formatting.Indented);
            File.WriteAllText(PaymentsDataFileName, paymentsJsonData);
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(EditorsDataFileName))
            {
                string editorsJsonData = File.ReadAllText(EditorsDataFileName);
                var editors = JsonConvert.DeserializeObject<ObservableCollection<Editor>>(editorsJsonData);
                _editors.Clear();
                foreach (var editor in editors)
                {
                    _editors.Add(editor);
                }
            }

            if (File.Exists(PaymentsDataFileName))
            {
                string paymentsJsonData = File.ReadAllText(PaymentsDataFileName);
                var payments = JsonConvert.DeserializeObject<ObservableCollection<Payment>>(paymentsJsonData);
                _payments.Clear();
                foreach (var payment in payments)
                {
                    _payments.Add(payment);
                }
            }
        }

        private void lstEditors_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstEditors.SelectedItem is Editor selectedEditor)
            {
                AddPaymentWindow addPaymentWindow = new AddPaymentWindow(selectedEditor);
                if (addPaymentWindow.ShowDialog() == true)
                {
                    Payment payment = addPaymentWindow.Payment;
                    _payments.Add(payment);
                    SaveDataToFile(); // Save data to the file
                }
            }
        }

        private void btnDeleteEditor_Click(object sender, RoutedEventArgs e)
        {
            if (lstEditors.SelectedItem is Editor selectedEditor)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedEditor.Name}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _editors.Remove(selectedEditor);
                    SaveDataToFile(); // Save data to the file
                }
            }
            else
            {
                MessageBox.Show("Please select an editor to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeletePayment_Click(object sender, RoutedEventArgs e)
        {
            if (lstPayments.SelectedItem is Payment selectedPayment)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the payment for {selectedPayment.Editor.Name} in {selectedPayment.MonthYear}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _payments.Remove(selectedPayment);
                    SaveDataToFile(); // Save data to the file
                }
            }
            else
            {
                MessageBox.Show("Please select a payment to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lstPayments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPayments.SelectedItem is Payment selectedPayment)
            {
                string paymentDetails = $"Wage: {selectedPayment.Editor.Wage:C}\n" +
                                         $"Videos: {selectedPayment.Editor.NumberOfVideos}\n" +
                                         $"Bonus: {selectedPayment.Editor.Bonus:C}\n" +
                                         $"Payment Date: {selectedPayment.PaymentDate}\n" +
                                         $"Video Names: {string.Join(", ", selectedPayment.VideoNames)}";

                tbPaymentDetails.Text = paymentDetails;

                string videoNames = string.Join(", ", selectedPayment.VideoNames);

                // Copy payment details to clipboard
                string formattedPaymentDetails = $"Date: {selectedPayment.PaymentDate}\n" +
                                                 $"==============================\n" +
                                                 $"Amount: {selectedPayment.Editor.Wage:C}\n" +
                                                 $"Videos: {selectedPayment.Editor.NumberOfVideos}\n" +
                                                 $"Misc: {selectedPayment.Editor.Bonus:C}\n" +
                                                 $"Video Names: {videoNames}\n" +
                                                 $"==============================\n" +
                                                 $"Total: {selectedPayment.Editor.Wage * selectedPayment.Editor.NumberOfVideos + selectedPayment.Editor.Bonus}";

                Clipboard.SetText(formattedPaymentDetails);
            }
        }

        private void btnShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow();

            var payments = _payments.ToArray();
            statisticsWindow.UpdateStatistics(payments);

            statisticsWindow.ShowDialog();
        }

    }
}
