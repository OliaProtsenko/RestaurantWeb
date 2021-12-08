using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWeb;
namespace RestaurantWeb.Managers
{
    public class DishesManager
    {
        private readonly RestaurantContext _context;

        public DishesManager(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            var restaurantContext = _context.Dishes.Include(d => d.Restaurant);
            return await restaurantContext.ToListAsync();
        }
        public async Task<ActionResult<Dish>> GetDish(int? id)
        {       
            var dish = await _context.Dishes
                .Include(e => e.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
                     return dish;
        }
        

        // GET: Dishes/Create
       

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Dish>> Create(int id, Dish dish)
            {
               
                _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
                    throw;
               
            }

            return dish;
            }
            //  dish.RestaurantId =restaurantId;
            
      

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        public async Task<ActionResult<Dish>> Edit(int id,  Dish dish)
        {
            

            
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                        throw;
                   
               
            }
           
            return dish;
        }

      
        /*
      
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return await _context.Dishes.ToListAsync();
        }*/

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
