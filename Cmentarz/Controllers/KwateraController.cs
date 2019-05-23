using Cmentarz.DAL;
using Cmentarz.Models;
using Cmentarz.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Cmentarz.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class KwateraController : Controller
    {
        private string blankImage = @"/Content/blank.png";

        private CmentarzContext db = new CmentarzContext();
        public ActionResult Details(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (id == null)
            {
                throw new Exception();
            }
            Kwatera kwatera = db.kwatera.Find(id);
            Rzad rzad = db.rzad.Find(kwatera.rzadId);
            Sektor sektor = db.sektor.Find(rzad.sektorId);
            if (kwatera == null)
            {
                throw new Exception();
            }
            if (kwatera.pusta != true)
            {
                ViewBag.pusta = false;
                IEnumerable<Osoba> osoby = db.osoba;
                osoby = osoby.Where(o => (o.kwateraId == kwatera.kwateraId)).ToList();
                var kwateraViewModel = new KwateryViewModel()
                {
                    kwateraId = kwatera.kwateraId,
                    pozycja = kwatera.pozycja,
                    glebiniowy = kwatera.glebiniowy,
                    zdjecie = kwatera.zdjecie ?? blankImage,
                    prolongata = kwatera.prolongata,
                    uwagi = kwatera.uwagi,
                    sektorId = sektor.sektorId,
                    sektor = sektor.sektor,
                    rzadId = rzad.rzadId,
                    rzad = rzad.rzad,
                    Osoba = osoby
                };
                return View(kwateraViewModel);
            }
            var kwateraViewModelPusta = new KwateryViewModel()
            {
                kwateraId = kwatera.kwateraId,
                pozycja = kwatera.pozycja,
                glebiniowy = kwatera.glebiniowy,
                zdjecie = kwatera.zdjecie ?? blankImage,
                prolongata = kwatera.prolongata,
                uwagi = kwatera.uwagi,
                sektorId = sektor.sektorId,
                sektor = sektor.sektor,
                rzadId = rzad.rzadId,
                rzad = rzad.rzad
            };
            ViewBag.pusta = true;
            return View(kwateraViewModelPusta);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
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
            ViewBag.sektor = new SelectList(sektory_send, "sektorId", "sektor", "Wybierz");

            return View();
        }

        [HttpPost, ActionName("Add")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AddConfirmed([Bind(Include = "sektor,liczba_kwater")] KwateraAddViewModel kwatera)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            if (ModelState.IsValid)
            {
                var liczba_kwater = Int32.Parse(kwatera.liczba_kwater);
                var sektorId = Int32.Parse(kwatera.sektor);
                List<Rzad> rzad = db.rzad.Where(r => r.sektorId == sektorId).ToList();
                List<Kwatera> kwatery = db.kwatera.ToList();
                List<Kwatera> add_kwatery = new List<Kwatera>();

                foreach (var value in rzad)
                {
                    for (int i = 1; i <= liczba_kwater; i++)
                    {
                        List<Kwatera> kwatera_max = db.kwatera.Where(k => k.rzadId == value.rzadId).ToList();
                        if (kwatera_max.Count != 0)
                        {
                            Kwatera kwatera_new = new Kwatera();
                            kwatera_new.rzadId = value.rzadId;
                            kwatera_new.pusta = true;
                            int last_pozycja = kwatera_max.Max(k => k.pozycja);
                            kwatera_new.pozycja = last_pozycja + 1;
                            db.kwatera.Add(kwatera_new);
                            db.SaveChanges();
                        }
                        else
                        {
                            Kwatera kwatera_new = new Kwatera();
                            kwatera_new.rzadId = value.rzadId;
                            kwatera_new.pusta = true;
                            kwatera_new.pozycja = 1;
                            db.kwatera.Add(kwatera_new);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.sektor = new SelectList(db.sektor, "sektorId", "sektor", "Wybierz");
            kwatera.liczba_kwater = kwatera.liczba_kwater;
            kwatera.sektor = kwatera.sektor;
            return View(kwatera);
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
            Kwatera kwatera = db.kwatera.Find(id);
            Rzad rzad = db.rzad.Find(kwatera.rzadId);
            Sektor sektor = db.sektor.Find(rzad.sektorId);

            if (kwatera == null)
            {
                throw new Exception();
            }
            var kwateraViewModel = new KwateryViewModel()
            {
                kwateraId = kwatera.kwateraId,
                pozycja = kwatera.pozycja,
                glebiniowy = kwatera.glebiniowy,
                zdjecie = kwatera.zdjecie ?? blankImage,
                prolongata = kwatera.prolongata,
                uwagi = kwatera.uwagi,
                sektorId = sektor.sektorId,
                sektor = sektor.sektor,
                rzadId = rzad.rzadId,
                rzad = rzad.rzad
            };

            DateTime startYear = DateTime.Now;
           
            int year = 0;
            List<SelectListItem> Years = new List<SelectListItem>();

            for(int i =0; i< 31; i++)
            {
                year = startYear.Year + i;
                Years.Add(new SelectListItem() { Text = "" + year + "", Value = "" + year + "" });
            }
            Years.Add(new SelectListItem() { Text = "", Value = "" });

            if (kwateraViewModel.prolongata != null)
            {
                var prolongata = int.Parse(kwateraViewModel.prolongata);
                if (prolongata < startYear.Year)
                {
                    Years.Add(new SelectListItem() { Text = "" + prolongata + "", Value = "" + prolongata + "" });
                }
            }
           
            if(kwateraViewModel.prolongata==null)
            {
                this.ViewBag.Provinces = new SelectList(Years, "Value", "Text", "");
            }
            else
            {
                this.ViewBag.Provinces = new SelectList(Years, "Value", "Text", kwateraViewModel.prolongata);
            }
            return View(kwateraViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditConfirmed([Bind(Include = "kwateraId,prolongata,uwagi,glebiniowy,sektorId,rzadId,zdjecie")] KwateryViewModel kwatera, HttpPostedFileBase file)
        {
            var imgPath = @"/Content/Kwatery/";
            var path = "";

            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            Kwatera kwatera_new = db.kwatera.Find(kwatera.kwateraId);

            if (file != null && file.ContentLength > 0)
            {
                var fileName = file.FileName;
                path = Server.MapPath(Path.Combine("~" + imgPath, fileName));
                if (System.IO.File.Exists(path))
                {
                    var rand = new Random();
                    fileName = rand.Next(1, 100) + file.FileName;
                    path = Server.MapPath(Path.Combine("~" + imgPath, fileName));
                }

                var fs = new FileStream(path, FileMode.Create);
                var bytesInStream = new byte[file.InputStream.Length];
                file.InputStream.Read(bytesInStream, 0, bytesInStream.Length);
                fs.Write(bytesInStream, 0, bytesInStream.Length);
                fs.Close();

                path = Path.Combine(imgPath, fileName);
            }
            else if (kwatera_new.zdjecie != null)
                path = kwatera_new.zdjecie;
            else
                path = blankImage;

            if (ModelState.IsValid)
            {
                kwatera_new.glebiniowy = kwatera.glebiniowy;
                kwatera_new.prolongata = kwatera.prolongata;
                kwatera_new.uwagi = kwatera.uwagi;
                kwatera_new.zdjecie = path;
                db.Entry(kwatera_new).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Kwatera", new { @id = kwatera_new.kwateraId });
            }
            kwatera.sektor = db.sektor.Find(kwatera.sektorId).sektor;
            kwatera.rzad = db.rzad.Find(kwatera.rzadId).rzad;

            return View(kwatera);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Find()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            ViewBag.sektor = db.sektor;
            return View();
        }

        public ActionResult GetRzadList(string stateID)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
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

        [HttpPost]
        public ActionResult List(string sektor = null, string rzad = null)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.Admin = User.IsInRole("Admin");
            List<Kwatera> kwatery_find = db.kwatera.Where(k => k.pusta == true).ToList();
            List<Rzad> rzadFind = db.rzad.ToList();
            List<Kwatera> kwatery_to_send = new List<Kwatera>();
            var ListaKwater = new List<KwateryListViewModel>();
            if (rzad != "" && sektor != "")
            {
                var sektorId = int.Parse(sektor);
                rzadFind = rzadFind.Where(r => r.rzad == rzad).Where(r => r.sektorId == sektorId).ToList();
            }
            else if (sektor != "")
            {
                var sektorId = int.Parse(sektor);
                rzadFind = rzadFind.Where(r => r.sektorId == sektorId).ToList();
            }
            if (rzadFind.Count > 0)
            {
                foreach (var value in rzadFind)
                {
                    foreach (var value1 in kwatery_find)
                    {
                        if (value1.rzadId == value.rzadId)
                        {
                            kwatery_to_send.Add(value1);
                        }
                    }
                }
            }

            foreach (var kwatera in kwatery_to_send)
            {
                var o = new KwateryListViewModel()
                {
                    kwateraId = kwatera.kwateraId,
                    pozycja = kwatera.pozycja,
                    rzadId = kwatera.rzadId
                };
                ListaKwater.Add(o);
            }

            foreach (var kwatera in ListaKwater)
            {
                kwatera.rzad = db.rzad.Where(r => r.rzadId == kwatera.rzadId).First().rzad;
                kwatera.sektorId = db.rzad.Where(r => r.rzadId == kwatera.rzadId).First().sektorId;
                kwatera.sektor = db.sektor.Where(s => s.sektorId == kwatera.sektorId).First().sektor;
            }
            ViewBag.liczba = ListaKwater.Count();
          
            return PartialView("_KwateryList", ListaKwater.OrderBy(c => c.sektor).ThenBy(n => n.rzad).ThenBy(p => p.pozycja));
        }

        public ActionResult RotateLeft(string img)
        {
            var path = Server.MapPath(img);
            var fs = new FileStream(path, FileMode.Open);
            var myImage = Image.FromStream(fs);
            fs.Close();

            myImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            myImage.Save(path);
            myImage.Dispose();

            ViewBag.zdjecie = img;
            return PartialView("_KwateraImage");
        }

        public ActionResult RotateRight(string img)
        {
            var path = Server.MapPath(img);
            var fs = new FileStream(path, FileMode.Open);
            var myImage = Image.FromStream(fs);
            fs.Close();

            myImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            myImage.Save(path);
            myImage.Dispose();

            ViewBag.zdjecie = img;
            return PartialView("_KwateraImage");
        }
    }
}