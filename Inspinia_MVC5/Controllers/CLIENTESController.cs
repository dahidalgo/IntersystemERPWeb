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
    public class CLIENTESController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /CLIENTES/
        public ActionResult Index()
        {
            var fechamod = db.CLIENTE.OrderByDescending(c => c.CODIGO_CLTE).Select(c => c.FECHA_CREACION).FirstOrDefault();
            ViewBag.fechamod = String.Format("{0:dd/MM/yyyy h:mm tt}", fechamod);
            ViewBag.count = db.CLIENTE.Count();
            var cliente = db.CLIENTE.Include(c => c.USUARIO);
            return View(cliente.ToList());
        }

        // GET: /CLIENTES/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // GET: /CLIENTES/Create
        public ActionResult Create()
        {
            var clienteID = db.CLIENTE.OrderByDescending(c => c.CODIGO_CLTE).Select(c => c.CODIGO_CLTE).FirstOrDefault().Value;
            ViewBag.ClienteID = clienteID + 1;
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");

            return View();
        }

        // POST: /CLIENTES/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CLIENTE_ID,USUARIO_ID,CODIGO_CLTE,NOMBRE_CLTE,DIRECCION,TELEFONO,CONTACTO,E_MAIL,BALANCE,CLTE_ACTIVO,FECHA_CREACION,MONTO_ULT_TRAN,FECHA_ULT_TRAN,DESC_ULT_TRAN, NIT")] CLIENTE cLIENTE)
        {
            var clienteID = db.CLIENTE.OrderByDescending(c => c.CODIGO_CLTE).Select(c => c.CODIGO_CLTE).FirstOrDefault().Value;
            if (ModelState.IsValid)
            {
                //cLIENTE.CLIENTE_ID = clienteID + 1;
                cLIENTE.USUARIO_ID = 1;
                cLIENTE.FECHA_CREACION = DateTime.Now;
                cLIENTE.CLTE_ACTIVO = true;

                db.CLIENTE.Add(cLIENTE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", cLIENTE.USUARIO_ID);
            return View(cLIENTE);
        }

        // GET: /CLIENTES/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", cLIENTE.USUARIO_ID);
            return View(cLIENTE);
        }

        // POST: /CLIENTES/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CLIENTE_ID,USUARIO_ID,CODIGO_CLTE,NOMBRE_CLTE,DIRECCION,TELEFONO,CONTACTO,E_MAIL,BALANCE,CLTE_ACTIVO,FECHA_CREACION,MONTO_ULT_TRAN,FECHA_ULT_TRAN,DESC_ULT_TRAN, NIT")] CLIENTE cLIENTE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLIENTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", cLIENTE.USUARIO_ID);
            return View(cLIENTE);
        }

        // GET: /CLIENTES/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // POST: /CLIENTES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            cLIENTE.CLTE_ACTIVO = false;
            db.Entry(cLIENTE).State = EntityState.Modified;
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
