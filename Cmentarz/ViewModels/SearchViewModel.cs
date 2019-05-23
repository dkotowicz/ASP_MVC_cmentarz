using Cmentarz.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cmentarz.ViewModels
{
    public class SearchViewModel
    {
        [CorrectNazwisko(ErrorMessage = "Podaj nazwisko osoby albo brak!!")]
        [DisplayName("Nazwisko")]
        public string nazwisko { get; set; }
        [DisplayName("Sektor")]
        public string sektor { get; set; }
        [DisplayName("Data śmierci")]
        [CorrectData_smierci(ErrorMessage = "Podaj datę śmierci osoby albo brak")]
        public string data_smierci { get; set; }
        [DisplayName("Zakres dat")]
        public string zakres_dat1 { get; set; }
        [DisplayName("Zakres dat")]
        public string zakres_dat2 { get; set; }
        [DisplayName("Prolongata")]
        public string prolongata { get; set; }
        public string rzad { get; set; }
        public int rzadId { get; set; }
    }
}