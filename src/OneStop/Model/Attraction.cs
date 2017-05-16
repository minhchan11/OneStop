﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneStop.Model
{
    [Table("Attractions")]
    public class Attraction
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
