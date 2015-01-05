using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SocialJusticeTerminal.ViewModels;

namespace SocialJusticeTerminal.Logic
{
    class DummyTerminalDataProvider : ITerminalDataProvider
    {
        public void AddPurchase(Guid customerId, Guid storeId, float price)
        {
            File.AppendAllText(AppendWithDataPath("Purchases.csv"), string.Format("{0},{1},{2},{3}{4}", customerId, storeId, price, price/10, Environment.NewLine));
        }

        public void WriteToLog(Exception exception)
        {
            
        }

        public void UseCoupon(CouponViewModel coupon)
        {
            File.AppendAllText(AppendWithDataPath("CouponsUsed.csv"), string.Format("{0},{1},{2},{3},{4}{5}", coupon.Id, coupon.CustomerId, coupon.StoreId, coupon.PointPrice, coupon.Description, Environment.NewLine));
        }

        public IEnumerable<CouponViewModel> GetCouponsOfCustomer(Guid customerId, Guid storeId)
        {
            if (new Random().Next(1000) < 500)
            {
                return new List<CustomerCouponViewModel>();
            }

            string path = AppendWithDataPath("Coupons.csv");
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            var lines = File.ReadAllLines(path, Encoding.Default);
            return lines.Select(line => FromLineToCoupon(line, customerId, storeId)).Take(new Random().Next(lines.Count()));
        }

        private static CouponViewModel FromLineToCoupon(string line, Guid customerId, Guid storeId)
        {
            var details = line.Split(',').SelectMany(x => x.Split('\t')).ToArray();
            return new CouponViewModel()
            {
                Id = new Guid(details[0]),
                CustomerId = customerId,
                StoreId = storeId,
                PointPrice = int.Parse(details[3]),
                Description = details[4]
            };
        }

        private string AppendWithDataPath(string fileName)
        {
            return Path.Combine(ConfigurationManager.AppSettings["DummyDataPath"], fileName);
        }
    }
}