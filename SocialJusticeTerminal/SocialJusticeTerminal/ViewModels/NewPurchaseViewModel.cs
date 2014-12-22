﻿using System;
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
    class NewPurchaseViewModel : BaseViewViewModel
    {
        #region Fields

        private float? _price;
        private ITerminalDataProvider _dataProvider;
        private Guid _customerId;
        private Guid _storeId;

        #endregion

        #region Ctor

        public NewPurchaseViewModel([NotNull] ITerminalDataProvider dataProvider, Guid customerId, Guid storeId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (customerId == null || customerId == Guid.Empty) throw new ArgumentNullException("customerId");
            if (storeId == null || storeId == Guid.Empty) throw new ArgumentNullException("storeId");

            _dataProvider = dataProvider;
            _customerId = customerId;
            _storeId = storeId;
        }

        #endregion

        #region Properties

        public string PriceString
        {
            get { return _price == null ? string.Empty : _price.Value.ToString(); }
            set
            {
                float actualValue;
                if (float.TryParse(value, out actualValue) && actualValue > 0)
                {
                    _price = actualValue;
                }
                else
                {
                    _price = null;
                }
                OnPropertyChanged("PriceString");
            }
        }

        #endregion

        #region Commands

        public ICommand AddPruchaseCommand { get { return new RelayCommand(AddPruchase, ShouldAddPurchase);} }

        #endregion

        #region Methods

        private void AddPruchase()
        {
            if (_price.HasValue)
            {
                try
                {
                    _dataProvider.AddPurchase(_customerId, _storeId, _price.Value);
                    MessageBox.Show("פעולת הלקוח נשמרה בהצלחה", "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                    OnWindowCloseRequested();
                }
                catch (Exception e)
                {
                    MessageBox.Show("נכשל לשמור את פעולת הלקוח, אנא נסה שוב. במידה והפעולה לא מצליחה באופן עקבי נא לפנות ל\"מועדון צדק חברתי\"", "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                    _dataProvider.WriteToLog(e);
                }
            }
            else
            {
                MessageBox.Show("נא להזין מחיר קנייה", "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);                
            }
        }

        private bool ShouldAddPurchase()
        {
            return _price != null && _price > 0;
        }

        #endregion
    }
}
