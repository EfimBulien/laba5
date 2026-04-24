using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace laba5
{
    public class OrderItem
    {
        public int CarId { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }

    public partial class OrderCheckPage : Page
    {
        private OrderCheckTableAdapter orderCheck = new OrderCheckTableAdapter();
        private OrderCarTableAdapter orderCar = new OrderCarTableAdapter();
        private CustomersTableAdapter customers = new CustomersTableAdapter();
        private EmployeesTableAdapter employees = new EmployeesTableAdapter();
        private PaymentMethodsTableAdapter paymentMethods = new PaymentMethodsTableAdapter();
        private CarsTableAdapter cars = new CarsTableAdapter();
        private CarModelsTableAdapter carModels = new CarModelsTableAdapter();

        private AdminWindow parentWindow;
        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
        private Dictionary<int, decimal> carPrices = new Dictionary<int, decimal>();

        public OrderCheckPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
            OrderCarsDataGrid.ItemsSource = orderItems;
            LoadComboBoxData();

          
            OrderDateBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            OrderTimeBox.Text = DateTime.Now.ToString("HH:mm");
            EditOrderDateBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            EditOrderTimeBox.Text = DateTime.Now.ToString("HH:mm");
        }

        private void LoadComboBoxData()
        {
            try
            {
                var customersData = customers.GetData();
                var customersList = new List<KeyValuePair<int, string>>();
                foreach (DataRow row in customersData.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string name = $"{row["Surname"]} {row["Firstname"]}";
                    if (!string.IsNullOrEmpty(row["Patronymic"].ToString()))
                        name += $" {row["Patronymic"]}";
                    customersList.Add(new KeyValuePair<int, string>(id, name));
                }
                CustomerComboBox.ItemsSource = customersList;
                EditCustomerComboBox.ItemsSource = customersList;

                var employeesData = employees.GetData();
                var employeesList = new List<KeyValuePair<int, string>>();
                foreach (DataRow row in employeesData.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string name = $"{row["Surname"]} {row["Firstname"]} ({row["Post"]})";
                    employeesList.Add(new KeyValuePair<int, string>(id, name));
                }
                EmployeeComboBox.ItemsSource = employeesList;
                EditEmployeeComboBox.ItemsSource = employeesList;

                var paymentMethodsData = paymentMethods.GetData();
                var paymentMethodsList = new List<KeyValuePair<int, string>>();
                foreach (DataRow row in paymentMethodsData.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string method = row["PaymentMethod"].ToString();
                    paymentMethodsList.Add(new KeyValuePair<int, string>(id, method));
                }
                PaymentMethodComboBox.ItemsSource = paymentMethodsList;
                EditPaymentMethodComboBox.ItemsSource = paymentMethodsList;

                LoadCars();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void LoadCars()
        {
            try
            {
                var carsData = cars.GetData();
                var modelsData = carModels.GetData();
                var carsList = new List<KeyValuePair<int, string>>();
                carPrices.Clear();

                foreach (DataRow carRow in carsData.Rows)
                {
                    int carId = Convert.ToInt32(carRow["ID"]);
                    int modelId = Convert.ToInt32(carRow["CarModel_ID"]);
                    string number = carRow["Number"].ToString();
                    decimal price = Convert.ToDecimal(carRow["Price"]);
                    int amount = Convert.ToInt32(carRow["Amount"]);

                    carPrices[carId] = price;

                    string modelName = "";
                    foreach (DataRow modelRow in modelsData.Rows)
                    {
                        if (Convert.ToInt32(modelRow["ID"]) == modelId)
                        {
                            modelName = $"{modelRow["Brand"]} {modelRow["Name"]} ({modelRow["Year"]})";
                            break;
                        }
                    }

                    if (amount > 0)
                    {
                        string displayText = $"{modelName} - {number} - {price:C} | Доступно: {amount} шт.";
                        carsList.Add(new KeyValuePair<int, string>(carId, displayText));
                    }
                }

                CarComboBox.ItemsSource = carsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке автомобилей: " + ex.Message);
            }
        }

        private void CalculateTotal()
        {
            decimal total = orderItems.Sum(item => item.Total);
            TotalPriceTextBlock.Text = $"{total:C}";
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите автомобиль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string quantityStr = Validation.ValidateInt(CarQuantityBox);
            if (quantityStr == null) return;

            try
            {
                int carId = ((KeyValuePair<int, string>)CarComboBox.SelectedItem).Key;
                int quantity = Convert.ToInt32(quantityStr);

                if (quantity <= 0)
                {
                    MessageBox.Show("Количество должно быть больше 0!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
              
                var carsData = cars.GetData();
                int availableAmount = 0;
                string modelName = "";
                foreach (DataRow row in carsData.Rows)
                {
                    if (Convert.ToInt32(row["ID"]) == carId)
                    {
                        availableAmount = Convert.ToInt32(row["Amount"]);
                        int modelId = Convert.ToInt32(row["CarModel_ID"]);
                        var modelsData = carModels.GetData();
                        foreach (DataRow modelRow in modelsData.Rows)
                        {
                            if (Convert.ToInt32(modelRow["ID"]) == modelId)
                            {
                                modelName = $"{modelRow["Brand"]} {modelRow["Name"]}";
                                break;
                            }
                        }
                        break;
                    }
                }

                if (quantity > availableAmount)
                {
                    MessageBox.Show($"Недостаточно автомобилей! Доступно: {availableAmount} шт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existingItem = orderItems.FirstOrDefault(item => item.CarId == carId);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    orderItems.Add(new OrderItem
                    {
                        CarId = carId,
                        Model = modelName,
                        Price = carPrices[carId],
                        Quantity = quantity
                    });
                }

                CarQuantityBox.Text = string.Empty;
                CarComboBox.SelectedIndex = -1;
                CalculateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderCarsDataGrid.SelectedItem is OrderItem item)
            {
                orderItems.Remove(item);
                CalculateTotal();
            }
        }

        public void RefreshData()
        {
            LoadComboBoxData();
            parentWindow?.RefreshOrderCheckTable();
        }

        private void ClearOrderBoxes()
        {
            CustomerComboBox.SelectedIndex = -1;
            EmployeeComboBox.SelectedIndex = -1;
            OrderDateBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            OrderTimeBox.Text = DateTime.Now.ToString("HH:mm");
            PaymentMethodComboBox.SelectedIndex = -1;
            orderItems.Clear();
            CarQuantityBox.Text = string.Empty;
            CarComboBox.SelectedIndex = -1;
            CalculateTotal();
        }

        private void ClearEditBoxes()
        {
            EditOrderCheckIDBox.Text = string.Empty;
            EditCustomerComboBox.SelectedIndex = -1;
            EditEmployeeComboBox.SelectedIndex = -1;
            EditOrderDateBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
            EditOrderTimeBox.Text = DateTime.Now.ToString("HH:mm");
            EditPaymentMethodComboBox.SelectedIndex = -1;
            EditPaidAmountBox.Text = string.Empty;
        }

        private bool ValidateDate(string dateStr, out DateTime date)
        {
            if (!DateTime.TryParse(dateStr, out date))
            {
                MessageBox.Show("Неверный формат даты! Используйте ГГГГ-ММ-ДД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool ValidateTime(string timeStr, out TimeSpan time)
        {
            if (!TimeSpan.TryParse(timeStr, out time))
            {
                MessageBox.Show("Неверный формат времени! Используйте ЧЧ:ММ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите покупателя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EmployeeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите сотрудника!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (PaymentMethodComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите способ оплаты!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один автомобиль в заказ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateDate(OrderDateBox.Text, out DateTime orderDate)) return;
            if (!ValidateTime(OrderTimeBox.Text, out TimeSpan orderTime)) return;

            try
            {
                int customerId = ((KeyValuePair<int, string>)CustomerComboBox.SelectedItem).Key;
                int employeeId = ((KeyValuePair<int, string>)EmployeeComboBox.SelectedItem).Key;
                int paymentMethodId = ((KeyValuePair<int, string>)PaymentMethodComboBox.SelectedItem).Key;
                decimal totalAmount = orderItems.Sum(item => item.Total);

                orderCheck.Insert(customerId, employeeId, orderDate, orderTime, paymentMethodId, totalAmount);

                var orderData = orderCheck.GetData();
                int newOrderId = 0;
                foreach (DataRow row in orderData.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    if (id > newOrderId) newOrderId = id;
                }

                foreach (var item in orderItems)
                {
                    orderCar.Insert(newOrderId, item.CarId, item.Quantity);
                }

                MessageBox.Show("Заказ успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearOrderBoxes();
                LoadCars();
                parentWindow?.RefreshOrderCheckTable();
                parentWindow?.RefreshOrderCarTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании заказа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string orderIdStr = Validation.ValidateInt(EditOrderCheckIDBox);

            if (orderIdStr == null) return;

            if (EditCustomerComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите покупателя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EditEmployeeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите сотрудника!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EditPaymentMethodComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите способ оплаты!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string paidAmount = Validation.ValidatePrice(EditPaidAmountBox);
            if (paidAmount == null) return;

            if (!ValidateDate(EditOrderDateBox.Text, out DateTime orderDate)) return;
            if (!ValidateTime(EditOrderTimeBox.Text, out TimeSpan orderTime)) return;

            try
            {
                int orderId = Convert.ToInt32(orderIdStr);
                int customerId = ((KeyValuePair<int, string>)EditCustomerComboBox.SelectedItem).Key;
                int employeeId = ((KeyValuePair<int, string>)EditEmployeeComboBox.SelectedItem).Key;
                int paymentMethodId = ((KeyValuePair<int, string>)EditPaymentMethodComboBox.SelectedItem).Key;
                decimal paidAmountValue = Convert.ToDecimal(paidAmount);

                var orderData = orderCheck.GetData();
                bool orderExists = false;
                foreach (DataRow row in orderData.Rows)
                {
                    if (Convert.ToInt32(row["ID"]) == orderId)
                    {
                        orderExists = true;
                        break;
                    }
                }

                if (!orderExists)
                {
                    MessageBox.Show($"Заказ с ID {orderId} не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult confirm = MessageBox.Show(
                    $"Вы уверены, что хотите изменить заказ №{orderId}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    orderCheck.UpdateOrder(customerId, employeeId, orderDate.ToString("yyyy-MM-dd"), orderTime.ToString(@"hh\:mm"), paymentMethodId, paidAmountValue, orderId);
                    MessageBox.Show("Заказ успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearEditBoxes();
                    parentWindow?.RefreshOrderCheckTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении заказа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string orderIdStr = Validation.ValidateInt(EditOrderCheckIDBox);

            if (orderIdStr == null) return;

            try
            {
                int orderId = Convert.ToInt32(orderIdStr);

                var orderData = orderCheck.GetData();
                bool orderExists = false;
                foreach (DataRow row in orderData.Rows)
                {
                    if (Convert.ToInt32(row["ID"]) == orderId)
                    {
                        orderExists = true;
                        break;
                    }
                }

                if (!orderExists)
                {
                    MessageBox.Show($"Заказ с ID {orderId} не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult confirm = MessageBox.Show(
                    $"Вы уверены, что хотите удалить заказ №{orderId}?\n\nВНИМАНИЕ: Все автомобили в этом заказе также будут удалены!",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    orderCheck.DeleteCheck(orderId);
                    MessageBox.Show("Заказ успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearEditBoxes();
                    parentWindow?.RefreshOrderCheckTable();
                    parentWindow?.RefreshOrderCarTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении заказа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string orderIdStr = Validation.ValidateInt(ViewOrderIDBox);

            if (orderIdStr == null) return;

            try
            {
                int orderId = Convert.ToInt32(orderIdStr);
                var orderCarData = orderCar.GetData();
                var carsData = cars.GetData();
                var modelsData = carModels.GetData();

                var carsInOrder = new List<string>();
                decimal totalPrice = 0;

                foreach (DataRow orderCarRow in orderCarData.Rows)
                {
                    if (Convert.ToInt32(orderCarRow["OrderCheck_ID"]) == orderId)
                    {
                        int carId = Convert.ToInt32(orderCarRow["Car_ID"]);
                        int amount = Convert.ToInt32(orderCarRow["Amount"]);

                        foreach (DataRow carRow in carsData.Rows)
                        {
                            if (Convert.ToInt32(carRow["ID"]) == carId)
                            {
                                int modelId = Convert.ToInt32(carRow["CarModel_ID"]);
                                string number = carRow["Number"].ToString();
                                decimal price = Convert.ToDecimal(carRow["Price"]);

                                foreach (DataRow modelRow in modelsData.Rows)
                                {
                                    if (Convert.ToInt32(modelRow["ID"]) == modelId)
                                    {
                                        string modelName = $"{modelRow["Brand"]} {modelRow["Name"]}";
                                        string displayText = $"{modelName} | {number} | {price:C} x {amount} шт. = {(price * amount):C}";
                                        carsInOrder.Add(displayText);
                                        totalPrice += price * amount;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                OrderCarsListBox.ItemsSource = carsInOrder;

                if (carsInOrder.Count > 0)
                {
                    TotalAmountText.Text = $"Общая сумма заказа: {totalPrice:C}";
                }
                else
                {
                    TotalAmountText.Text = "В этом заказе нет автомобилей";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при просмотре заказа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}