using ProductCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.ViewModels
{
    public class PaginatedItemsViewModel // definition of what data you add to list and sending back to view
    {
        public int PageIndex { get; set; } //which page you are looking at; data you want to send back
        public int PageSize { get; set; } //sending back actual count now
        public long Count { get; set; } //total count of records in database =15
        public IEnumerable<CatalogItem> Data { get; set; } //list of catalog item

    }
}
