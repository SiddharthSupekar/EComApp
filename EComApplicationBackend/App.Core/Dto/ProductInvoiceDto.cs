using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dto
{
    public class ProductInvoiceDto
    {
        public string productCode { get; set; }
        public int saleQuantity { get; set; }
        public float sellingPrice { get; set; }

    }
}
