using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SocialJusticTerminal.Annotations;
using SocialJusticTerminal.Logic;

namespace SocialJusticTerminal.ViewModels
{
    class NewPurchaseViewModel : BaseViewModel
    {
        #region Fields

        private float? _price;
        private ITerminalDataProvider _dataProvider;
        private string _userId;
        private string _storeId;

        public NewPurchaseViewModel([NotNull] ITerminalDataProvider dataProvider, [NotNull] string userId,
            [NotNull] string storeId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (userId == null) throw new ArgumentNullException("userId");
            if (storeId == null) throw new ArgumentNullException("storeId");

            _dataProvider = dataProvider;
            _userId = userId;
            _storeId = storeId;
        }

        #endregion


        #region Properties

        public string PriceString
        {
            get { return Price == null ? string.Empty : Price.Value.ToString(); }
            set
            {
                float actualValue;
                if (float.TryParse(value, out actualValue) && actualValue > 0)
                {
                    Price = actualValue;
                }
                else
                {
                    Price = null;
                }
                OnPropertyChanged("PriceString");
            }
        }

        public float? Price {get; set; }

        #endregion

        #region Commands

        public ICommand AddPruchaseCommand { get { return new RelayCommand(AddPruchase, ShouldAddPurchase);} }

        #endregion

        #region Methods

        private void AddPruchase()
        {
            if (Price.HasValue)
            {
                _dataProvider.AddPurchase(_userId, _storeId, Price.Value);
                MessageBox.Show("ההתקשרות התבצעה בהצלחה", "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
            }
            else
            {
                MessageBox.Show("נא להזין מחיר קנייה", "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);                
            }
        }

        private bool ShouldAddPurchase()
        {
            return Price != null && Price > 0;
        }

        #endregion

    }
}
