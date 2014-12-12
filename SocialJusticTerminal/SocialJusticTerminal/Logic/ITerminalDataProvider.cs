using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SocialJusticTerminal.Logic
{
    interface ITerminalDataProvider
    {
        void AddPurchase(string userId, string storeId, float price);
    }

    class DummyTerminalDataProvider : ITerminalDataProvider
    {
        public void AddPurchase(string userId, string storeId, float price)
        {
            File.AppendAllText("D:\\Purchases.csv", string.Format("{0},{1},{2},{3}{4}", userId, storeId, price, price/10, Environment.NewLine));
        }
    }
}
