using AppWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppWPF.pages
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class Logs : Page
    {
        public Logs()
        {
            InitializeComponent();
            LoadData();
        }

        ContactManagementContext context = new ContactManagementContext();

        private int pageSize = 20; // số lượng mục trên mỗi trang
        private int currentPage = 1;
        private int totalPageGenres = 1;
        private int totalPage = 1;
        void LoadData()
        {
            dgData.ItemsSource = null;
            var listGenres = context.Notifications
                .Select(m => new
                {
                    m.NotificationId,
                    userName = m.User.FullName,
                    m.Message,
                    m.Status,
                    m.CreatedAt
                })
                .ToList();
            totalPageGenres = (int)Math.Ceiling(listGenres.Count / (double)pageSize);
            var pagedGenre = listGenres
                         .Skip((currentPage - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
            dgData.ItemsSource = pagedGenre;
            lblPageInfoGnres.Content = $"Page {currentPage} of {totalPageGenres}";
        }

        private void FirstPageGenre_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        private void PreviousPageGenre_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void NextPageGenre_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPageGenres)
            {
                currentPage++;
                LoadData();
            }
        }

        private void LastPageGenre_Click(object sender, RoutedEventArgs e)
        {
            currentPage = totalPageGenres;
            LoadData();
        }

        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itemSelect = dgData.SelectedItem as dynamic;
            if (itemSelect == null) return;
            else
            {
                int idItem = itemSelect.NotificationId;
                var selectedItem = context.Notifications.SingleOrDefault(m => m.NotificationId == idItem);
                context.Notifications.Remove(selectedItem);
                //MessageBox.Show("Đã xóa thông báo thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                context.SaveChanges();
                LoadData();
            }
        }

        private void txtName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var listData=context.Notifications.Where(m=>m.User.FullName.Contains(txtName.Text)).Select(m => new
            {
                m.NotificationId,
                userName = m.User.FullName,
                m.Message,
                m.Status,
                m.CreatedAt
            }).ToList();
            dgData.ItemsSource = listData;
        }

        private void dbSearchTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                var listData = context.Notifications.Where(m => m.CreatedAt >= dpSearchTime.SelectedDate)
                .Select(m => new
                {
                    m.NotificationId,
                    userName = m.User.FullName,
                    m.Message,
                    m.Status,
                    m.CreatedAt
                }).ToList();
                dgData.ItemsSource = listData;
            }
            else
            {
                var listData = context.Notifications.Where(m => m.CreatedAt >= dpSearchTime.SelectedDate && m.User.FullName.Contains(txtName.Text))
                .Select(m => new
                {
                    m.NotificationId,
                    userName = m.User.FullName,
                    m.Message,
                    m.Status,
                    m.CreatedAt
                }).ToList();
                dgData.ItemsSource = listData;
            }
            
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            dpSearchTime.Text = "";
            LoadData();
        }
    }
}
