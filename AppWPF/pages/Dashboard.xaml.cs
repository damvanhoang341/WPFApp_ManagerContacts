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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadData();
        }

        ContactManagementContext context = new ContactManagementContext();

        void LoadData()
        {
            dgData.ItemsSource = null;
            var listData = context.ContactGroups
                .Select(m => new
                {
                    group=m.Group.GroupName,
                    contact = m.Contact.FullName,
                    phone = m.Contact.Phone,
                    email = m.Contact.Email,
                    birth = m.Contact.Birthday,
                    m.AddedAt
                })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchPhone_Click(object sender, RoutedEventArgs e)
        {

        }

        void LoadCboGroup()
        {
            var listData = context.Groups
        .GroupBy(g => g.GroupName)
        .Select(g => g.First()) // lấy nhóm đầu tiên theo tên
        .ToList();

            cboGroup.ItemsSource = listData;
            cboGroup.DisplayMemberPath = "GroupName";
            cboGroup.SelectedValuePath = "GroupName";
        }
        private void cboGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
