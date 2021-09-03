using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string cartId); //given the userId, it should tell where the cart is located and give the cart data
        Task<Cart> UpdateCartAsync(Cart basket); //if add or deleteing sth in the cart, it should be able to update the cart with newly updated information
        Task<bool> DeleteCartAsync(string id); //given the buyer id, it should delete the cart. once the cart is converted into order it shouldn't exist anymore
    }
}
