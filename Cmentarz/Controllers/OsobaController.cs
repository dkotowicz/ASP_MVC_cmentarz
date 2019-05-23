using Cmentarz.DAL;
using Cmentarz.Models;
using Cmentarz.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Cmentarz.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class OsobaController : Controller
    {
        private CmentarzContext db = new CmentarzContext();

        public ActionResult Details(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (id == null)
            {
                throw new Exception();
            }
            Osoba osoba = db.osoba.Find(id);
            if (osoba == null)
            {
                throw new Exception();
            }
            Kwatera kwatera = db.kwatera.Find(osoba.kwateraId);
            Rzad rzad = db.rzad.Find(kwatera.rzadId);
            Sektor sektor = db.sektor.Find(rzad.sektorId);
            
            IEnumerable<Osoba> osoby = db.osoba;
            osoby = osoby.Where(o => (o.kwateraId == kwatera.kwateraId)).ToList();

            var osobaViewModel = new DetailOsobyViewModel()
            {
                osobaId = osoba.osobaId,
                imie = osoba.imie,
                nazwisko = osoba.nazwisko,
                data_smierci = osoba.data_smierci,
                kwateraId = kwatera.kwateraId,
                pozycja = kwatera.pozycja,
                glebiniowy = kwatera.glebiniowy,
                zdjecie = kwatera.zdjecie,
                prolongata = kwatera.prolongata,
                uwagi = kwatera.uwagi,
                sektorId = sektor.sektorId,
                sektor = sektor.sektor,
                rzadId = rzad.rzadId,
                rzad = rzad.rzad,
                Osoba = osoby
            };
			if(Request.IsAjaxRequest())
				return PartialView(osobaViewModel);

	        return View(osobaViewModel);
        }
        public ActionResult DetailsAdmin(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (id == null)
            {
                throw new Exception();
            }
            Osoba osoba = db.osoba.Find(id);
            if (osoba == null)
            {
                throw new Exception();
            }
            Kwatera kwatera = db.kwatera.Find(osoba.kwateraId);
            Rzad rzad = db.rzad.Find(kwatera.rzadId);
            Sektor sektor = db.sektor.Find(rzad.sektorId);

            IEnumerable<Osoba> osoby = db.osoba;
            osoby = osoby.Where(o => (o.kwateraId == kwatera.kwateraId)).ToList();

            var osobaViewModel = new DetailOsobyViewModel()
            {
                osobaId = osoba.osobaId,
                imie = osoba.imie,
                nazwisko = osoba.nazwisko,
                data_smierci = osoba.data_smierci,
                kwateraId = kwatera.kwateraId,
                pozycja = kwatera.pozycja,
                glebiniowy = kwatera.glebiniowy,
                zdjecie = kwatera.zdjecie,
                prolongata = kwatera.prolongata,
                uwagi = kwatera.uwagi,
                sektorId = sektor.sektorId,
                sektor = sektor.sektor,
                rzadId = rzad.rzadId,
                rzad = rzad.rzad,
                Osoba = osoby
            };
            if (Request.IsAjaxRequest())
                return PartialView(osobaViewModel);

            return View(osobaViewModel);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (id == null)
            {
                throw new Exception();
            }
            Osoba osoba = db.osoba.Find(id);
            if (osoba == null)
            {
                throw new Exception();
            }
            Kwatera kwatera = db.kwatera.Find(osoba.kwateraId);
            Rzad rzad = db.rzad.Find(kwatera.rzadId);
            Sektor sektor = db.sektor.Find(rzad.sektorId);
            var osobaViewModel = new DetailOsobyViewModel()
            {
                osobaId = osoba.osobaId,
                imie = osoba.imie,
                nazwisko = osoba.nazwisko,
                data_smierci = osoba.data_smierci,
                kwateraId = kwatera.kwateraId,
                pozycja = kwatera.pozycja,
                glebiniowy = kwatera.glebiniowy,
                zdjecie = kwatera.zdjecie,
                prolongata = kwatera.prolongata,
                uwagi = kwatera.uwagi,
                sektorId = sektor.sektorId,
                sektor = sektor.sektor,
                rzadId = rzad.rzadId,
                rzad = rzad.rzad

            };
            return View(osobaViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirmed([Bind(Include = "osobaId,nazwisko,imie,data_smierci,uwagi,kwateraId,rzadId,sektorId")] DetailOsobyViewModel osoba)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (ModelState.IsValid)
            {
                Osoba osoba_new = db.osoba.Find(osoba.osobaId);
                osoba_new.imie = UppercaseFirst(osoba.imie);
                osoba_new.nazwisko = UppercaseFirst(osoba.nazwisko);
                osoba_new.data_smierci = UppercaseFirst(osoba.data_smierci);
                osoba_new.kwateraId = osoba_new.kwateraId;
                Kwatera kwatera_new = db.kwatera.Find(osoba_new.kwateraId);
                kwatera_new.uwagi = osoba.uwagi;
                kwatera_new.glebiniowy = kwatera_new.glebiniowy;
                kwatera_new.pozycja = kwatera_new.pozycja;
                kwatera_new.prolongata = kwatera_new.prolongata;
                kwatera_new.rzadId = kwatera_new.rzadId;
                kwatera_new.pusta = false;
           
                db.Entry(osoba_new).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DetailsAdmin", "Osoba", new { @id = osoba_new.osobaId});
            }
            Kwatera kwatera = db.kwatera.Find(osoba.kwateraId);
            osoba.pozycja = kwatera.pozycja;
            osoba.prolongata = kwatera.prolongata;
            osoba.zdjecie = kwatera.zdjecie;
            osoba.glebiniowy = kwatera.glebiniowy;
            osoba.sektor = db.sektor.Find(osoba.sektorId).sektor;
            osoba.rzad = db.rzad.Find(osoba.rzadId).rzad;
            return View(osoba);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            Osoba osoba = db.osoba.Find(id);
            db.osoba.Remove(osoba);
            db.SaveChanges();
            List<Osoba> osoby_w_kwaterze = db.osoba.Where(o => o.kwateraId == osoba.kwateraId).ToList();
            if(osoby_w_kwaterze.Count==0)
            {
                Kwatera kwatera = db.kwatera.Find(osoba.kwateraId);
                kwatera.pusta = true;
                db.Entry(kwatera).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Kwatera", new { @id =osoba.kwateraId});
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (id == null)
            {
                throw new Exception();
            }
            Kwatera kwatera = db.kwatera.Find(id);
            if (kwatera == null)
            {
                throw new Exception();
            }
            Rzad rzad = db.rzad.Find(kwatera.rzadId);
            Sektor sektor = db.sektor.Find(rzad.sektorId);
            var osobaViewModel = new DetailOsobyViewModel()
            {
                kwateraId = kwatera.kwateraId,
                pozycja = kwatera.pozycja,
                glebiniowy = kwatera.glebiniowy,
                zdjecie = kwatera.zdjecie,
                prolongata = kwatera.prolongata,
                uwagi = kwatera.uwagi,
                sektorId = sektor.sektorId,
                sektor = sektor.sektor,
                rzadId = rzad.rzadId,
                rzad = rzad.rzad
            };
            return View(osobaViewModel);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "kwateraId,nazwisko,data_smierci,imie,sektorId,rzadId,uwagi")] DetailOsobyViewModel osoba)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (ModelState.IsValid)
            {
                Osoba osoba_new = new Osoba();
                osoba_new.imie = UppercaseFirst(osoba.imie);
                osoba_new.nazwisko = UppercaseFirst(osoba.nazwisko);
                osoba_new.data_smierci = UppercaseFirst(osoba.data_smierci);
                osoba_new.kwateraId = osoba.kwateraId;
                Kwatera kwatera_new = db.kwatera.Find(osoba.kwateraId);
                kwatera_new.pusta = false;
                kwatera_new.uwagi = osoba.uwagi;
                db.Entry(kwatera_new).State = EntityState.Modified;
                db.osoba.Add(osoba_new);
                db.SaveChanges();
                return RedirectToAction("Details", "Kwatera", new { @id = osoba.kwateraId});
            }
            Kwatera kwatera = db.kwatera.Find(osoba.kwateraId);
            osoba.pozycja = kwatera.pozycja;
            osoba.prolongata = kwatera.prolongata;
            osoba.zdjecie = kwatera.zdjecie;
            osoba.glebiniowy = kwatera.glebiniowy;
            osoba.sektor = db.sektor.Find(osoba.sektorId).sektor;
            osoba.rzad = db.rzad.Find(osoba.rzadId).rzad;
            return View(osoba);
        }
        static string UppercaseFirst(string s)
        {
            if(s!=null)
            {
                s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
            }
            return s;
        }
    }
}