using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string Username { get; set; }
    public int UserId { get; set; }
    public MainWindow(string username, int userId)
    {
        Username = username;
        UserId = userId;

        InitializeComponent();
        MainFrame.Navigate(new pages.UserManagement(Username, UserId));
    }
    // Sự kiện khi click vào "Dashboard"
    //private void SidebarAdmin_DashboardClicked(object sender, EventArgs e)
    //{
    //    MainFrame.Navigate(new pages.Dashboard());
    //}

    // Sự kiện khi click vào "User Management"
    private void SidebarAdmin_UserManagementClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.UserManagement(Username, UserId));
    }

    // Sự kiện khi click vào "Contacts Management"
    private void SidebarAdmin_ContactsManagementClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.ContactsManagement());
    }

    // Sự kiện khi click vào "Favorite Contacts Management"
    private void SidebarAdmin_FavoriteContactsManagementClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.FavoriteContacts(Username, UserId));
    }

    // Sự kiện khi click vào "Logs"
    private void SidebarAdmin_LogsClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.Logs());
    }
    private void SidebarAdmin_LogOutClicked(object sender, EventArgs e)
    {
        LoginWindow log = new LoginWindow();
        this.Close();
        log.Show();
    }

    // Sự kiện khi click vào "Groups Management"
    private void SidebarAdmin_GroupsManagementClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.GroupsManagement(Username, UserId));
    }

    // Sự kiện khi click vào "Notification Management"
    private void SidebarAdmin_NotificationManagementClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.NotificationManagement());
    }

    // Sự kiện khi click vào "Profile"
    private void SidebarAdmin_ProfileClicked(object sender, EventArgs e)
    {
        MainFrame.Navigate(new pages.Profile(Username, UserId));
    }
}