using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string productName {  get; set; }
        public string productCode { get; set; }
        public string category {  get; set; }
        public string brand { get; set; }
        public float sellingPrice {  get; set; }
        public float purchasePrice { get; set; }
        public DateTime purchaseDate { get; set; }
        public int stock {  get; set; }
        public string productImage { get; set; }
        public bool isActive { get; set; } = true;

    }
}
