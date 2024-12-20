using Project.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
