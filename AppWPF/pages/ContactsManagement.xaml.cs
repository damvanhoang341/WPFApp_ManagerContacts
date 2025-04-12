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
    /// Interaction logic for ContactsManagement.xaml
    /// </summary>
    public partial class ContactsManagement : Page
    {
        public ContactsManagement()
        {
            InitializeComponent();
            LoadData();
            LoadCboChooseUser();
        }

        ContactManagementContext context = new ContactManagementContext();
        int count = 1;
        //void LoadData()
        //{
        //    dgContacts.ItemsSource = null;
        //    var listContacts = context.Contacts
        //        .Select(m=> new
        //        {
        //            stt=count+
        //            m.ContactId,
        //            user=m.User.FullName,
        //            m.FullName,
        //            m.Phone,
        //            m.Email
        //        })
        //        .ToList();
        //    dgContacts.ItemsSource = listContacts;
        //}

        //private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var itemSelect = dgContacts.SelectedItem as dynamic;
        //    if (itemSelect == null) return;
        //    else
        //    {
        //        int idItem = itemSelect.ContactId;
        //        var itemDeleted = context.Contacts.SingleOrDefault(m => m.ContactId == idItem);
        //        context.Contacts.Remove(itemDeleted);
        //        context.SaveChanges();
        //        LoadData();
        //    }
        //}
        void LoadData()
        {
            dgContacts.ItemsSource = null;
            int count = 1; // bắt đầu từ 1

            var listContacts = context.Contacts
                .Select(m => new
                {
                    ContactId = m.ContactId,
                    UserName = m.User.FullName,
                    FullName = m.FullName,
                    Phone = m.Phone,
                    Email = m.Email
                })
                .ToList()
                .Select(m => new
                {
                    STT = count++, // thêm số thứ tự
                    m.ContactId,
                    m.UserName,
                    m.FullName,
                    m.Phone,
                    m.Email
                })
                .ToList();

            dgContacts.ItemsSource = listContacts;
        }

        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itemSelect = dgContacts.SelectedItem as dynamic;
            if (itemSelect == null) return;

            int idItem = itemSelect.ContactId;

            // ✅ Xóa tất cả log liên quan trước
            var logs = context.Logs.Where(l => l.ContactId == idItem).ToList();
            context.Logs.RemoveRange(logs);

            // ✅ Xóa các liên kết nhóm (nếu có)
            var contactGroups = context.ContactGroups.Where(cg => cg.ContactId == idItem).ToList();
            context.ContactGroups.RemoveRange(contactGroups);

            // ✅ Sau đó mới xóa liên hệ
            var itemDeleted = context.Contacts.SingleOrDefault(m => m.ContactId == idItem);
            if (itemDeleted != null)
            {
                context.Contacts.Remove(itemDeleted);
                context.SaveChanges();

                MessageBox.Show("Đã xóa liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
        }



        private void txtSearch2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string search = txtSearchNameOrPhone.Text;
            var listData=context.Contacts.Where(m=>m.FullName.Contains(search) || m.Phone.Contains(search))
                .Select(m => new
                {
                    m.ContactId,
                    user = m.User.FullName,
                    m.FullName,
                    m.Phone,
                    m.Email
                })
                .ToList();
            dgContacts.ItemsSource = listData;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var listData= context.Contacts.Where(m => m.Address==txtSearchAddress.Text)
                .Select(m => new
                {
                    m.ContactId,
                    user = m.User.FullName,
                    m.FullName,
                    m.Phone,
                    m.Email
                })
                .ToList();
            dgContacts.ItemsSource = listData;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtSearchNameOrPhone.Clear();
            txtSearchAddress.Clear();
        }

        private void dgContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemSelect = dgContacts.SelectedItem as dynamic;
            if (itemSelect == null) return;
            else
            {
                int idItem = itemSelect.ContactId;
                var contactSelect = context.Contacts.SingleOrDefault(m => m.ContactId == idItem);
                txbContactID.Text = contactSelect.ContactId.ToString();
                txtName.Text = contactSelect.FullName;
                txtPhone.Text = contactSelect.Phone;
                txtEmail.Text = contactSelect.Email;
                txtAddress.Text = contactSelect.Address;
                txtRelationship.Text = contactSelect.Notes;
                dpBirth.Text = contactSelect.Birthday.ToString();
                dpCreate.Text = contactSelect.CreatedAt.ToString();
                dpUpdate.Text = contactSelect.UpdatedAt.ToString();

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var itemSelect = dgContacts.SelectedItem as dynamic;
            if (itemSelect == null) return;
            else
            {
                int idItem = itemSelect.ContactId;
                var contactSelect = context.Contacts.SingleOrDefault(m => m.ContactId == idItem);
                //txbContactID.Text = contactSelect.ContactId.ToString();
                contactSelect.FullName= txtName.Text ;
                contactSelect.Phone= txtPhone.Text ;
                contactSelect.Email = txtEmail.Text;
                contactSelect.Address = txtAddress.Text;
                contactSelect.Notes = txtRelationship.Text;
                contactSelect.Birthday = DateOnly.Parse(dpBirth.Text);
                contactSelect.CreatedAt = DateTime.Parse(dpCreate.Text);
                contactSelect.UpdatedAt = DateTime.Parse(dpUpdate.Text);
                context.Contacts.Update(contactSelect);
                context.SaveChanges();
                LoadData();
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            txbContactID.Text = "";
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtRelationship.Clear();
            dpBirth.Text = "";
            dpCreate.Text = "";
            dpUpdate.Text = "";
        }

        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    Contact newContact = new Contact()
        //    {
        //        FullName = txtName.Text.Trim(),
        //        Phone = txtPhone.Text.Trim(),
        //        Email = txtEmail.Text.Trim(),
        //        Address = txtAddress.Text.Trim(),
        //        Notes = txtRelationship.Text.Trim(),
        //        Birthday = dpBirth.SelectedDate.HasValue ? DateOnly.FromDateTime(dpBirth.SelectedDate.Value) : null,

        //        UserId = Convert.ToInt32(cbonChooseUser.SelectedValue),
        //        Avatar = "" // Tùy bạn muốn xử lý ảnh đại diện
        //    };
        //    context.Contacts.Add(newContact);
        //    context.SaveChanges();
        //}
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // ✅ Kiểm tra người dùng đã chọn chưa
            if (cbonChooseUser.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int userId = Convert.ToInt32(cbonChooseUser.SelectedValue);

            // ✅ Kiểm tra UserId có tồn tại trong DB không
            bool userExists = context.Users.Any(u => u.UserId == userId);
            if (!userExists)
            {
                MessageBox.Show("Người dùng không tồn tại trong hệ thống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // ✅ Tạo Contact mới
            Contact newContact = new Contact()
            {
                FullName = txtName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Notes = txtRelationship.Text.Trim(),
                Birthday = dpBirth.SelectedDate.HasValue ? DateOnly.FromDateTime(dpBirth.SelectedDate.Value) : null,
                CreatedAt = dpCreate.SelectedDate ?? DateTime.Now,
                UpdatedAt = dpUpdate.SelectedDate ?? DateTime.Now,
                UserId = userId,
                Avatar = "" // bạn có thể xử lý avatar riêng nếu muốn
            };

            try
            {
                context.Contacts.Add(newContact);
                context.SaveChanges();

                MessageBox.Show("Thêm liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData(); // load lại danh sách sau khi thêm
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm liên hệ: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        void LoadCboChooseUser()
        {
            var listData = context.Users.ToList();
            cbonChooseUser.ItemsSource = listData;
            cbonChooseUser.DisplayMemberPath = "Username";
            cbonChooseUser.SelectedValuePath = "UserId";
        }
        private void cboChooseUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
