using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DibaEShop.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }//selectlist =combobox


    }
}
