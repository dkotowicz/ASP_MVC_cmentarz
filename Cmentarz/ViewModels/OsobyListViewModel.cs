
using Cmentarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cmentarz.ViewModels
{
    public class OsobyListViewModel
    {
        public int OsobaId { get; set; }
        public string Nazwisko { get; set; }
        public string Imie { get; set; }
        public string Sektor { get; set; }
        public string Rzad { get; set; }
        public int RzadId { get; set; }
        public int SektorId { get; set; }
        public string Data_smierci { get; set; }
        public string prolongata { get; set; }
        public int Pozycja { get; set; }
        public int KwateraId { get; set; }
    }
}