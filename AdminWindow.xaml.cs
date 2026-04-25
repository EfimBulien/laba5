using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using laba5.AutoDBDataSetTableAdapters;

namespace laba5
{
    public partial class AdminWindow : Window
    {
        private AccountsTableAdapter accounts = new AccountsTableAdapter();
        private CarCountriesTableAdapter carCountries = new CarCountriesTableAdapter();
        private CarModelsTableAdapter carModels = new CarModelsTableAdapter();
        private CarStatusTableAdapter carStatus = new CarStatusTableAdapter();
        private CarsTableAdapter cars = new CarsTableAdapter();
        private CustomersTableAdapter customers = new CustomersTableAdapter();
        private EmployeesTableAdapter employees = new EmployeesTableAdapter();
        private OrderCarTableAdapter orderCar = new OrderCarTableAdapter();
        private OrderCheckTableAdapter orderCheck = new OrderCheckTableAdapter();
        private PaymentMethodsTableAdapter paymentMethods = new PaymentMethodsTableAdapter();
        private RolesTableAdapter roles = new RolesTableAdapter();

        private AutoDBDataSet dataSet = new AutoDBDataSet();

        private Dictionary<string, string> tableDisplayNames = new Dictionary<string, string>
        {
            { "Accounts", "Аккаунты" },
            { "CarCountries", "Страны" },
            { "CarModels", "Модели машин" },
            { "Cars", "Машины" },
            { "CarStatus", "Статусы машин" },
            { "Customers", "Клиенты" },
            { "Employees", "Сотрудники" },
            { "OrderCar", "Заказ-Машина" },
            { "OrderCheck", "Заказы" },
            { "PaymentMethods", "Способы оплаты" },
            { "Roles", "Роли" }
        };

        private Dictionary<string, string> displayToTableName = new Dictionary<string, string>();

        private RegPage regPage;
        private CarPage carPage;
        private CarModelPage modelPage;
        private CountryPage countryPage;
        private CarStatusPage carStatusPage;
        private PaymentMethodsPage paymentMethodsPage;
        private RolesPage rolesPage;
        private EmployeesPage employeesPage;
        private CustomersPage customersPage;
        private OrderCheckPage orderCheckPage;

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
            orderCheckPage = new OrderCheckPage(this);
            FillBox();
        }

        private void TableBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableBox.SelectedItem == null) return;

            string selectedDisplayName = TableBox.SelectedItem.ToString();
            if (!displayToTableName.ContainsKey(selectedDisplayName)) return;

            string tableName = displayToTableName[selectedDisplayName];

            TableData.ItemsSource = null;
            EditFrame.Content = null;

            try
            {
                switch (tableName)
                {
                    case "Accounts":
                        TableData.ItemsSource = accounts.GetData().DefaultView;
                        EditFrame.Content = regPage;
                        break;
                    case "CarCountries":
                        TableData.ItemsSource = carCountries.GetData().DefaultView;
                        EditFrame.Content = countryPage;
                        break;
                    case "CarModels":
                        TableData.ItemsSource = carModels.GetData().DefaultView;
                        modelPage.RefreshComboBoxData();
                        EditFrame.Content = modelPage;
                        break;
                    case "Cars":
                        TableData.ItemsSource = cars.GetData().DefaultView;
                        carPage.RefreshCarModelsList();
                        EditFrame.Content = carPage;
                        break;
                    case "CarStatus":
                        TableData.ItemsSource = carStatus.GetData().DefaultView;
                        EditFrame.Content = carStatusPage;
                        break;
                    case "Customers":
                        TableData.ItemsSource = customers.GetData().DefaultView;
                        customersPage.RefreshAccountsList();
                        EditFrame.Content = customersPage;
                        break;
                    case "Employees":
                        TableData.ItemsSource = employees.GetData().DefaultView;
                        employeesPage.RefreshAccountsList();
                        EditFrame.Content = employeesPage;
                        break;
                    case "OrderCar":
                        TableData.ItemsSource = orderCar.GetData().DefaultView;
                        EditFrame.Content = null;
                        break;
                    case "OrderCheck":
                        TableData.ItemsSource = orderCheck.GetData().DefaultView;
                        EditFrame.Content = orderCheckPage;
                        break;
                    case "PaymentMethods":
                        TableData.ItemsSource = paymentMethods.GetData().DefaultView;
                        EditFrame.Content = paymentMethodsPage;
                        break;
                    case "Roles":
                        TableData.ItemsSource = roles.GetData().DefaultView;
                        EditFrame.Content = rolesPage;
                        break;
                    default:
                        TableData.ItemsSource = accounts.GetData().DefaultView;
                        EditFrame.Content = regPage;
                        break;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке таблицы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            displayToTableName.Clear();
            foreach (DataTable table in dataSet.Tables)
            {
                string techName = table.TableName;
                string displayName = tableDisplayNames.ContainsKey(techName) ? tableDisplayNames[techName] : techName;
                displayToTableName[displayName] = techName;
            }

            TableBox.ItemsSource = displayToTableName.Keys.ToList();
        }

        public void GetFullData()
        {
            TableData.ItemsSource = accounts.GetData().DefaultView;
        }

        public void RefreshAccountsTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["Accounts"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = accounts.GetData().DefaultView;
            }
        }

        public void RefreshCountriesTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["CarCountries"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = carCountries.GetData().DefaultView;
            }
        }

        public void RefreshCarStatusTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["CarStatus"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = carStatus.GetData().DefaultView;
            }
        }

        public void RefreshCarModelsTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["CarModels"])
            {
                modelPage.RefreshComboBoxData();
                TableData.ItemsSource = null;
                TableData.ItemsSource = carModels.GetData().DefaultView;
            }
        }

        public void RefreshPaymentMethodsTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["PaymentMethods"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = paymentMethods.GetData().DefaultView;
            }
        }

        internal void RefreshRolesTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["Roles"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = roles.GetData().DefaultView;
            }
        }

        public void RefreshCarsTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["Cars"])
            {
                carPage.RefreshCarModelsList();
                TableData.ItemsSource = null;
                TableData.ItemsSource = cars.GetData().DefaultView;
            }
        }

        public void RefreshEmployeesTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["Employees"])
            {
                employeesPage.RefreshAccountsList();
                TableData.ItemsSource = null;
                TableData.ItemsSource = employees.GetData().DefaultView;
            }
        }

        public void RefreshCustomersTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["Customers"])
            {
                customersPage.RefreshAccountsList();
                TableData.ItemsSource = null;
                TableData.ItemsSource = customers.GetData().DefaultView;
            }
        }

        public void RefreshOrderCheckTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["OrderCheck"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = orderCheck.GetData().DefaultView;
            }
        }

        public void RefreshOrderCarTable()
        {
            if (TableBox.SelectedItem != null &&
                TableBox.SelectedItem.ToString() == tableDisplayNames["OrderCar"])
            {
                TableData.ItemsSource = null;
                TableData.ItemsSource = orderCar.GetData().DefaultView;
            }
        }
    }
}