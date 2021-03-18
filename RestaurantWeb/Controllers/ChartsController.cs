using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RestaurantWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly RestaurantContext _context;
        public ChartsController(RestaurantContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData") ]
        public JsonResult JsonData()
        {
            var products = _context.Products.Include(d => d.Orders).ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Продукт", "Кількість замовлень" });
            foreach (var c in products)
            {
                catBook.Add(new object[] { c.Name, c.Orders.Count() });
            }
            return new JsonResult(catBook);
        }
        [HttpGet("ProviderOrders")]
        public JsonResult ProviderOrders()
        {
            var providers = _context.Providers.Include(d => d.Orders).ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Продукт", "Кількість замовлень" });
            foreach (var c in providers)
            {
                catBook.Add(new object[] { c.Name, c.Orders.Count() });
            }
            return new JsonResult(catBook);
        }


    }
}
