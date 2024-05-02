using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DibaEShop.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public string ApplicatioUserId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }

        public string? PaymentStatus { get; set; }

        public string? TrckingNumber { get; set; }

        public string? Carrier { get; set; }

        public DateTime PeymentDate { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntenId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public int? PostalCode { get; set; }

        [Required]
        public string Name { get; set; }


        #region Relation

        [ForeignKey("ApplicatioUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }



        #endregion
    }
}
