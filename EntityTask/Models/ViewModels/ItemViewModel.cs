using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EntityTask.Models.ViewModels
{
    public class ItemViewModel
    {
        public Item Item { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UnitSelectList { get; set; }
    }
}
