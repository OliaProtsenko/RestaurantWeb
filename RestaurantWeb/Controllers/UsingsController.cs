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
    public class UsingsController : Controller
    {
        private readonly RestaurantContext _context;

        public UsingsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Usings
        public async Task<IActionResult> Index(int? id,string? name)
        {   if (id == null) return RedirectToAction("Dishes", "Index");
            ViewBag.DishId = id;
            ViewBag.DishName = name;
            var productForDish = _context.Usings.Where(b => b.DishId == id).Include(b => b.Product).Include(b=>b.Dish);
           // var restaurantContext = _context.Usings.Include(@ => @.Dish).Include(@ => @.Product);
            return View(await productForDish.ToListAsync());
        }

        // GET: Usings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @using = await _context.Usings
                .Include(b => b.Product)
                .Include(b => b.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@using == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index","Products", new { id = @using.ProductId });
        }

        // GET: Usings/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name");
            
            return View();
        }

        // POST: Usings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int dishId,[Bind("Id,Amount(g),ProductId,DishId")] Using @using)
        {
            @using.DishId = dishId;
            if (ModelState.IsValid)
            {
                _context.Add(@using);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Usings", new { id = dishId, name = _context.Dishes.Where(c => c.Id == dishId).FirstOrDefault().Name }) ;
            }

            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", @using.DishId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", @using.ProductId);
            return RedirectToAction("Index", "Usings", new { id = dishId, name = _context.Dishes.Where(c => c.Id == dishId).FirstOrDefault().Name });
        }

        // GET: Usings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @using = await _context.Usings.FindAsync(id);
            if (@using == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", @using.ProductId);
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", @using.DishId);
           
            return View(@using);
        }

        // POST: Usings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount(g),ProductId,DishId")] Using @using)
        {
            if (id != @using.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@using);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsingExists(@using.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", @using.ProductId);
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", @using.DishId);
            
            return View(@using);
        }

        // GET: Usings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @using = await _context.Usings
                .Include(b => b.Dish)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@using == null)
            {
                return NotFound();
            }

            return View(@using);
        }

        // POST: Usings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @using = await _context.Usings.FindAsync(id);
            _context.Usings.Remove(@using);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsingExists(int id)
        {
            return _context.Usings.Any(e => e.Id == id);
        }
    }
}
