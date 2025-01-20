using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartDetail
    {
        public int id { get; set; }
        [ForeignKey("CartMaster")]
        public int cartId {  get; set; }
        public CartMaster CartMaster { get; set; }
        [ForeignKey("Product")]
        public int productId {  get; set; }
        public Product product { get; set; }
        public int quantity {  get; set; }
    }
}
