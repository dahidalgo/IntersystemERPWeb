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
    public class NOTA_ABONOController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /NOTA_ABONO/
        public ActionResult Index()
        {
            var nota_abono = db.NOTA_ABONO.Include(n => n.CLIENTE).Include(n => n.SERIE_DOCUMENTO).Include(n => n.USUARIO);
            return View(nota_abono.ToList());
        }

        // GET: /NOTA_ABONO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTA_ABONO nOTA_ABONO = db.NOTA_ABONO.Find(id);
            if (nOTA_ABONO == null)
            {
                return HttpNotFound();
            }
            return View(nOTA_ABONO);
        }

        // GET: /NOTA_ABONO/Create
        public ActionResult Create()
        {
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE");
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE");
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            return View();
        }

        // POST: /NOTA_ABONO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="NOTA_ABONO_ID,USUARIO_ID,CLIENTE_ID,SERIE_DOC_ID,NRO_NOTA_ABONO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADA,CAUSA_ANULADA,ESTADO_DOC,PAGOS,FECHA_ACTUALIZADO,FECHA_VENCIMIENTO, DESCRIPCION")] NOTA_ABONO nOTA_ABONO)
        {
            if (ModelState.IsValid)
            {
                db.NOTA_ABONO.Add(nOTA_ABONO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", nOTA_ABONO.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", nOTA_ABONO.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", nOTA_ABONO.USUARIO_ID);
            return View(nOTA_ABONO);
        }

        // GET: /NOTA_ABONO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTA_ABONO nOTA_ABONO = db.NOTA_ABONO.Find(id);
            if (nOTA_ABONO == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", nOTA_ABONO.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", nOTA_ABONO.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", nOTA_ABONO.USUARIO_ID);
            return View(nOTA_ABONO);
        }

        // POST: /NOTA_ABONO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="NOTA_ABONO_ID,USUARIO_ID,CLIENTE_ID,SERIE_DOC_ID,NRO_NOTA_ABONO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADA,CAUSA_ANULADA,ESTADO_DOC,PAGOS,FECHA_ACTUALIZADO,FECHA_VENCIMIENTO, DESCRIPCION")] NOTA_ABONO nOTA_ABONO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOTA_ABONO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", nOTA_ABONO.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", nOTA_ABONO.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", nOTA_ABONO.USUARIO_ID);
            return View(nOTA_ABONO);
        }

        // GET: /NOTA_ABONO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTA_ABONO nOTA_ABONO = db.NOTA_ABONO.Find(id);
            if (nOTA_ABONO == null)
            {
                return HttpNotFound();
            }
            return View(nOTA_ABONO);
        }

        // POST: /NOTA_ABONO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTA_ABONO nOTA_ABONO = db.NOTA_ABONO.Find(id);
            db.NOTA_ABONO.Remove(nOTA_ABONO);
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
