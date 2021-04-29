using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWeb;

namespace RestaurantWeb.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly RestaurantContext _context;

        public ProvidersController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Providers
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return View(await _context.Providers.ToListAsync());
            ViewBag.Id = id;
            var providerOfOrder= _context.Providers.Where(b => b.Id == id).Include(b => b.Orders);
            return View(await providerOfOrder.ToListAsync());
           
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phone")] Provider provider)
        {
            if (string.IsNullOrEmpty(provider.Name))
            {
                ModelState.AddModelError("Name", "Некорректне ім'я");
            }
            if (string.IsNullOrEmpty(provider.Address))
            {
                ModelState.AddModelError("Address", "Некорректна адреса");
            }
            if (ModelState.IsValid)
            {
                _context.Add(provider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(provider);
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Phone")] Provider provider)
        {
            if (id != provider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(provider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderExists(provider.Id))
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
            return View(provider);
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProviderExists(int id)
        {
            return _context.Providers.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Provider newcat;
                                var c = (from cat in _context.Providers
                                         where cat.Name.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Provider();
                                    newcat.Address = "";
                                    newcat.Phone = "";

                                    newcat.Name = worksheet.Name;
                                   // newcat.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Providers.Add(newcat);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Order book = new Order();
                                        //  book.Date = row.Cell(1).Value.ToDateTime;
                                        book.Price = Convert.ToInt32(row.Cell(3).Value);
                                        book.Amount = Convert.ToInt32(row.Cell(4).Value);
                                        book.Status = Convert.ToInt32(row.Cell(6).Value);
                                        book.Date = DateTime.Parse(row.Cell(2).Value.ToString());
                                        book.PlanReturn= DateTime.Parse(row.Cell(7).Value.ToString());
                                        book.Provider = newcat;
                                        _context.Orders.Add(book);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                       
                                            if (row.Cell(1).Value.ToString().Length > 0)
                                            {
                                                Product author;

                                                var a = (from aut in _context.Products
                                                         where aut.Name.Contains(row.Cell(1).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    author = a[0];
                                                }
                                                else
                                                {
                                                    author = new Product();
                                                    author.Name = row.Cell(1).Value.ToString();
                                                    author.QuantityAvailabale = 0;
                                                 //   author.Info = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Add(author);
                                                }
                                            // AuthorsBooks ab = new AuthorsBooks();
                                            // ab.Book = book;
                                            // ab.Author = author;
                                            //_context.AuthorsBooks.Add(ab);
                                            book.Product = author;

                                            }
                                        if (row.Cell(5).Value.ToString().Length > 0)
                                        {
                                            Restaurant rest;
                                            var r = (from res in _context.Restaurants
                                                     where res.Name.Contains(row.Cell(5).Value.ToString())
                                                     select res).ToList();
                                            if (r.Count > 0)
                                            {
                                                rest = r[0];
                                            }
                                            else
                                            {
                                                rest = new Restaurant();
                                                rest.Name = row.Cell(5).Value.ToString();
                                            }
                                            book.Restaurant = rest;
                                        }
                                        
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
 

        public IActionResult  Export(Order criteria)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
              //  ViewBag.ProviderLists = new SelectList(_context.Providers.Where(l => l.Orders.Count > 0), "Id", "Name");
              // var orders = _context.Providers.Where(w => w.Id == criteria.ProviderId).Include("Orders").ToList();

                var orders = _context.Providers.Where(w=>w.Name=="OvoProd").Include("Orders").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in orders)
                {
                    var worksheet = workbook.Worksheets.Add(c.Name);
                   
                    worksheet.Cell("A1").Value = "Продукт";
                    worksheet.Cell("B1").Value = "Дата замовлення";
                    worksheet.Cell("C1").Value = "Кількість";
                    worksheet.Cell("D1").Value = "Вартість";
                    worksheet.Cell("E1").Value = "Постачальник";
                    worksheet.Cell("F1").Value = "Планова дата доставки";
                    worksheet.Cell("G1").Value = "Статус";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var books = c.Orders.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < books.Count; i++)
                    {  
                        var productOfOrder = _context.Products.Where(b => b.Id == books[i].ProductId).Include(b => b.Orders).ToList();
                        worksheet.Cell(i + 2, 1).Value = productOfOrder[0].Name;
                        worksheet.Cell(i + 2, 2).Value = (books[i].Date).ToString();
                        worksheet.Cell(i + 2, 3).Value = books[i].Amount;
                        worksheet.Cell(i + 2, 4).Value = books[i].Price;
                        worksheet.Cell(i + 2, 5).Value = books[i].Provider.Name;
                        worksheet.Cell(i + 2, 6).Value = (books[i].PlanReturn).ToString();
                        worksheet.Cell(i + 2, 7).Value = books[i].Status;
                        

                       /* var ab = _context.AuthorsBooks.Where(a => a.BookId == books[i].Id).Include("Author").ToList();
                        //більше 4-ох нікуди писати
                        int j = 0;
                        foreach (var a in ab)
                        {
                            if (j < 5)
                            {
                                worksheet.Cell(i + 2, j + 2).Value = a.Author.Name;
                                j++;
                            }
                        }*/

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
        private void FillSelectLists()
        {
            // new SelectList(_context.Providers.Where(l => l.Orders.Count()>0), "Id", "Name");
            ViewBag.ProductList = new SelectList(_context.Products.Where(c => c.Orders.Count() > 0), "Id", "Name");
        }



    }

}
