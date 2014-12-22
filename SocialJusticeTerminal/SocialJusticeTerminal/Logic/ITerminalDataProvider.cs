using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialJusticeTerminal.ViewModels;

namespace SocialJusticeTerminal.Logic
{
    interface ITerminalDataProvider
    {
        void AddPurchase(Guid customerId, Guid storeId, float price);
        void WriteToLog(Exception exception);
        void UseCoupon(CouponViewModel coupon);
        IEnumerable<CouponViewModel> GetCouponsOfCustomer(Guid customerId, Guid storeId);
    }
}
