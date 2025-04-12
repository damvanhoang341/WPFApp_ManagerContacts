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
using System.Windows.Shapes;

namespace AppWPF
{
    /// <summary>
    /// Interaction logic for MainWindowUser.xaml
    /// </summary>
    public partial class MainWindowUser : Window
    {
        ContactManagementContext context = new ContactManagementContext();
        public string Username { get; set; }
        public int UserId { get; set; }
        public MainWindowUser(string username, int userId)
        {
            InitializeComponent();
            Username = username;
            UserId = userId;

            var userSelect = context.Users.SingleOrDefault(m => m.UserId == UserId);
            lblUsername.Content = userSelect.FullName;
            MainFrame.Navigate(new pagesUser.ContactsManagement(Username, UserId));
        }

        // Sự kiện khi click vào "Dashboard"
        private void SidebarUser_DashboardClicked(object sender, EventArgs e)
        {
            MainFrame.Navigate(new pagesUser.Group(Username, UserId));
        }

        // Sự kiện khi click vào "Contacts Management"
        private void SidebarUser_ContactsManagementClicked(object sender, EventArgs e)
        {
            MainFrame.Navigate(new pagesUser.ContactsManagement(Username, UserId));
        }

        // Sự kiện khi click vào "Favorite Contacts Management"
        private void SidebarUser_FavoriteContactsManagementClicked(object sender, EventArgs e)
        {
            MainFrame.Navigate(new pagesUser.FavoriteContacts(Username, UserId));
        }

        // Sự kiện khi click vào "Notification Management"
        private void SidebarUser_NotificationManagementClicked(object sender, EventArgs e)
        {
            MainFrame.Navigate(new pagesUser.Notifications(Username, UserId));
        }

        // Sự kiện khi click vào "Profile"
        private void SidebarUser_ProfileClicked(object sender, EventArgs e)
        {
            MainFrame.Navigate(new pagesUser.Profile(Username, UserId));
        }
        private void SidebarUser_LogOutClicked(object sender, EventArgs e)
        {
            LoginWindow log = new LoginWindow();
            this.Close();
            log.Show();
        }
    }
}
