using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using GestionalePizzeria.Models;

namespace GestionalePizzeria.Controllers
{
    [Authorize]
    public class OrdiniController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Ordini
        public ActionResult Index()
        {
            var ordini = db.Ordini.Include(o => o.Pizze).Include(o => o.Utenti);
            return View(ordini.ToList());
        }

        // GET: Ordini/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // GET: Ordini/Create
        public ActionResult Create()
        {
            ViewBag.IdPizza = new SelectList(db.Pizze, "IdPizza", "Nome");
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtenti", "Username");
            return View();
        }

        // POST: Ordini/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOrdine,Quantità,IndirizzoSpedizione,Nota,OrdineConfermato,OrdineConsegnato,IdPizza,IdUtente")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Ordini.Add(ordini);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPizza = new SelectList(db.Pizze, "IdPizza", "Nome", ordini.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtenti", "Username", ordini.IdUtente);
            return View(ordini);
        }

        // GET: Ordini/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPizza = new SelectList(db.Pizze, "IdPizza", "Nome", ordini.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtenti", "Username", ordini.IdUtente);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "IdOrdine,Quantità,IndirizzoSpedizione,Nota,OrdineConfermato,OrdineConsegnato,IdPizza,IdUtente")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                ordini.OrdineConfermato = true;
                db.Entry(ordini).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AmministraOrdini");
            }
            ViewBag.IdPizza = new SelectList(db.Pizze, "IdPizza", "Nome", ordini.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtenti", "Username", ordini.IdUtente);
            return View(ordini);
        }

        // GET: Ordini/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // POST: Ordini/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
            db.SaveChanges();
            return RedirectToAction("Carrello");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult PWCreateOrderByClient()
        {
            return PartialView("_PWCreateOrderByClient");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PWCreateOrderByClient(Ordini ordine, int id)
        {
            if (ModelState.IsValid)
            {
                Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).First();
                Pizze pizza = db.Pizze.Find(id);
                ordine.IndirizzoSpedizione = " ";
                ordine.IdPizza = pizza.IdPizza;
                ordine.IdUtente = utente.IdUtenti;
                db.Ordini.Add(ordine);
                db.SaveChanges();              
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult Carrello()
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).First();
            return View(db.Ordini.Where(m => m.IdUtente == utente.IdUtenti).ToList().OrderByDescending( m => m.IdOrdine)); ;
        }

        public ActionResult PWSetIndirizzo()
        {
            return PartialView("_PWSetIndirizzo");
        }

        [HttpPost]
        public ActionResult PWSetIndirizzo(Ordini ordine)
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).First();
            List<Ordini> OrdiniNonConfermati = db.Ordini.Where(u => u.IdUtente == utente.IdUtenti && u.OrdineConfermato == false).ToList();

            if(ordine.IndirizzoSpedizione == null)
            {
                ViewBag.ErroreIndirizzo = "L'indirizzo è OBBLIGATORIO";
                return RedirectToAction("Carrello");
            }
            else
            {
                foreach (var item in OrdiniNonConfermati)
                {
                    item.DataOrdine = DateTime.Now;
                    item.IndirizzoSpedizione = ordine.IndirizzoSpedizione;
                    item.OrdineConfermato = true;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Carrello");

        }
        [Authorize(Roles ="Admin")]
        public ActionResult AmministraOrdini()
        {

            return View(db.Ordini.Where(u => u.OrdineConfermato == true).ToList());
        }

        public ActionResult ModificaOrdine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPizza = new SelectList(db.Pizze, "IdPizza", "Nome", ordini.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtenti", "Username", ordini.IdUtente);
            return View(ordini);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaOrdine([Bind(Include = "IdOrdine,Quantità,Nota,IdPizza,IdUtente")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                Ordini ordiniDB = db.Ordini.Find(ordini.IdOrdine);
                ordiniDB.Quantità = ordini.Quantità;
                ordiniDB.Nota = ordini.Nota;
                ordiniDB.IdPizza = ordini.IdPizza;
                db.Entry(ordiniDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Carrello");
            }
            ViewBag.IdPizza = new SelectList(db.Pizze, "IdPizza", "Nome", ordini.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtenti", "Username", ordini.IdUtente);
            return View(ordini);
        }

        public ActionResult Riepilogo()
        {
            return View();
        }

        public JsonResult TotOrdiniCons()
        {
            int totOrdini = db.Ordini.Where(x => x.OrdineConsegnato == true).Count();
            return Json(totOrdini, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TotaleIncasso(DateTime data)
        {
            List<Ordini> TotaleOrdini = db.Ordini.Where(x => x.DataOrdine == data && x.OrdineConsegnato == true).ToList();
            decimal totale = 0;
            foreach(Ordini item in TotaleOrdini)
            {
                decimal CostoTotale = item.Quantità * item.Pizze.Prezzo;
                totale += CostoTotale;
            }
            return Json(totale, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminaOrdineAmm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // POST: Ordini/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaOrdineAmm(int id)
        {
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
            db.SaveChanges();
            return RedirectToAction("AmministraOrdini", "Ordini");
        }

    }
}
