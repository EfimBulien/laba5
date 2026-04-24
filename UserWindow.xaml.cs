using laba5.AutoDBDataSetTableAdapters;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Data;
using System;

namespace laba5
{
    public partial class UserWindow : Window
    {
        private OrderCheckTableAdapter orderCheck = new OrderCheckTableAdapter();

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
            List<CarClass> cars = JsonConverter.DeserializeObject<List<CarClass>>();
            foreach (var car in cars)
            {
                carsTable.Insert(car.CarModel_ID, car.Number, car.Mileage, car.Price, car.Condtion, car.Color, car.Amount);
            }
            RefreshDataGrid();
        }

        public void RefreshDataGrid()
        {
            carsdg.ItemsSource = null;
            carsdg.ItemsSource = carsTable.GetData();
            modelsdg.ItemsSource = carModels.GetData();
        }

        private void imptButton_Click(object sender, RoutedEventArgs e)
        {
            List<ModelCarClass> carmodels = JsonConverter.DeserializeObject<List<ModelCarClass>>();
            foreach (var modelCar in carmodels)
            {
                carModels.Insert(modelCar.Brand, modelCar.Name, modelCar.Year, modelCar.Country_ID, modelCar.Status_ID);
            }
            RefreshDataGrid();
        }

        private void WriteFile()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filePath = Path.Combine(desktopPath, $"Чек_{currentDate}.txt");

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
                        string carBrand = row["Car_Brand"].ToString();
                        string carName = row["Car_Name"].ToString();
                        decimal carPrice = Convert.ToDecimal(row["Car_Price"]);

                        writer.WriteLine($"Заказ №{orderCheckId}");
                        writer.WriteLine($"Дата заказа: {formattedOrderDate} {orderTime}");
                        writer.WriteLine($"Автомобиль: {carBrand} {carName}");
                        writer.WriteLine($"Стоимость: {carPrice:F2} руб.");
                        writer.WriteLine("-".PadRight(40, '-'));

                        decimal paymentInfo = PromptForPaidAmount(carPrice, carBrand, carName, orderCheckId);

                        if (paymentInfo == -1)
                        {
                            writer.WriteLine("СТАТУС: ОПЛАТА ОТМЕНЕНА");
                            writer.WriteLine();
                            writer.WriteLine("-".PadRight(60, '-'));
                            writer.WriteLine();
                            continue;
                        }

                        decimal change = paymentInfo - carPrice;

                        writer.WriteLine($"Сумма к оплате: {carPrice:F2} руб.");
                        writer.WriteLine($"Внесено: {paymentInfo:F2} руб.");
                        writer.WriteLine($"Сдача: {change:F2} руб.");

                    

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

        private decimal PromptForPaidAmount(decimal totalPrice, string carBrand, string carName, int orderId)
        {
            string prompt = $"ОФОРМЛЕНИЕ ОПЛАТЫ\n\n" +
                           $"Заказ №: {orderId}\n" +
                           $"Автомобиль: {carBrand} {carName}\n" +
                           $"Сумма к оплате: {totalPrice:F2} руб.\n\n" +
                           $"Введите сумму оплаты (должна быть не менее {totalPrice:F2} руб.):";

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
                                   $"Требуется: {totalPrice:F2} руб.\n" +
                                   $"Внесено: {paidAmount:F2} руб.\n\n" +
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