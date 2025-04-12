using AppWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public UserManagement(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
            LoadCombobox();
        }
        ContactManagementContext context = new ContactManagementContext();

        void LoadData()
        {
            dgData.ItemsSource = null;
            var listData = context.Users
                .Select(m => new
                {
                    m.Username,
                    m.FullName,
                    m.Email,
                    m.Phone,
                    m.UserId,
                    m.Status,
                    m.PasswordHash,
                    m.CreatedAt,
                    role=m.Role==1?"Admin":"Customer"
                })
                .ToList();
            dgData.ItemsSource = listData;
        }
        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var userItem = dgData.SelectedItem as dynamic;
            if (userItem == null) return;
            else
            {
                int idItem = userItem.UserId;
                var userDeleted = context.Users.SingleOrDefault(m => m.UserId == idItem);
                context.Users.Remove(userDeleted);
                context.SaveChanges();
                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                AddNotification($"Admin{UserId} deleted User{idItem}");
                LoadData();
            }
        }

        void LoadCombobox()
        {
            //var listData = context.Users.ToList();
            cboRole.Items.Add("Role");
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Customer");
            cboRole.SelectedIndex = 0;
        }
        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboRole.SelectedItem.Equals("Admin"))
            {
                var listData = context.Users.Where(m => m.Role == 1).Select(m => new
                {
                    m.Username,
                    m.FullName,
                    m.Email,
                    m.Phone,
                    m.UserId,
                    m.Status,
                    role = m.Role == 1 ? "Admin" : "Customer"
                })
                .ToList();
                dgData.ItemsSource = listData;
            }
            else if (cboRole.SelectedItem.Equals("Customer"))
            {
                var listData = context.Users.Where(m => m.Role == 2).Select(m => new
                {
                    m.Username,
                    m.FullName,
                    m.Email,
                    m.Phone,
                    m.UserId,
                    m.Status,
                    role = m.Role == 1 ? "Admin" : "Customer"
                })
                .ToList();
                dgData.ItemsSource = listData;
            }
            else LoadData();
        }
        private void AddNotification( string message)
        {
            Notification notify = new Notification
            {
                UserId=UserId,
                Message = message,
                Status = "Unread",
                CreatedAt = DateTime.Now
            };

            context.Notifications.Add(notify);
            context.SaveChanges();
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string search = txtSearch.Text;
            var listData=context.Users.Where(m=>m.Phone.Contains(search) || m.FullName.Contains(search)).Select(m => new
            {
                m.Username,
                m.FullName,
                m.Email,
                m.Phone,
                m.UserId,
                m.Status,
                role = m.Role == 1 ? "Admin" : "Customer"
            })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            cboRole.SelectedIndex = 0;
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var userItem = dgData.SelectedItem as dynamic;
            if (userItem == null) return;
            else
            {
                txbId.Text = userItem.UserId.ToString();
                txtUser.Text = userItem.Username;
                txtContactName.Text = userItem.FullName;
                pbPassword.Password = new string('●', 8);
                txtPhone.Text = userItem.Phone;
                txtEmail.Text = userItem.Email;
                txtStatus.Text = userItem.Status;
                dpTime.Text = userItem.CreatedAt.ToString();
                txtRole.Text = userItem.role;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(pbPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new User()
            {
                Username = txtUser.Text,
                PasswordHash = pbPassword.Password, // Bạn nên hash nếu bảo mật!
                FullName = txtContactName.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                Avatar="",
                Status = "Active",
                CreatedAt = dpTime.SelectedDate ?? DateTime.Now,
                Role = txtRole.Text.Equals("Admin")?1:2
            };

            context.Users.Add(newUser);
            context.SaveChanges();
            AddNotification($"Admin{UserId} Add User{newUser.UserId}");
            MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txbId.Text, out int userId))
            {
                MessageBox.Show("Không xác định được ID người dùng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = context.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy người dùng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            user.Username = txtUser.Text;
            user.FullName = txtContactName.Text;
            user.Phone = txtPhone.Text;
            user.Email = txtEmail.Text;
            user.Status = txtStatus.Text;
            user.CreatedAt = dpTime.SelectedDate ?? DateTime.Now;
            user.Role = txtRole.Text.Equals("Admin") ? 1 : 2;

            // Nếu bạn cho phép đổi mật khẩu (nếu cần)
            if (!string.IsNullOrWhiteSpace(pbPassword.Password) && pbPassword.Password != "********")
            {
                user.PasswordHash = pbPassword.Password;
            }
            AddNotification($"Admin{UserId} Update User{user.UserId}");
            context.SaveChanges();

            MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }


        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            txbId.Text = "";
            txtUser.Text = "";
            txtContactName.Clear();
            pbPassword.Password = new string('●', 8);
            txtPhone.Clear();
            txtEmail.Clear();
            txtStatus.Clear();
            dpTime.Text = "";
            txtRole.Clear();
        }
    }
}
