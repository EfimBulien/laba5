using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class RegPage : Page
    {
        AccountsTableAdapter accounts = new AccountsTableAdapter();
        private AdminWindow parentWindow;

        public RegPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
        }

        private void ClearBoxes()
        {
            LoginBox.Text = string.Empty;
            PasswordBox.Text = string.Empty;
            RoleBox.SelectedIndex = -1;
            
            SearchLoginBox.Text = string.Empty;
            EditLoginBox.Text = string.Empty;
            EditPasswordBox.Text = string.Empty;
        }

        private int GetRoleID(string role)
        {
            return role == "Админ" ? 1 : 2;
        }

        private int GetUserIDByLogin(string login)
        {
            try
            {
                var data = accounts.GetData();
                foreach (DataRow row in data.Rows)
                {
                    if (row["Login"].ToString() == login)
                    {
                        return Convert.ToInt32(row["ID"]);
                    }
                }
                MessageBox.Show($"Пользователь с логином '{login}' не найден.");
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске пользователя: {ex.Message}");
                return -1;
            }
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string newLogin = Validation.ValidateLogin(LoginBox);
            string newPassword = Validation.ValidatePassword(PasswordBox);

            if (newLogin == null || newPassword == null)
            {
                return;
            }

            if (RoleBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите роль.");
                return;
            }

            try
            {
                int roleID = GetRoleID(RoleBox.SelectedItem.ToString());
                accounts.NewAccount(newLogin, newPassword, roleID);
                MessageBox.Show("Пользователь успешно создан!");
                ClearBoxes();
                parentWindow?.RefreshAccountsTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании пользователя: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string searchLogin = SearchLoginBox.Text.Trim();
            string updateLogin = EditLoginBox.Text.Trim();
            string updatePassword = EditPasswordBox.Text.Trim();

            if (string.IsNullOrEmpty(searchLogin))
            {
                MessageBox.Show("Введите логин пользователя для поиска.");
                return;
            }

            if (string.IsNullOrEmpty(updateLogin) || string.IsNullOrEmpty(updatePassword))
            {
                MessageBox.Show("Заполните новые логин и пароль.");
                return;
            }

            int userID = GetUserIDByLogin(searchLogin);
            if (userID == -1)
            {
                return;
            }

            try
            {
                accounts.UpdateAccount(updateLogin, updatePassword, userID);
                MessageBox.Show("Пользователь успешно обновлен!");
                ClearBoxes();
                parentWindow?.RefreshAccountsTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении пользователя: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string searchLogin = SearchLoginBox.Text.Trim();

            if (string.IsNullOrEmpty(searchLogin))
            {
                MessageBox.Show("Введите логин пользователя для удаления.");
                return;
            }

            int userID = GetUserIDByLogin(searchLogin);
            if (userID == -1)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пользователя '{searchLogin}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    accounts.DeleteAccount(userID);
                    MessageBox.Show("Пользователь успешно удален!");
                    ClearBoxes();
                    parentWindow?.RefreshAccountsTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}");
                }
            }
        }
    }
}
