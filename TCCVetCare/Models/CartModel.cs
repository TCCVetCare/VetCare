using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCCVetCare.Models
{
    public class CartModel
    {
        public string idSale { get; set; }

        public string dateSale { get; set; }

        public string idCustomer { get; set; }

        public string timeSale { get; set; }

        public double valueTotal { get; set; }

        public List<ItemsCartModel> ItemsCart = new List<ItemsCartModel>();
    }
}