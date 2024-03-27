using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class RegPage : Page
    {
        AccountsTableAdapter accounts = new AccountsTableAdapter();

        public RegPage()
        {
            InitializeComponent();
        }

        private void ClearBoxes()
        {
            LoginBox.Text = string.Empty;
            PasswordBox.Text = string.Empty;
            IDBox.Text = string.Empty;
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string newID = Validation.ValidateInt(IDBox);
            string newLogin = Validation.ValidateInput(LoginBox);
            string newPassword = Validation.ValidateInput(PasswordBox);

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
            string updateID = Validation.ValidateInt(IDBox);
            string updateLogin = Validation.ValidateInput(LoginBox);
            string updatePassword = Validation.ValidateInput(PasswordBox);
            
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
            string deleteID = Validation.ValidateInt(IDBox);
            if(deleteID != null)
            {
                try
                {
                    accounts.DeleteAccount(Convert.ToInt32(deleteID));
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.FillBox();
                    adminWindow.GetFullData();
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
