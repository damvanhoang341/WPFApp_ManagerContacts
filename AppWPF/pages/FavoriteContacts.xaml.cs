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
using Microsoft.EntityFrameworkCore;


namespace AppWPF.pages
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
            var dataLoad = context.FavoriteContacts
                .Select(m => new
                {
                    user = m.User.FullName,
                    phoneUser = m.User.Phone,
                    contact = m.Contact.FullName,
                    contactPhone = m.Contact.Phone,
                    contactEmail = m.Contact.Email,
                    contactNote = m.Contact.Notes,
                    m.AddedAt,
                    m.UserId,
                    m.ContactId
                })
                .ToList();
                dgData.ItemsSource = dataLoad;
        }

        void AddNotification(string acction,string message,int contactID,int userID)
        {
            Log notification = new Log()
            {
                UserId = userID,
                ContactId=contactID,
                Action=acction,
                Details=message
            };
            context.Logs.Add(notification);
            context.SaveChanges();
        }

        private void btnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itemSelect = dgData.SelectedItem as dynamic;
            if (itemSelect == null) return;
            else
            {
                int idUserItem = itemSelect.UserId;
                int idContactItem = itemSelect.ContactId;
                var selectItem = context.FavoriteContacts.Include(m => m.User).Include(m => m.Contact).SingleOrDefault(m => m.UserId == idUserItem && m.ContactId == idContactItem);
                if (selectItem == null) return;
                else
                {
                    context.FavoriteContacts.Remove(selectItem);
                    context.SaveChanges();
                    if (selectItem.Contact == null || selectItem.User == null)
                    {
                        MessageBox.Show("Dữ liệu Contact hoặc User bị thiếu trong selectItem.", "Lỗi dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        AddNotification("Deleted", $"Admin deleted contact {selectItem.Contact.FullName} of {selectItem.User.FullName}",selectItem.ContactId,selectItem.UserId);
                    }
                    LoadData();
                }
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemSelect = dgData.SelectedItem as dynamic;
            if (itemSelect == null) return;
            else
            {
                txbUser.Text = itemSelect.user;
                txbPhoneUser.Text = itemSelect.phoneUser;
                txtContact.Text = itemSelect.contact;
                txtPhone.Text = itemSelect.contactPhone;
                txtEmail.Text= itemSelect.contactEmail;
                txtGroup.Text = itemSelect.contactNote;
            }
        }

        private void txtSearchUser_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var listData=context.FavoriteContacts.Where(m=>m.User.FullName.Contains(txtSearchUser.Text)).Select(m => new
            {
                user = m.User.FullName,
                contact = m.Contact.FullName,
                contactPhone = m.Contact.Phone,
                contactEmail = m.Contact.Email,
                contactNote = m.Contact.Notes,
                m.AddedAt
            })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var listData = context.FavoriteContacts.Where(m => m.Contact.FullName.Equals(txtSearch2.Text)|| m.Contact.Phone.Equals(txtSearch2.Text))
                .Select(m => new
            {
                user = m.User.FullName,
                contact = m.Contact.FullName,
                contactPhone = m.Contact.Phone,
                contactEmail = m.Contact.Email,
                contactNote = m.Contact.Notes,
                m.AddedAt
            })
                .ToList();
            dgData.ItemsSource = listData;
        }

        private void btnClearn_Click(object sender, RoutedEventArgs e)
        {
            txtSearch2.Clear();
            txtSearchUser.Clear();
            LoadData();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            txbUser.Text = "";
            txbPhoneUser.Text = "";
            txtContact.Clear();
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtGroup.Text = "";
        }
    }
}
