using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void CreateUseCouponeView(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId, IEnumerable<CouponViewModel> couponsOfCustomer)
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
