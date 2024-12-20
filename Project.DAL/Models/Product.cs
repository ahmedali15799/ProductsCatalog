using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime StartDate { get; set; }
        public int Durationdays { get; set; }
        public decimal Price { get; set; }
        public string ImageName { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
