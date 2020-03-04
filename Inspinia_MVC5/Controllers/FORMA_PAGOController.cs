using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers
{
    public class FORMA_PAGOController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /FORMA_PAGO/
        public ActionResult Index()
        {
            return View(db.FORMA_PAGO.ToList());
        }

        // GET: /FORMA_PAGO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMA_PAGO fORMA_PAGO = db.FORMA_PAGO.Find(id);
            if (fORMA_PAGO == null)
            {
                return HttpNotFound();
            }
            return View(fORMA_PAGO);
        }

        // GET: /FORMA_PAGO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FORMA_PAGO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FORMA_PAGO_ID,DESCRIPCION")] FORMA_PAGO fORMA_PAGO)
        {
            if (ModelState.IsValid)
            {
                db.FORMA_PAGO.Add(fORMA_PAGO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fORMA_PAGO);
        }

        // GET: /FORMA_PAGO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMA_PAGO fORMA_PAGO = db.FORMA_PAGO.Find(id);
            if (fORMA_PAGO == null)
            {
                return HttpNotFound();
            }
            return View(fORMA_PAGO);
        }

        // POST: /FORMA_PAGO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="FORMA_PAGO_ID,DESCRIPCION")] FORMA_PAGO fORMA_PAGO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fORMA_PAGO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fORMA_PAGO);
        }

        // GET: /FORMA_PAGO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMA_PAGO fORMA_PAGO = db.FORMA_PAGO.Find(id);
            if (fORMA_PAGO == null)
            {
                return HttpNotFound();
            }
            return View(fORMA_PAGO);
        }

        // POST: /FORMA_PAGO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FORMA_PAGO fORMA_PAGO = db.FORMA_PAGO.Find(id);
            db.FORMA_PAGO.Remove(fORMA_PAGO);
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
    }
}
