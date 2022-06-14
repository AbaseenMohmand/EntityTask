using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EntityTask.Models.ViewModels
{
    public class ItemUnitViewModel
    {
        public int ItemId { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UnitSelectList { get; set; }
        [Required]
        public int UnitId { get; set; }
    }
}
