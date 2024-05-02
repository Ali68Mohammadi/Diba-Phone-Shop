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
    public  class ShoppingCart
    {
        public int Id { get; set; }
            
        public int ProductId { get; set; }

        public string ApplicationUserId { get; set; }

        [Range(1,100,ErrorMessage ="please enter a value between 1 to 100")]
        public int Count { get; set; }

        [NotMapped]//در بانک درج نشود 
        public double TotalPrice { get; set; }

        #region Relation

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        #endregion

    }
}
