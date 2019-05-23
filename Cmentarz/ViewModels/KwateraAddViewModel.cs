using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cmentarz.Validators;
using System.ComponentModel.DataAnnotations;

namespace Cmentarz.ViewModels
{
    public class KwateraAddViewModel
    {
        public int sektorId { get; set; }
        [Required(ErrorMessage = "Wybierz sektor")]
        public string sektor { get; set; }

        [Required(ErrorMessage = "Podaj liczbę kwater")]
        [CorrectLiczba_kwater(ErrorMessage = "Podaj liczbę kwater")]
        public string liczba_kwater { get; set; }
    }
}