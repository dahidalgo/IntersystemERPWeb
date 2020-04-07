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
    public class RECIBOSController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /RECIBOS/
        public ActionResult Index()
        {
            var recibo = db.RECIBO.Include(r => r.CLIENTE).Include(r => r.FORMA_PAGO).Include(r => r.USUARIO);
            return View(recibo.ToList());
        }

        // GET: /RECIBOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RECIBO rECIBO = db.RECIBO.Find(id);
            if (rECIBO == null)
            {
                return HttpNotFound();
            }
            return View(rECIBO);
        }

        // GET: /RECIBOS/Create
        public ActionResult Create()
        {
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE");
            ViewBag.FORMA_PAGO_ID = new SelectList(db.FORMA_PAGO, "FORMA_PAGO_ID", "DESCRIPCION");
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            return View();
        }

        // POST: /RECIBOS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RECIBO_ID,USUARIO_ID,CLIENTE_ID,FORMA_PAGO_ID,NRO_RECIBO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADO,CAUSA_ANULADO,DESCRIPCION,NRO_DOC_PAGO")] RECIBO rECIBO)
        {
            if (ModelState.IsValid)
            {
                db.RECIBO.Add(rECIBO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", rECIBO.CLIENTE_ID);
            ViewBag.FORMA_PAGO_ID = new SelectList(db.FORMA_PAGO, "FORMA_PAGO_ID", "DESCRIPCION", rECIBO.FORMA_PAGO_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", rECIBO.USUARIO_ID);
            return View(rECIBO);
        }

        // GET: /RECIBOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RECIBO rECIBO = db.RECIBO.Find(id);
            if (rECIBO == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", rECIBO.CLIENTE_ID);
            ViewBag.FORMA_PAGO_ID = new SelectList(db.FORMA_PAGO, "FORMA_PAGO_ID", "DESCRIPCION", rECIBO.FORMA_PAGO_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", rECIBO.USUARIO_ID);
            return View(rECIBO);
        }

        // POST: /RECIBOS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RECIBO_ID,USUARIO_ID,CLIENTE_ID,FORMA_PAGO_ID,NRO_RECIBO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADO,CAUSA_ANULADO,DESCRIPCION,NRO_DOC_PAGO")] RECIBO rECIBO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rECIBO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", rECIBO.CLIENTE_ID);
            ViewBag.FORMA_PAGO_ID = new SelectList(db.FORMA_PAGO, "FORMA_PAGO_ID", "DESCRIPCION", rECIBO.FORMA_PAGO_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", rECIBO.USUARIO_ID);
            return View(rECIBO);
        }

        // GET: /RECIBOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RECIBO rECIBO = db.RECIBO.Find(id);
            if (rECIBO == null)
            {
                return HttpNotFound();
            }
            return View(rECIBO);
        }

        // POST: /RECIBOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RECIBO rECIBO = db.RECIBO.Find(id);
            db.RECIBO.Remove(rECIBO);
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
