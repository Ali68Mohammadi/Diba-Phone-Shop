using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.Models
{

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display order must be between 1 to 100 only !!")]
        public string DisplayOrder { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

    

        //#region relation
        //public List<Product>? Products { get; set; }
        //#endregion

    }
}
