using AppWPF.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AppWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        try
        {
            using (var context = new ContactManagementContext())
            {
                // Giả sử bạn có biến static lưu user hiện tại
                var user = GlobalSession.CurrentUser; // bạn cần lưu thông tin user khi login

                if (user != null)
                {
                    var notification = new Notification
                    {
                        UserId = user.UserId,
                        Message = $"{user.FullName} đã thoát chương trình",
                        Status = "Info",
                        CreatedAt = DateTime.Now
                    };

                    context.Notifications.Add(notification);
                    context.SaveChanges();
                }
            }
        }
        catch (Exception ex)
        {
            // log nếu cần
        }
    }
}

