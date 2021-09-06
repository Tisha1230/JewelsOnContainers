using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;
using WebMvc.Models.CartModels;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using WebMvc;
using WebMvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using WebMvc.Models.OrderModels;

namespace WebMvc.Services
{
    public class CartService : ICartService
    {
        private readonly IConfiguration _config;
        private IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private IHttpContextAccessor _httpContextAccesor; //current browser sesssion
        private readonly ILogger _logger;
        public CartService(IConfiguration config, IHttpContextAccessor httpContextAccesor, 
            IHttpClient httpClient, ILoggerFactory logger)
        {
            _config = config;
            _remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/cart"; //path to cart microservice
            _httpContextAccesor = httpContextAccesor; 
            _apiClient = httpClient;  //client- help make request; clicking send in get and post method
            _logger = logger.CreateLogger<CartService>();
        }


        public async Task AddItemToCart(ApplicationUser user, CartItem product)
        {
            var cart = await GetCart(user);
            _logger.LogDebug("User Name: " + user.Email);
           
            var basketItem = cart.Items
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();
            if (basketItem == null)
            {
                cart.Items.Add(product);
            }
            else
            {
                basketItem.Quantity += 1;
            }


            await UpdateCart(cart);
        }

        public async Task ClearCart(ApplicationUser user) //once you place order, all the cart items disappears and get converted into order items
        {
            var token = await GetUserTokenAsync();
            var cleanBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogDebug("Clean Basket uri : " + cleanBasketUri);
            var response = await _apiClient.DeleteAsync(cleanBasketUri);
            _logger.LogDebug("Basket cleaned");
        }

        public async Task<Cart> GetCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync(); //gets the token
            _logger.LogInformation(" We are in get basket and user id " + user.Email);
            _logger.LogInformation(_remoteServiceBaseUrl);

            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Email); //using email, ask apipath to give api url you need to make call to
            _logger.LogInformation(getBasketUri);
            var dataString = await _apiClient.GetStringAsync(getBasketUri, token); //
            _logger.LogInformation(dataString);

            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ?? //goes to cartapi getcart method
               new Cart() //if no cart then vreates a new cart
               {
                   BuyerId = user.Email
               };
            return response;
        }

        public Order MapCartToOrder(Cart cart) //Converting cart into an order
        {
            var order = new Order();
            order.OrderTotal = 0;

            cart.Items.ForEach(x =>
            {
                order.OrderItems.Add(new OrderItem()
                {
                    ProductId = int.Parse(x.ProductId),

                    PictureUrl = x.PictureUrl,
                    ProductName = x.ProductName,
                    Units = x.Quantity,
                    UnitPrice = x.UnitPrice
                });
                order.OrderTotal += (x.Quantity * x.UnitPrice);
            });

            return order;
        }


        public async Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities) //dictionary<product id(string), new product quantity(int)
        {
            var basket = await GetCart(user);

            basket.Items.ForEach(x =>
            {
                // Simplify this logic by using the
                // new out variable initializer.
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });

            return basket;
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {

              var token = await GetUserTokenAsync();
            _logger.LogDebug("Service url: " + _remoteServiceBaseUrl);
            var updateBasketUri = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update Basket url: " + updateBasketUri);
            var response = await _apiClient.PostAsync(updateBasketUri, cart,token); 
            response.EnsureSuccessStatusCode();

            return cart;
        }

        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccesor.HttpContext; //context will now give you the current session information

            return await context.GetTokenAsync("access_token"); //within that session it will ask for the token,
                                                                //it will grab the token in current browswer session
          
        }
    }
}
