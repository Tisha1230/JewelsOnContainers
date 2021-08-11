using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.ViewModels
{
    public class CatalogIndexViewModel 
    {
        public IEnumerable<SelectListItem> Brands { get; set; } //brand dropdown
        public IEnumerable<SelectListItem> Types { get; set; } //types dropdown
        public IEnumerable<CatalogItem> CatalogItems { get; set; } //items displayed in page
        public PaginationInfo PaginationInfo { get; set; } //page info

        //tracks what the user had selected before
        public int? BrandFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }
    }
}
