using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace myCrud3.Pages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ILogger<EditModel> _logger;

        public EditModel(AppDbContext db, ILogger<EditModel> logger)
        {
            _db = db;
            _logger = logger;
        }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool Success { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _db.Customers.FindAsync(id);
            if (Customer == null)
            {
                Success = false;
                Message = "Unable to find user";
                return RedirectToPage("./Index");
            }
            return Page();
        }
      public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.Attach(Customer).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                Message = $"Customer {Customer.Id} edited!";
                Success = true;
            }
            catch(DbUpdateConcurrencyException e)
            {
                //throw new Exception($"Customer {Customer.Id} not found", e);
                _logger.LogError($"Customer {Customer.Id} not found", e);
                Message = $"Customer {Customer.Id} not found";
                Success = false;
            }
            return RedirectToPage("./Index");

        }
        public IActionResult OnPostCancel() => RedirectToPage("./Index");
    }
  
}
