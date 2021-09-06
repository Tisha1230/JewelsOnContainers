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
                if (brand.HasValue && type.HasValue)
                {
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    var typeQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs = $"/type/{typeQs}/brand/{brandQs}";

                    return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
                }

                else if ( type.HasValue)
                {
                    var typeQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs = $"/type/{typeQs}";

                    return $"{baseUri}types{filterQs}?pageIndex={page}&pageSize={take}";
                }

                else if (brand.HasValue)
                {
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    filterQs = $"/brand/{brandQs}";

                    return $"{baseUri}brands{filterQs}?pageIndex={page}&pageSize={take}";
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }

            public static string UpdateBasket(string baseUri)
            {
                return baseUri;
            }

            public static string CleanBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }

        public static class Order
        {
            public static string GetOrder(string baseUri, string orderId) //given the id it will get your order
            {
                return $"{baseUri}/{orderId}";
            }

            public static string GetOrders(string baseUri) // it will give you all your orders that has been placed
            {
                return baseUri;
            }
            public static string AddNewOrder(string baseUri) //add a new order
            {
                return $"{baseUri}/new";
            }
        }
    }
}
