using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Lifetime;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace laba5
{
    public partial class CarPage : Page
    {
        CarsTableAdapter cars = new CarsTableAdapter();
        public CarPage()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string id = ValidateInput(IDBox);
            string number = ValidateInput(NumberBox);
            string mileage = ValidateInput(MileageBox);
            string price = ValidateInput(PriceBox);
            string condition = ValidateInput(ConditionBox);
            string color = ValidateInput(ColorBox);
            string amount = ValidateInput(AmountBox);
            if (id != null && number != null && mileage != null && price != null && condition != null && color != null && amount != null)
            {
                try
                {
                    cars.Insert(Convert.ToInt32(id), number, Convert.ToInt32(mileage), Convert.ToInt32(price), condition, color, Convert.ToInt32(amount));
                }
                catch
                {
                    MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                    return;
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string id = ValidateInput(IDBox);

            if (id != null)
            {
                try
                {
                    cars.DeleteCar(Convert.ToInt32(id));
                }
                catch
                {
                    MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                    return;
                }
            }
        }

        private string ValidateInput(TextBox textBox)
        {
            string input = textBox.Text;
            if (ContainsInvalidCharacters(input))
            {
                MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                ClearBoxes();
                return null;
            }
            else return input;
        }

        private bool ContainsInvalidCharacters(string input)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            return regex.IsMatch(input);
        }

        private void ClearBoxes()
        {
            IDBox.Text = string.Empty;
            NumberBox.Text = string.Empty;
            
        }
    }
}
