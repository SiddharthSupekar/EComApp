using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalesDetail
    {
        public int id { get; set; }
        public string invoiceId { get; set; }
        public int productId { get; set; }
        public string productCode { get; set; }
        public int saleQuantity { get; set; }
        public float sellingPrice { get; set; }
    }
}


