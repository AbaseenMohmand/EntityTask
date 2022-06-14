using EntityTask.Data;
using EntityTask.ExtensionsMethod;
using EntityTask.Models;
using EntityTask.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EntityTask.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HelperMethod _helperMethod;
        public ItemController(ApplicationDbContext context, HelperMethod helperMethod)
        {
            _context = context;
            _helperMethod = helperMethod;
        }
        public async Task<IActionResult> Index()
        {
            


                var itemIndexVM = new ItemIndexViewModel()
                {
                    Items =  await _context.Items.Include(a => a.ItemUnit).ThenInclude(b => b.Unit)
                        .OrderBy(a => a.Price)
                            .Reverse().ToListAsync(),
            UnitSelectList =  _context.Units.Include(x => x.ItemUnit).ToListAsync().GetAwaiter().GetResult()
                .Select(a => new SelectListItem
                {
                    Text = a.UnitName,
                    Value = a.Id.ToString()
                })
                };

                return View(itemIndexVM);
            }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            
                var addItem = await _context.Items.AddAsync(item);
                if (addItem != null)
                {
                    await _context.SaveChangesAsync();
                }
            
            return View(item);
        }

        public IActionResult Delete(int Id)
        {
            var item  = _context.Items.FirstOrDefault(x => x.Id == Id);
            if (item == null)
                return NotFound("Unit Not Found");
            return View(item);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int Id)
        {
            var itemId = _context.Items.FirstOrDefault(x => x.Id == Id);
            _context.Items.Remove(itemId);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int Id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == Id);
            if (item == null)
                return NotFound("Unit Not Found");
            return View(item);
        }

        public IActionResult AddUnitsToItem(int Id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == Id);

            if (item != null)
            {
                ItemUnitViewModel vm = new ItemUnitViewModel()
                {
                    ItemId = Id,

                    UnitSelectList = _context.Units.Include(x => x.ItemUnit).ToListAsync()
                        .GetAwaiter()
                            .GetResult()
                                .Where(a => !a.ItemUnit.Select(a => a.ItemId)
                                    .Contains(Id))
                        .Select(a => new SelectListItem
                        {
                            Text = a.UnitName,
                            Value = a.Id.ToString()
                        })
                };
                return View(vm);
            }
            return NotFound("Item Not Found!");
        }
        [HttpPost]
        [ActionName("AddUnitsToItem")]
        public IActionResult AddUnitsToItem(ItemUnitViewModel vm)
        {
           
            if (_helperMethod.addItemUnit(vm.ItemId, vm.UnitId))
            {
                return RedirectToAction(nameof(Index));
            }
            vm = new ItemUnitViewModel()
            {
                ItemId = vm.ItemId,
                UnitSelectList = _context.Units.Include(x => x.ItemUnit).ToListAsync().GetAwaiter().GetResult()
                        .Select(a => new SelectListItem
                        {
                            Text = a.UnitName,
                            Value = a.Id.ToString()
                        })
            };
            return View(vm);
        }

        public IActionResult Edit(int Id, int unitId)
        {
            if (unitId > 0)
            {
                _helperMethod.removeItemUnit(Id, unitId);
            }
             var item = _context.Items
                .Include(a => a.ItemUnit)
                    .ThenInclude(b => b.Unit)
                        .FirstOrDefault(a => a.Id == Id); 
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item itemVm)
        {
            
            
                var result =  _context.Update(itemVm);
                await _context.SaveChangesAsync(); 
                
                return RedirectToAction(nameof(Index));
            

            return View(itemVm);
        }
    }
}
