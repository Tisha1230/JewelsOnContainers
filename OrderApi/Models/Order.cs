using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public enum OrderStatus
    {
        Preparing = 1,
        Shipped = 2,
        Delivered = 3,
    }
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public string BuyerId { get; set; }

        public string UserName { get; set; } //BuyerID and Username is the same in this case which is the Email address

        public OrderStatus OrderStatus { get; set; }//only for demo purpose; this case only be 1. preparing

        public string Address { get; set; }
        public string PaymentAuthCode { get; set; }
        public decimal OrderTotal { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; } //actual list of order items
        protected Order() //constructor for order to basically create empty list of order item that you can work with
        {
            OrderItems = new List<OrderItem>();
        }

  
    }
}
