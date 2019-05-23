using Cmentarz.DAL;
using Cmentarz.Models;
using Cmentarz.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cmentarz.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class RzadController : Controller
    {
        private CmentarzContext db = new CmentarzContext();
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            var rzadviewModel = new RzadAddViewModel();
            List<Sektor> sektory = db.sektor.ToList();
            List<Sektor> sektory_send = new List<Sektor>();
            int n;
            foreach (var i in sektory)
            {
                if (int.TryParse(i.sektor, out n) == true)
                {
                    sektory_send.Add(i);
                }
                else if (i.sektor == "1A")
                {
                    sektory_send.Add(i);
                }
            };
            ViewBag.sektor = new SelectList(sektory_send, "sektorId", "sektor");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult List(string sektor = null)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            IEnumerable<Rzad> rzedy = db.rzad;
            IEnumerable<Kwatera> kwatery = db.kwatera;

            if (sektor != "")
            {
                var sektorId = int.Parse(sektor);
                List<Rzad> rzedy_sektor = db.rzad.Where(r => r.sektorId == sektorId).ToList();
                var liczba_rzedow = rzedy_sektor.Count();
                if (liczba_rzedow != 0)
                {
                    var last_rzad = rzedy_sektor.Last();
                    int last_rzad_int = RomanToInteger(last_rzad.rzad);
                    string newrzad = ToRoman(last_rzad_int + 1);
                    var first_rzad = rzedy_sektor.First();
                    var liczba_kwater = db.kwatera.Where(k => k.rzadId == first_rzad.rzadId).Count();
                    var Informacja = new RzadAddViewModel()
                    {
                        SektorId = sektorId,
                        Last_rzad = last_rzad.rzad,
                        Liczba_rzedow = liczba_rzedow,
                        Liczba_kwater = liczba_kwater,
                        Rzad_new = newrzad
                    };
                    return PartialView("_SektorList", Informacja);
                }
                return PartialView();
            }
            return PartialView("_SektorList");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Add(RzadAddViewModel rzad)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (ModelState.IsValid)
            {
                var sektroId = rzad.SektorId;
                var rzad_nazwa = rzad.Rzad_new;
                var liczba_kwater = rzad.Liczba_kwater;
                Rzad rzad_new = new Rzad();
                rzad_new.rzad = rzad_nazwa;
                rzad_new.sektorId = sektroId;
                db.rzad.Add(rzad_new);
                db.SaveChanges();

                var rzad_find = db.rzad.Where(r => r.rzad == rzad_nazwa).Where(r => r.sektorId == sektroId).First();
                var rzadId = rzad_find.rzadId;

                for (int i = 1; i <= liczba_kwater; i++)
                {
                    Kwatera kwatera_new = new Kwatera();
                    kwatera_new.glebiniowy = false;
                    kwatera_new.pusta = true;
                    kwatera_new.rzadId = rzadId;
                    kwatera_new.pozycja = i;
                    db.kwatera.Add(kwatera_new);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Create", "Rzad");
        }
        static string[] roman1 = { "MMM", "MM", "M" };
        static string[] roman2 = { "CM", "DCCC", "DCC", "DC", "D", "CD", "CCC", "CC", "C" };
        static string[] roman3 = { "XC", "LXXX", "LXX", "LX", "L", "XL", "XXX", "XX", "X" };
        static string[] roman4 = { "IX", "VIII", "VII", "VI", "V", "IV", "III", "II", "I" };

        public static bool TryParse(string text, out int value)
        {
            value = 0;
            if (String.IsNullOrEmpty(text)) return false;
            text = text.ToUpper();
            int len = 0;

            for (int i = 0; i < 3; i++)
            {
                if (text.StartsWith(roman1[i]))
                {
                    value += 1000 * (3 - i);
                    len = roman1[i].Length;
                    break;
                }
            }

            if (len > 0)
            {
                text = text.Substring(len);
                len = 0;
            }

            for (int i = 0; i < 9; i++)
            {
                if (text.StartsWith(roman2[i]))
                {
                    value += 100 * (9 - i);
                    len = roman2[i].Length;
                    break;
                }
            }

            if (len > 0)
            {
                text = text.Substring(len);
                len = 0;
            }

            for (int i = 0; i < 9; i++)
            {
                if (text.StartsWith(roman3[i]))
                {
                    value += 10 * (9 - i);
                    len = roman3[i].Length;
                    break;
                }
            }

            if (len > 0)
            {
                text = text.Substring(len);
                len = 0;
            }

            for (int i = 0; i < 9; i++)
            {
                if (text.StartsWith(roman4[i]))
                {
                    value += 9 - i;
                    len = roman4[i].Length;
                    break;
                }
            }

            if (text.Length > len)
            {
                value = 0;
                return false;
            }

            return true;
        }

        public static string ToRoman(int num)
        {
            if (num > 3999) throw new ArgumentException("Too big - can't exceed 3999");
            if (num < 1) throw new ArgumentException("Too small - can't be less than 1");
            int thousands, hundreds, tens, units;
            thousands = num / 1000;
            num %= 1000;
            hundreds = num / 100;
            num %= 100;
            tens = num / 10;
            units = num % 10;
            var sb = new StringBuilder();
            if (thousands > 0) sb.Append(roman1[3 - thousands]);
            if (hundreds > 0) sb.Append(roman2[9 - hundreds]);
            if (tens > 0) sb.Append(roman3[9 - tens]);
            if (units > 0) sb.Append(roman4[9 - units]);
            return sb.ToString();
        }
        private static Dictionary<char, int> RomanMap = new Dictionary<char, int>()
        {
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D', 500},
        {'M', 1000}
        };
        public static int RomanToInteger(string roman)
        {
            int number = 0;
            char previousChar = roman[0];
            foreach (char currentChar in roman)
            {
                number += RomanMap[currentChar];
                if (RomanMap[previousChar] < RomanMap[currentChar])
                {
                    number -= RomanMap[previousChar] * 2;
                }
                previousChar = currentChar;
            }
            return number;
        }
    }


}
