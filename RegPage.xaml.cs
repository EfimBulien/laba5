using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace laba5
{
    public partial class RegPage : Page
    {
        AccountsTableAdapter accounts = new AccountsTableAdapter();

        public RegPage()
        {
            InitializeComponent();
            
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string newLogin = ValidateInput(LoginBox);
            string newPassword = ValidateInput(PasswordBox);
            string newID = ValidateInput(IDBox);

            if (newLogin != null && newPassword != null && newID != null)
            {
                try
                {
                    accounts.NewAccount(newLogin, newPassword, Convert.ToInt32(newID));
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
            string updateLogin = ValidateInput(LoginBox);
            string updatePassword = ValidateInput(PasswordBox);
            string updateID = ValidateInput(IDBox);

            if (updateLogin != null && updatePassword != null && updateID != null)
            {
                try
                {
                    accounts.UpdateAccount(updateLogin, updatePassword, Convert.ToInt32(updateID));
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
            var deleteID = ValidateInput(IDBox);
            if(deleteID != null)
            {
                try
                {
                    
                    accounts.DeleteAccount(Convert.ToInt32(deleteID));
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.FillBox();
                    adminWindow.gfff();
                    ClearBoxes();
                }
                catch
                {
                    MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                    return;
                }
            }
        }

        private void ClearBoxes()
        {
            LoginBox.Text = string.Empty;
            PasswordBox.Text = string.Empty;
            IDBox.Text = string.Empty;
        }

        private string ValidateInput(TextBox textBox)
        {
            string input = textBox.Text;
            if (ContainsInvalidCharacters(input))
            {
                MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                ClearBoxes();
                return null;
            }
            else return input;
        }

        private bool ContainsInvalidCharacters(string input)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            return regex.IsMatch(input);
        }
    }
}
