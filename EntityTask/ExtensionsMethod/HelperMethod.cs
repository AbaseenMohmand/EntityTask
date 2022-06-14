using EntityTask.Data;
using EntityTask.Models;

namespace EntityTask.ExtensionsMethod
{
    public class HelperMethod
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public HelperMethod(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool addItemUnit(int itemId, int unitId)
        {
            _applicationDbContext.ItemUnits.Add(new ItemUnit() { ItemId = itemId, UnitId = unitId });
            var result = _applicationDbContext.SaveChanges();
            if (result > 0)
                return true;
            return false;
        }
        public bool removeItemUnit(int itemId, int unitId)
        {
            _applicationDbContext.ItemUnits.Remove(new ItemUnit() { ItemId = itemId, UnitId = unitId });
            var result = _applicationDbContext.SaveChanges();
            if (result > 0)
                return true;
            return false;
        }
    }
}
