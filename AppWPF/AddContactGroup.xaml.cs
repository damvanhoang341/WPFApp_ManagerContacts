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
    /// Interaction logic for AddContactGroup.xaml
    /// </summary>
    public partial class AddContactGroup : Window
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public int IdContact { get; set; }
        public AddContactGroup(string username, int userId, int idContact)
        {
            this.Username = username;
            this.UserId = userId;
            this.IdContact = idContact;

            InitializeComponent();
            LoadCombobox();
            IdContact = idContact;
        }
        ContactManagementContext context = new ContactManagementContext();
        void LoadCombobox()
        {
            //cboGroup.ItemsSource = null;
            var listData = context.Groups.Where(m => m.UserId == UserId).ToList();
            //listData.Insert(0, new { GroupId = 0, GroupName = "Group" });
            cboGroup.ItemsSource = listData;
            cboGroup.DisplayMemberPath = "GroupName";
            cboGroup.SelectedValuePath = "GroupId";
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
        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    var contactAddGroup = context.Contacts.SingleOrDefault(m => m.ContactId == IdContact);
        //    ContactGroup newAdd = new ContactGroup()
        //    {
        //        ContactId = IdContact,
        //        GroupId=(int)cboGroup.SelectedValue
        //    };
        //    context.ContactGroups.Add(newAdd);
        //    context.SaveChanges();
        //    AddNotification(UserId, IdContact, "Add Group", $"Add a contatc: {contactAddGroup.FullName} in {cboGroup.Text} group");
        //    MessageBox.Show("Thêm liên hệ vào nhóm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //    this.Close();
        //}
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var contactAddGroup = context.Contacts.SingleOrDefault(m => m.ContactId == IdContact);
            int groupId = (int)cboGroup.SelectedValue;

            // ✅ Kiểm tra xem contact đã nằm trong nhóm chưa
            bool exists = context.ContactGroups.Any(cg => cg.ContactId == IdContact && cg.GroupId == groupId);

            if (exists)
            {
                MessageBox.Show("Liên hệ này đã tồn tại trong nhóm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Nếu chưa thì thêm
            ContactGroup newAdd = new ContactGroup()
            {
                ContactId = IdContact,
                GroupId = groupId
            };

            context.ContactGroups.Add(newAdd);
            context.SaveChanges();

            AddNotification(UserId, IdContact, "Add Group", $"Add a contact: {contactAddGroup.FullName} to group");

            MessageBox.Show("Thêm liên hệ vào nhóm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

    }
}
