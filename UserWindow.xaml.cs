using laba5.AutoDBDataSetTableAdapters;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Data;
using System;
using System.Linq;

namespace laba5
{
    public partial class UserWindow : Window
    {
        private OrderCheckTableAdapter orderCheck = new OrderCheckTableAdapter();
        private OrderCarTableAdapter orderCar = new OrderCarTableAdapter();
        private CarsTableAdapter carsTable = new CarsTableAdapter();
        private CarModelsTableAdapter carModels = new CarModelsTableAdapter();

        public UserWindow()
        {
            InitializeComponent();
            RefreshDataGrid();
            dg.ItemsSource = orderCheck.GetOrders();
        }

        private void dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "OrderDate")
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;
                if (column != null) column.Binding.StringFormat = "dd.MM.yyyy";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dg.Columns[0].Visibility = Visibility.Collapsed;
            dg.Columns[1].Visibility = Visibility.Collapsed;
            dg.Columns[2].Visibility = Visibility.Collapsed;
            dg.Columns[5].Visibility = Visibility.Collapsed;
            dg.Columns[7].Visibility = Visibility.Collapsed;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ImortButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CarClass> cars = JsonConverter.DeserializeObject<List<CarClass>>();
                if (cars == null)
                {
                    return; // Пользователь отменил выбор файла
                }
                foreach (var car in cars)
                {
                    carsTable.Insert(car.CarModel_ID, car.Number, car.Mileage, car.Price, car.Condtion, car.Color, car.Amount);
                }
                RefreshDataGrid();
                MessageBox.Show("Машины успешно импортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте машин: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RefreshDataGrid()
        {
            carsdg.ItemsSource = null;
            carsdg.ItemsSource = carsTable.GetData();
            modelsdg.ItemsSource = carModels.GetData();
        }

        private void imptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ModelCarClass> carmodels = JsonConverter.DeserializeObject<List<ModelCarClass>>();
                if (carmodels == null)
                {
                    return; // Пользователь отменил выбор файла
                }
                foreach (var modelCar in carmodels)
                {
                    carModels.Insert(modelCar.Brand, modelCar.Name, modelCar.Year, modelCar.Country_ID, modelCar.Status_ID);
                }
                RefreshDataGrid();
                MessageBox.Show("Модели успешно импортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте моделей: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FormatPrice(decimal price)
        {
            return price.ToString("N0").Replace(",", " ");
        }

        private void WriteFile()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filePath = Path.Combine(desktopPath, $"Чек_{currentDate}.txt");

                var orderCarData = orderCar.GetData();
                var carsData = carsTable.GetData();
                var carModelsData = carModels.GetData();

                using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("=".PadRight(60, '='));
                    writer.WriteLine("                    АВТОСАЛОН                     ");
                    writer.WriteLine("=".PadRight(60, '='));
                    writer.WriteLine($"Дата и время чека: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine("-".PadRight(60, '-'));
                    writer.WriteLine();

                    foreach (var item in dg.Items)
                    {
                        var row = item as DataRowView;
                        int orderCheckId = Convert.ToInt32(row["OrderCheck_ID"]);
                        DateTime orderDate = Convert.ToDateTime(row["OrderDate"]);
                        string formattedOrderDate = orderDate.ToString("dd.MM.yyyy");
                        string orderTime = row["OrderTime"].ToString();

                        // Получаем все машины для этого заказа
                        decimal totalOrderPrice = 0;
                        var orderItems = new List<string>();

                        foreach (DataRow orderCarRow in orderCarData.Rows)
                        {
                            if (Convert.ToInt32(orderCarRow["OrderCheck_ID"]) == orderCheckId)
                            {
                                int carId = Convert.ToInt32(orderCarRow["Car_ID"]);
                                int amount = Convert.ToInt32(orderCarRow["Amount"]);

                                // Находим машину
                                foreach (DataRow carRow in carsData.Rows)
                                {
                                    if (Convert.ToInt32(carRow["ID"]) == carId)
                                    {
                                        int modelId = Convert.ToInt32(carRow["CarModel_ID"]);
                                        decimal carPrice = Convert.ToDecimal(carRow["Price"]);
                                        string number = carRow["Number"].ToString();
                                        string color = carRow["Color"].ToString();

                                        // Находим модель
                                        foreach (DataRow modelRow in carModelsData.Rows)
                                        {
                                            if (Convert.ToInt32(modelRow["ID"]) == modelId)
                                            {
                                                string brand = modelRow["Brand"].ToString();
                                                string name = modelRow["Name"].ToString();
                                                int year = Convert.ToInt32(modelRow["Year"]);

                                                decimal itemTotal = carPrice * amount;
                                                totalOrderPrice += itemTotal;

                                                string itemInfo = $"{brand} {name} ({year})\n" +
                                                                $"  Цвет: {color}, Гос.номер: {number}\n" +
                                                                $"  Кол-во: {amount} шт. × {FormatPrice(carPrice)} руб. = {FormatPrice(itemTotal)} руб.";
                                                orderItems.Add(itemInfo);
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        writer.WriteLine($"Заказ №{orderCheckId}");
                        writer.WriteLine($"Дата заказа: {formattedOrderDate} {orderTime}");
                        writer.WriteLine("-".PadRight(40, '-'));

                        // Выводим все машины в заказе
                        foreach (var itemInfo in orderItems)
                        {
                            writer.WriteLine(itemInfo);
                        }

                        writer.WriteLine("-".PadRight(40, '-'));

                        decimal paymentInfo = PromptForPaidAmount(totalOrderPrice, orderCheckId);

                        if (paymentInfo == -1)
                        {
                            writer.WriteLine("СТАТУС: ОПЛАТА ОТМЕНЕНА");
                            writer.WriteLine();
                            writer.WriteLine("-".PadRight(60, '-'));
                            writer.WriteLine();
                            continue;
                        }

                        decimal change = paymentInfo - totalOrderPrice;

                        writer.WriteLine($"Сумма к оплате: {FormatPrice(totalOrderPrice)} руб.");
                        writer.WriteLine($"Внесено: {FormatPrice(paymentInfo)} руб.");
                        writer.WriteLine($"Сдача: {FormatPrice(change)} руб.");

                        writer.WriteLine();
                        writer.WriteLine("-".PadRight(60, '-'));
                        writer.WriteLine();
                    }

                    writer.WriteLine();
                    writer.WriteLine("=".PadRight(60, '='));
                    writer.WriteLine("              СПАСИБО ЗА ПОКУПКУ!                ");
                    writer.WriteLine("=".PadRight(60, '='));
                }

                MessageBox.Show($"Чек успешно сохранен на рабочем столе!\nПуть: {filePath}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении чека: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private decimal PromptForPaidAmount(decimal totalPrice, int orderId)
        {
            string prompt = $"ОФОРМЛЕНИЕ ОПЛАТЫ\n\n" +
                           $"Заказ №: {orderId}\n" +
                           $"Сумма к оплате: {FormatPrice(totalPrice)} руб.\n\n" +
                           $"Введите сумму оплаты (должна быть не менее {FormatPrice(totalPrice)} руб.):";

            InputWindow input = new InputWindow(prompt);

            while (true)
            {
                bool? dialogResult = input.ShowDialog();

                if (dialogResult == false)
                {
                    return (-1);
                }

                string userInput = input.Answer;

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    MessageBox.Show("Пожалуйста, введите сумму оплаты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    input.inputBox.Clear();
                    continue;
                }

                if (!decimal.TryParse(userInput, out decimal paidAmount))
                {
                    MessageBox.Show("Введенное значение некорректно. Пожалуйста, введите число.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    input.inputBox.Clear();
                    continue;
                }

                if (paidAmount <= 0)
                {
                    MessageBox.Show("Сумма оплаты должна быть больше нуля.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    input.inputBox.Clear();
                    continue;
                }

                if (paidAmount < 0.01m)
                {
                    MessageBox.Show("Минимальная сумма оплаты - 0.01 руб.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    input.inputBox.Clear();
                    continue;
                }

                if (paidAmount < totalPrice)
                {
                    MessageBox.Show($"Внесенной суммы недостаточно!\n" +
                                   $"Требуется: {FormatPrice(totalPrice)} руб.\n" +
                                   $"Внесено: {FormatPrice(paidAmount)} руб.\n\n" +
                                   $"Пожалуйста, внесите полную сумму.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    input.inputBox.Clear();
                    continue;
                }

                return paidAmount;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            WriteFile();
        }
    }
}