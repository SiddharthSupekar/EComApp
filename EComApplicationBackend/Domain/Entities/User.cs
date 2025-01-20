using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entities
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        [ForeignKey("Role")]
        public int roleId { get; set; }
        public Role role { get; set; }
        public DateTime dob { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        [ForeignKey("Country")]
        public int countryId { get; set; }
        public Country country {  get; set; }
        [ForeignKey("State")]
        public int stateId { get; set; }
        public State state {  get; set; }
        public long zipcode { get; set; }
        public string profileImage { get; set; }
        public bool isActive { get; set; } = true;
    }
}
