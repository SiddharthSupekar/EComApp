using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Otp
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int userId {  get; set; }
        public User user { get; set; }
        public string otp {  get; set; }
    }
}
