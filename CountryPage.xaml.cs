using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class CountryPage : Page
    {
        CarCountriesTableAdapter countries = new CarCountriesTableAdapter();
        public CountryPage()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newCountry = ValidateInput(CountryBox);
            if (newCountry != null)
            {
                try
                {
                    countries.Insert(newCountry);
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
            string updateID = ValidateInput(IDbox);
            string updateCountry = ValidateInput(CountryBox);
            if (updateCountry != null && updateID != null)
            {
                try
                {
                    countries.UpdateCountry(updateCountry, Convert.ToInt32(updateID));
                }
                catch
                {
                    MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                    return;
                }
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            string delID = ValidateInput(IDbox);;
            if (delID != null)
            {
                try
                {
                    countries.DeleteCountry(Convert.ToInt32(delID));
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
            IDbox.Text = string.Empty;
            CountryBox.Text = string.Empty;
        }
    }
}
