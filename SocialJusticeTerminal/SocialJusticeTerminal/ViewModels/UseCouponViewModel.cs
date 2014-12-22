using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SocialJusticeTerminal.Helpers;
using SocialJusticeTerminal.Logic;

namespace SocialJusticeTerminal.ViewModels
{
    class UseCouponViewModel : BaseViewViewModel
    {
        #region Fields

        private ITerminalDataProvider _dataProvider;
        private readonly Guid _customerId;
        private readonly Guid _storeId;

        #endregion


        public UseCouponViewModel(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId, IEnumerable<CouponViewModel> couponsOfCustomer)
        {
            CouponsOfCustomer = couponsOfCustomer;
            _dataProvider = dataProvider;
            _customerId = customerId;
            _storeId = storeId;
        }

        #region Properties

        public IEnumerable<CouponViewModel> CouponsOfCustomer { get; set; }
        public CouponViewModel SelectedCoupon { get; set; }

        #endregion

        public ICommand UseCouponCommand { get { return new RelayCommand(UseCoupon, DoesCouponSelected); } }
        public ICommand MakeANromalPurcaseCommand { get { return new RelayCommand(MakeANromalPurcase); } }
        public ICommand CloseWindowCommand { get { return new RelayCommand(CloseWindow); } }

        #region Methods

        private void CloseWindow()
        {
            OnWindowCloseRequested();
        }

        private void MakeANromalPurcase()
        {
            OnWindowCloseRequested();
            Navigator.Instance.CreateNewPurchaseView(_dataProvider, _customerId, _storeId);
        }

        private bool DoesCouponSelected()
        {
            return SelectedCoupon != null;
        }

        private void UseCoupon()
        {
            if (SelectedCoupon == null)
            {
                MessageBox.Show("אנא בחר קופון מהרשימה", "בחירת קופון", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                return;
            }

            var selection = MessageBox.Show(
                string.Format("האם אתה בטוח שברצונך לנצל את הקופון \"{0}\" במחיר {1} נקודות ?", SelectedCoupon.Description, SelectedCoupon.PointPrice), "בחירת קופון",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.RtlReading);

            if (selection == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _dataProvider.UseCoupon(SelectedCoupon);
                MessageBox.Show(string.Format("הקופון \"{0}\" נוצל, אנא זכה את הלקוח במוצר המפורט בקופון", SelectedCoupon.Description), "מועדון צדק חברתי",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);

            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "נכשל לשמור את פעולת הלקוח, אנא נסה שוב. במידה והפעולה לא מצליחה באופן עקבי נא לפנות ל\"מועדון צדק חברתי\"",
                    "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK,
                    MessageBoxOptions.RtlReading);
                _dataProvider.WriteToLog(e);
            }

            OnWindowCloseRequested();
        }

        #endregion

    }
}
