using AppWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppWPF.pagesUser
{
    /// <summary>
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public Notifications(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
        }

        ContactManagementContext context = new ContactManagementContext();

        private int pageSize = 10; // số lượng mục trên mỗi trang
        private int currentPage = 1;
        private int totalPageGenres = 1;
        private int totalPage = 1;

        void LoadData()
        {
            dgData.ItemsSource = null;
            var listGenres = context.Logs.Where(m=>m.UserId==UserId)
                .Select(m => new
                {
                    m.LogId,
                    contactName = m.Contact.FullName,
                    m.Action,
                    m.Details,
                    m.UserId,
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
            currentPage = totalPage;
            LoadData();
        }

        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itemSelect = dgData.SelectedItem as dynamic;
            if (itemSelect == null) return;
            else
            {
                int idLog = itemSelect.LogId;
                var logDeleted = context.Logs.SingleOrDefault(m => m.LogId == idLog);
                context.Logs.Remove(logDeleted);
                context.SaveChanges();
                LoadData();
            }
        }

        private void txtName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var listData = context.Logs.Where(m => m.Contact.FullName.Contains(txtName.Text))
                .Select(m => new
                {
                    m.LogId,
                    contactName = m.Contact.FullName,
                    m.Action,
                    m.Details,
                    m.UserId,
                    m.CreatedAt
                })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void dpSearchTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var listData = context.Logs.Where(m => m.CreatedAt>=dpSearchTime.SelectedDate)
                .Select(m => new
                {
                    m.LogId,
                    contactName = m.Contact.FullName,
                    m.Action,
                    m.Details,
                    m.UserId,
                    m.CreatedAt
                })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            dpSearchTime.Text = "";
            LoadData();
        }
    }
}
