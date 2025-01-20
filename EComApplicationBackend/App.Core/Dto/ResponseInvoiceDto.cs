using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dto
{
    public class ResponseInvoiceDto
    {
        public string invoiceId {  get; set; }
        public DateTime invoiceDate { get; set; }
        public double subTotal {  get; set; }
        public string address {  get; set; }
        public long zipcode {  get; set; }
        public string state {  get; set; }
        public string country {  get; set; }
        public List<ProductInvoiceDto> productInvoices { get; set; }
    }
}
