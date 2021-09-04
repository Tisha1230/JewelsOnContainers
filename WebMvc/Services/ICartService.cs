using WebMvc.Models;
using WebMvc.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using WebMvc.Models.OrderModels;

namespace WebMvc.Services
{
    public interface ICartService
    {
        Task<Cart> GetCart(ApplicationUser user);
        Task AddItemToCart(ApplicationUser user, CartItem product); //add 1 item in the cart; CartItem product calls update cart
        Task<Cart> UpdateCart(Cart Cart); //this is used when there is changes in quantity; modifying cart eg: remove
        Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities); //change quantity
        //Order MapCartToOrder(Cart Cart);
        Task ClearCart(ApplicationUser user);

    }
}
