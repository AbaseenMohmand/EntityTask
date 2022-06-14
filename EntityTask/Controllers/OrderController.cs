using EntityTask.Data;
using EntityTask.Models;
using EntityTask.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityTask.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var orderlist = await _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit)
                          .OrderBy(a => (a.OrderItem.Select(a => a.Quanity * a.Item.Price).Sum()))
                              .Reverse().ToListAsync();



            //Joins 
            var joinList = (from order in _context.Orders
                            join OrderItem in _context.OrderItems on order.Id equals OrderItem.OrderId
                            join item in _context.Items on OrderItem.ItemId equals item.Id
                            select new Order
                            {
                                Id = order.Id,
                                OrderName = order.OrderName


                            }).ToListAsync();





            return View(orderlist);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            var order = _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit).ThenInclude(a => a.Unit).FirstOrDefault(a => a.Id == Id);
            if (order == null)
                return NotFound("Order Not Found");
            return View(order);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int Id)
        {
            var order = _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit).ThenInclude(a => a.Unit).FirstOrDefault(a => a.Id == Id);
            foreach (var x in order.OrderItem)
            {
                var item = x.Item;
                item.Quantity = x.Quanity + item.Quantity;
                _context.Update(item);
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int Id)
        {
            var order = _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit).ThenInclude(a => a.Unit).FirstOrDefault(a => a.Id == Id);
            return View(order);
        }
        public IActionResult Edit(int Id)
        {
            var order = _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit).ThenInclude(a => a.Unit).FirstOrDefault(a => a.Id == Id);
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        public async Task<IActionResult> OrderItems(int Id)
        {
            var vm = new OrderItemViewModel()
            {
                Order = _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit).ThenInclude(a => a.Unit).FirstOrDefault(a => a.Id == Id),
                AllItems = await _context.Items.Include(a => a.ItemUnit).ThenInclude(b => b.Unit).OrderBy(a => a.Price).Reverse().ToListAsync(),
                ItemsWithinOrder = _context.Orders.Include(a => a.OrderItem).ThenInclude(b => b.Item).ThenInclude(c => c.ItemUnit).ThenInclude(d => d.Unit).Where(d => d.Id == Id).Select(a => a.OrderItem).FirstOrDefault()
            };
            return View(vm);
        }

        public void addItemToCart(int orderId, int itemId)
        {
            var updateItem = _context.Items.FirstOrDefault(a => a.Id == itemId);
            var result = _context.OrderItems.FirstOrDefault(a => a.OrderId == orderId && a.ItemId == itemId);
            if (result == null)
            {
                if (updateItem.Quantity > 0)
                {
                    _context.Add(new OrderItem() { ItemId = itemId, OrderId = orderId, Quanity = 1 });
                    updateItem.Quantity = updateItem.Quantity - 1;
                }
            }
            else
            {
                if (updateItem.Quantity > 0)
                {
                    updateItem.Quantity = updateItem.Quantity - 1;
                    result.Quanity = result.Quanity + 1;
                    _context.Update(result);
                    _context.Update(updateItem);
                }
            }
            _context.SaveChanges();
        }

        public void removeItemFromCart(int orderId, int itemId)
        {

            var result = _context.OrderItems.FirstOrDefault(a => a.OrderId == orderId && a.ItemId == itemId);
            if (result != null)
            {
                var updateItem = _context.Items.FirstOrDefault(a => a.Id == itemId);
                updateItem.Quantity = updateItem.Quantity + 1;
                result.Quanity = result.Quanity - 1;
                if (result.Quanity == 0)
                    _context.Remove(result);
                else
                    _context.Update(result);
                _context.Update(updateItem);
                _context.SaveChanges();
            }

        }
    }
}
