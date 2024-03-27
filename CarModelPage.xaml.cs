using System.Windows;
using System;
using System.Windows.Controls;
using laba5.AutoDBDataSetTableAdapters;

namespace laba5
{
    public partial class CarModelPage : Page
    {
        CarModelsTableAdapter carModels = new CarModelsTableAdapter();
        public CarModelPage()
        {
            InitializeComponent();
        }

        private void ClearBoxes()
        {
            IDBox.Text = string.Empty;
            BrandBox.Text = string.Empty;
            NameBox.Text = string.Empty;
            YearBox.Text = string.Empty;
            CountryIDBox.Text = string.Empty;
            StatusIDBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string id = Validation.ValidateInt(IDBox);
            string brand = Validation.ValidateInput(BrandBox);
            string name = Validation.ValidateInput(NameBox);
            string year = Validation.ValidateInput(YearBox);
            string country = Validation.ValidateInt(CountryIDBox);
            string status = Validation.ValidateInt(StatusIDBox);

            if (id != null && brand != null && name != null && year != null && country != null && status != null)
            {
                try
                {
                    carModels.NewModel(brand, name, Convert.ToInt32(year), Convert.ToInt32(country), Convert.ToInt32(status)); 
                    ClearBoxes();
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
            string id = Validation.ValidateInt(IDBox);
            string brand = Validation.ValidateInput(BrandBox);
            string name = Validation.ValidateInput(NameBox);
            string year = Validation.ValidateInput(YearBox);
            string country = Validation.ValidateInt(CountryIDBox);
            string status = Validation.ValidateInt(StatusIDBox);

            if (id != null && brand != null && name != null && year != null && country != null && status != null)
            {
                try
                {
                    carModels.UpdateModel(brand, name, Convert.ToInt32(year), Convert.ToInt32(country), Convert.ToInt32(status), Convert.ToInt32(id));
                    ClearBoxes();
                }
                catch
                {
                    MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                    return;
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string id = Validation.ValidateInt(IDBox);

            if (id != null)
            {
                try
                {
                    carModels.DeleteModel(Convert.ToInt32(id));
                    ClearBoxes();
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
