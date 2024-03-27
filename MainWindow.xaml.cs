using laba5.AutoDBDataSetTableAdapters;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class MainWindow : Window
    {
        AccountsTableAdapter accountsTable = new AccountsTableAdapter();
        AdminWindow adminWindow = new AdminWindow();
        UserWindow userWindow = new UserWindow();
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
            var allLogins = accountsTable.GetData().Rows;

            Auth(allLogins);
        }

        private void Auth(DataRowCollection allLogins)
        {
            for (int i = 0; i < allLogins.Count; i++)
            {
                if (allLogins[i][1].ToString() == LoginBox.Text &&
                    allLogins[i][2].ToString() == PasswordBox.Password)
                {
                    int role = (int)allLogins[i][3];
                    switch (role)
                    {
                        case 1:
                            adminWindow.Show();
                            ResetMainWindow();
                            Close();
                        return;

                        case 2:
                            userWindow.Show();
                            ResetMainWindow();
                            Close();
                        return;

                    }
                }
            }
        }
    }
}