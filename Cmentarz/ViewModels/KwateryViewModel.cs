using Cmentarz.Models;
using Cmentarz.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cmentarz.ViewModels
{
    public class KwateryViewModel
    {
        public IEnumerable<Osoba> Osoba { get; set; }
        public int kwateraId { get; set; }
        [DisplayName("Nr kwatery")]
        public int pozycja { get; set; }


        [DisplayName("Głębiniowy")]
        public bool glebiniowy { get; set; }


        [DisplayName("Zdjęcie")]
        public string zdjecie { get; set; }

        [CorrectProlongata(ErrorMessage = "Podaj datę albo wpisz brak")]
        [DisplayName("Prolongata do")]
        public string prolongata { get; set; }
        [DisplayName("Uwagi")]
        public string uwagi { get; set; }
        [DisplayName("sektor")]
        public int sektorId { get; set; }
        [DisplayName("Sektor")]
        public string sektor { get; set; }
        [DisplayName("rząd")]
        public int rzadId { get; set; }
        public int liczba_rzedow { get; set; }
        [DisplayName("Rząd")]
        public string rzad { get; set; }
    }
}