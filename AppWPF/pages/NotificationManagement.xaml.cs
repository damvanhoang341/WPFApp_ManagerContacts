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
    /// Interaction logic for NotificationManagement.xaml
    /// </summary>
    public partial class NotificationManagement : Page
    {
        public NotificationManagement()
        {
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
            var listGenres = context.Logs
                .Select(m => new
                {
                    m.LogId,
                    userName = m.User.FullName,
                    m.Details,
                    m.Action,
                    contact=m.Contact.FullName,
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
            var itemSelected = dgData.SelectedItem as dynamic;
            if (itemSelected == null) return;
            else
            {
                int idItem = itemSelected.LogId;
                var selectedItem = context.Logs.SingleOrDefault(m => m.LogId == idItem);
                context.Logs.Remove(selectedItem);
                context.SaveChanges();
                LoadData();
            }
        }

        private void txtName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var listData = context.Logs.Where(m=>m.User.FullName.Contains(txtName.Text))
                .Select(m => new
                {
                    m.LogId,
                    userName = m.User.FullName,
                    m.Details,
                    m.Action,
                    contact = m.Contact.FullName,
                    m.CreatedAt
                })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void dpSearchTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var listData = context.Logs.Where(m => m.CreatedAt >= dpSearchTime.SelectedDate)
                .Select(m => new
                {
                    m.LogId,
                    userName = m.User.FullName,
                    m.Details,
                    m.Action,
                    contact = m.Contact.FullName,
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
