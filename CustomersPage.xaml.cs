using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class CustomersPage : Page
    {
        private CustomersTableAdapter customers = new CustomersTableAdapter();
        private AccountsTableAdapter accounts = new AccountsTableAdapter();
        private AdminWindow parentWindow;

        public CustomersPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            try
            {
                var accountsData = accounts.GetData();
                var accountsList = new List<KeyValuePair<int, string>>();

                foreach (DataRow row in accountsData.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string login = row["Login"].ToString();
                    int roleId = Convert.ToInt32(row["Role_ID"]);
                    string role = roleId == 1 ? "Админ" : "Пользователь";
                    string displayText = $"{login} ({role}) - ID: {id}";

                    accountsList.Add(new KeyValuePair<int, string>(id, displayText));
                }

                AccountComboBox.ItemsSource = accountsList;
                EditAccountComboBox.ItemsSource = accountsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке аккаунтов: " + ex.Message);
            }
        }

        public void RefreshAccountsList()
        {
            LoadAccounts();
        }

        private void ClearBoxes()
        {
            FirstnameBox.Text = string.Empty;
            SurnameBox.Text = string.Empty;
            PatronymicBox.Text = string.Empty;
            PhoneBox.Text = string.Empty;
            AccountComboBox.SelectedIndex = -1;

            IDbox.Text = string.Empty;
            EditFirstnameBox.Text = string.Empty;
            EditSurnameBox.Text = string.Empty;
            EditPatronymicBox.Text = string.Empty;
            EditPhoneBox.Text = string.Empty;
            EditAccountComboBox.SelectedIndex = -1;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstname = Validation.ValidateRussianInput(FirstnameBox);
            string surname = Validation.ValidateRussianInput(SurnameBox);
            string patronymic = Validation.ValidateRussianInput(PatronymicBox);
            string phone = Validation.ValidatePhone(PhoneBox);

            if (AccountComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите аккаунт для покупателя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (firstname != null && surname != null && phone != null)
            {
                try
                {
                    int accountId = ((KeyValuePair<int, string>)AccountComboBox.SelectedItem).Key;

                    var existingData = customers.GetData();
                    foreach (DataRow row in existingData.Rows)
                    {
                        if (Convert.ToInt32(row["Account_ID"]) == accountId)
                        {
                            MessageBox.Show("Этот аккаунт уже привязан к другому покупателю!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    foreach (DataRow row in existingData.Rows)
                    {
                        if (row["Phone"].ToString() == phone)
                        {
                            MessageBox.Show("Покупатель с таким номером телефона уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    customers.Insert(firstname, surname, patronymic, phone, accountId);
                    MessageBox.Show("Покупатель успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearBoxes();
                    parentWindow?.RefreshCustomersTable();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Phone"))
                    {
                        MessageBox.Show("Покупатель с таким номером телефона уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            string updateFirstname = Validation.ValidateRussianInput(EditFirstnameBox);
            string updateSurname = Validation.ValidateRussianInput(EditSurnameBox);
            string updatePatronymic = Validation.ValidateRussianInput(EditPatronymicBox);
            string updatePhone = Validation.ValidatePhone(EditPhoneBox);

            if (EditAccountComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите новый аккаунт для покупателя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (updateID != null && updateFirstname != null && updateSurname != null && updatePhone != null &&
                int.TryParse(updateID, out int id))
            {
                try
                {
                    var data = customers.GetData();
                    string originalFirstname = null;
                    string originalSurname = null;
                    bool customerExists = false;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == updateID)
                        {
                            originalFirstname = row["Firstname"].ToString();
                            originalSurname = row["Surname"].ToString();
                            customerExists = true;
                            break;
                        }
                    }

                    if (!customerExists)
                    {
                        MessageBox.Show($"Покупатель с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int accountId = ((KeyValuePair<int, string>)EditAccountComboBox.SelectedItem).Key;

                    var existingData = customers.GetData();
                    foreach (DataRow row in existingData.Rows)
                    {
                        if (Convert.ToInt32(row["Account_ID"]) == accountId && row["ID"].ToString() != updateID)
                        {
                            MessageBox.Show("Этот аккаунт уже привязан к другому покупателю!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    foreach (DataRow row in existingData.Rows)
                    {
                        if (row["Phone"].ToString() == updatePhone && row["ID"].ToString() != updateID)
                        {
                            MessageBox.Show("Покупатель с таким номером телефона уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    MessageBoxResult confirm = MessageBox.Show(
                        $"Вы уверены, что хотите изменить покупателя?\n\n" +
                        $"ID: {id}\n" +
                        $"Старый покупатель: {originalFirstname} {originalSurname}\n" +
                        $"Новый покупатель: {updateFirstname} {updateSurname}",
                        "Подтверждение изменения",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (confirm == MessageBoxResult.Yes)
                    {
                        customers.UpdateCustomer(updateFirstname, updateSurname, updatePatronymic, updatePhone, accountId, id);
                        MessageBox.Show("Покупатель успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearBoxes();
                        parentWindow?.RefreshCustomersTable();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Phone"))
                    {
                        MessageBox.Show("Покупатель с таким номером телефона уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    var data = customers.GetData();
                    string customerFirstname = null;
                    string customerSurname = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == delID)
                        {
                            customerFirstname = row["Firstname"].ToString();
                            customerSurname = row["Surname"].ToString();
                            break;
                        }
                    }

                    if (customerFirstname != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы уверены, что хотите удалить покупателя?\n\n" +
                            $"ID: {id}\n" +
                            $"Покупатель: {customerFirstname} {customerSurname}\n\n" +
                            "ВНИМАНИЕ: Если этот покупатель связан с заказами, удаление будет невозможно!",
                            "Подтверждение удаления",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            customers.DeleteCustomer(id);
                            MessageBox.Show("Покупатель успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearBoxes();
                            parentWindow?.RefreshCustomersTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Покупатель с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("REFERENCE"))
                    {
                        MessageBox.Show("Невозможно удалить покупателя, так как он связан с заказами!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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