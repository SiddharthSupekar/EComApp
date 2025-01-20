using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dto
{
    public class ProductDto
    {
        public string productName { get; set; }
        public string productCode { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public float sellingPrice { get; set; }
        public float purchasePrice { get; set; }
        public DateTime purchaseDate { get; set; }
        public int stock { get; set; }
        
    }
}
