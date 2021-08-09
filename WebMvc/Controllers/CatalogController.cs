using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Services;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service) //injecting the service
        {
            _service = service;
        }

        public async Task<IActionResult>  Index(int? page, int? brandFilterApplied, int? typesFilterApplied)
        //this is the entry point from UI. Whatever user selects in view, these are the parameters that are going to come in as value
        {
            var itemsOnPage = 10; //hardcoding the pageSize.

            var catalog = await _service.GetCatalogItemsAsync(page ?? 0, itemsOnPage, brandFilterApplied, typesFilterApplied); //?? is simplified version of if statement for nullable check



            return View();
        }
    }
}
