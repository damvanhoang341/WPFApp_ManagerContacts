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
    /// Interaction logic for LoginReturnWindow.xaml
    /// </summary>
    public partial class LoginReturnWindow : Window
    {
        public LoginReturnWindow()
        {
            InitializeComponent();
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
                    if (string.IsNullOrEmpty(pwPassword2.Password))
                    {
                        errorMessages.AppendLine("Nhập mật khẩu xác thực !!!");
                    }
                    else
                    {
                        if (!pwPassword.Password.Equals(pwPassword2.Password))
                        {
                            errorMessages.AppendLine("Mật khẩu không khớp!!!");
                        }
                    }

                    if (errorMessages.Length > 0)
                    {
                        MessageBox.Show(errorMessages.ToString(), "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Dừng thực thi nếu có lỗi nhập liệu
                    }

                    var userLog = context.Users
                    .FirstOrDefault(m => m.Username == txtUser.Text && m.PasswordHash == pwPassword.Password);

                    if (userLog == null)
                        MessageBox.Show("Không tìm thấy tài khoản!");
                    else if(userLog.Role==2)
                    {
                        MainWindowUser mainWindow = new MainWindowUser(userLog.Username, userLog.UserId);

                        mainWindow.Show();
                        this.Close();
                    }else if(userLog.Role == 1)
                    {
                        MainWindow mainWindow = new MainWindow(userLog.Username, userLog.UserId);
                        this.Close();
                        mainWindow.Show();
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
