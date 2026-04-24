using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfBowlingClub.AppData;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Pages
{
    public partial class OrdersPage : Page
    {
        private DatabaseHelper db;
        private List<Orders> orders;

        public OrdersPage()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                orders = db.GetOrders();

                // Проверка что dgOrders не null
                if (dgOrders == null)
                {
                    MessageBox.Show("Ошибка инициализации таблицы заказов", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                FilterOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetStatusId(string statusName)
        {
            switch (statusName)
            {
                case "Новый": return 1;
                case "В обработке": return 2;
                case "Оплачен": return 3;
                case "Завершен": return 4;
                case "Отменен": return 5;
                default: return 0;
            }
        }

        private void FilterOrders()
        {
            try
            {
                if (cmbStatus == null || dgOrders == null) return;

                var selectedItem = cmbStatus.SelectedItem as ComboBoxItem;
                if (selectedItem == null) return;

                string statusText = selectedItem.Content.ToString();
                List<Orders> filtered = new List<Orders>();

                if (statusText == "Все заказы")
                {
                    filtered = orders;
                }
                else
                {
                    int statusId = GetStatusId(statusText);
                    filtered = orders.Where(o => o.Status == statusId).ToList();
                }

                dgOrders.ItemsSource = filtered;
                lblTotal.Text = $"Всего заказов: {filtered.Count} | Сумма: {filtered.Sum(o => o.TotalAmount):N0} ₽";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderStatusChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var combo = sender as ComboBox;
                var order = combo?.Tag as Orders;
                var selectedItem = combo?.SelectedItem as ComboBoxItem;

                if (order != null && selectedItem != null)
                {
                    int newStatus = GetStatusId(selectedItem.Content.ToString());
                    if (newStatus != order.Status)
                    {
                        db.UpdateOrderStatus(order.Id, newStatus);
                        LoadOrders();
                        MessageBox.Show("Статус заказа обновлен", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterOrders();
        }
    }
}