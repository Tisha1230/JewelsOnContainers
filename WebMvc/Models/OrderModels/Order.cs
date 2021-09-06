using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models.OrderModels
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public DateTime OrderDate { get; set; }

      
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal OrderTotal { get; set; }

        [Required] //when you receive data from the API you expect these [Required] fields to be populated
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }

        [BindNever] //Even the API send you this data, Don't fill it cuz it will be populated by UI; just a choice
        public string UserName { get; set; }
        [BindNever]
        public string BuyerId { get; set; }
        public string StripeToken { get; set; } // the 2 API keys in stripe; public key token 
        
        public OrderStatus OrderStatus { get; set; }

        // See the property initializer syntax below. This
        // initializes the compiler generated field for this
        // auto-implemented property.
        public List<OrderItem> OrderItems { get; } = new List<OrderItem>();

       
        public string PaymentAuthCode { get; set; } //confirmation that the payment is completed
    }
    public enum OrderStatus
    {
        Preparing = 1,
        Shipped = 2,
        Delivered = 3
    }


}
 
