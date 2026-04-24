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
        CarPage carPage;
        CarModelPage modelPage;
        CountryPage countryPage;
        CarStatusPage carStatusPage;
        PaymentMethodsPage paymentMethodsPage;
        RolesPage rolesPage;
        EmployeesPage employeesPage;
        CustomersPage customersPage;


        public AdminWindow()
        {
            InitializeComponent();
            regPage = new RegPage(this);
            countryPage = new CountryPage(this);
            carStatusPage = new CarStatusPage(this);
            modelPage = new CarModelPage(this);
            carPage = new CarPage(this);
            paymentMethodsPage = new PaymentMethodsPage(this);
            rolesPage = new RolesPage(this);
            employeesPage = new EmployeesPage(this);
            customersPage = new CustomersPage(this);
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
                    carPage.RefreshCarModelsList();
                    EditFrame.Content = carPage;
                    break;
                case 4:
                    TableData.ItemsSource = carStatus.GetData();
                    EditFrame.Content = carStatusPage;
                    break;
                case 5:
                    TableData.ItemsSource = customers.GetData();
                    customersPage.RefreshAccountsList();
                    EditFrame.Content = customersPage;
                    break;
                case 6:
                    TableData.ItemsSource = employees.GetData();
                    employeesPage.RefreshAccountsList();
                    EditFrame.Content = employeesPage;
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
                    EditFrame.Content = paymentMethodsPage;
                    break;
                case 10:
                    TableData.ItemsSource = roles.GetData();
                    EditFrame.Content = rolesPage;
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

        public void RefreshPaymentMethodsTable()
        {
            if (TableBox.SelectedIndex == 9)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = paymentMethods.GetData();
            }
        }

        internal void RefreshRolesTable()
        {
            if (TableBox.SelectedIndex == 10)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = roles.GetData();
            }
        }

        public void RefreshCarsTable()
        {
            if (TableBox.SelectedIndex == 3)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = cars.GetData();
            }
        }

        public void RefreshEmployeesTable()
        {
            if (TableBox.SelectedIndex == 6)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = employees.GetData();
            }
        }

        public void RefreshCustomersTable()
        {
            if (TableBox.SelectedIndex == 5)
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = customers.GetData();
            }
        }
    }
}