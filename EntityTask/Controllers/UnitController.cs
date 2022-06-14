using EntityTask.Data;
using EntityTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityTask.Controllers
{
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UnitController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var unit = await _context.Units.Include(x => x.ItemUnit).ToListAsync();
            return View(unit);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Unit unit)
        {
            if (unit.UnitName != null)
            {
                _context.Units.Add(unit);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var unit = _context.Units.FirstOrDefault(x => x.Id == Id);
            if (unit == null)
                return NotFound("Unit Not Found");
            return View(unit);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int Id)
        {
            var unitID = _context.Units.FirstOrDefault(x => x.Id == Id);
            _context.Units.Remove(unitID);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
            return View(unit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Unit unit)
        {
            var model =  await _context.Units.FindAsync(unit.Id);
            if(model != null)
            {
                   // model.UnitName = unit.UnitName;
                
                    _context.Units.Update(model);
                     _context.SaveChanges();
                
            }
          

            return View("Index");
        }
    }
}
