using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cmentarz.Models
{
    public class Kwatera
    {
        [DisplayName("kwatera")]
        public int kwateraId { get; set; }
        [DisplayName("Nr kwatery")]
        public int pozycja { get; set; }
        [DisplayName("Pusta")]
        public bool pusta { get; set; }
        [DisplayName("Głębiniowy")]
        public bool glebiniowy { get; set; }
        [DisplayName("Zdjęcie")]
        public string zdjecie { get; set; }
        [DisplayName("Prolongata do")]
        public string prolongata { get; set; }
        [DisplayName("Uwagi")]
        public string uwagi { get; set; }
        [DisplayName("rzad")]
        public int rzadId { get; set; }
        public virtual Rzad Rzad { get; set; }

    }
}