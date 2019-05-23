using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cmentarz.ViewModels
{
    public class KwateryListViewModel
    {
        public int kwateraId { get; set; }
        public string kwatera { get; set; }
        public int pozycja { get; set; }
        public int sektorId { get; set; }
        public string sektor { get; set; }
        public int rzadId { get; set; }
        public string rzad { get; set; }

    }
}