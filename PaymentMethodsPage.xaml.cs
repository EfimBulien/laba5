using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class PaymentMethodsPage : Page
    {
        private PaymentMethodsTableAdapter paymentMethods = new PaymentMethodsTableAdapter();
        private AdminWindow parentWindow;

        public PaymentMethodsPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
        }

        private void ClearBoxes()
        {
            PaymentMethodBox.Text = string.Empty;
            IDbox.Text = string.Empty;
            EditPaymentMethodBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newPaymentMethod = Validation.ValidateRussianInput(PaymentMethodBox);
            if (newPaymentMethod != null)
            {
                try
                {
                    paymentMethods.Insert(newPaymentMethod);
                    MessageBox.Show("Способ оплаты успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearBoxes();
                    parentWindow?.RefreshPaymentMethodsTable();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        MessageBox.Show("Такой способ оплаты уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            string updatePaymentMethod = Validation.ValidateRussianInput(EditPaymentMethodBox);

            if (updatePaymentMethod != null && updateID != null && int.TryParse(updateID, out int id))
            {
                try
                {
                    var data = paymentMethods.GetData();
                    string originalPaymentMethod = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == updateID)
                        {
                            originalPaymentMethod = row["PaymentMethod"].ToString();
                            break;
                        }
                    }

                    if (originalPaymentMethod != null)
                    {
                        MessageBoxResult confirm = MessageBox.Show(
                            $"Вы уверены, что хотите изменить способ оплаты?\n\n" +
                            $"ID: {id}\n" +
                            $"Старое значение: {originalPaymentMethod}\n" +
                            $"Новое значение: {updatePaymentMethod}",
                            "Подтверждение изменения",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (confirm == MessageBoxResult.Yes)
                        {
                            paymentMethods.UpdatePaymentMethod(updatePaymentMethod, id);
                            MessageBox.Show("Способ оплаты успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearBoxes();
                            parentWindow?.RefreshPaymentMethodsTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Способ оплаты с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        MessageBox.Show("Такой способ оплаты уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    var data = paymentMethods.GetData();
                    string paymentMethodToDelete = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == delID)
                        {
                            paymentMethodToDelete = row["PaymentMethod"].ToString();
                            break;
                        }
                    }

                    if (paymentMethodToDelete != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы уверены, что хотите удалить способ оплаты '{paymentMethodToDelete}'?\n\n" +
                            "ВНИМАНИЕ: Если этот способ оплаты используется в заказах, удаление будет невозможно!",
                            "Подтверждение удаления",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            paymentMethods.DeletePaymentMethod(id);
                            MessageBox.Show("Способ оплаты успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearBoxes();
                            parentWindow?.RefreshPaymentMethodsTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Способ оплаты с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("REFERENCE"))
                    {
                        MessageBox.Show("Невозможно удалить способ оплаты, так как он используется в заказах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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