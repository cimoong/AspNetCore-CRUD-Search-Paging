using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCRUD.Models;
using Rotativa.AspNetCore;
using ReflectionIT.Mvc.Paging;

namespace CoreCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly BaseDbContext _context;

        public EmployeeController(BaseDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index(int page =1, string query=null)
        {
            ViewBag.query = query;
            if (query != null)
            {
                var data = _context.Employees.Where(a => EF.Functions.Like(a.FullName, "%" + query + "%") ||
                EF.Functions.Like(a.Empcode, "%" + query + "%") ||
                EF.Functions.Like(a.Position, "%" + query + "%") ||
                EF.Functions.Like(a.OfficeLocation, "%" + query + "%")).AsNoTracking().OrderBy(a => a.FullName);
                var model = await PagingList.CreateAsync(data, int.MaxValue, 1);
                return View(model);
            }
            else {
                var data = _context.Employees.AsNoTracking().OrderBy(a => a.FullName);
                var model = await PagingList.CreateAsync(data, 5, page);

                return View(model);
            }


        }

        // GET: Employee/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Employee());
            else
            {
                var model = _context.Employees.Find(id);
                return View(model);
            }
            
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("EmployeeId,FullName,Empcode,Position,OfficeLocation")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.EmployeeId == 0)
                    _context.Add(employee);
                else 
                    _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else {
                var employee = await _context.Employees.FindAsync(id);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Employee
        public async Task<IActionResult> Print()
        {
            var report = new ViewAsPdf(await _context.Employees.ToListAsync())
            {
                //PageSize = Rotativa.AspNetCore.Options.Size.A6,
                PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                //CustomSwitches =  "--page-offset 0 --footer-center [page] --footer-font-size 12",
                CustomSwitches = "--footer-center \"  Created Date: " +
          DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
          " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
            };

            return report;
        }

    }
}
