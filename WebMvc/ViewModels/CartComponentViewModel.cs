using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.ViewModels
{
    public class CartComponentViewModel
    {
        public int ItemsInCart { get; set; }
        public decimal TotalCost { get; set; }
        public string Disabled => (ItemsInCart == 0) ? "is-disabled" : ""; //if cart item is 0, it will put html is disabled
    }
}
