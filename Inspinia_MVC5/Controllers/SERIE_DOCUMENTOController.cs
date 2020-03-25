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
    public class SERIE_DOCUMENTOController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /SERIE_DOCUMENTO/
        public ActionResult Index()
        {
            var serie_documento = db.SERIE_DOCUMENTO.Include(s => s.TIPO_DOCUMENTO).Include(s => s.USUARIO);
            return View(serie_documento.ToList());
        }

        // GET: /SERIE_DOCUMENTO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SERIE_DOCUMENTO sERIE_DOCUMENTO = db.SERIE_DOCUMENTO.Find(id);
            if (sERIE_DOCUMENTO == null)
            {
                return HttpNotFound();
            }
            return View(sERIE_DOCUMENTO);
        }

        // GET: /SERIE_DOCUMENTO/Create
        public ActionResult Create()
        {
            ViewBag.TIPO_DOC_ID = new SelectList(db.TIPO_DOCUMENTO, "TIPO_DOC_ID", "DESCRIPCION");
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            return View();
        }

        // POST: /SERIE_DOCUMENTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SERIE_DOC_ID,TIPO_DOC_ID,USUARIO_ID,SERIE,RESOLUCION,DESDE,HASTA,FECHA_CREACION,FECHA_VIGENTE,FECHA_RESOLUCION")] SERIE_DOCUMENTO sERIE_DOCUMENTO)
        {
            if (ModelState.IsValid)
            {
                db.SERIE_DOCUMENTO.Add(sERIE_DOCUMENTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TIPO_DOC_ID = new SelectList(db.TIPO_DOCUMENTO, "TIPO_DOC_ID", "DESCRIPCION", sERIE_DOCUMENTO.TIPO_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", sERIE_DOCUMENTO.USUARIO_ID);
            return View(sERIE_DOCUMENTO);
        }

        // GET: /SERIE_DOCUMENTO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SERIE_DOCUMENTO sERIE_DOCUMENTO = db.SERIE_DOCUMENTO.Find(id);
            if (sERIE_DOCUMENTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.TIPO_DOC_ID = new SelectList(db.TIPO_DOCUMENTO, "TIPO_DOC_ID", "DESCRIPCION", sERIE_DOCUMENTO.TIPO_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", sERIE_DOCUMENTO.USUARIO_ID);
            return View(sERIE_DOCUMENTO);
        }

        // POST: /SERIE_DOCUMENTO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SERIE_DOC_ID,TIPO_DOC_ID,USUARIO_ID,SERIE,RESOLUCION,DESDE,HASTA,FECHA_CREACION,FECHA_VIGENTE,FECHA_RESOLUCION")] SERIE_DOCUMENTO sERIE_DOCUMENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sERIE_DOCUMENTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TIPO_DOC_ID = new SelectList(db.TIPO_DOCUMENTO, "TIPO_DOC_ID", "DESCRIPCION", sERIE_DOCUMENTO.TIPO_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", sERIE_DOCUMENTO.USUARIO_ID);
            return View(sERIE_DOCUMENTO);
        }

        // GET: /SERIE_DOCUMENTO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SERIE_DOCUMENTO sERIE_DOCUMENTO = db.SERIE_DOCUMENTO.Find(id);
            if (sERIE_DOCUMENTO == null)
            {
                return HttpNotFound();
            }
            return View(sERIE_DOCUMENTO);
        }

        // POST: /SERIE_DOCUMENTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SERIE_DOCUMENTO sERIE_DOCUMENTO = db.SERIE_DOCUMENTO.Find(id);
            db.SERIE_DOCUMENTO.Remove(sERIE_DOCUMENTO);
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
