using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication909.Areas.Admin.Models
{
    public class ChangeRoleViewModel
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; } = new();
    }
}