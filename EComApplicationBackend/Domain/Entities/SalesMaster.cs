using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalesMaster
    {
        public int id { get; set; }
        public string invoiceId { get; set; }
        public DateTime invoiceDate { get; set; }
        public int userId {  get; set; }
        public float subTotal { get; set; }
        public string deliveryAddress { get; set; }
        public long deliveryZipcode { get; set; }
        public int deliveryState { get; set; }
        public int deliveryCountry { get; set; }
    }
}
