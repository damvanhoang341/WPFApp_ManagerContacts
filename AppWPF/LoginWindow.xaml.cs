using AppWPF.Models;
using System.Text;
using System.Windows;

namespace AppWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private int count = 0;

        ContactManagementContext context1 = new ContactManagementContext();

        void AddLogs(int userID)
        {
            Notification notification = new Notification()
            {
                UserId = userID,
                Message = "đã login vô tài khoản.",
                Status="Unread"
            };
            context1.Notifications.Add(notification);
            context1.SaveChanges();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            using (var context = new ContactManagementContext())
            {

                try
                {
                    StringBuilder errorMessages = new StringBuilder();

                    if (string.IsNullOrEmpty(txtUser.Text))
                    {
                        errorMessages.AppendLine("Nhập tên đăng nhập !!!");
                    }
                    if (string.IsNullOrEmpty(pwPassword.Password))
                    {
                        errorMessages.AppendLine("Nhập mật khẩu !!!");
                    }
                    if (errorMessages.Length > 0)
                    {
                        MessageBox.Show(errorMessages.ToString(), "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Dừng thực thi nếu có lỗi nhập liệu
                    }


                    var userLog = context.Users.FirstOrDefault(m => m.Username == txtUser.Text && m.PasswordHash == pwPassword.Password);

                    if (userLog == null)
                    {
                        count++;
                        MessageBox.Show("Không tìm thấy tài khoản!");

                        if (count > 1)
                        {
                            LoginReturnWindow loginReturn = new LoginReturnWindow();
                            this.Close();
                            loginReturn.Show();
                        }
                    }
                    else if (userLog.Role == 1)
                    {
                        GlobalSession.CurrentUser = userLog;
                        MainWindow mainWindow = new MainWindow(userLog.Username, userLog.UserId);
                        AddLogs(userLog.UserId);
                        mainWindow.Show();
                        this.Close();
                    }
                    else if (userLog.Role == 2)
                    {
                        GlobalSession.CurrentUser = userLog;
                        MainWindowUser mainWindow = new MainWindowUser(userLog.Username, userLog.UserId);
                        AddLogs(userLog.UserId);
                        mainWindow.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi không xác định: " + ex.Message, "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangedPassWindow changedPassWindow = new ChangedPassWindow();
            this.Close();
            changedPassWindow.Show();
        }

        private void btnRegistor_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            this.Close();
            registerWindow.Show();
        }
    }
}
