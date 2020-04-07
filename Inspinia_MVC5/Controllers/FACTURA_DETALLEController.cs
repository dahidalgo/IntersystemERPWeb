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
    public class FACTURA_DETALLEController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /FACTURA_DETALLE/
        public ActionResult Index()
        {
            var factura_detalle = db.FACTURA_DETALLE.Include(f => f.FACTURA).Include(f => f.PRODUCTO);
            return View(factura_detalle.ToList());
        }

        // GET: /FACTURA_DETALLE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURA_DETALLE fACTURA_DETALLE = db.FACTURA_DETALLE.Find(id);
            if (fACTURA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(fACTURA_DETALLE);
        }

        // GET: /FACTURA_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.FACTURA_ID = new SelectList(db.FACTURA, "FACTURA_ID", "CAUSA_ANULADA");
            ViewBag.PRODUCTO_ID = new SelectList(db.PRODUCTO, "PRODUCTO_ID", "DESCRIPCION");
            return View();
        }

        // POST: /FACTURA_DETALLE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DETALLE_ID,FACTURA_ID,PRODUCTO_ID,CANTIDAD,PRECIO_FACTURA,ANULADA")] FACTURA_DETALLE fACTURA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.FACTURA_DETALLE.Add(fACTURA_DETALLE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FACTURA_ID = new SelectList(db.FACTURA, "FACTURA_ID", "CAUSA_ANULADA", fACTURA_DETALLE.FACTURA_ID);
            ViewBag.PRODUCTO_ID = new SelectList(db.PRODUCTO, "PRODUCTO_ID", "DESCRIPCION", fACTURA_DETALLE.PRODUCTO_ID);
            return View(fACTURA_DETALLE);
        }

        // GET: /FACTURA_DETALLE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURA_DETALLE fACTURA_DETALLE = db.FACTURA_DETALLE.Find(id);
            if (fACTURA_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.FACTURA_ID = new SelectList(db.FACTURA, "FACTURA_ID", "CAUSA_ANULADA", fACTURA_DETALLE.FACTURA_ID);
            ViewBag.PRODUCTO_ID = new SelectList(db.PRODUCTO, "PRODUCTO_ID", "DESCRIPCION", fACTURA_DETALLE.PRODUCTO_ID);
            return View(fACTURA_DETALLE);
        }

        // POST: /FACTURA_DETALLE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DETALLE_ID,FACTURA_ID,PRODUCTO_ID,CANTIDAD,PRECIO_FACTURA,ANULADA")] FACTURA_DETALLE fACTURA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fACTURA_DETALLE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FACTURA_ID = new SelectList(db.FACTURA, "FACTURA_ID", "CAUSA_ANULADA", fACTURA_DETALLE.FACTURA_ID);
            ViewBag.PRODUCTO_ID = new SelectList(db.PRODUCTO, "PRODUCTO_ID", "DESCRIPCION", fACTURA_DETALLE.PRODUCTO_ID);
            return View(fACTURA_DETALLE);
        }

        // GET: /FACTURA_DETALLE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURA_DETALLE fACTURA_DETALLE = db.FACTURA_DETALLE.Find(id);
            if (fACTURA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(fACTURA_DETALLE);
        }

        // POST: /FACTURA_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FACTURA_DETALLE fACTURA_DETALLE = db.FACTURA_DETALLE.Find(id);
            db.FACTURA_DETALLE.Remove(fACTURA_DETALLE);
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
