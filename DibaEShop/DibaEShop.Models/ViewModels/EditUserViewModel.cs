using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.Models.ViewModels;

public class EditUserViewModel
{
    
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }

}
