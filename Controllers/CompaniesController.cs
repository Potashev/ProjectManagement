using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Context;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly PMContext _context;

        public CompaniesController(PMContext context) {
            _context = context;
        }
        public async Task<IActionResult> AddCompany([Bind("Id,Name")] Company company) {
            if (ModelState.IsValid) {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return Redirect("~/Projects/SelectCompanies");

            }
            return Redirect("~/Projects/SelectCompanies");

        }
    }
}