using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using SocialJusticeTerminal.Helpers;
using SocialJusticeTerminal.Logic;
using SocialJusticeTerminal.ViewModels;
using SocialJusticeTerminal.Views;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using MessageBoxOptions = System.Windows.MessageBoxOptions;

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
            ITerminalDataProvider dataProvider = new DummyTerminalDataProvider();
            // TODO: Get the real data
            var customerId = Guid.NewGuid();
            var storeId = Guid.NewGuid();
            try
            {
                var couponsOfCustomer = dataProvider.GetCouponsOfCustomer(customerId, storeId);
                if (couponsOfCustomer.Any())
                {
                    Navigator.Instance.CreateUseCouponeView(dataProvider, customerId, storeId, couponsOfCustomer);
                }
                else
                {
                    Navigator.Instance.CreateNewPurchaseView(dataProvider, customerId, storeId);
                }
            }
            catch (Exception e)
            {
                dataProvider.WriteToLog(e);
                var selection = MessageBox.Show(
                    "נכשל להשיג את רשימת הקופונים של המשתמש, האם ברצונך לעשות פעולה רגילה במקום?",
                    "מועדון צדק חברתי", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes,
                    MessageBoxOptions.RtlReading);
                if (selection == MessageBoxResult.Yes)
                {
                    Navigator.Instance.CreateNewPurchaseView(dataProvider, customerId, storeId);                    
                }
            }
        }


    }
}
