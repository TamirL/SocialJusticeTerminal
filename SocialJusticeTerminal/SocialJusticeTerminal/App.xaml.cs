using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using SocialJusticeTerminal.Logic;
using SocialJusticeTerminal.ViewModels;
using SocialJusticeTerminal.Views;
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

            _notifyIcon.MouseClick += (a, b) => CreateNewPurchaseView();

            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Second == 13)
            {
                CreateNewPurchaseView();
            }
        }

        private void CreateNewPurchaseView()
        {
            var viewModel = new NewPurchaseViewModel(new DummyTerminalDataProvider(), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
            var view = new NewPurchaseView() {DataContext = viewModel};
            viewModel.WindowCloseRequested += (a,b) => view.Close();
            view.Show();
            view.FocusOnTextbox();
        }
    }
}
