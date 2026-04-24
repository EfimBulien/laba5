using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class CarPage : Page
    {
        private CarsTableAdapter cars = new CarsTableAdapter();

        public CarPage()
        {
            InitializeComponent();
        }

        private void ClearBoxes()
        {
            IDBox.Text = string.Empty;
            NumberBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string id = Validation.ValidateInt(IDBox);
            string number = Validation.ValidateCarNumber(NumberBox);
            string mileage = Validation.ValidateMileage(MileageBox);
            string price = Validation.ValidateInt(PriceBox);
            string condition = Validation.ValidateRussianInput(ConditionBox);
            string color = Validation.ValidateRussianInput(ColorBox);
            string amount = Validation.ValidateInt(AmountBox);

            if (id != null && number != null && mileage != null && price != null && condition != null && color != null && amount != null)
            {
                try
                {
                    cars.Insert(Convert.ToInt32(id), number, Convert.ToInt32(mileage), Convert.ToInt32(price), condition, color, Convert.ToInt32(amount));
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
            string number = Validation.ValidateCarNumber(NumberBox);
            string mileage = Validation.ValidateMileage(MileageBox);
            string price = Validation.ValidateInt(PriceBox);
            string condition = Validation.ValidateRussianInput(ConditionBox);
            string color = Validation.ValidateRussianInput(ColorBox);
            string amount = Validation.ValidateInt(AmountBox);

            if (id != null && number != null && mileage != null && price != null && condition != null && color != null && amount != null)
            {
                try
                {
                    cars.UpdateCar(number, Convert.ToInt32(mileage), Convert.ToInt32(price), condition, color, Convert.ToInt32(amount), Convert.ToInt32(id));
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
                    cars.DeleteCar(Convert.ToInt32(id));
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
