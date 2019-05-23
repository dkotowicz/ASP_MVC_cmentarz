using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cmentarz.Models
{
    public class Rzad
    {
        [DisplayName("rząd")]
        public int rzadId { get; set; }
        [DisplayName("Rząd")]
        public string rzad { get; set;}
        [DisplayName("sektor")]
        public int sektorId { get; set; }
        public virtual Sektor Sektor { get; set; }
        public ICollection<Kwatera> Kwatery { get; set; }

    }
}