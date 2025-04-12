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

namespace AppWPF.pagesUser
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public Profile(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
        }

        ContactManagementContext context = new ContactManagementContext();

        void LoadData()
        {
            var userLoad = context.Users.SingleOrDefault(m => m.UserId == UserId);
            txtUser.Text = userLoad.Username;
            txtName.Text = userLoad.FullName;
            pbPassword.Password = userLoad.PasswordHash;
            txtEmail.Text = userLoad.Email;
            txtPhone.Text = userLoad.Phone;
            txtRole.Text= userLoad.Role == 1 ? "Admin" : "Customer";
        }

        private void cbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtPasswordVisible.Text = pbPassword.Password;
            pbPassword.Visibility = Visibility.Collapsed;
            txtPasswordVisible.Visibility = Visibility.Visible;
        }

        private void cbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            pbPassword.Password = txtPasswordVisible.Text;
            pbPassword.Visibility = Visibility.Visible;
            txtPasswordVisible.Visibility = Visibility.Collapsed;
        }
        void AddLogs(int userID)
        {
            Notification notification = new Notification()
            {
                UserId = userID,
                Message = "đã Update tài khoản.",
                Status = "Unread"
            };
            context.Notifications.Add(notification);
            context.SaveChanges();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||string.IsNullOrWhiteSpace(pbPassword.Password) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra định dạng email đơn giản
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var userLoad = context.Users.SingleOrDefault(m => m.UserId == UserId);
            if (userLoad != null)
            {
                userLoad.Username = txtUser.Text;
                userLoad.FullName = txtName.Text;
                userLoad.PasswordHash = pbPassword.Password;
                userLoad.Email = txtEmail.Text;
                userLoad.Phone = txtPhone.Text;
                userLoad.Avatar = "";
                userLoad.Status = "Active";

                context.Users.Update(userLoad);
                context.SaveChanges();
                AddLogs(UserId);
                MessageBox.Show("Cập nhật thành công! Vui lòng đăng nhập lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Đăng xuất: quay về màn hình login
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                // Đóng window chứa trang hiện tại (Profile.xaml)
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy người dùng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            txtUser.Text = "";
            txtName.Text = "";
            pbPassword.Password = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtRole.Clear();
        }
    }
}
