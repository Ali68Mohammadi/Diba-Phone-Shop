using DibaEShop.DataAccess.Data;
using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void SoftDelete(Category category )
        {
          
                var CategoryFromDb = _context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
                if (CategoryFromDb != null)
                {
                    CategoryFromDb.IsDeleted = true;
                }
           
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
