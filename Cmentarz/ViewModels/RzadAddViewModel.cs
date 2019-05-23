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
    public class RzadAddViewModel
    {
        public int SektorId { get; set; }
        public string Sektor { get; set; }
        public int RzadId { get; set; }
        public string Last_rzad { get; set; }
        public int Liczba_rzedow { get; set; }
        public string Rzad_new { get; set; }
        public int Liczba_kwater { get; set; }
    }
}