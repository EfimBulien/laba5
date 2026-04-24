using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class CarPage : Page
    {
        private CarsTableAdapter cars = new CarsTableAdapter();
        private CarModelsTableAdapter carModels = new CarModelsTableAdapter();
        private AdminWindow parentWindow;

        public CarPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
            LoadCarModels();
        }

        private void LoadCarModels()
        {
            try
            {
                var models = new List<KeyValuePair<int, string>>();
                var data = carModels.GetData();

                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string brand = row["Brand"].ToString();
                    string name = row["Name"].ToString();
                    int year = Convert.ToInt32(row["Year"]);
                    string displayText = $"{brand} {name} ({year})";

                    models.Add(new KeyValuePair<int, string>(id, displayText));
                }

                CarModelComboBox.ItemsSource = models;
                EditCarModelComboBox.ItemsSource = models;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки моделей: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RefreshCarModelsList()
        {
            LoadCarModels();
        }

        private void ClearBoxes()
        {
            NumberBox.Text = string.Empty;
            MileageBox.Text = string.Empty;
            PriceBox.Text = string.Empty;
            ConditionBox.Text = string.Empty;
            ColorBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            CarModelComboBox.SelectedItem = null;

            IDbox.Text = string.Empty;
            EditNumberBox.Text = string.Empty;
            EditMileageBox.Text = string.Empty;
            EditPriceBox.Text = string.Empty;
            EditConditionBox.Text = string.Empty;
            EditColorBox.Text = string.Empty;
            EditAmountBox.Text = string.Empty;
            EditCarModelComboBox.SelectedItem = null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string number = Validation.ValidateCarNumber(NumberBox);
            string mileage = Validation.ValidateMileage(MileageBox);
            string price = Validation.ValidatePrice(PriceBox);
            string condition = Validation.ValidateRussianInput(ConditionBox);
            string color = Validation.ValidateRussianInput(ColorBox);
            string amount = Validation.ValidateInt(AmountBox);

            if (CarModelComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите модель автомобиля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int carModelId = ((KeyValuePair<int, string>)CarModelComboBox.SelectedItem).Key;

            if (number != null && mileage != null && price != null && condition != null &&
                color != null && amount != null)
            {
                try
                {
                    cars.Insert(carModelId, number, Convert.ToInt32(mileage), Convert.ToDecimal(price),
                               condition, color, Convert.ToInt32(amount));
                    MessageBox.Show("Автомобиль успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearBoxes();
                    parentWindow?.RefreshCarsTable();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Number"))
                    {
                        MessageBox.Show("Автомобиль с таким номером уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string updateID = Validation.ValidateInt(IDbox);
            string updateNumber = Validation.ValidateCarNumber(EditNumberBox);
            string updateMileage = Validation.ValidateMileage(EditMileageBox);
            string updatePrice = Validation.ValidatePrice(EditPriceBox);
            string updateCondition = Validation.ValidateRussianInput(EditConditionBox);
            string updateColor = Validation.ValidateRussianInput(EditColorBox);
            string updateAmount = Validation.ValidateInt(EditAmountBox);

            if (EditCarModelComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите новую модель автомобиля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int carModelId = ((KeyValuePair<int, string>)EditCarModelComboBox.SelectedItem).Key;

            if (updateID != null && updateNumber != null && updateMileage != null && updatePrice != null &&
                updateCondition != null && updateColor != null && updateAmount != null &&
                int.TryParse(updateID, out int id))
            {
                try
                {
                    var data = cars.GetData();
                    string originalNumber = null;
                    bool carExists = false;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == updateID)
                        {
                            originalNumber = row["Number"].ToString();
                            carExists = true;
                            break;
                        }
                    }

                    if (!carExists)
                    {
                        MessageBox.Show($"Автомобиль с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    MessageBoxResult confirm = MessageBox.Show(
                        $"Вы уверены, что хотите изменить автомобиль?\n\n" +
                        $"ID: {id}\n" +
                        $"Старый номер: {originalNumber}\n" +
                        $"Новый номер: {updateNumber}",
                        "Подтверждение изменения",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (confirm == MessageBoxResult.Yes)
                    {
                        cars.UpdateCar(updateNumber, Convert.ToInt32(updateMileage), Convert.ToDecimal(updatePrice),
                                       updateCondition, updateColor, Convert.ToInt32(updateAmount), carModelId);
                        MessageBox.Show("Автомобиль успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearBoxes();
                        parentWindow?.RefreshCarsTable();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Number"))
                    {
                        MessageBox.Show("Автомобиль с таким номером уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            string delID = Validation.ValidateInt(IDbox);

            if (delID != null && int.TryParse(delID, out int id))
            {
                try
                {
                    var data = cars.GetData();
                    string carNumber = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == delID)
                        {
                            carNumber = row["Number"].ToString();
                            break;
                        }
                    }

                    if (carNumber != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы уверены, что хотите удалить автомобиль?\n\n" +
                            $"ID: {id}\n" +
                            $"Номер: {carNumber}\n\n" +
                            "ВНИМАНИЕ: Если этот автомобиль есть в заказах, удаление будет невозможно!",
                            "Подтверждение удаления",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            cars.DeleteCar(id);
                            MessageBox.Show("Автомобиль успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearBoxes();
                            parentWindow?.RefreshCarsTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Автомобиль с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("REFERENCE"))
                    {
                        MessageBox.Show("Невозможно удалить автомобиль, так как он есть в заказах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}