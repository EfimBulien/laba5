using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class CountryPage : Page
    {
        CarCountriesTableAdapter countries = new CarCountriesTableAdapter();
        private AdminWindow parentWindow;

        public CountryPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
        }

        private void ClearBoxes()
        {
            IDbox.Text = string.Empty;
            CountryBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newCountry = Validation.ValidateRussianInput(CountryBox);
            if (newCountry != null)
            {
                try
                {
                    countries.Insert(newCountry);
                    MessageBox.Show("Страна успешно добавлена!");
                    ClearBoxes();
                    parentWindow?.RefreshCountriesTable();
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
            string updateID = Validation.ValidateInt(IDbox);
            string updateCountry = Validation.ValidateRussianInput(CountryBox);
            if (updateCountry != null && updateID != null)
            {
                try
                {
                    countries.UpdateCountry(updateCountry, Convert.ToInt32(updateID));
                    MessageBox.Show("Страна успешно обновлена!");
                    ClearBoxes();
                    parentWindow?.RefreshCountriesTable();
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
            string delID = Validation.ValidateInt(IDbox);
            if (delID != null)
            {
                try
                {
                    countries.DeleteCountry(Convert.ToInt32(delID));
                    MessageBox.Show("Страна успешно удалена!");
                    ClearBoxes();
                    parentWindow?.RefreshCountriesTable();
                }
                catch
                {
                    MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                    return;
                }
            }
        }
    }
}
