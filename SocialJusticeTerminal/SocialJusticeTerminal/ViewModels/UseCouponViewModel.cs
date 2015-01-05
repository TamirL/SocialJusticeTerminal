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


        public UseCouponViewModel(ITerminalDataProvider dataProvider, Guid customerId, Guid storeId, IEnumerable<CustomerCouponViewModel> couponsOfCustomer)
        {
            CouponsOfCustomer = couponsOfCustomer;
            _dataProvider = dataProvider;
            _customerId = customerId;
            _storeId = storeId;
        }

        #region Properties

        public IEnumerable<CustomerCouponViewModel> CouponsOfCustomer { get; set; }
        public CustomerCouponViewModel SelectedCoupon { get; set; }

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
            const string caption = "בחירת קופון";
            if (SelectedCoupon == null)
            {
                TerminalMessageBox.ShowError("אנא בחר קופון מהרשימה", caption);
                return;
            }

            var questionText = string.Format("האם אתה בטוח שברצונך לנצל את הקופון \"{0}\" במחיר {1} נקודות ?", SelectedCoupon.Description, SelectedCoupon.PointPrice);
            if (!TerminalMessageBox.ShowQuestion(questionText, caption)) return;

            try
            {
                _dataProvider.UseCoupon(SelectedCoupon);
                var qouponTakenText = string.Format("הקופון \"{0}\" נוצל, אנא זכה את הלקוח במוצר המפורט בקופון", SelectedCoupon.Description);
                TerminalMessageBox.ShowInfo(qouponTakenText);
                OnWindowCloseRequested();
            }
            catch (Exception e)
            {
                const string useCouponFailedText = "נכשל לשמור את פעולת הלקוח, אנא נסה שוב. במידה והפעולה לא מצליחה באופן עקבי נא לפנות ל\"מועדון צדק חברתי\"";
                TerminalMessageBox.ShowError(useCouponFailedText);
                _dataProvider.WriteToLog(e);
            }
        }



        #endregion
    }
}
