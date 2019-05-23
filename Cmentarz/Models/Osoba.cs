using Cmentarz.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cmentarz.Models
{
    public class Osoba
    {
        [DisplayName("osoba")]
        public int osobaId { get; set; }
        [DisplayName("imie")]
        public string imie { get; set; }
        [DisplayName("Nazwisko")]
        public string nazwisko { get; set; }
        [DisplayName("Data śmierci")]
        public string data_smierci { get; set; }
        [DisplayName("Nr kwatery")]
        public int kwateraId { get; set; }
        public virtual Kwatera Kwatera { get; set; }
    }
}