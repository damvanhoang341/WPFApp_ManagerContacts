using AppWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        ContactManagementContext context = new ContactManagementContext();

        void AddLogs(int idUser, string message, string status)
        {
            Notification newlog = new Notification()
            {
                UserId = idUser,
                Message = message,
                Status = status
            };
            context.Notifications.Add(newlog);
            context.SaveChanges();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string errolMessage = "";
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                errolMessage += "Full name is not null\n";
            }
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                errolMessage += "Name user is not null\n";
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                errolMessage += "Email is not null\n";
            }
            else
            {
                // Kiểm tra định dạng Email
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    errolMessage += "Invalid email format!\n";
                }
            }
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                errolMessage += "Phone is not null\n";
            }
            else
            {
                // Kiểm tra định dạng Phone (Số điện thoại)
                var phonePattern = @"^\d{10}$";  // Ví dụ cho số điện thoại có 10 chữ số
                if (!Regex.IsMatch(txtPhone.Text, phonePattern))
                {
                    errolMessage += "Phone number must be 10 digits!\n";
                }
            }
            if (string.IsNullOrEmpty(pbPassword.Password)) errolMessage += "Password is not null\n";

            if (errolMessage.Length > 0)
            {
                MessageBox.Show(errolMessage.ToString(), "Input Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var newUser = new User()
                {
                    Username = txtFullName.Text,
                    FullName = txtFullName.Text,
                    PasswordHash = pbPassword.Password,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Avatar ="demo.pjg",
                    Status = "Active",
                    CreatedAt = DateTime.Now,
                    Role = 2
                };
                context.Users.Add(newUser);
                
                context.SaveChanges();
                AddLogs(newUser.UserId, "Add", "You add account is successfully");
                MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginWindow logwindow = new LoginWindow();
                logwindow.Show();
                this.Close();
            }
        }
    }
}
