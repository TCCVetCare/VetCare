using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCCVetCare.Models
{
    public class ItemsCartModel
    {
        [Key]
        public Guid idItemCart { get; set; }

        public string idProduct { get; set; }
        public string product { get; set; }
        public string idCart { get; set; }
        public double unitPrice { get; set; }
        public double valuePartial { get; set; }
        public int quantity { get; set; }
    }
}