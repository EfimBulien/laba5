using System.Windows;
using System;
using System.Data;
using System.Windows.Controls;
using laba5.AutoDBDataSetTableAdapters;

namespace laba5
{
    public partial class CarModelPage : Page
    {
        CarModelsTableAdapter carModels = new CarModelsTableAdapter();
        CarCountriesTableAdapter carCountries = new CarCountriesTableAdapter();
        CarStatusTableAdapter carStatus = new CarStatusTableAdapter();
        private AdminWindow parentWindow;

        public CarModelPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            try
            {
                var countriesData = carCountries.GetData();
                CountryComboBox.ItemsSource = countriesData;
                CountryComboBox.DisplayMemberPath = "CarCountry";
                CountryComboBox.SelectedValuePath = "ID";

                EditCountryComboBox.ItemsSource = countriesData;
                EditCountryComboBox.DisplayMemberPath = "CarCountry";
                EditCountryComboBox.SelectedValuePath = "ID";

                var statusData = carStatus.GetData();
                StatusComboBox.ItemsSource = statusData;
                StatusComboBox.DisplayMemberPath = "CarStatus";
                StatusComboBox.SelectedValuePath = "ID";

                EditStatusComboBox.ItemsSource = statusData;
                EditStatusComboBox.DisplayMemberPath = "CarStatus";
                EditStatusComboBox.SelectedValuePath = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        public void RefreshComboBoxData()
        {
            LoadComboBoxData();
        }

        private void ClearBoxes()
        {
            BrandBox.Text = string.Empty;
            NameBox.Text = string.Empty;
            YearBox.Text = string.Empty;
            CountryComboBox.SelectedIndex = -1;
            StatusComboBox.SelectedIndex = -1;

            IDBox.Text = string.Empty;
            EditBrandBox.Text = string.Empty;
            EditNameBox.Text = string.Empty;
            EditYearBox.Text = string.Empty;
            EditCountryComboBox.SelectedIndex = -1;
            EditStatusComboBox.SelectedIndex = -1;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string brand = Validation.ValidateInput(BrandBox);
            string name = Validation.ValidateInput(NameBox);
            string year = Validation.ValidateCarYear(YearBox);
            
            if (CountryComboBox.SelectedIndex == -1 || StatusComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите страну и статус.");
                return;
            }

            if (brand != null && name != null && year != null)
            {
                try
                {
                    int countryID = (int)CountryComboBox.SelectedValue;
                    int statusID = (int)StatusComboBox.SelectedValue;

                    carModels.NewModel(brand, name, Convert.ToInt32(year), countryID, statusID);
                    MessageBox.Show("Модель успешно добавлена!");
                    ClearBoxes();
                    parentWindow?.RefreshCarModelsTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                    return;
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string id = Validation.ValidateInt(IDBox);
            string brand = Validation.ValidateInput(EditBrandBox);
            string name = Validation.ValidateInput(EditNameBox);
            string year = Validation.ValidateCarYear(EditYearBox);

            if (EditCountryComboBox.SelectedIndex == -1 || EditStatusComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите страну и статус.");
                return;
            }

            if (id != null && brand != null && name != null && year != null)
            {
                try
                {
                    int countryID = (int)EditCountryComboBox.SelectedValue;
                    int statusID = (int)EditStatusComboBox.SelectedValue;

                    carModels.UpdateModel(brand, name, Convert.ToInt32(year), countryID, statusID, Convert.ToInt32(id));
                    MessageBox.Show("Модель успешно обновлена!");
                    ClearBoxes();
                    parentWindow?.RefreshCarModelsTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении: " + ex.Message);
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
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту модель?", "Подтверждение", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        carModels.DeleteModel(Convert.ToInt32(id));
                        MessageBox.Show("Модель успешно удалена!");
                        ClearBoxes();
                        parentWindow?.RefreshCarModelsTable();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                    return;
                }
            }
        }
    }
}
