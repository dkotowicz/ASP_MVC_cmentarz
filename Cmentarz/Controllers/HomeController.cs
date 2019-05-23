using Cmentarz.DAL;
using Cmentarz.Models;
using Cmentarz.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList.Mvc;
using PagedList;

namespace Cmentarz.Controllers
{
    [HandleError(View = "Error")]
    public class HomeController : Controller
    {
        public static List<OsobyListViewModel> OsobyAll = new List<OsobyListViewModel>();
        private CmentarzContext db = new CmentarzContext();
        public ActionResult Index()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            ViewBag.sektor = db.sektor;
            ViewBag.kwatera_null = 0;
            return View();
        }

        public ActionResult Contact()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            return View();
        }
        public ActionResult About()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            return View();
        }

        [HttpPost]
        public ActionResult List(string nazwisko = "", string sektor = "", string data_smierci = "", string zakres_dat1 = "", string zakres_dat2 = "", bool prolongata = false, string rzad = "")
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            IEnumerable<Osoba> osoby = db.osoba;
            if(nazwisko=="" && sektor=="" && data_smierci=="" && zakres_dat1=="" && zakres_dat2=="")
            {
                ViewBag.count_osoby = 0;
                ViewBag.NotFind = true;
                return PartialView("_OsobyList");
            }
            if (ModelState.IsValid)
            {
                IEnumerable<Kwatera> kwatery = db.kwatera;
                IEnumerable<Sektor> sektory = db.sektor;
                IEnumerable<Rzad> rzedy = db.rzad;
                var ListaOsob = new List<OsobyListViewModel>();
                List<Osoba> ListaOsob_toOsoby = new List<Osoba>();
                List<int> list_kwatera = new List<int>();
                if (sektor != "" && rzad != "")
                {
                    var sektorId = int.Parse(sektor);
                    var rzadFind = db.rzad.Where(r => r.sektorId == sektorId).Where(r => r.rzad == rzad).FirstOrDefault();
	                if (rzadFind != null)
	                {
		                List<Kwatera> kwatery_find = db.kwatera.Where(k => k.rzadId == rzadFind.rzadId).ToList();
		                foreach (var osoba in osoby)
		                {
			                foreach (var item in kwatery_find)
			                {
				                if (osoba.kwateraId == item.kwateraId)
				                {
					                var o = new Osoba()
					                {
						                nazwisko = osoba.nazwisko,
						                imie = osoba.imie,
						                data_smierci = osoba.data_smierci,
						                osobaId = osoba.osobaId,
						                kwateraId = osoba.kwateraId
					                };
					                ListaOsob_toOsoby.Add(o);
				                }
			                }
		                }
	                }
                    osoby = ListaOsob_toOsoby;
                }
                else if (sektor != "")
                {

                    var sektorId = int.Parse(sektor);
                    List<Rzad> rzadFind = db.rzad.Where(r => r.sektorId == sektorId).ToList();
                    foreach (Kwatera item in kwatery)
                    {
                        foreach (Rzad item1 in rzadFind)
                        {
                            if (item.rzadId == item1.rzadId)
                            {
                                list_kwatera.Add(item.kwateraId);
                            }
                        }
                    }
                    foreach (var osoba in osoby)
                    {
                        foreach (var item in list_kwatera)
                        {
                            if (osoba.kwateraId == item)
                            {
                                var o = new Osoba()
                                {
                                    nazwisko = osoba.nazwisko,
                                    imie = osoba.imie,
                                    data_smierci = osoba.data_smierci,
                                    osobaId = osoba.osobaId,
                                    kwateraId = osoba.kwateraId
                                };
                                ListaOsob_toOsoby.Add(o);
                            }
                        }
                    }
                    osoby = ListaOsob_toOsoby;
                }
                if (nazwisko != "")
                {
                    osoby = osoby.Where(o => o.nazwisko != null);
                    osoby = osoby.Where(o => (o.nazwisko.ToLower().StartsWith(nazwisko.ToLower())));
                }

                if (data_smierci != "")
                {
                    osoby = osoby.Where(o => o.data_smierci != null);
                    osoby = osoby.Where(o => (o.data_smierci.ToLower().Contains(data_smierci.ToLower())));
                }

                if (zakres_dat1 != "")
                {
                    int data = Int32.Parse(zakres_dat1);
                    osoby = osoby.Where(o => o.data_smierci != null);
                    osoby = Osoba_data_zakres1(osoby, data);
                }

                if (zakres_dat2 != "")
                {
                    int data = Int32.Parse(zakres_dat2);
                    osoby = osoby.Where(o => o.data_smierci != null);
                    osoby = Osoba_data_zakres2(osoby, data);

                }
                if (prolongata == true)
                {
                    int data = DateTime.Now.Year;
                    IEnumerable<Kwatera> kwatery_new = db.kwatera.Where(k => k.pusta == false);
                    osoby = Osoba_prolongata(osoby, data, kwatery_new);
                }

                foreach (var osoba in osoby)
                {
                    var o = new OsobyListViewModel()
                    {
                        OsobaId = osoba.osobaId,
                        Nazwisko = osoba.nazwisko,
                        Imie = osoba.imie,
                        Data_smierci = osoba.data_smierci,
                        KwateraId = osoba.kwateraId,
                    };
                    ListaOsob.Add(o);
                }

                foreach (var osoba in ListaOsob)
                {
                    osoba.Pozycja = kwatery.Where(k => k.kwateraId == osoba.KwateraId).First().pozycja;
                    osoba.Rzad = kwatery.Where(k => k.kwateraId == osoba.KwateraId).First().Rzad.rzad;
                    osoba.RzadId = kwatery.Where(k => k.kwateraId == osoba.KwateraId).First().rzadId;
                    osoba.SektorId = rzedy.Where(k => k.rzadId == osoba.RzadId).First().sektorId;
                    osoba.Sektor = sektory.Where(s => s.sektorId == osoba.SektorId).First().sektor;
                }
                if (Request.IsAjaxRequest())
                {
                    if (ListaOsob.Count == 0)
                    {
                        OsobyAll = ListaOsob;
                        ViewBag.count_osoby = 0;
                        return PartialView("_OsobyList", ListaOsob.OrderBy(x => x.Sektor).ThenBy(k => k.Rzad).ThenBy(s => s.Pozycja).Take(10));
                    }
                    if (ListaOsob.Count() > 10)
                    {
                        OsobyAll = ListaOsob;
                        ViewBag.count_osoby = 10;
                        return PartialView("_OsobyList", ListaOsob.OrderBy(x => x.Sektor).ThenBy(k => k.Rzad).ThenBy(s => s.Pozycja).Take(10));
                    }
                    else
                    {
                        OsobyAll = ListaOsob;
                        return PartialView("_OsobyList", ListaOsob.OrderBy(x => x.Sektor).ThenBy(k => k.Rzad).ThenBy(s => s.Pozycja).Take(10));
                    }
                }
                if (osoby.Count() > 10)
                {
                    OsobyAll = ListaOsob;
                    ViewBag.count_osoby = 10;
                    return View(ListaOsob.OrderBy(x => x.Sektor).ThenBy(k => k.Rzad).ThenBy(s => s.Pozycja).Take(10));
                }
                else
                {
                    return View(ListaOsob.OrderBy(x => x.Sektor).ThenBy(k => k.Rzad).ThenBy(s => s.Pozycja));
                }
            }
            return View(osoby);
        }

        public ActionResult GetRzadList(string stateID)
        {
            List<Rzad> lstcity = new List<Rzad>();
            int stateiD = Convert.ToInt32(stateID);
            IEnumerable<Rzad> rzedy = db.rzad.Where(r => r.sektorId == stateiD).ToList();
            lstcity = rzedy.GroupBy(x => x.rzad).Select(y => y.First()).ToList();
            string result = JsonConvert.SerializeObject(lstcity, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public static IEnumerable<Osoba> Osoba_data_zakres1(IEnumerable<Osoba> osoba, int data_zakres1)
        {
            var osoby = osoba.ToList();
            foreach (Osoba item in osoba)
            {
                int osobaId = item.osobaId;
                string data = item.data_smierci.Substring(item.data_smierci.Length - 4);
                int data_int = Int32.Parse(data);
                if (data_int < data_zakres1)
                {
                    osoby.RemoveAll(o => o.osobaId == osobaId);
                }
            }
            return osoby;
        }
        public static IEnumerable<Osoba> Osoba_data_zakres2(IEnumerable<Osoba> osoba, int data_zakres2)
        {
            var osoby = osoba.ToList();
            foreach (Osoba item in osoba)
            {
                int osobaId = item.osobaId;
                string data = item.data_smierci.Substring(item.data_smierci.Length - 4);
                int data_int = Int32.Parse(data);
                if (data_zakres2 < data_int)
                {
                    osoby.RemoveAll(o => o.osobaId == osobaId);
                }
            }
            return osoby;
        }
        public ActionResult ListAll(int? page)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            var ile = OsobyAll.Count();
            var pageNumber = page ?? 1;

            List<OsobyListViewModel> OsobAll = OsobyAll.OrderBy(x => x.Sektor).OrderBy(k => k.Rzad).OrderBy(s => s.Pozycja).ToList();
            PagedList<OsobyListViewModel> osobyAll = new PagedList<OsobyListViewModel>(OsobyAll.OrderBy(x => x.Sektor).OrderBy(k => k.Rzad).OrderBy(s => s.Pozycja), pageNumber, 25);
            return View(osobyAll);
        }
        public ActionResult ListAllSort(string stateID, int? page)
        {
            int id = Convert.ToInt32(stateID);
            var pageNumber = page ?? 1;

            if (id == 1)
            {
                PagedList<OsobyListViewModel> osobyAll = new PagedList<OsobyListViewModel>(OsobyAll.OrderBy(x => x.Sektor).ThenBy(k => k.Rzad).ThenBy(s => s.Pozycja), pageNumber, 25);
                return PartialView("_ListAll", osobyAll );
               
            }
            else 
            {
                PagedList<OsobyListViewModel> osobyAll = new PagedList<OsobyListViewModel>(OsobyAll.OrderBy(x => x.Nazwisko).ThenBy(i => i.Imie), pageNumber, 25);
                return PartialView("_ListAll", osobyAll);
            }
        }
        public static IEnumerable<Osoba> Osoba_prolongata(IEnumerable<Osoba> osoba, int prolongata, IEnumerable<Kwatera> kwatery)
        {
            var osoby = osoba.ToList();

            foreach (Kwatera item in kwatery)
            {
                if (item.prolongata != "")
                {
                    if (item.prolongata != null)
                    {
                        if (Int32.Parse(item.prolongata) >= prolongata)
                        {
                            osoby.RemoveAll(o => o.kwateraId == item.kwateraId);
                        }
                    }
                }
            }
            return osoby;
        }
    }
}
