using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWeb;

namespace RestaurantWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RestaurantContext _context;

        public OrdersController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int?id,string ?name)

        {
            //var restaurantContext = _context.Orders.Include(o => o.Product).Include(o => o.Provider).Include(o => o.Restaurant);
            if (id == null) return RedirectToAction("Products", "Index");
            ViewBag.ProductId = id;
            ViewBag.ProductName = name;
            var productOrder = _context.Orders.Include(o => o.ProductId == id).Include(o => o.Product);
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

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Address");
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Address");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Date,Price,Amount,ProviderId,ProductId,RestaurantId,Status,PlanReturn,FactReturn")] Order order)
        {
            order.ProductId = id;
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Orders", new { Id = id, name = _context.Products.Where(p => p.Id == id).FirstOrDefault().Name });
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Address", order.ProviderId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Address", order.RestaurantId);
            return RedirectToAction("Index", "Orders", new { Id = id, name = _context.Products.Where(p => p.Id == id).FirstOrDefault().Name });
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

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
