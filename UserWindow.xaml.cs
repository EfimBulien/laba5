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
        
        OrderCheckTableAdapter orderCheck = new OrderCheckTableAdapter();
        CarsTableAdapter carsTable = new CarsTableAdapter();
        CarModelsTableAdapter carModels = new CarModelsTableAdapter();
        string filePath = "check.txt";

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
            List<CarClass> cars = LabaConverter.DeserializeObject<List<CarClass>>();
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
            List<ModelCarClass> carmodels = LabaConverter.DeserializeObject<List<ModelCarClass>>();
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
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("<Автосалон>");

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
                        decimal paidAmount = Convert.ToDecimal(row["PaidAmount"]);
                        decimal paidPrice = PromptForPaidAmount();
                        decimal change = paidPrice - carPrice;

                        writer.WriteLine($"\tКассовый чек №{orderCheckId}");
                        writer.WriteLine($"\t{formattedOrderDate} {orderTime}");
                        writer.WriteLine($"\t{carBrand} {carName} - {carPrice}$");
                        writer.WriteLine($"\nИтого к оплате: {paidAmount}$");
                        writer.WriteLine($"Внесено: {paidPrice}$");
                        writer.WriteLine($"Сдача: {change}$");
                        writer.WriteLine("\n");
                    }
                }

                MessageBox.Show("Данные успешно сохранены в файл.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            WriteFile();
        }

        private decimal PromptForPaidAmount()
        {
            decimal paidAmount;
            InputWindow input = new InputWindow("Введите сумму для этого заказа:");
            bool? dialogResult = input.ShowDialog();
            if (dialogResult == false)
            {
                
                return 0;
            }

            string userInput = input.Answer;
            if (decimal.TryParse(userInput, out paidAmount))
            {
                return paidAmount;
            }
            else
            {
                MessageBox.Show("Введенное значение некорректно. Пожалуйста, введите число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return PromptForPaidAmount();
            }
        }
    }
}
