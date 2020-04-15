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
    public class PRODUCTOSController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /PRODUCTOS/
        public ActionResult Index()
        {
            var producto = db.PRODUCTO.Include(p => p.TIPO_PRODUCTO).Include(p => p.USUARIO);
            return View(producto.ToList());
        }

        // GET: /PRODUCTOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCTO pRODUCTO = db.PRODUCTO.Find(id);
            if (pRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCTO);
        }

        // GET: /PRODUCTOS/Create
        public ActionResult Create()
        {
            PRODUCTO producto = new PRODUCTO();
            var tipo_prodList = new List<SelectListItem>();
            tipo_prodList.Add(new SelectListItem() { Value = "0", Text = "-Elija Tipo de Producto-", Selected = producto.TIPO_PRODUCTO_ID == 0 });
            tipo_prodList.AddRange(db.TIPO_PRODUCTO.Select(r => new SelectListItem()
            {
                Value = r.TIPO_PRODUCTO_ID + "",
                Text = r.DESCRIPCION,
                Selected = producto.TIPO_PRODUCTO_ID == r.TIPO_PRODUCTO_ID
            }));
            var codproducto = db.PRODUCTO.OrderByDescending(p => p.CODIGO_PRODUCTO).Select(p => p.CODIGO_PRODUCTO).FirstOrDefault();
            if (codproducto.HasValue)
            {
                ViewBag.CODIGO_PRODUCTO = codproducto.Value + 1;
            }
            else
            {
                ViewBag.CODIGO_PRODUCTO = 1;
            }
            ViewBag.TIPO_PRODUCTO_ID = tipo_prodList;
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            return View();
        }

        // POST: /PRODUCTOS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PRODUCTO_ID,TIPO_PRODUCTO_ID,USUARIO_ID,CODIGO_PRODUCTO,DESCRIPCION,PRECIO1,PRECIO2,PRECIO3,FECHA_CREACION,FECHA_ULTIMA_VENTA,ACTIVO,NOMBRE")] PRODUCTO pRODUCTO)
        {
            if (ModelState.IsValid)
            {
                pRODUCTO.USUARIO_ID = 1;
                pRODUCTO.FECHA_CREACION = DateTime.Now;
                pRODUCTO.ACTIVO = true;
                db.PRODUCTO.Add(pRODUCTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TIPO_PRODUCTO_ID = new SelectList(db.TIPO_PRODUCTO, "TIPO_PRODUCTO_ID", "DESCRIPCION", pRODUCTO.TIPO_PRODUCTO_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", pRODUCTO.USUARIO_ID);
            return View(pRODUCTO);
        }

        // GET: /PRODUCTOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCTO pRODUCTO = db.PRODUCTO.Find(id);
            if (pRODUCTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.TIPO_PRODUCTO_ID = new SelectList(db.TIPO_PRODUCTO, "TIPO_PRODUCTO_ID", "DESCRIPCION", pRODUCTO.TIPO_PRODUCTO_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", pRODUCTO.USUARIO_ID);
            return View(pRODUCTO);
        }

        // POST: /PRODUCTOS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PRODUCTO_ID,TIPO_PRODUCTO_ID,USUARIO_ID,CODIGO_PRODUCTO,DESCRIPCION,PRECIO1,PRECIO2,PRECIO3,FECHA_CREACION,FECHA_ULTIMA_VENTA,ACTIVO,NOMBRE")] PRODUCTO pRODUCTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRODUCTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TIPO_PRODUCTO_ID = new SelectList(db.TIPO_PRODUCTO, "TIPO_PRODUCTO_ID", "DESCRIPCION", pRODUCTO.TIPO_PRODUCTO_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", pRODUCTO.USUARIO_ID);
            return View(pRODUCTO);
        }

        // GET: /PRODUCTOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCTO pRODUCTO = db.PRODUCTO.Find(id);
            if (pRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCTO);
        }

        // POST: /PRODUCTOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PRODUCTO pRODUCTO = db.PRODUCTO.Find(id);
            db.PRODUCTO.Remove(pRODUCTO);
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
