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
    public class NOTA_CARGOController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /NOTA_CARGO/
        public ActionResult Index()
        {
            var nota_cargo = db.NOTA_CARGO.Include(n => n.CLIENTE).Include(n => n.SERIE_DOCUMENTO).Include(n => n.USUARIO);
            return View(nota_cargo.ToList());
        }

        // GET: /NOTA_CARGO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTA_CARGO nOTA_CARGO = db.NOTA_CARGO.Find(id);
            if (nOTA_CARGO == null)
            {
                return HttpNotFound();
            }
            return View(nOTA_CARGO);
        }

        // GET: /NOTA_CARGO/Create
        public ActionResult Create()
        {

            NOTA_CARGO NotaCargo = new NOTA_CARGO();
            //Clientes
            var clienteList = new List<SelectListItem>();
            clienteList.Add(new SelectListItem() { Value = "0", Text = "-Elija Cliente-", Selected = NotaCargo.CLIENTE_ID == 0 });
            clienteList.AddRange(db.CLIENTE.Select(r => new SelectListItem()
            {
                Value = r.CLIENTE_ID + "",
                Text = r.NOMBRE_CLTE,
                Selected = NotaCargo.CLIENTE_ID == r.CLIENTE_ID
            }));

            //Serie
            var seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = "0", Text = "-Elija Serie-", Selected = NotaCargo.SERIE_DOC_ID == 0 });
            seriesList.AddRange(db.SERIE_DOCUMENTO.Where(r => r.SERIE_ACTIVO == true && r.TIPO_DOC_ID == 1002).Select(r => new SelectListItem()
            {
                Value = r.SERIE_DOC_ID + "",
                Text = r.SERIE,
                Selected = NotaCargo.SERIE_DOC_ID == r.SERIE_DOC_ID
            }));

            ViewBag.CLIENTE_ID = clienteList;
            ViewBag.SERIE_DOC_ID = seriesList;
            ViewBag.FECHA_EMISION = DateTime.Now.Date.ToShortDateString();
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            return View();
        }

        // POST: /NOTA_CARGO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="NOTA_CARGO_ID,USUARIO_ID,CLIENTE_ID,SERIE_DOC_ID,NRO_NOTA_CARGO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADA,CAUSA_ANULADA,ESTADO_DOC,PAGOS,FECHA_ACTUALIZADO,FECHA_VENCIMIENTO, DESCRIPCION")] NOTA_CARGO nOTA_CARGO)
        {
            if (ModelState.IsValid)
            {


                nOTA_CARGO.USUARIO_ID = 1;
                nOTA_CARGO.ANULADA = false;
                nOTA_CARGO.ESTADO_DOC = false;
                nOTA_CARGO.SUBTOTAL = nOTA_CARGO.TOTAL;
                db.NOTA_CARGO.Add(nOTA_CARGO);
                db.SaveChanges();

                DOCS_CC docs_cc = new DOCS_CC();
                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(nOTA_CARGO.SERIE_DOC_ID);
                docs_cc.TIPO_DOC_ID = sERIE.TIPO_DOC_ID;
                docs_cc.USUARIO_ID = 1;
                docs_cc.CLIENTE_ID = nOTA_CARGO.CLIENTE_ID;
                docs_cc.NRO_DOC = nOTA_CARGO.NRO_NOTA_CARGO;
                docs_cc.FECHA_EMISION = nOTA_CARGO.FECHA_EMISION;
                docs_cc.MONTO_DOC = nOTA_CARGO.TOTAL;
                docs_cc.MONTO_PARCIAL = nOTA_CARGO.TOTAL;
                docs_cc.FECHA_HORA = DateTime.Now;
                docs_cc.TIPO = "D";
                docs_cc.FECHA_VENCIMIENTO = nOTA_CARGO.FECHA_VENCIMIENTO;
                docs_cc.NRO_PAGOS = 0;
                docs_cc.BALANCE = nOTA_CARGO.TOTAL;
                db.DOCS_CC.Add(docs_cc);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", nOTA_CARGO.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", nOTA_CARGO.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", nOTA_CARGO.USUARIO_ID);
            return View(nOTA_CARGO);
        }

        // GET: /NOTA_CARGO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTA_CARGO nOTA_CARGO = db.NOTA_CARGO.Find(id);
            if (nOTA_CARGO == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", nOTA_CARGO.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", nOTA_CARGO.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", nOTA_CARGO.USUARIO_ID);
            return View(nOTA_CARGO);
        }

        // POST: /NOTA_CARGO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="NOTA_CARGO_ID,USUARIO_ID,CLIENTE_ID,SERIE_DOC_ID,NRO_NOTA_CARGO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADA,CAUSA_ANULADA,ESTADO_DOC,PAGOS,FECHA_ACTUALIZADO,FECHA_VENCIMIENTO")] NOTA_CARGO nOTA_CARGO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOTA_CARGO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", nOTA_CARGO.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", nOTA_CARGO.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", nOTA_CARGO.USUARIO_ID);
            return View(nOTA_CARGO);
        }

        // GET: /NOTA_CARGO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTA_CARGO nOTA_CARGO = db.NOTA_CARGO.Find(id);
            if (nOTA_CARGO == null)
            {
                return HttpNotFound();
            }
            return View(nOTA_CARGO);
        }

        // POST: /NOTA_CARGO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTA_CARGO nOTA_CARGO = db.NOTA_CARGO.Find(id);
            db.NOTA_CARGO.Remove(nOTA_CARGO);
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
        public int? GetNumeroNotaCargo(int? id)
        {
            int? ncnumero = null;

            if (id.HasValue && id.Value > 0)
            {
                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(id);
                if (sERIE.SERIE_ACTIVO.HasValue && sERIE.SERIE_ACTIVO == true)
                {
                    var numeronc = db.NOTA_CARGO.Where(f => f.SERIE_DOC_ID == id.Value).OrderByDescending(f => f.NRO_NOTA_CARGO).Select(f => f.NRO_NOTA_CARGO).FirstOrDefault();
                    if (numeronc.HasValue)
                    {
                        ncnumero = numeronc.Value + 1;
                    }
                    else
                    {
                        ncnumero = Convert.ToInt32(sERIE.DESDE);
                    }
                    return ncnumero;
                }
                else
                {
                    return -1;
                }
            }
            return ncnumero;
        }
    }
}
