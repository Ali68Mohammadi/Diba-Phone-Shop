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
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; } //order header id

        [Required]
        public int ProducId { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }

        #region Relation

        [ForeignKey("OrderId")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        
        
        [ForeignKey("ProducId")]
        [ValidateNever]
        public Product Product { get; set; }
        #endregion
    }
}
