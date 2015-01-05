using System;
using System.Windows;
using System.Windows.Forms;
using SocialJusticeTerminal.Helpers;
using SocialJusticeTerminal.Logic;
using Application = System.Windows.Application;

namespace SocialJusticeTerminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon _notifyIcon;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(@"D:\Work\SocialJustice\SocialJusticeTerminal\SocialJusticeTerminal\SocialJusticeTerminal\Images\ic_account_child_24px.ico"),
                Visible = true,
                Text = "מועדון צדק חברתי"
            };

            _notifyIcon.MouseClick += (a, b) => DoLogic();
        }

        private void DoLogic()
        {
            Navigator.Instance.CreateEnterCustomerTzView(new DummyTerminalDataProvider(), Guid.NewGuid(), Guid.NewGuid());
        }


    }
}
