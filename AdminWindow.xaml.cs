using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using laba5.AutoDBDataSetTableAdapters;

namespace laba5
{
    public partial class AdminWindow : Window
    {
        AccountsTableAdapter accounts = new AccountsTableAdapter();
        CarCountriesTableAdapter carCountries = new CarCountriesTableAdapter();
        CarModelsTableAdapter carModels = new CarModelsTableAdapter();
        CarStatusTableAdapter carStatus = new CarStatusTableAdapter();
        CarsTableAdapter cars = new CarsTableAdapter();
        CustomersTableAdapter customers = new CustomersTableAdapter();
        EmployeesTableAdapter employees = new EmployeesTableAdapter();
        OrderCarTableAdapter orderCar = new OrderCarTableAdapter();
        OrderCheckTableAdapter orderCheck = new OrderCheckTableAdapter();
        PaymentMethodsTableAdapter paymentMethods = new PaymentMethodsTableAdapter();
        RolesTableAdapter roles = new RolesTableAdapter();
        AutoDBDataSet dataSet = new AutoDBDataSet();
        List<string> tableNames = new List<string>();
        RegPage regPage;
        CarPage carPage = new CarPage();
        CarModelPage modelPage;
        CountryPage countryPage;
        CarStatusPage carStatusPage;


        public AdminWindow()
        {
            InitializeComponent();
            regPage = new RegPage(this);
            countryPage = new CountryPage(this);
            carStatusPage = new CarStatusPage(this);
            modelPage = new CarModelPage(this);
            FillBox();
        }

        private void TableBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected_table = TableBox.SelectedIndex;

            switch (selected_table)
            {
                case 0:
                    TableData.ItemsSource = accounts.GetData();
                    EditFrame.Content = regPage;
                    break;
                case 1:
                    TableData.ItemsSource = carCountries.GetData();
                    EditFrame.Content = countryPage;
                    break;
                case 2:
                    TableData.ItemsSource = carModels.GetData();
                    modelPage.RefreshComboBoxData();
                    EditFrame.Content = modelPage;
                    break;
                case 3:
                    TableData.ItemsSource = cars.GetData();
                    EditFrame.Content = carPage;
                    break;
                case 4:
                    TableData.ItemsSource = carStatus.GetData();
                    EditFrame.Content = carStatusPage;
                    break;
                case 5:
                    TableData.ItemsSource = customers.GetData();
                    EditFrame.Content = null;
                    break;
                case 6:
                    TableData.ItemsSource = employees.GetData();
                    EditFrame.Content = null;
                    break;
                case 7:
                    TableData.ItemsSource = orderCar.GetData();
                    EditFrame.Content = null;
                    break;
                case 8:
                    TableData.ItemsSource = orderCheck.GetData();
                    EditFrame.Content = null;
                    break;
                case 9:
                    TableData.ItemsSource = paymentMethods.GetData();
                    EditFrame.Content = null;
                    break;
                case 10:
                    TableData.ItemsSource = roles.GetData();
                    EditFrame.Content = null;
                    break;
                default:
                    TableData.ItemsSource = accounts.GetData();
                    EditFrame.Content = regPage;
                    break;
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        public void FillBox()
        {
            foreach (DataTable table in dataSet.Tables)
            {
                tableNames.Add(table.TableName);
            }

            TableBox.ItemsSource = tableNames;
        }

        public void GetFullData()
        {
            TableData.ItemsSource = accounts.GetData();
        }

        public void RefreshAccountsTable()
        {
            if (TableBox.SelectedIndex == 0)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = accounts.GetData();
            }
        }

        public void RefreshCountriesTable()
        {
            if (TableBox.SelectedIndex == 1)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = carCountries.GetData();
            }
        }

        public void RefreshCarStatusTable()
        {
            if (TableBox.SelectedIndex == 4)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = carStatus.GetData();
            }
        }

        public void RefreshCarModelsTable()
        {
            if (TableBox.SelectedIndex == 2)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = carModels.GetData();
            }
        }
    }
}