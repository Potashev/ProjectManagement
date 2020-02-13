using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Context;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly PMContext _context;

        public ProjectsController(PMContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pMContext = _context.Projects.Include(p => p.Customer).Include(p => p.Manager).Include(p => p.Performer);
            return View(await pMContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.Manager)
                .Include(p => p.Performer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public IActionResult Create()
        {
            return RedirectToAction(nameof(SelectCompanies));
        }

        
        public IActionResult SelectCompanies() {
            ViewData["CustomerId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["PerformerId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        public IActionResult SelectManager(int? CustomerId, int? PerformerId) {
            if ((CustomerId == null) || (PerformerId == null)) {
                return NotFound();
            }
            ViewBag.CustomerId = CustomerId;
            ViewBag.PerformerId = PerformerId;
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Surname");
            return View();
        }

        public IActionResult InputProjectData(int? CustomerId, int? PerformerId, int? ManagerId) {
            if ((CustomerId == null) || (PerformerId == null)||(ManagerId == null)) {
                return NotFound();
            }
            ViewBag.CustomerId = CustomerId;
            ViewBag.PerformerId = PerformerId;
            ViewBag.ManagerId = ManagerId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,FinishDate,Priority,ManagerId,CustomerId,PerformerId")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Companies, "Id", "Name", project.CustomerId);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Surname", project.ManagerId);
            ViewData["PerformerId"] = new SelectList(_context.Companies, "Id", "Name", project.PerformerId);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,FinishDate,Priority,ManagerId,CustomerId,PerformerId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Companies, "Id", "Id", project.CustomerId);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Id", project.ManagerId);
            ViewData["PerformerId"] = new SelectList(_context.Companies, "Id", "Id", project.PerformerId);
            return View(project);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.Manager)
                .Include(p => p.Performer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
