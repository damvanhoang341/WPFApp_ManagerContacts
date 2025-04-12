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

namespace AppWPF.components
{
    /// <summary>
    /// Interaction logic for SidebarAdmin.xaml
    /// </summary>
    public partial class SidebarAdmin : UserControl
    {

        //public event EventHandler DashboardClicked;
        public event EventHandler UserManagementClicked;
        public event EventHandler ContactsManagementClicked;
        public event EventHandler FavoriteContactsManagementClicked;
        public event EventHandler LogsClicked;
        public event EventHandler LogOutClick;
        public event EventHandler GroupsManagementClicked;
        public event EventHandler NotificationManagementClicked;
        public event EventHandler ProfileClicked;

        public SidebarAdmin()
        {
            InitializeComponent();
            string userName = "Admin";
            tblName.Text = userName;
        }

        //private void btnDashboard_Click(object sender, RoutedEventArgs e)
        //{
        //    DashboardClicked?.Invoke(this, EventArgs.Empty);
        //}

        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagementClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnContactsManagement_Click(object sender, RoutedEventArgs e)
        {
            ContactsManagementClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnFavoriteContactsManagement_Click(object sender, RoutedEventArgs e)
        {
            FavoriteContactsManagementClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            LogsClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LogOutClick?.Invoke(this, EventArgs.Empty);
        }

        private void btnGroupsManagement_Click(object sender, RoutedEventArgs e)
        {
            GroupsManagementClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnNotificationManagement_Click(object sender, RoutedEventArgs e)
        {
            NotificationManagementClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

