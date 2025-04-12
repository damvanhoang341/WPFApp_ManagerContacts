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
    /// Interaction logic for CreateGroup.xaml
    /// </summary>
    public partial class CreateGroup : Window
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public CreateGroup(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
        }
        ContactManagementContext context = new ContactManagementContext();
        

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Group groupNew = new Group()
            {
                UserId = UserId,
                GroupName = txtGroup.Text,
                Description = txtDes.Text
            };
            context.Groups.Add(groupNew);
            context.SaveChanges();
            MessageBox.Show("Thêm nhóm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
