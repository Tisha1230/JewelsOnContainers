using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersContext _ordersContext;

        private readonly IConfiguration _config;

        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrdersContext ordersContext,
            ILogger<OrdersController> logger,
            IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _ordersContext = ordersContext;
        }

        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrder(int id) //show the order of the given order id
        {

            var item = await _ordersContext.Orders
                .Include(x => x.OrderItems)
                .SingleOrDefaultAsync(ci => ci.OrderId == id);
            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();

        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrders() //display all the orders you have placed so far
        {
            var orders = await _ordersContext.Orders.ToListAsync();


            return Ok(orders);
        }

        [Route("new")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order) //putting this order into the sql server
        {
            order.OrderStatus = OrderStatus.Preparing;
            order.OrderDate = DateTime.UtcNow;

            _logger.LogInformation(" In Create Order");
            _logger.LogInformation(" Order" + order.UserName);


            _ordersContext.Orders.Add(order); //adding orders to order table
            _ordersContext.OrderItems.AddRange(order.OrderItems); //adding multiple items to table
            _logger.LogInformation(" Order added to context");
            _logger.LogInformation(" Saving........");

            try
            {
                await _ordersContext.SaveChangesAsync(); //saving all the order changes
                return Ok(new { order.OrderId }); //sending back the order id so that the user can see what the order id is
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError("An error occored during Order saving .." + ex.Message);
                return BadRequest();
            }
        }

    }

}