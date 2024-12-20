using Microsoft.AspNetCore.Http;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime StartDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 day.")]
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
