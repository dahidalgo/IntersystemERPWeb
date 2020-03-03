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
    public class TIPO_PRODUCTOController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /TIPO_PRODUCTO/
        public ActionResult Index()
        {
            return View(db.TIPO_PRODUCTO.ToList());
        }

        // GET: /TIPO_PRODUCTO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_PRODUCTO tIPO_PRODUCTO = db.TIPO_PRODUCTO.Find(id);
            if (tIPO_PRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_PRODUCTO);
        }

        // GET: /TIPO_PRODUCTO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TIPO_PRODUCTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TIPO_PRODUCTO_ID,CODIGO_TIPO_PRODUCTO,DESCRIPCION")] TIPO_PRODUCTO tIPO_PRODUCTO)
        {
            if (ModelState.IsValid)
            {
                db.TIPO_PRODUCTO.Add(tIPO_PRODUCTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tIPO_PRODUCTO);
        }

        // GET: /TIPO_PRODUCTO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_PRODUCTO tIPO_PRODUCTO = db.TIPO_PRODUCTO.Find(id);
            if (tIPO_PRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_PRODUCTO);
        }

        // POST: /TIPO_PRODUCTO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TIPO_PRODUCTO_ID,CODIGO_TIPO_PRODUCTO,DESCRIPCION")] TIPO_PRODUCTO tIPO_PRODUCTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIPO_PRODUCTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tIPO_PRODUCTO);
        }

        // GET: /TIPO_PRODUCTO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_PRODUCTO tIPO_PRODUCTO = db.TIPO_PRODUCTO.Find(id);
            if (tIPO_PRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_PRODUCTO);
        }

        // POST: /TIPO_PRODUCTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TIPO_PRODUCTO tIPO_PRODUCTO = db.TIPO_PRODUCTO.Find(id);
            db.TIPO_PRODUCTO.Remove(tIPO_PRODUCTO);
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
