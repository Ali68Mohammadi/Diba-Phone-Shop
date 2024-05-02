using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.Models
{
    public class Product
    {


        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string? ImageUrl { get; set; } 
        
        public bool IsDeleted { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

       
        #region relation  //navigation propperty

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        #endregion

    }
}
