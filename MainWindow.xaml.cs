using laba5.AutoDBDataSetTableAdapters;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class MainWindow : Window
    {
        private AccountsTableAdapter accountsTable = new AccountsTableAdapter();

        private AdminWindow adminWindow = new AdminWindow();

        private UserWindow userWindow = new UserWindow();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResetMainWindow()
        {
            LoginBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            var allLogins = accountsTable.GetData().Rows;
            Auth(allLogins, login, password);
        }

        private void Auth(DataRowCollection allLogins, string login, string password)
        {
            bool userNotFound = true;

            for (int i = 0; i < allLogins.Count; i++)
            {
                if (allLogins[i][1].ToString() == login && allLogins[i][2].ToString() == password)
                {
                    int role = (int)allLogins[i][3];
                    switch (role)
                    {
                        case 1:
                            MessageBox.Show("Добро пожаловать, администратор!");
                            adminWindow.Show();
                            ResetMainWindow();
                            Close();
                            return;

                        case 2:
                            MessageBox.Show("Добро пожаловать, пользователь!");
                            userWindow.Show();
                            ResetMainWindow();
                            Close();
                            return;

                        default:
                            MessageBox.Show("Неизвестная роль пользователя.");
                            return;
                    }
                }
            }

            if (userNotFound)
            {
                MessageBox.Show("Неверный логин или пароль. Пожалуйста, попробуйте снова.");
                return;
            }
        }
    }
}