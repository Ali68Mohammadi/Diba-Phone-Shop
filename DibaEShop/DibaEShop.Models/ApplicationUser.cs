using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.Models
{
    public class ApplicationUser : IdentityUser // Identityuser=AspNetUsers table in db
        //for ihertans from IdentityUser should install pakage microsoft.extensions.Identity.Stores
    {//برای تغییر در جداولی که خود Identity ساخته
        //برای تغییرات در جدول Applacation user
        // یک جدول می سازم و از اینترفیس IDentityUser ارث بری میکنم
        //حالا فیلد هایی که میخواهم به جدولی که خود Identity ساخته  را اضافه می کنم 
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? PostalCode { get; set; }
       

    }
}
