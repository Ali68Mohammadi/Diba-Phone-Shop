using DibaEShop.Models;
using DibaEShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        void Update(string id);
        ApplicationUser GetUserForEdit(string id);
        //EditUserViewModel EditUser(EditUserViewModel userViewModel);

    }
}
