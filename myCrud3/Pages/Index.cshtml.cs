using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myCrud3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext db, ILogger<IndexModel> logger)
        {
            _db = db;
            _logger = logger;
        }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool Success { get; set; }
        public IList<Customer> Customers { get; private set; }
        public async Task OnGetAsync(string sort)
        {
            if (sort == null || sort == "id") { 
                Customers = await _db.Customers.OrderBy(a => a.Id).AsNoTracking().ToListAsync();
            }
            else if (sort == "name")
            {
                Customers = await _db.Customers.OrderBy(a => a.Name).AsNoTracking().ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync (int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer != null)
            {
                try { 
                    _db.Customers.Remove(customer);
                    await _db.SaveChangesAsync();
                    Message = $"Customer {customer.Id} deleted";
                    Success = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    //throw new Exception($"Customer {Customer.Id} not found", e);
                    _logger.LogError($"Customer {customer.Id} not found", e);
                    Message = $"Customer {customer.Id} not found";
                    Success = false;
                }
            }
            return RedirectToPage();
        }
    }
}
