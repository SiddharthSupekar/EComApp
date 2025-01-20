using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class State
    {
        public int id { get; set; }
        public string name { get; set; }

        [ForeignKey("Country")]
        public int countryId { get; set; }

    }
}
