using DibaEShop.DataAccess.Data;
using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using DibaEShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(string id)
        {
            var user=_context.applicationUsers.FirstOrDefault(u => u.Id == id); 
            if (user != null)
            {
                _context.applicationUsers.Update(user);

            }

            
        }

        
        public ApplicationUser GetUserForEdit(string id)
        {
            return  _context.applicationUsers.FirstOrDefault(u => u.Id == id);
        }
  

    }
}
