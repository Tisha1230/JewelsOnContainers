using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _context;
        private readonly IConfiguration _config;

        public CatalogController(CatalogContext context, IConfiguration config) //injecting database connection and configuration
        {
            _context = context;
            _config = config;
        }

        //[HttpGet("[action]/{pageIndex}/{pageSize}")]
        [HttpGet("[action]")]
        public async Task<IActionResult> Items(
            [FromQuery] int pageIndex = 0, 
            [FromQuery] int pageSize = 6)
        {
           
            var itemsCount = _context.Catalog.LongCountAsync(); // query to database to find total count;
                                                                // query catalog and ask for longcountasync

            var items = await _context.Catalog //items is an array
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(); //this will run this query in secondary thread and wait for results to come back

            
            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result, //calling the thread and asking for result i.e long,
                                          ////will force the main thread to wait then only move to next one
                Data = items

         
            };

            return Ok(model); //returning a json object (status) and data of items to caller
        }


        [HttpGet("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")] //TRY FROM QUERY
        public async Task<IActionResult> Items(
            int? catalogTypeId,        //int? = nullable int 
            int? catalogBrandId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<CatalogItem>)_context.Catalog; //converting into queryable type of Catalog table.
                                                                   //saying it is just a query, don't execute it right now

            if (catalogTypeId.HasValue)
            {
                query = query.Where(c => c.CatalogTypeId == catalogTypeId); 
            }
            if (catalogBrandId.HasValue)
            {
                query = query.Where(c => c.CatalogBrandId == catalogBrandId);
            }

            var itemsCount = query.LongCountAsync(); 

            var items = await query  
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(); 


            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result, 
                Data = items


            };

            return Ok(model); //returning a json object (status) and data of items to caller
        }

        //Route for catalogType
        [HttpGet("[action]/type/{catalogTypeId}")] //TRY FROM QUERY
        public async Task<IActionResult> Types(
           int? catalogTypeId,        //int? = nullable int 
           
           [FromQuery] int pageIndex = 0,
           [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<CatalogItem>)_context.Catalog; //converting into queryable type of Catalog table.
                                                                   //saying it is just a query, don't execute it right now

            if (catalogTypeId.HasValue)
            {
                query = query.Where(c => c.CatalogTypeId == catalogTypeId);
            }
           

            var itemsCount = query.LongCountAsync();

            var items = await query
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();


            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result,
                Data = items
            };

            return Ok(model); //returning a json object (status) and data of items to caller
        }


        //Route for catalogBrands
        [HttpGet("[action]/brand/{catalogBrandId}")] //TRY FROM QUERY
        public async Task<IActionResult> Brands(
           int? catalogBrandId,
           [FromQuery] int pageIndex = 0,
           [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<CatalogItem>)_context.Catalog; //converting into queryable type of Catalog table.
                                                                   //saying it is just a query, don't execute it right now
            if (catalogBrandId.HasValue)
            {
                query = query.Where(c => c.CatalogBrandId == catalogBrandId);
            }

            var itemsCount = query.LongCountAsync();

            var items = await query
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();


            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result,
                Data = items

            };

            return Ok(model); //returning a json object (status) and data of items to caller
        }

        //method to replace the http://externalcatalogbaseurltobereplaced Url of each item with config value(ExternalCatalogUrl)

        private List<CatalogItem> ChangePictureUrl(List<CatalogItem> items)
        {
            items.ForEach(item => 
            item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", _config["ExternalCatalogUrl"]));
            //replace method gives a new string back, so need to overwrite the picture Url with the change

            return items;
        }

        //method for Types Filter
        [HttpGet("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var types = await _context.CatalogTypes.ToListAsync();
            return Ok(types);
        }

        //method for Brands filter
        [HttpGet("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var brands = await _context.CatalogBrands.ToListAsync();
            return Ok(brands);
        }
    }
}
