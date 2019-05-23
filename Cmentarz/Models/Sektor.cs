using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cmentarz.Models
{
    public class Sektor
    {
        [DisplayName("sektor")]
        public int sektorId { get; set; }
        [DisplayName("Sektor")]
        public string sektor { get; set; }
        public virtual ICollection<Rzad> Rzady { get; set; }
    }
}