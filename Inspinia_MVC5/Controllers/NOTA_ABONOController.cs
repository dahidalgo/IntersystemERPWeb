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
            NOTA_ABONO NotaAbono = new NOTA_ABONO();
            //Clientes
            var clienteList = new List<SelectListItem>();
            clienteList.Add(new SelectListItem() { Value = "0", Text = "-Elija Cliente-", Selected = NotaAbono.CLIENTE_ID == 0 });
            clienteList.AddRange(db.CLIENTE.Select(r => new SelectListItem()
            {
                Value = r.CLIENTE_ID + "",
                Text = r.NOMBRE_CLTE,
                Selected = NotaAbono.CLIENTE_ID == r.CLIENTE_ID
            }));

            //Serie
            var seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = "0", Text = "-Elija Serie-", Selected = NotaAbono.SERIE_DOC_ID == 0 });
            seriesList.AddRange(db.SERIE_DOCUMENTO.Where(r => r.SERIE_ACTIVO == true && r.TIPO_DOC_ID == 1003).Select(r => new SelectListItem()
            {
                Value = r.SERIE_DOC_ID + "",
                Text = r.SERIE,
                Selected = NotaAbono.SERIE_DOC_ID == r.SERIE_DOC_ID
            }));

            ViewBag.CLIENTE_ID = clienteList;
            ViewBag.SERIE_DOC_ID = seriesList;
            ViewBag.FECHA_EMISION = DateTime.Now.Date.ToShortDateString();
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
                nOTA_ABONO.USUARIO_ID = 1;
                nOTA_ABONO.ANULADA = false;
                nOTA_ABONO.ESTADO_DOC = false;
                nOTA_ABONO.SUBTOTAL = nOTA_ABONO.TOTAL;
                db.NOTA_ABONO.Add(nOTA_ABONO);
                db.SaveChanges();

                DOCS_CC docs_cc = new DOCS_CC();
                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(nOTA_ABONO.SERIE_DOC_ID);
                docs_cc.TIPO_DOC_ID = sERIE.TIPO_DOC_ID;
                docs_cc.USUARIO_ID = 1;
                docs_cc.CLIENTE_ID = nOTA_ABONO.CLIENTE_ID;
                docs_cc.NRO_DOC = nOTA_ABONO.NRO_NOTA_ABONO;
                docs_cc.FECHA_EMISION = nOTA_ABONO.FECHA_EMISION;
                docs_cc.MONTO_DOC = nOTA_ABONO.TOTAL;
                docs_cc.MONTO_PARCIAL = nOTA_ABONO.TOTAL;
                docs_cc.FECHA_HORA = DateTime.Now;
                docs_cc.TIPO = "A";
                docs_cc.FECHA_VENCIMIENTO = nOTA_ABONO.FECHA_VENCIMIENTO;
                docs_cc.NRO_PAGOS = 0;
                docs_cc.BALANCE = nOTA_ABONO.TOTAL;
                db.DOCS_CC.Add(docs_cc);
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

        [HttpPost]
        public int? GetNumeroNotaAbono(int? id)
        {
            int? nanumero = null;

            if (id.HasValue && id.Value > 0)
            {
                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(id);
                if (sERIE.SERIE_ACTIVO.HasValue && sERIE.SERIE_ACTIVO == true)
                {
                    var numerona = db.NOTA_ABONO.Where(f => f.SERIE_DOC_ID == id.Value).OrderByDescending(f => f.NRO_NOTA_ABONO).Select(f => f.NRO_NOTA_ABONO).FirstOrDefault();
                    if (numerona.HasValue)
                    {
                        nanumero = numerona.Value + 1;
                    }
                    else
                    {
                        nanumero = Convert.ToInt32(sERIE.DESDE);
                    }
                    return nanumero;
                }
                else
                {
                    return -1;
                }
            }
            return nanumero;
        }
    }
}
