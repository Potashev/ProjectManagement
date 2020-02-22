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
            var projects = _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.Manager)
                .Include(p => p.Performer);
            return View(await projects.ToListAsync());
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

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null) {
                return NotFound();
            }

            var companies = _context.Companies.ToList();
            var employees = _context.Employees.ToList();

            ViewData["CustomerId"] = new SelectList(companies, "Id", "Name", project.CustomerId);
            ViewData["PerformerId"] = new SelectList(companies, "Id", "Name", project.PerformerId);
            ViewData["ManagerId"] = new SelectList(employees, "Id", "Surname", project.ManagerId);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,FinishDate,Priority,ManagerId,CustomerId,PerformerId")] Project project) {
            if (id != project.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ProjectExists(project.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var companies = _context.Companies.ToList();
            var employees = _context.Employees.ToList();

            ViewData["CustomerId"] = new SelectList(companies, "Id", "Name", project.CustomerId);
            ViewData["PerformerId"] = new SelectList(companies, "Id", "Name", project.PerformerId);
            ViewData["ManagerId"] = new SelectList(employees, "Id", "Surname", project.ManagerId);
            return View(project);
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.Manager)
                .Include(p => p.Performer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null) {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SelectCompanies() {

            var companies = _context.Companies.ToList();

            ViewData["CustomerId"] = new SelectList(companies, "Id", "Name");
            ViewData["PerformerId"] = new SelectList(companies, "Id", "Name");
            return View();
        }

        public IActionResult SelectManager(int? CustomerId, int? PerformerId) {
            if ((CustomerId == null) || (PerformerId == null)) {
                return NotFound();
            }
            ViewData["CustomerId"] = CustomerId;
            ViewData["PerformerId"] = PerformerId;
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Surname");
            return View();
        }


        public IActionResult InputProjectData(int? CustomerId, int? PerformerId, int? ManagerId) {
            if ((CustomerId == null) || (PerformerId == null)||(ManagerId == null)) {
                return NotFound();
            }
            ViewData["CustomerId"] = CustomerId;
            ViewData["PerformerId"] = PerformerId;
            ViewData["ManagerId"] = ManagerId;
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

                AddEmployeeOnProject((int)project.ManagerId, project.Id);

                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public IActionResult ShowMembers(int? id) {

            if(id == null) {
                return NotFound();
            }

            ViewData["ProjectId"] = id;
            
            var members = GetProjectMembersQuery((int)id);

            return View(members.ToList());
        }

        public IActionResult RemoveMember(int? projectId, int? memberId) {

            if((projectId == null)||(memberId == null)) {
                return NotFound();
            }

            if (IsManager((int)memberId, (int)projectId)) {
                RemoveManager((int)projectId);
            }
            RemoveEmployeeFromProject((int)memberId, (int)projectId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult SelectCandidate(int? id) {

            if(id == null) {
                return NotFound();
            }

            var projectMembers = GetProjectMembersQuery((int)id);
            var candidates = _context.Employees.Except(projectMembers);

            ViewData["CandidateId"] = new SelectList(candidates, "Id", "Surname");
            ViewData["ProjectId"] = id;
            return View();
        }

        public IActionResult AddMember(int? projectId, int? candidateId) {

            if ((projectId == null) || (candidateId == null)) {
                return NotFound();
            }

            AddEmployeeOnProject((int)candidateId, (int)projectId);

            return RedirectToAction(nameof(Index));
        }

        

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
        private void AddEmployeeOnProject(int employeeId, int projectId) {
            _context.ProjectEmployees.Add(new ProjectEmployee { ProjectId = projectId, EmployeeId = employeeId });
            _context.SaveChanges();
        }

        private void RemoveEmployeeFromProject(int employeeId, int projectId) {
            var projectEmployee = _context.ProjectEmployees
                .Where(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId)
                .SingleOrDefault();

            if(projectEmployee != null) {
                _context.ProjectEmployees.Remove(projectEmployee);
                _context.SaveChanges();
            }
        }

        private IQueryable<Employee> GetProjectMembersQuery(int projectId) {
            var resultQuery = from pe in _context.ProjectEmployees
                              where pe.ProjectId == projectId
                              select pe.Employee;
            return resultQuery;
        }
        private bool IsManager(int employeeId, int projectId) {
            var managerId = _context.Projects
                .Where(p => p.Id == projectId)
                .Select(p => p.ManagerId)
                .SingleOrDefault();

            if (managerId == employeeId)
                return true;
            else
                return false;
        }
        private void RemoveManager(int projectId) {
            var project = _context.Projects.Where(p => p.Id == projectId).SingleOrDefault();

            if (project != null) {
                project.ManagerId = null;
                _context.SaveChanges();
            }
        }
    }
}
