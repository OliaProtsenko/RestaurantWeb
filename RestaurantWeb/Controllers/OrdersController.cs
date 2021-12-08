using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using RestaurantWeb;

namespace RestaurantWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly RestaurantContext _context;
        public OrdersApiController(RestaurantContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);


            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrdersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostArtist(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/OrdersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }


    }
    public class OrdersController : Controller
    {
        private readonly RestaurantContext _context;

        public OrdersController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int?id,string? name)

        {
            var restaurantContext = _context.Orders.Include(o => o.Product);
            if (id == null) return View(await restaurantContext.ToListAsync());
            ViewBag.ProductId = id;
            ViewBag.ProductName = name;
            var productOrder = _context.Orders.Where(o => o.ProductId == id).Include(o => o.Product);
            return View(await productOrder.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Provider)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Providers", new { id = order.ProviderId });
        }

        // GET: Orders/Create
        public IActionResult Create(int productId)
        {
            ViewBag.ProductId = productId;
            ViewBag.ProductName = _context.Products.Where(c => c.Id == productId).FirstOrDefault().Name;
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
             ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Name");
             ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, [Bind("Id,Date,Price,Amount,ProviderId,ProductId,RestaurantId,Status,PlanReturn,FactReturn")] Order order)
        {
            order.ProductId = productId;
            var todayDate = DateTime.Now;
            if (order.Date > todayDate)
            {
                ModelState.AddModelError("Date", "Некорректна дата");
            }
            if (order.PlanReturn < order.Date)
            {
                ModelState.AddModelError("PlanReturn", "Некорректна дата повернення");
            }
            if (order.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Некорректна кількість");
            }
            if(order.Price<0)
            {
                ModelState.AddModelError("Price", "Некорректна вартість");
            }

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Orders", new { Id = productId, name = _context.Products.Where(p => p.Id == productId).FirstOrDefault().Name });
                    //_context.Products.Where(p => p.Id == id).FirstOrDefault().Name });
            }
         //   ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Address", order.ProviderId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Address", order.RestaurantId);
            return RedirectToAction("Index", "Orders", new { Id = productId, name = _context.Products.Where(p => p.Id == productId).FirstOrDefault().Name });
        }
            // GET: Orders/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Address", order.ProviderId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Address", order.RestaurantId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Price,Amount,ProviderId,ProductId,RestaurantId,Status,PlanReturn,FactReturn")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            var todayDate = DateTime.Now;
            if (order.Date > todayDate)
            {
                ModelState.AddModelError("Date", "Некорректна дата");
            }
            if (order.PlanReturn < order.Date)
            {
                ModelState.AddModelError("PlanReturn", "Некорректна дата повернення");
            }
            if (order.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Некорректна кількість");
            }
            if (order.Price < 0)
            {
                ModelState.AddModelError("Price", "Некорректна вартість");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Address", order.ProviderId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Address", order.RestaurantId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Provider)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Search(string term,int type)
        {
            try
            {
                switch (type)
                {
                    case 0:
                        {
                            
                            var models = _context.Restaurants.Where(a => a.Name.Contains(term))
                                            .Select(a => new { label = a.Name, value = a.Id })
                                            .Distinct().ToList();
                            return Ok(models);
                        }
                    case 1: {
                            var models = _context.Providers.Where(a => a.Name.Contains(term))
                                          .Select(a => new { label = a.Name, value = a.Id })
                                          .Distinct().ToList();
                            return Ok(models);
                        }
                    case 2:
                        {
                            var models = _context.Products.Where(a => a.Name.Contains(term))
                                                                     .Select(a => new { label = a.Name, value = a.Id })
                                                                     .Distinct().ToList();
                            return Ok(models);
                        }
                    default:throw new  Exception("Wrong field");
            }
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        
        

    }
}
