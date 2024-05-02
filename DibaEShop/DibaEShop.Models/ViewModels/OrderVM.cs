using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.Models.ViewModels
{
    public class OrderVM
    {
        public List<OrderDetail> OrderDetail { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
