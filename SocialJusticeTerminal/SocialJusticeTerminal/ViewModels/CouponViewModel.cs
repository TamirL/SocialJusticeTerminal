using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialJusticeTerminal.ViewModels
{
    class CouponViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid StoreId { get; set; }
        public string Description { get; set; }
        public int PointPrice { get; set; }
    }
}
