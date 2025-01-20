using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dto
{
    public class CardDto
    {
        public string cardNumber { get; set; }
        public string cvv { get; set; }
        public DateTime expiryDate { get; set; }
    }
}
