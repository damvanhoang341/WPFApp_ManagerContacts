using AppWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Interaction logic for ContactsManagement.xaml
    /// </summary>
    public partial class ContactsManagement : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public ContactsManagement(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
        }

        ContactManagementContext context = new ContactManagementContext();

        //void LoadData()
        //{
        //    dgContacts.ItemsSource = null;
        //    var listContacts = context.Contacts.Where(m=>m.UserId==UserId)
        //        .Select(m=> new
        //        {
        //            Stt,
        //            m.FullName,
        //            user=m.User.FullName,
        //            m.Phone,
        //            m.Email,
        //            m.Address
        //        })
        //        .ToList();
        //    dgContacts.ItemsSource = listContacts;
        //}
        void LoadData()
        {
            dgContacts.ItemsSource = null;

            int count = 1;

            var listContacts = context.Contacts
                .Where(m => m.UserId == UserId)
                .Select(m => new
                {
                    m.FullName,
                    UserName = m.User.FullName,
                    m.Phone,
                    m.ContactId,
                    m.Email,
                    m.Address
                })
                .ToList() // Truy vấn xong, bắt đầu xử lý trong C#
                .Select(m => new
                {
                    STT = count++,
                    m.FullName,
                    m.UserName,
                    m.ContactId,
                    m.Phone,
                    m.Email,
                    m.Address
                })
                .ToList();

            dgContacts.ItemsSource = listContacts;
        }


        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var userSelectDemo = dgContacts.SelectedItem as dynamic;
            if (userSelectDemo == null) return;
            else
            {
                int idContact = userSelectDemo.ContactId;
                var userSelect = context.Contacts.SingleOrDefault(m => m.ContactId == idContact);

                txbContactID.Text = userSelect.ContactId.ToString();
                txtName.Text = userSelect.FullName;
                txtPhone.Text = userSelect.Phone;
                txtEmail.Text = userSelect.Email;
                txtAddress.Text = userSelect.Address;
                txtRelationship.Text = userSelect.Notes;
                dpBirth.Text = userSelect.Birthday?.ToString("yyyy-MM-dd") ?? "";
                //txtCreate.Text = userSelect.CreatedAt?.ToString("yyyy-MM-dd") ?? "";
                //txtUpdate.Text = userSelect.UpdatedAt?.ToString("yyyy-MM-dd") ?? "";
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var stringSearch = txtSearchNameOrPhone.Text;
            if (string.IsNullOrEmpty(stringSearch)) LoadData();
            else
            {
                var contactSearch = context.Contacts.Where(m => m.Phone.Equals(stringSearch) || m.FullName.Equals(stringSearch)).ToList();
                if (contactSearch == null) MessageBox.Show("No find the contact!!!");
                else
                {
                    dgContacts.ItemsSource = contactSearch;
                }
            }
        }

        private void searchAddress_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var stringSearch = txtSearchAddress.Text;
            if (string.IsNullOrEmpty(stringSearch)) LoadData();
            else
            {
                var contactSearch = context.Contacts.Where(m => m.Address.Contains(stringSearch)).ToList();
                if (contactSearch == null) MessageBox.Show("No find the contact!!!");
                else
                {
                    dgContacts.ItemsSource = contactSearch;
                }
            }
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtSearchAddress.Clear();
            txtSearchNameOrPhone.Clear();
        }

        private void AddNotification(int userId,int idContact, string action,string detail)
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


        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    Contact newContact = new Contact()
        //    {
        //        UserId=UserId,
        //        FullName = txtName.Text,
        //        Phone = txtPhone.Text,
        //        Email = txtEmail.Text,
        //        Address = txtAddress.Text,
        //        Notes = txtRelationship.Text,
        //        Birthday = dpBirth.SelectedDate.HasValue? DateOnly.FromDateTime(dpBirth.SelectedDate.Value): null,
        //        Avatar = ""
        //    };
        //    context.Contacts.Add(newContact);
        //    context.SaveChanges();
        //    AddNotification(UserId,newContact.ContactId,"Add",$"Add a new contact: {newContact.FullName}");
        //    MessageBox.Show("Đã thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //    LoadData();
        //}
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // ✅ VALIDATE HỌ TÊN
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ✅ VALIDATE SỐ ĐIỆN THOẠI
            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Chỉ được chứa số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ✅ VALIDATE EMAIL
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ✅ VALIDATE NGÀY SINH
            if (dpBirth.SelectedDate.HasValue)
            {
                DateTime birthday = dpBirth.SelectedDate.Value;

                if (birthday > DateTime.Today)
                {
                    MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (birthday.Year < 1900)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ! Vui lòng nhập từ năm 1900 trở đi.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // ✅ Nếu mọi thứ hợp lệ thì tạo contact mới
            Contact newContact = new Contact()
            {
                UserId = UserId,
                FullName = txtName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Notes = txtRelationship.Text.Trim(),
                Birthday = dpBirth.SelectedDate.HasValue ? DateOnly.FromDateTime(dpBirth.SelectedDate.Value) : null,
                Avatar = ""
            };

            context.Contacts.Add(newContact);
            context.SaveChanges();

            // ✅ Ghi log
            AddNotification(UserId, newContact.ContactId, "Add", $"Add a new contact: {newContact.FullName}");

            MessageBox.Show("Đã thêm liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadData();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int idUpdate = int.Parse(txbContactID.Text);
            var contactUpdate = context.Contacts.SingleOrDefault(m => m.ContactId == idUpdate);
            if (contactUpdate == null) return;
            else
            {
                contactUpdate.FullName = txtName.Text;
                contactUpdate.Phone = txtPhone.Text;
                contactUpdate.Email = txtEmail.Text;
                contactUpdate.Address = txtAddress.Text;
                contactUpdate.Notes = txtRelationship.Text;
                contactUpdate.Birthday = dpBirth.SelectedDate.HasValue? DateOnly.FromDateTime(dpBirth.SelectedDate.Value): null;
                
            }
            context.Contacts.Update(contactUpdate);
            context.SaveChanges();
            AddNotification(UserId, contactUpdate.ContactId, "Update", $"Update a contact: {contactUpdate.FullName}");
            MessageBox.Show("Cập nhật liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }

        private void btnDeleted_Click(object sender, RoutedEventArgs e)
        {
            int idDeleted = int.Parse(txbContactID.Text);
            var contactDeleted = context.Contacts.SingleOrDefault(m => m.ContactId == idDeleted);
            if (contactDeleted == null) return;
            else
            {
                var relatedGroups = context.ContactGroups.Where(cg => cg.ContactId == contactDeleted.ContactId);
                context.ContactGroups.RemoveRange(relatedGroups);
                var logsToDelete = context.Logs.Where(log => log.ContactId == contactDeleted.ContactId).ToList();
                context.Logs.RemoveRange(logsToDelete);
                AddNotification(UserId, contactDeleted.ContactId, "Deleted", $"Deleted a contact: {contactDeleted.FullName}");
                context.Contacts.Remove(contactDeleted);
                context.SaveChanges();
                
                MessageBox.Show("Xóa liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnContact();
                LoadData();
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            ReturnContact();
        }

        void ReturnContact()
        {
            txtName.Clear();
            txbContactID.Text = "";
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtRelationship.Clear();
            dpBirth.SelectedDate = null;
            //txtCreate.Clear();
            //txtUpdate.Clear();
        }

        //private void btnLike_Click(object sender, RoutedEventArgs e)
        //{
        //    var contactSelect = dgContacts.SelectedItem as dynamic;
        //    if (contactSelect == null) return;
        //    int idContact = contactSelect.ContactId;
        //    FavoriteContact favorite = new FavoriteContact()
        //    {
        //        UserId = UserId,
        //        ContactId = idContact
        //    };
        //    context.FavoriteContacts.Add(favorite);
        //    context.SaveChanges();
        //    AddNotification(UserId,idContact, "Favorite", $"Add a contact: {contactSelect.FullName} in my favorite");
        //    MessageBox.Show("Thêm liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //}
        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            var contactSelect = dgContacts.SelectedItem as dynamic;
            if (contactSelect == null) return;

            int idContact = contactSelect.ContactId;

            // ✅ Kiểm tra xem đã có trong danh sách yêu thích chưa
            bool isExists = context.FavoriteContacts.Any(f => f.UserId == UserId && f.ContactId == idContact);

            if (isExists)
            {
                MessageBox.Show("Liên hệ này đã có trong mục yêu thích!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ✅ Nếu chưa có thì thêm vào
            FavoriteContact favorite = new FavoriteContact()
            {
                UserId = UserId,
                ContactId = idContact
            };

            context.FavoriteContacts.Add(favorite);
            context.SaveChanges();

            AddNotification(UserId, idContact, "Favorite", $"Add a contact: {contactSelect.FullName} in my favorite");

            MessageBox.Show("Thêm liên hệ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            var contactSelect = dgContacts.SelectedItem as dynamic;
            if (contactSelect == null) return;
            int idContact = contactSelect.ContactId;
            AddContactGroup newTap = new AddContactGroup(Username, UserId, idContact);
            newTap.Show();

        }
    }
}
