using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GestionalePizzeria.Models;

namespace GestionalePizzeria.Controllers
{
    public class UtentiController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Utenti
        //public ActionResult Index()
        //{
        //    return View(db.Utenti.ToList());
        //}

        // GET: Utenti/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Utenti utenti = db.Utenti.Find(id);
        //    if (utenti == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(utenti);
        //}

        // GET: Utenti/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Utenti/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,Nome,Cognome")] Utenti utenti)
        {
            if (ModelState.IsValid == true && db.Utenti.Where(x => x.Username == utenti.Username).Count() == 0)
            {
                utenti.Ruolo = "Cliente";
                db.Utenti.Add(utenti);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return RedirectToAction("Login");
        }

        // GET: Utenti/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Utenti utenti = db.Utenti.Find(id);
        //    if (utenti == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(utenti);
        //}

        // POST: Utenti/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IdUtenti,Username,Password,Nome,Cognome,Ruolo")] Utenti utenti)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(utenti).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(utenti);
        //}

        // GET: Utenti/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Utenti utenti = db.Utenti.Find(id);
        //    if (utenti == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(utenti);
        //}

        // POST: Utenti/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Utenti utenti = db.Utenti.Find(id);
        //    db.Utenti.Remove(utenti);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult LogIn() 
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn( Utenti utenti)
        {
            if(db.Utenti.Where(x => x.Username == utenti.Username && x.Password == utenti.Password).Count() == 1)
            {
                FormsAuthentication.SetAuthCookie(utenti.Username, false);
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            return View();
        }

        [Authorize]
        public ActionResult LogOut() {

            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
