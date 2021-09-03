using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class RedisCartRepository : ICartRepository 
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        public RedisCartRepository(ConnectionMultiplexer redis) //pass the IP Address
        {
            _redis = redis; //server location
            _database = redis.GetDatabase(); //folder within that server that can store all json object
        }

        //Given the user id delete the entire cart; just need the database not redis server
        public async Task<bool> DeleteCartAsync(string id) 
        {
            return await _database.KeyDeleteAsync(id);
        }

        //Getting a cart if it exist else create one
        public async Task<Cart> GetCartAsync(string cartId)
        {
            var data = await _database.StringGetAsync(cartId);
            if (data.IsNullOrEmpty) 
                return null;        //means no cart

            return JsonConvert.DeserializeObject<Cart>(data); //converting string back into cart object
        }

        public async Task<Cart> UpdateCartAsync(Cart basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket)); //creating a file in ReDis repository if it doesn't exist; if file exists it will locate it by its Id and update it
            if (!created)
                return null;

            return await GetCartAsync(basket.BuyerId); // using GetCartAsync method cuz if what we updated in cart is successful it will return back that data
        }
    }
}
