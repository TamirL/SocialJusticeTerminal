using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SocialJusticTerminal.ViewModels
{
    class AddPurchaseViewModel : BaseViewModel
    {
        #region Fields

        private float? _price;

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
            //TODO: implement logic
            MessageBox.Show("ההתקשרות התבצעה בהצלחה", "מועדון צדק חברתי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
        }

        private bool ShouldAddPurchase()
        {
            return Price != null && Price > 0;
        }

        #endregion

    }
}
