using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dto
{
    public class UserDto
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? mobile { get; set; }
        public int roleId { get; set; }
        public DateTime dob { get; set; }
        public string? address { get; set; }
        public int countryId { get; set; }
        public int stateId { get; set; }
        public long zipcode { get; set; }
    }
}
