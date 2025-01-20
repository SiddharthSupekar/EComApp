using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CardDetails
    {
        public int id {  get; set; }
        public string cardNumber {  get; set; }
        public string cvv { get; set; }
        public DateTime expiryDate {  get; set; }

    }
}
