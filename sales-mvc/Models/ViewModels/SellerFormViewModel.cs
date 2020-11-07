using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sales_mvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
