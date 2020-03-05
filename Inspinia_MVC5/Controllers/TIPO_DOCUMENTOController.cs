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
    public class TIPO_DOCUMENTOController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /TIPO_DOCUMENTO/
        public ActionResult Index()
        {
            return View(db.TIPO_DOCUMENTO.ToList());
        }

        // GET: /TIPO_DOCUMENTO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_DOCUMENTO tIPO_DOCUMENTO = db.TIPO_DOCUMENTO.Find(id);
            if (tIPO_DOCUMENTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_DOCUMENTO);
        }

        // GET: /TIPO_DOCUMENTO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TIPO_DOCUMENTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TIPO_DOC_ID,DESCRIPCION")] TIPO_DOCUMENTO tIPO_DOCUMENTO)
        {
            if (ModelState.IsValid)
            {
                db.TIPO_DOCUMENTO.Add(tIPO_DOCUMENTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tIPO_DOCUMENTO);
        }

        // GET: /TIPO_DOCUMENTO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_DOCUMENTO tIPO_DOCUMENTO = db.TIPO_DOCUMENTO.Find(id);
            if (tIPO_DOCUMENTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_DOCUMENTO);
        }

        // POST: /TIPO_DOCUMENTO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TIPO_DOC_ID,DESCRIPCION")] TIPO_DOCUMENTO tIPO_DOCUMENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIPO_DOCUMENTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tIPO_DOCUMENTO);
        }

        // GET: /TIPO_DOCUMENTO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_DOCUMENTO tIPO_DOCUMENTO = db.TIPO_DOCUMENTO.Find(id);
            if (tIPO_DOCUMENTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_DOCUMENTO);
        }

        // POST: /TIPO_DOCUMENTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TIPO_DOCUMENTO tIPO_DOCUMENTO = db.TIPO_DOCUMENTO.Find(id);
            db.TIPO_DOCUMENTO.Remove(tIPO_DOCUMENTO);
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
