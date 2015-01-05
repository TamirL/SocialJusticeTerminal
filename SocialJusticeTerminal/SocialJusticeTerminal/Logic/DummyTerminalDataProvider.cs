using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

        public void UseCoupon(CustomerCouponViewModel coupon)
        {
            File.AppendAllText(AppendWithDataPath("CouponsUsed.csv"), string.Format("{0},{1},{2},{3},{4}{5}", coupon.Id, coupon.CustomerId, coupon.StoreId, coupon.PointPrice, coupon.Description, Environment.NewLine));
        }

        public IEnumerable<CustomerCouponViewModel> GetCouponsOfCustomer(Guid customerId, Guid storeId)
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

        public Guid GetSelectedCustomer(string customerTz)
        {
            var filePath = AppendWithDataPath("Customers.csv");
            var customers = from line in File.ReadAllLines(filePath)
                let details = line.Split(',')
                select new {Tz = details[0], Id = new Guid(details[1])};
            var selected = customers.SingleOrDefault(x => x.Tz == customerTz);
            if (selected != null)
            {
                return selected.Id;
            }

            File.AppendAllText(filePath, string.Format("{0}, {1}", customerTz, Guid.NewGuid()));

            return Guid.Empty;
        }

        private static CustomerCouponViewModel FromLineToCoupon(string line, Guid customerId, Guid storeId)
        {
            var details = line.Split(',').SelectMany(x => x.Split('\t')).ToArray();
            return new CustomerCouponViewModel()
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