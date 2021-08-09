using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public static class ApiPaths  //render URl for all of the api calls
    {
        public static class Catalog
        {
            public static string GetAllTypes(string baseUri) //baseUri =http://localhost:7000/api/catalog
            {
                return $"{baseUri}catalogtypes";
            }
            public static string GetAllBrands(string baseUri)
            {
                return $"{baseUri}catalogbrands";
            }
            public static string GetAllCatalogItems(string baseUri, int page, int take, int? brand, int? type)
            {
                var filterQs = string.Empty;
                if (brand.HasValue || type.HasValue)
                {
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    var typeQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs = $"/type/{typeQs}/brand/{brandQs}";
                }
                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }
        }
    }
}
