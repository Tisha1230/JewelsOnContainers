using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service) //injecting the service
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? page, int? brandFilterApplied, int? typesFilterApplied)
        //this is the entry point from UI. Whatever user selects in view, these are the parameters that are going to come in as value
        {
            var itemsOnPage = 10; //hardcoding the pageSize.

            var catalog = await _service.GetCatalogItemsAsync(page ?? 0, itemsOnPage, brandFilterApplied, typesFilterApplied); //?? is simplified version of if statement for nullable check

            var vm = new CatalogIndexViewModel
            {
                Brands = await _service.GetBrandsAsync(), //data for dropdown. making a service call to get the data
                Types = await _service.GetTypesAsync(),
                CatalogItems = catalog.Data,  //the middle section of the page
                PaginationInfo = new PaginationInfo
                {
                    TotalItems = catalog.Count,
                    ItemsPerPage = catalog.PageSize,
                    ActualPage = page ?? 0, //if null then 0th page otherwise whatever page it is

                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count /itemsOnPage) 
                                //Count is long value, itemsOnPage is an int. this will lose the round value when divided. eg: 33/10 = 3 pages but we need 4 pages
                                //typecaste one of them, this case numerator into decimal, then Ceiling func will round it to next whole number
                                //then converting it back to an int cuz total page is an int and result was
                },

                BrandFilterApplied = brandFilterApplied ?? 0, //whatever user selected, if it is null then set it to 0
                TypesFilterApplied = typesFilterApplied ?? 0
            };


            return View(vm); //passing the data to the page
        }
    }
}
