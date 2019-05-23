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
    public class DetailOsobyViewModel
    {
        public int osobaId { get; set; }
        public IEnumerable<Osoba> Osoba { get; set; }

        [CorrectImie(ErrorMessage = "Podaj imię osoby albo brak!!")]
        [DisplayName("Imie")]
        public string imie { get; set; }

        [CorrectNazwisko(ErrorMessage = "Podaj nazwisko osoby albo brak")]
        [DisplayName("Nazwisko")]
        public string nazwisko { get; set; }

        [CorrectData_smierci(ErrorMessage = "Podaj datę śmierci osoby albo brak")]
        [DisplayName("Data śmierci")]
        public string data_smierci { get; set; }

        [DisplayName("Nr kwatery")]
        public int kwateraId { get; set; }

        [DisplayName("Nr kwatery")]
        public int pozycja { get; set; }

        [DisplayName("Głębiniowy")]
        public bool glebiniowy { get; set; }

        [DisplayName("Zdjęcie")]
        public string zdjecie { get; set; }

        [DisplayName("Prolongata do")]
        public string prolongata { get; set; }
        public bool pusta { get; set; }

        [DisplayName("Uwagi")]
        public string uwagi { get; set; }

        [DisplayName("sektor")]
        public int sektorId { get; set; }

        [DisplayName("Sektor")]
        public string sektor { get; set; }

        [DisplayName("rząd")]
        public int rzadId { get; set; }

        [DisplayName("Rząd")]
        public string rzad { get; set; }

    }
}