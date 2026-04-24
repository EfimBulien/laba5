using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class EmployeesPage : Page
    {
        private EmployeesTableAdapter employees = new EmployeesTableAdapter();
        private AccountsTableAdapter accounts = new AccountsTableAdapter();
        private AdminWindow parentWindow;

        public EmployeesPage(AdminWindow admin = null)
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
            PostBox.Text = string.Empty;
            AccountComboBox.SelectedIndex = -1;

            IDbox.Text = string.Empty;
            EditFirstnameBox.Text = string.Empty;
            EditSurnameBox.Text = string.Empty;
            EditPatronymicBox.Text = string.Empty;
            EditPostBox.Text = string.Empty;
            EditAccountComboBox.SelectedIndex = -1;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstname = Validation.ValidateRussianInput(FirstnameBox);
            string surname = Validation.ValidateRussianInput(SurnameBox);
            string patronymic = Validation.ValidateRussianInput(PatronymicBox);
            string post = Validation.ValidateRussianInput(PostBox);

            if (AccountComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите аккаунт для сотрудника.");
                return;
            }

            if (firstname != null && surname != null && post != null)
            {
                try
                {
                    int accountId = ((KeyValuePair<int, string>)AccountComboBox.SelectedItem).Key;

                    var existingData = employees.GetData();
                    foreach (DataRow row in existingData.Rows)
                    {
                        if (Convert.ToInt32(row["Account_ID"]) == accountId)
                        {
                            MessageBox.Show("Этот аккаунт уже привязан к другому сотруднику!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    employees.Insert(firstname, surname, patronymic, post, accountId);
                    MessageBox.Show("Сотрудник успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearBoxes();
                    parentWindow?.RefreshEmployeesTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string updateID = Validation.ValidateInt(IDbox);
            string updateFirstname = Validation.ValidateRussianInput(EditFirstnameBox);
            string updateSurname = Validation.ValidateRussianInput(EditSurnameBox);
            string updatePatronymic = Validation.ValidateRussianInput(EditPatronymicBox);
            string updatePost = Validation.ValidateRussianInput(EditPostBox);

            if (EditAccountComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите новый аккаунт для сотрудника.");
                return;
            }

            if (updateID != null && updateFirstname != null && updateSurname != null && updatePost != null &&
                int.TryParse(updateID, out int id))
            {
                try
                {
                    var data = employees.GetData();
                    string originalFirstname = null;
                    string originalSurname = null;
                    bool employeeExists = false;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == updateID)
                        {
                            originalFirstname = row["Firstname"].ToString();
                            originalSurname = row["Surname"].ToString();
                            employeeExists = true;
                            break;
                        }
                    }

                    if (!employeeExists)
                    {
                        MessageBox.Show($"Сотрудник с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int accountId = ((KeyValuePair<int, string>)EditAccountComboBox.SelectedItem).Key;

                    var existingData = employees.GetData();
                    foreach (DataRow row in existingData.Rows)
                    {
                        if (Convert.ToInt32(row["Account_ID"]) == accountId && row["ID"].ToString() != updateID)
                        {
                            MessageBox.Show("Этот аккаунт уже привязан к другому сотруднику!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    MessageBoxResult confirm = MessageBox.Show(
                        $"Вы уверены, что хотите изменить сотрудника?\n\n" +
                        $"ID: {id}\n" +
                        $"Старый сотрудник: {originalFirstname} {originalSurname}\n" +
                        $"Новый сотрудник: {updateFirstname} {updateSurname}",
                        "Подтверждение изменения",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (confirm == MessageBoxResult.Yes)
                    {
                        employees.UpdateEmployee(updateFirstname, updateSurname, updatePatronymic, updatePost, accountId, id);
                        MessageBox.Show("Сотрудник успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearBoxes();
                        parentWindow?.RefreshEmployeesTable();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    var data = employees.GetData();
                    string employeeFirstname = null;
                    string employeeSurname = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == delID)
                        {
                            employeeFirstname = row["Firstname"].ToString();
                            employeeSurname = row["Surname"].ToString();
                            break;
                        }
                    }

                    if (employeeFirstname != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы уверены, что хотите удалить сотрудника?\n\n" +
                            $"ID: {id}\n" +
                            $"Сотрудник: {employeeFirstname} {employeeSurname}\n\n" +
                            "ВНИМАНИЕ: Если этот сотрудник связан с заказами, удаление будет невозможно!",
                            "Подтверждение удаления",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            employees.DeleteEmployee(id);
                            MessageBox.Show("Сотрудник успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearBoxes();
                            parentWindow?.RefreshEmployeesTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Сотрудник с ID = {id} не найден.", "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("REFERENCE"))
                    {
                        MessageBox.Show("Невозможно удалить сотрудника, так как он связан с заказами!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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