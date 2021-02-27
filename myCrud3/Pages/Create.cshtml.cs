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
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel (AppDbContext db, ILogger<CreateModel> logger) 
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


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try { 
                _db.Customers.Add(Customer);
                await _db.SaveChangesAsync();
                Message = $"Customer {Customer.Name} added!";
                Success = true;
            }
            catch (DbUpdateConcurrencyException e)
            {
                //throw new Exception($"Customer {Customer.Id} not found", e);
                _logger.LogError($"Customer {Customer.Name} not added", e);
                Message = $"Customer {Customer.Name} not added";
                Success = false;
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostCancel() => RedirectToPage("/Index");
        public void OnGet()
        {
        }
    }
}
