using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialJusticeTerminal.Logic
{
    interface ITerminalDataProvider
    {
        void AddPurchase(string userId, string storeId, float price);
    }
}
