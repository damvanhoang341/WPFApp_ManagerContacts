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
    /// Interaction logic for FavoriteContacts.xaml
    /// </summary>
    public partial class FavoriteContacts : Page
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public FavoriteContacts(string username, int userId)
        {
            this.Username = username;
            this.UserId = userId;

            InitializeComponent();
            LoadData();
        }


        ContactManagementContext context = new ContactManagementContext();

        void LoadData()
        {
            dgData.ItemsSource = null;
            var dataLoad = context.FavoriteContacts.Where(m=>m.UserId==UserId)
                .Select(m => new
                {
                    user = m.User.Username,
                    m.ContactId,
                    contact = m.Contact.FullName,
                    contactPhone = m.Contact.Phone,
                    contactEmail = m.Contact.Email,
                    contactNote = m.Contact.Notes,
                    m.AddedAt
                })
                .ToList();
                dgData.ItemsSource = dataLoad;
        }
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
            var contactSelect = dgData.SelectedItem as dynamic;
            int idDelecte = contactSelect.ContactId;
            var contactDeleted = context.FavoriteContacts.SingleOrDefault(m => m.ContactId == idDelecte);
            if (contactDeleted != null)
            {
                context.FavoriteContacts.Remove(contactDeleted);
                context.SaveChanges();
                AddNotification(UserId, contactDeleted.ContactId, "Deleted Favorite", $"Deleted a contact: {contactDeleted.Contact.FullName} in my favorite");
                LoadData();
            }
            else
            {
                MessageBox.Show("Contact not found in favorite contacts.");
            }
        }

        //private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var contactSelect = dgData.SelectedItem as dynamic;
        //    if (contactSelect == null) return;
        //    int idContactSelect = contactSelect.ContactId;
        //    var selectContact = context.Contacts.SingleOrDefault(m => m.ContactId == idContactSelect);
        //    var userLogin = context.Users.SingleOrDefault(m => m.UserId == UserId);
        //    var contactGroup = context.ContactGroups.SingleOrDefault(m => m.ContactId == idContactSelect);
        //    txtPhoneUser.Text = userLogin.Phone;
        //    txbUser.Text = Username;
        //    txtContact.Text = selectContact.FullName;
        //    txtPhone.Text = selectContact.Phone;
        //    txtEmail.Text = selectContact.Email;
        //    if (contactGroup != null )
        //    {
        //        int idGroup = contactGroup.GroupId;
        //        var groupSelect = context.Groups.SingleOrDefault(m => m.GroupId == idGroup);
        //        txtGroup.Text = groupSelect.GroupName;
        //    }
        //    else
        //    {
        //        txtGroup.Text = "Không có nhóm";
        //    }

        //}

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var contactSelect = dgData.SelectedItem as dynamic;
            if (contactSelect == null) return;

            int idContactSelect = contactSelect.ContactId;

            var selectContact = context.Contacts.SingleOrDefault(m => m.ContactId == idContactSelect);
            var userLogin = context.Users.SingleOrDefault(m => m.UserId == UserId);

            // ✅ Lấy tất cả các nhóm liên quan đến contact này
            var contactGroups = context.ContactGroups
                .Where(m => m.ContactId == idContactSelect)
                .ToList();

            txtPhoneUser.Text = userLogin?.Phone ?? "";
            txbUser.Text = Username;
            txtContact.Text = selectContact?.FullName ?? "";
            txtPhone.Text = selectContact?.Phone ?? "";
            txtEmail.Text = selectContact?.Email ?? "";

            if (contactGroups.Any())
            {
                // Lấy danh sách tên các nhóm
                var groupNames = contactGroups
                    .Select(g => context.Groups.SingleOrDefault(m => m.GroupId == g.GroupId)?.GroupName)
                    .Where(name => name != null)
                    .ToList();

                txtGroup.Text = string.Join(", ", groupNames); // ghép lại thành chuỗi
            }
            else
            {
                txtGroup.Text = "Không có nhóm";
            }
        }


        private void txtSearchEmail_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearchEmail.Text.ToLower();

            var filteredFavorites = context.FavoriteContacts
                .Where(f => f.Contact.Email.ToLower().Contains(searchText) && f.UserId==UserId)
                .Select(m => new
                {
                    user = m.User.Username,
                    m.ContactId,
                    contact = m.Contact.FullName,
                    contactPhone = m.Contact.Phone,
                    contactEmail = m.Contact.Email,
                    contactNote = m.Contact.Notes,
                    m.AddedAt
                })
                .ToList();
            dgData.ItemsSource = filteredFavorites;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtSearchEmail.Clear();
            txtSearch2.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string valueSearch = txtSearch2.Text;
            var listSearch=context.FavoriteContacts.Where(m=>m.Contact.FullName.Equals(valueSearch) || m.Contact.Phone.Equals(valueSearch))
                .Select(m => new
                {
                    user = m.User.Username,
                    m.ContactId,
                    contact = m.Contact.FullName,
                    contactPhone = m.Contact.Phone,
                    contactEmail = m.Contact.Email,
                    contactNote = m.Contact.Notes,
                    m.AddedAt
                })
                .ToList();
            dgData.ItemsSource = listSearch;
        }
    }
}
