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
    /// Interaction logic for GroupsManagement.xaml
    /// </summary>
    public partial class GroupsManagement : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public GroupsManagement(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
            LoadCboGroup();
        }


        ContactManagementContext context = new ContactManagementContext();

        void LoadData()
        {
            dgData.ItemsSource = null;
            var dataLoad = context.Groups
                .Select(m => new
                {
                    m.GroupId,
                    nameUser=m.User.FullName,
                    phone = m.User.Phone,
                    emailUser =m.User.Email,
                    m.GroupName,
                    m.Description,
                    m.CreatedAt
                })
                .ToList();
            dgData.ItemsSource = dataLoad;
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
            string nameGroup = cboGroup.SelectedValue.ToString();
            var listGroup=context.Groups.Where(m=>m.GroupName==nameGroup).Select(m => new
            {
                m.GroupId,
                nameUser = m.User.FullName,
                phone = m.User.Phone,
                emailUser = m.User.Email,
                m.GroupName,
                m.Description,
                m.CreatedAt
            })
                .ToList();
            dgData.ItemsSource = listGroup;
        }

        private void txtSearchName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var listGroup = context.Groups.Where(m => m.User.FullName.Contains(txtSearchName.Text)).Select(m => new
            {
                m.GroupId,
                nameUser = m.User.FullName,
                phone = m.User.Phone,
                emailUser = m.User.Email,
                m.GroupName,
                m.Description,
                m.CreatedAt
            })
                .ToList();
            dgData.ItemsSource = listGroup;
        }

        private void btnSearchPhone_Click(object sender, RoutedEventArgs e)
        {
            var listGroup = context.Groups.Where(m => m.User.Phone==txtSearchPhone.Text).Select(m => new
            {
                m.GroupId,
                nameUser = m.User.FullName,
                phone = m.User.Phone,
                emailUser = m.User.Email,
                m.GroupName,
                m.Description,
                m.CreatedAt
            })
                .ToList();
            dgData.ItemsSource = listGroup;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            cboGroup.SelectedItem = "";
            txtSearchPhone.Clear();
            txtSearchName.Clear();
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemSelected = dgData.SelectedItem as dynamic;
            if (itemSelected == null) return;
            else
            {
                txbGroupID.Text = itemSelected.GroupId.ToString();
                txtName.Text = itemSelected.nameUser;
                txtPhone.Text = itemSelected.phone;
                txtEmail.Text = itemSelected.emailUser;
                txtGroup.Text = itemSelected.GroupName;
                txtDescription.Text = itemSelected.Description;
            }
        }

        void AddNotification(int idUser,int idContact,string action,string details)
        {
            Log newNotification = new Log()
            {
                UserId = idUser,
                ContactId = idContact,
                Action = action,
                Details=details
            };
            context.Logs.Add(newNotification);
            context.SaveChanges();
        }

        //private void btnUpdate_Click(object sender, RoutedEventArgs e)
        //{
        //    int idGroup = int.Parse(txbGroupID.Text);
        //    var groupUpdate = context.Groups.SingleOrDefault(m => m.GroupId == idGroup);
        //    groupUpdate.GroupName = txtGroup.Text;
        //    groupUpdate.Description = txtDescription.Text;
        //    context.Groups.Update(groupUpdate);
        //    context.SaveChanges();
        //    LoadData();
        //}

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn cập nhật nhóm này không?",
                "Xác nhận cập nhật",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                int idGroup = int.Parse(txbGroupID.Text);
                var groupUpdate = context.Groups.SingleOrDefault(m => m.GroupId == idGroup);

                if (groupUpdate != null)
                {
                    groupUpdate.GroupName = txtGroup.Text;
                    groupUpdate.Description = txtDescription.Text;

                    context.Groups.Update(groupUpdate);
                    context.SaveChanges();

                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    AddNotification(UserId, groupUpdate.UserId, "Update", $"Admin{UserId} update to user{groupUpdate.UserId}'s group");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhóm cần cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            txbGroupID.Text = "";
            txtEmail.Clear();
            txtGroup.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtDescription.Clear();
        }
    }
}
