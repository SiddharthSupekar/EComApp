﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartMaster
    {
        public int id { get; set; }
        [ForeignKey("User")]
        public int userId {  get; set; }
        public User user { get; set; }
    }
}
    