using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EntityTask.Models.ViewModels
{
    public class ItemIndexViewModel
    {
        public IList<Item> Items { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UnitSelectList { get; set; }
    }
}
