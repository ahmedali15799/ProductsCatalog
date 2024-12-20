using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.BLL.Interfaces;
using Project.DAL.Data;
using Project.DAL.Models;

namespace Project.BLL.repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDBContext context) : base(context)
        {
        }
    }
}
