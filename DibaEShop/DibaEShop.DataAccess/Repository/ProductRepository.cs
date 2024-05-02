using DibaEShop.DataAccess.Data;
using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.DataAccess.Repository
{

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);  
        }

        public void SoftDelete(Product product)
        {
          var  productFromDb = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (productFromDb != null)
            {
                productFromDb.IsDeleted = true;
            }
        }

    }

}
