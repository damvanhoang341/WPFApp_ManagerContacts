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
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public Group(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
            LoadCombobox();
            LoadComboboxGroup();
        }
        ContactManagementContext context = new ContactManagementContext();

        void LoadData()
        {
            dgData.ItemsSource = null;
            var listData = context.ContactGroups
                .Where(m => m.Group.UserId == UserId)
                .Select(m => new
                    {
                    group = m.Group.GroupName,
                    contact = m.Contact.FullName,
                    phone = m.Contact.Phone,
                    email = m.Contact.Email,
                    birth = m.Contact.Birthday,
                    m.AddedAt,
                    m.GroupId,
                    m.ContactId
                }).ToList();


            dgData.ItemsSource = listData;
        }

        void LoadCombobox()
        {
            //cboGroup.ItemsSource = null;
            var listData = context.Groups.Where(m=>m.UserId==UserId).ToList();
            //listData.Insert(0, new { GroupId = 0, GroupName = "Group" });
            cboGroup.ItemsSource = listData;
            cboGroup.DisplayMemberPath = "GroupName";
            cboGroup.SelectedValuePath = "GroupId";
        }

        //private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var groupSelect = dgData.SelectedItem as dynamic;
        //    if (groupSelect == null) return;
        //    int idContact = groupSelect.ContactId;
        //    var groupDeleted = context.ContactGroups.SingleOrDefault(m => m.ContactId == idContact);
        //    context.ContactGroups.Remove(groupDeleted);
        //    context.SaveChanges();
        //    LoadData();
        //}
        private void AddNotification(int userId, int idContact, string action, string detail)
        {
            Log notify = new Log
            {
                UserId = UserId,
                ContactId = idContact,
                Action = action,
                Details = detail,
                CreatedAt = DateTime.Now
            };

            context.Logs.Add(notify);
            context.SaveChanges();
        }
        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var groupSelect = dgData.SelectedItem as dynamic;
            if (groupSelect == null) return;

            int idContact = groupSelect.ContactId;
            int idGroup = groupSelect.GroupId;

            // ✅ Chỉ xóa bản ghi có ContactId + GroupId đúng với dòng được chọn
            var groupDeleted = context.ContactGroups
                .SingleOrDefault(m => m.ContactId == idContact && m.GroupId == idGroup);

            if (groupDeleted != null)
            {
                AddNotification(UserId, groupDeleted.Contact.ContactId, "Deleted Group", $"Deleted group: {groupDeleted.Group.GroupName}");
                context.ContactGroups.Remove(groupDeleted);
                context.SaveChanges();
                MessageBox.Show("Đã xóa liên hệ khỏi nhóm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void cboGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboGroup.SelectedValue == null) return;

            int idSelect = (int)cboGroup.SelectedValue;

            var listData = context.ContactGroups
                .Where(m => m.GroupId == idSelect && m.Group.UserId == UserId)
                .Select(m => new
                {
                    group = m.Group.GroupName,
                    contact = m.Contact.FullName,
                    phone = m.Contact.Phone,
                    email = m.Contact.Email,
                    birth = m.Contact.Birthday,
                    m.AddedAt
                }).ToList();

            dgData.ItemsSource = listData;
        }

        private void btnSearchPhone_Click(object sender, RoutedEventArgs e)
        {
            string phone = txtPhone.Text?.Trim();

            if (string.IsNullOrEmpty(phone)) return;

            // Tìm contact theo số điện thoại
            var contactByPhone = context.Contacts
                .SingleOrDefault(m => m.Phone == phone);

            if (contactByPhone == null)
            {
                MessageBox.Show("Không tìm thấy liên hệ với số điện thoại này.");
                return;
            }

            // Lấy danh sách group liên quan đến contact
            var groupNeed = context.ContactGroups
                .Where(m => m.ContactId == contactByPhone.ContactId)
                .Select(m => new
                {
                    group = m.Group.GroupName,
                    contact = m.Contact.FullName,
                    phone = m.Contact.Phone,
                    email = m.Contact.Email,
                    birth = m.Contact.Birthday,
                    m.AddedAt,
                    m.GroupId
                }).ToList();

            dgData.ItemsSource = groupNeed;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            cboGroup.SelectedItem = null;
            txtPhone.Clear();
            LoadData();
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectItem = dgData.SelectedItem as dynamic;
            if (selectItem == null) return;
            else
            {
                cboGroupDescriptions.Text = selectItem.group;
                txtContact.Text = selectItem.contact;
                txtEmail.Text = selectItem.email;
                txtPhoneDescription.Text = selectItem.phone;
                dpBirth.Text = selectItem.birth.ToString();
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            cboGroupDescriptions.SelectedItem=null;
            txtContact.Clear();
            txtEmail.Clear();
            txtPhoneDescription.Clear();
            dpBirth.Text="";
        }

        void LoadComboboxGroup()
        {
            //cboGroup.ItemsSource = null;
            var listData = context.Groups.Where(m => m.UserId == UserId).ToList();
            //listData.Insert(0, new { GroupId = 0, GroupName = "Group" });
            cboGroupDescriptions.ItemsSource = listData;
            cboGroupDescriptions.DisplayMemberPath = "GroupName";
            cboGroupDescriptions.SelectedValuePath = "GroupId";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectItem = dgData.SelectedItem as dynamic;
            if (selectItem == null) return;

            int idContactGroup = (int)selectItem.ContactId;
            int oldGroupId = (int)selectItem.GroupId;
            int newGroupId = (int)cboGroupDescriptions.SelectedValue;

            // Nếu người dùng chọn đúng lại group cũ thì không cần làm gì
            if (oldGroupId == newGroupId)
            {
                MessageBox.Show("Bạn chưa thay đổi nhóm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Xoá bản ghi cũ
            var contactGroupOld = context.ContactGroups
                .SingleOrDefault(m => m.ContactId == idContactGroup && m.GroupId == oldGroupId);

            if (contactGroupOld != null)
                context.ContactGroups.Remove(contactGroupOld);

            // Tạo bản ghi mới
            var contactGroupNew = new ContactGroup()
            {
                ContactId = idContactGroup,
                GroupId = newGroupId,
                AddedAt = DateTime.Now
            };

            context.ContactGroups.Add(contactGroupNew);
            context.SaveChanges();

            MessageBox.Show("Cập nhật nhóm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }


        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateGroup newTap = new CreateGroup(Username, UserId);
            newTap.Show();
        }
    }
}
