using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace laba5
{
    public partial class RolesPage : Page
    {
        private RolesTableAdapter roles = new RolesTableAdapter();
        private AdminWindow parentWindow;

        public RolesPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
        }

        private void ClearBoxes()
        {
            RoleBox.Text = string.Empty;
            StatusBox.Text = string.Empty;
            IDbox.Text = string.Empty;
            EditRoleBox.Text = string.Empty;
            EditStatusBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newRole = Validation.ValidateRussianInput(RoleBox);
            string newStatus = Validation.ValidateRussianInput(StatusBox);

            if (newRole != null && newStatus != null)
            {
                try
                {
                    roles.Insert(newRole, newStatus);
                    MessageBox.Show("Роль успешно добавлена!");
                    ClearBoxes();
                    parentWindow?.RefreshRolesTable();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        MessageBox.Show("Такая роль уже существует!");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string updateID = Validation.ValidateInt(IDbox);
            string updateRole = Validation.ValidateRussianInput(EditRoleBox);
            string updateStatus = Validation.ValidateRussianInput(EditStatusBox);

            if (updateRole != null && updateStatus != null && updateID != null && int.TryParse(updateID, out int id))
            {
                try
                {
                    var data = roles.GetData();
                    string originalRole = null;
                    string originalStatus = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == updateID)
                        {
                            originalRole = row["Role"].ToString();
                            originalStatus = row["Status"].ToString();
                            break;
                        }
                    }

                    if (originalRole != null)
                    {
                        MessageBoxResult confirm = MessageBox.Show(
                            $"Вы уверены, что хотите изменить роль?\n\n" +
                            $"ID: {id}\n" +
                            $"Старая роль: {originalRole}\n" +
                            $"Новая роль: {updateRole}\n" +
                            $"Старый статус: {originalStatus}\n" +
                            $"Новый статус: {updateStatus}",
                            "Подтверждение изменения",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (confirm == MessageBoxResult.Yes)
                        {
                            roles.UpdateRole(updateRole, updateStatus, id);
                            MessageBox.Show("Роль успешно обновлена!");
                            ClearBoxes();
                            parentWindow?.RefreshRolesTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Роль с таким ID не найдена.");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        MessageBox.Show("Такая роль уже существует!");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении: " + ex.Message);
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
                    var data = roles.GetData();
                    string roleToDelete = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == delID)
                        {
                            roleToDelete = row["Role"].ToString();
                            break;
                        }
                    }

                    if (roleToDelete != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы уверены, что хотите удалить роль '{roleToDelete}'?\n\n" +
                            "ВНИМАНИЕ: Если эта роль используется в аккаунтах, удаление будет невозможно!",
                            "Подтверждение удаления",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            roles.DeleteRole(id);
                            MessageBox.Show("Роль успешно удалена!");
                            ClearBoxes();
                            parentWindow?.RefreshRolesTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Роль с таким ID не найдена.");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("REFERENCE"))
                    {
                        MessageBox.Show("Невозможно удалить роль, так как она используется в аккаунтах!");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении: " + ex.Message);
                    }
                }
            }
        }
    }
}