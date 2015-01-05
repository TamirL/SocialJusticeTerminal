using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SocialJusticeTerminal.Annotations;
using SocialJusticeTerminal.Helpers;
using SocialJusticeTerminal.Logic;

namespace SocialJusticeTerminal.ViewModels
{
    class EnterCustomerTzViewModel : BaseViewViewModel
    {
        #region Fields

        private ITerminalDataProvider _dataProvider;
        private Guid _storeId;

        #endregion

        #region Ctor

        public EnterCustomerTzViewModel([NotNull] ITerminalDataProvider dataProvider, Guid storeId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (storeId == null || storeId == Guid.Empty) throw new ArgumentNullException("storeId");

            _dataProvider = dataProvider;
            _storeId = storeId;
        }

        #endregion

        #region Properties

        public string CustomerTz { get; set; }

        #endregion

        #region Commands

        public ICommand ContinueWithCustomerCommand { get { return new RelayCommand(ContinueWithCustomer, CanContinueWithCustomer);} }

        #endregion

        #region Methods

        private void ContinueWithCustomer()
        {
            if (CanContinueWithCustomer())
            {
                try
                {
                    var customerId = _dataProvider.GetSelectedCustomer(CustomerTz);
                    if (customerId == Guid.Empty)
                    {
                        TerminalMessageBox.ShowWarning("פרטי הלקוח לא נמצאו במערכת");
                    }
                    else
                    {
                        Navigator.Instance.MoveToCustomerActionScreen(_dataProvider, customerId, _storeId);
                        OnWindowCloseRequested();
                    }
                }
                catch (Exception e)
                {
                    const string errorMessage = "נכשל להשיג את פרטי הלקוח, אנא נסה שוב. במידה והפעולה לא מצליחה באופן עקבי נא לפנות ל\"מועדון צדק חברתי\"";
                    TerminalMessageBox.ShowError(errorMessage);
                    _dataProvider.WriteToLog(e);
                }
            }
            else
            {
                TerminalMessageBox.ShowError("נא להזין את מספר תעודת זהות הלקוח");
            }
        }

        private bool CanContinueWithCustomer()
        {
            return (!string.IsNullOrEmpty(CustomerTz)) && CustomerTz.Length == 9 &&
                   HasCorrectValidationNumber(CustomerTz);
        }

        private static bool HasCorrectValidationNumber(string customerTz)
        {
            var total = 0;
            var lastDigitChar = customerTz[8];
            if (lastDigitChar < '0' || lastDigitChar > '9')
                    return false;

            var lastDigit = lastDigitChar - '0';

            for (int i = 0; i < 8; i++)
            {
                char c = customerTz[i];
                if (c < '0' || c > '9')
                    return false;

                var digit = c - '0';
                var compnent = digit*((i%2) + 1);
                if (compnent >= 10)
                {
                    compnent = compnent/10 + compnent%10;
                }

                total += compnent;
            }

            return (10 - total % 10) % 10 == lastDigit;
            
        }

        #endregion
    }
}
