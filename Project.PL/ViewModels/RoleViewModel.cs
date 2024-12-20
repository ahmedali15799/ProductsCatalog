using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
