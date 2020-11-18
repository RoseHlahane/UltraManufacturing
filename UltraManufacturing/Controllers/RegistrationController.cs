using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UltraManufacturing.Models.Entities;

namespace UltraManufacturing.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly employeesContext _context;

        public RegistrationController(employeesContext context)
        {
            _context = context;
        }

        // GET: Registration
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cmpg323Project2Dataset.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string empSearch)
        {
            ViewData["GetEmployeeDetails"] = empSearch;

            var empQuery = from x in _context.Cmpg323Project2Dataset select x;
            if (!String.IsNullOrEmpty(empSearch))
            {
                empQuery = empQuery.Where(x => x.JobRole.Contains(empSearch) || x.EmployeeNumber.Contains(empSearch));
            }
            return View(await empQuery.AsNoTracking().ToListAsync());
        }

        // GET: Registration/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmpg323Project2Dataset = await _context.Cmpg323Project2Dataset
                .FirstOrDefaultAsync(m => m.EmployeeNumber == id);
            if (cmpg323Project2Dataset == null)
            {
                return NotFound();
            }

            return View(cmpg323Project2Dataset);
        }

        // GET: Registration/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Age,Attrition,BusinessTravel,DailyRate,Department,DistanceFromHome,Education,EducationField,EmployeeCount,EmployeeNumber,EnvironmentSatisfaction,Gender,HourlyRate,JobInvolvement,JobLevel,JobRole,JobSatisfaction,MaritalStatus,MonthlyIncome,MonthlyRate,NumCompaniesWorked,Over18,OverTime,PercentSalaryHike,PerformanceRating,RelationshipSatisfaction,StandardHours,StockOptionLevel,TotalWorkingYears,TrainingTimesLastYear,WorkLifeBalance,YearsAtCompany,YearsInCurrentRole,YearsSinceLastPromotion,YearsWithCurrManager")] Cmpg323Project2Dataset cmpg323Project2Dataset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cmpg323Project2Dataset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cmpg323Project2Dataset);
        }

        // GET: Registration/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmpg323Project2Dataset = await _context.Cmpg323Project2Dataset.FindAsync(id);
            if (cmpg323Project2Dataset == null)
            {
                return NotFound();
            }
            return View(cmpg323Project2Dataset);
        }

        // POST: Registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Age,Attrition,BusinessTravel,DailyRate,Department,DistanceFromHome,Education,EducationField,EmployeeCount,EmployeeNumber,EnvironmentSatisfaction,Gender,HourlyRate,JobInvolvement,JobLevel,JobRole,JobSatisfaction,MaritalStatus,MonthlyIncome,MonthlyRate,NumCompaniesWorked,Over18,OverTime,PercentSalaryHike,PerformanceRating,RelationshipSatisfaction,StandardHours,StockOptionLevel,TotalWorkingYears,TrainingTimesLastYear,WorkLifeBalance,YearsAtCompany,YearsInCurrentRole,YearsSinceLastPromotion,YearsWithCurrManager")] Cmpg323Project2Dataset cmpg323Project2Dataset)
        {
            if (id != cmpg323Project2Dataset.EmployeeNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
              
                    _context.Update(cmpg323Project2Dataset);
                    await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(Index));
            }
            return View(cmpg323Project2Dataset);
        }

        // GET: Registration/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmpg323Project2Dataset = await _context.Cmpg323Project2Dataset
                .FirstOrDefaultAsync(m => m.EmployeeNumber == id);
            if (cmpg323Project2Dataset == null)
            {
                return NotFound();
            }

            return View(cmpg323Project2Dataset);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cmpg323Project2Dataset = await _context.Cmpg323Project2Dataset.FindAsync(id);
            _context.Cmpg323Project2Dataset.Remove(cmpg323Project2Dataset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Cmpg323Project2DatasetExists(string id)
        {
            return _context.Cmpg323Project2Dataset.Any(e => e.EmployeeNumber == id);
        }
    }
}
