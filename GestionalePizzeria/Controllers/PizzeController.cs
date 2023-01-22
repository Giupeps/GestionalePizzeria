using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionalePizzeria.Models;

namespace GestionalePizzeria.Controllers
{
    [Authorize]
    public class PizzeController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Pizze
        public ActionResult Index()
        {
            return View(db.Pizze.ToList());
        }

        // GET: Pizze/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizze pizze = db.Pizze.Find(id);
            if (pizze == null)
            {
                return HttpNotFound();
            }
            return View(pizze);
        }

        // GET: Pizze/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pizze/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "IdPizza,Nome,Prezzo,TempoPreparazione,Ingredienti")] Pizze pizze, HttpPostedFileBase FotoPizza)
        {
           
            if (ModelState.IsValid && FotoPizza != null)
            {
                
                pizze.Foto = FotoPizza.FileName;
                FotoPizza.SaveAs(Server.MapPath("/Content/img/" + pizze.Foto));
               
                db.Pizze.Add(pizze);
                db.SaveChanges();
                return RedirectToAction("AmministraArticoli");
            }

            return View(pizze);
        }

        // GET: Pizze/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizze pizze = db.Pizze.Find(id);
            if (pizze == null)
            {
                return HttpNotFound();
            }
            return View(pizze);
        }

        // POST: Pizze/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "IdPizza,Nome,Foto,Prezzo,TempoPreparazione,Ingredienti")] Pizze pizze, HttpPostedFileBase FotoPizza)
        {
            Pizze pizzaDB = db.Pizze.Find(pizze.IdPizza);

            if (ModelState.IsValid)
            {               
                pizzaDB.Prezzo = pizze.Prezzo;
                pizzaDB.TempoPreparazione = pizze.TempoPreparazione;
                pizzaDB.Ingredienti = pizze.Ingredienti;
                pizzaDB.Nome = pizze.Nome;

                if(FotoPizza != null)
                {
                    pizzaDB.Foto = FotoPizza.FileName;
                }
            }

            db.Entry(pizzaDB).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Pizze/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizze pizze = db.Pizze.Find(id);
            if (pizze == null)
            {
                return HttpNotFound();
            }
            return View(pizze);
        }

        // POST: Pizze/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pizze pizze = db.Pizze.Find(id);
            db.Pizze.Remove(pizze);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AmministraArticoli()
        {
            return View(db.Pizze.ToList());
        }
    }
}
