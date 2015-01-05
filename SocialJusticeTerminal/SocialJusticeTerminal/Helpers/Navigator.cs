using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SocialJusticeTerminal.Logic;
using SocialJusticeTerminal.ViewModels;
using SocialJusticeTerminal.Views;

namespace SocialJusticeTerminal.Helpers
{
    internal class Navigator
    {
        private static volatile Navigator _instance;

        public static Navigator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(Navigator))
                    {
                        if (_instance == null)
                        {
                            _instance = new Navigator();
                        }
                    }
                }

                return _instance;
            }
        }

        public void MoveToCustomerActionScreen(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId)
        {
            try
            {
                var couponsOfCustomer = dataProvider.GetCouponsOfCustomer(customerId, storeId);
                if (couponsOfCustomer.Any())
                {
                    CreateUseCouponeView(dataProvider, customerId, storeId, couponsOfCustomer);
                }
                else
                {
                    CreateNewPurchaseView(dataProvider, customerId, storeId);
                }
            }
            catch (Exception e)
            {
                dataProvider.WriteToLog(e);
                const string ordinaryPurchaseQuestionText = "נכשל להשיג את רשימת הקופונים של המשתמש, האם ברצונך לעשות פעולה רגילה במקום?";;
                if (TerminalMessageBox.ShowQuestion(ordinaryPurchaseQuestionText))
                {
                    CreateNewPurchaseView(dataProvider, customerId, storeId);
                }
            }
        }

        public void CreateEnterCustomerTzView(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId)
        {
            var viewModel = new EnterCustomerTzViewModel(dataProvider, storeId);
            var view = new EnterCustomerTzView() { DataContext = viewModel };
            viewModel.WindowCloseRequested += (a, b) => view.Close();
            view.Show();
            view.FocusOnTextbox();
        }

        public void CreateUseCouponeView(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId, IEnumerable<CustomerCouponViewModel> couponsOfCustomer)
        {
            var viewModel = new UseCouponViewModel(dataProvider, customerId, storeId, couponsOfCustomer);
            var view = new UseCouponView() { DataContext = viewModel };
            viewModel.WindowCloseRequested += (a, b) => view.Close();
            view.Show();
        }

        public void CreateNewPurchaseView(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId)
        {
            var viewModel = new NewPurchaseViewModel(dataProvider, customerId, storeId);
            var view = new NewPurchaseView() { DataContext = viewModel };
            viewModel.WindowCloseRequested += (a, b) => view.Close();
            view.Show();
            view.FocusOnTextbox();
        }
    }
}
