using laba5.AutoDBDataSetTableAdapters;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public partial class CarStatusPage : Page
    {
        private CarStatusTableAdapter carStatus = new CarStatusTableAdapter();

        private AdminWindow parentWindow;

        public CarStatusPage(AdminWindow admin = null)
        {
            InitializeComponent();
            parentWindow = admin;
        }

        private void ClearBoxes()
        {
            StatusBox.Text = string.Empty;
            IDbox.Text = string.Empty;
            EditStatusBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newStatus = Validation.ValidateRussianInput(StatusBox);
            if (newStatus != null)
            {
                try
                {
                    carStatus.Insert(newStatus);
                    MessageBox.Show("Статус успешно добавлен!");
                    ClearBoxes();
                    parentWindow?.RefreshCarStatusTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                    return;
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string updateID = Validation.ValidateInt(IDbox);
            string updateStatus = Validation.ValidateRussianInput(EditStatusBox);

            if (updateStatus != null && updateID != null && int.TryParse(updateID, out int id))
            {
                try
                {
                    var data = carStatus.GetData();
                    string originalStatus = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == updateID)
                        {
                            originalStatus = row["CarStatus"].ToString();
                            break;
                        }
                    }

                    if (originalStatus != null)
                    {
                        carStatus.UpdateCarStatus(updateStatus, id);
                        MessageBox.Show("Статус успешно обновлен!");
                        ClearBoxes();
                        parentWindow?.RefreshCarStatusTable();
                    }
                    else
                    {
                        MessageBox.Show("Статус с таким ID не найден.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении: " + ex.Message);
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
                    var data = carStatus.GetData();
                    string statusToDelete = null;

                    foreach (DataRow row in data.Rows)
                    {
                        if (row["ID"].ToString() == delID)
                        {
                            statusToDelete = row["CarStatus"].ToString();
                            break;
                        }
                    }

                    if (statusToDelete != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы уверены, что хотите удалить статус '{statusToDelete}'?",
                            "Подтверждение",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            carStatus.DeleteCarStatus(id);
                            MessageBox.Show("Статус успешно удален!");
                            ClearBoxes();
                            parentWindow?.RefreshCarStatusTable();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Статус с таким ID не найден.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }
        }
    }
}