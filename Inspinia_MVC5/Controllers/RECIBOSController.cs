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
            var recibo = db.RECIBO.OrderByDescending(r => r.NRO_RECIBO).Include(r => r.CLIENTE).Include(r => r.FORMA_PAGO).Include(r => r.USUARIO);
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
            RECIBO Recibo = new RECIBO();
            //Clientes
            var clienteList = new List<SelectListItem>();
            clienteList.Add(new SelectListItem() { Value = "0", Text = "-Elija Cliente-", Selected = Recibo.CLIENTE_ID == 0 });
            clienteList.AddRange(db.CLIENTE.Select(r => new SelectListItem()
            {
                Value = r.CLIENTE_ID + "",
                Text = r.NOMBRE_CLTE,
                Selected = Recibo.CLIENTE_ID == r.CLIENTE_ID
            }));

            //Serie
            var seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = "0", Text = "-Elija Serie-", Selected = Recibo.SERIE_DOC_ID == 0 });
            seriesList.AddRange(db.SERIE_DOCUMENTO.Where(r => r.SERIE_ACTIVO == true && r.TIPO_DOC_ID == 2).Select(r => new SelectListItem()
            {
                Value = r.SERIE_DOC_ID + "",
                Text = r.SERIE,
                Selected = Recibo.SERIE_DOC_ID == r.SERIE_DOC_ID
            }));

            //Seleccionar que va a pagar
            var itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem() { Value = "0", Text = "-Elija que va a pagar-", Selected = true });
            itemList.Add(new SelectListItem() { Value = "1", Text = "Factura"});
            itemList.Add(new SelectListItem() { Value = "3", Text = "Nota de cargo" });
            itemList.Add(new SelectListItem() { Value = "4", Text = "Otro" });

            //Forma de pago
            var formaPagoList = new List<SelectListItem>();
            formaPagoList.Add(new SelectListItem() { Value = "0", Text = "-Elija Forma de pago-", Selected = Recibo.FORMA_PAGO_ID == 0 });
            formaPagoList.AddRange(db.FORMA_PAGO.Select(r => new SelectListItem()
            {
                Value = r.FORMA_PAGO_ID + "",
                Text = r.DESCRIPCION,
                Selected = Recibo.FORMA_PAGO_ID == r.FORMA_PAGO_ID
            }));

            //DOC_ID
            var documentosList = new List<SelectListItem>();
            documentosList.Add(new SelectListItem() { Value = "0", Text = "-Elija documento-", Selected = true });

            ViewBag.CLIENTE_ID = clienteList;
            ViewBag.SERIE_DOC_ID = seriesList;
            ViewBag.FORMA_PAGO_ID = formaPagoList;
            ViewBag.ITEMLIST = itemList;
            ViewBag.DOC_ID = documentosList;
            ViewBag.FECHA_EMISION = DateTime.Now.Date.ToShortDateString();
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            return View();
        }

        // POST: /RECIBOS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RECIBO_ID,USUARIO_ID,CLIENTE_ID,FORMA_PAGO_ID,NRO_RECIBO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADO,CAUSA_ANULADO,DESCRIPCION,NRO_DOC_PAGO,SERIE_DOC_ID")] RECIBO rECIBO)
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
        public ActionResult Edit([Bind(Include= "RECIBO_ID,USUARIO_ID,CLIENTE_ID,FORMA_PAGO_ID,NRO_RECIBO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADO,CAUSA_ANULADO,DESCRIPCION,NRO_DOC_PAGO,SERIE_DOC_ID")] RECIBO rECIBO)
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


        /*Obtener número de recibo */
        [HttpPost]
        public int? GetNumeroRecibo(int? id)
        {
            int? recnumero = null;

            if (id.HasValue && id.Value > 0)
            {
                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(id);
                if (sERIE.SERIE_ACTIVO.HasValue && sERIE.SERIE_ACTIVO == true && sERIE.TIPO_DOC_ID == 2)
                {
                    var numerorecibo = db.RECIBO.Where(f => f.SERIE_DOC_ID == id.Value).OrderByDescending(f => f.NRO_RECIBO).Select(f => f.NRO_RECIBO).FirstOrDefault();
                    if (numerorecibo.HasValue)
                    {
                        recnumero = numerorecibo.Value + 1;
                    }
                    else
                    {
                        recnumero = Convert.ToInt32(sERIE.DESDE);
                    }
                    return recnumero;
                }
                else
                {
                    return -1;
                }
            }
            return recnumero;
        }

        /*Obtener listado de facturas o notas de cargo por cliente seleccionado */
        [HttpPost]
        public JsonResult GetListaDocumentos(int? id, int? cliente)
        {
            var documentosList = new List<SelectListItem>();
            if (id.HasValue && id > 0)
            {
                documentosList.AddRange(db.DOCS_CC.Where(r => r.TIPO_DOC_ID == id && r.CLIENTE_ID == cliente && r.BALANCE > 0).Select(r => new SelectListItem()
                {
                    Value = r.DOC_ID + "",
                    Text = r.NRO_DOC.ToString(),
                    Selected = true
                }));
            }
            return Json(documentosList);
        }

        //Obtener monto del documento
        [HttpPost]
        public decimal? GetMontoDocumento(int? id)
        {
            decimal? valdocumento = null;

            if (id.HasValue && id.Value > 0)
            {
                DOCS_CC dOCS = db.DOCS_CC.Find(id);
                valdocumento = dOCS.BALANCE;
            }
            return valdocumento;
        }

        /*GuardarRecibo */
        [HttpPost]
        public ActionResult GuardarRecibo(RECIBO R)
        {
            bool status = false;
            
            //Comprueba si el ModelState es valido
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid)
            {
                using (intersystemerpEntities dc = new intersystemerpEntities())
                {
                    var reciboID = db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.RECIBO_ID).FirstOrDefault();
                    RECIBO rECIBO = new RECIBO {FECHA_EMISION = R.FECHA_EMISION, SERIE_DOC_ID = R.SERIE_DOC_ID, 
                        NRO_RECIBO = R.NRO_RECIBO, CLIENTE_ID = R.CLIENTE_ID, SUBTOTAL = R.TOTAL, TOTAL = R.TOTAL, 
                        FORMA_PAGO_ID = R.FORMA_PAGO_ID, NRO_DOC_PAGO = R.NRO_DOC_PAGO, USUARIO_ID = 1 };

                    foreach (var i in R.RECIBO_DETALLE)
                    {
                        i.RECIBO_ID = reciboID + 1;
                        rECIBO.RECIBO_DETALLE.Add(i);
                    }
                    dc.RECIBO.Add(rECIBO);
                    dc.SaveChanges();

                    foreach (var i in R.RECIBO_DETALLE)
                    {
                        if (i.TIPO_DOC_ID == 1) //Si es factura lo que cancelan
                        {
                            /*Actualizo datos de factura*/
                            #region
                            FACTURA fACTURA = db.FACTURA.FirstOrDefault(f => f.NRO_FACTURA == i.DOC_NRO);
                            fACTURA.FECHA_ACTUALIZADO = DateTime.Today;
                            fACTURA.PAGOS = fACTURA.PAGOS.HasValue ? fACTURA.PAGOS + i.MONTO : i.MONTO;
                            if(fACTURA.TOTAL - fACTURA.PAGOS == 0)
                            {
                                fACTURA.ESTADO_DOC = true;
                            }
                            else
                            {
                                fACTURA.ESTADO_DOC = false;
                            }
                            db.Entry(fACTURA).State = EntityState.Modified;
                            db.SaveChanges();
                            #endregion

                            /*Para guardar el recibo en tabla DOCS_CC */
                            #region
                            DOCS_CC dOCS = new DOCS_CC();
                            dOCS.TIPO_DOC_ID = 2;
                            dOCS.FORMA_PAGO_ID = rECIBO.FORMA_PAGO_ID;
                            dOCS.USUARIO_ID = 1;
                            dOCS.CLIENTE_ID = rECIBO.CLIENTE_ID;
                            dOCS.NRO_DOC = db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.NRO_RECIBO).FirstOrDefault();
                            dOCS.FECHA_EMISION = R.FECHA_EMISION;
                            dOCS.DESC_DOC = "Pago de factura No. " + i.DOC_NRO;
                            dOCS.MONTO_DOC = i.MONTO;
                            dOCS.MONTO_PARCIAL = i.MONTO;
                            dOCS.NRO_PAGOS = 1;
                            dOCS.BALANCE = 0;
                            dOCS.NRO_DOC_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == fACTURA.NRO_FACTURA).Select(f => f.NRO_DOC).FirstOrDefault();
                            dOCS.ID_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == fACTURA.NRO_FACTURA).Select(f => f.DOC_ID).FirstOrDefault();
                            dOCS.FECHA_HORA = DateTime.Now;
                            dOCS.ID_BASE = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == fACTURA.NRO_FACTURA).Select(f => f.DOC_ID).FirstOrDefault();
                            db.DOCS_CC.Add(dOCS);
                            db.SaveChanges();
                            #endregion

                            /*Para actualizar factura en DOCS_CC*/
                            #region
                            DOCS_CC dOC1 = db.DOCS_CC.Find(dOCS.ID_PAGADO);
                            dOC1.NRO_PAGOS = dOC1.NRO_PAGOS + 1;
                            dOC1.BALANCE = dOC1.BALANCE - i.MONTO;
                            dOC1.NRO_DOC_PAGADO = dOCS.NRO_DOC;
                            dOC1.ID_PAGADO = dOCS.DOC_ID;
                            db.Entry(dOC1).State = EntityState.Modified;
                            db.SaveChanges();
                            #endregion

                        }
                        else if (i.TIPO_DOC_ID == 3) //Si es nota de cargo lo que cancelan
                        {
                            /*Actualizo datos de nota de cargo*/
                            #region Actualizar nota de cargo
                            NOTA_CARGO nOTA_CARGO = db.NOTA_CARGO.FirstOrDefault(n => n.NRO_NOTA_CARGO == i.DOC_NRO);
                            nOTA_CARGO.FECHA_ACTUALIZADO = DateTime.Today;
                            nOTA_CARGO.PAGOS = nOTA_CARGO.PAGOS.HasValue ? nOTA_CARGO.PAGOS + i.MONTO : i.MONTO;
                            if(nOTA_CARGO.TOTAL - nOTA_CARGO.PAGOS == 0)
                            {
                                nOTA_CARGO.ESTADO_DOC = true;
                            }
                            else
                            {
                                nOTA_CARGO.ESTADO_DOC = false;
                            }
                            db.Entry(nOTA_CARGO).State = EntityState.Modified;
                            db.SaveChanges();
                            #endregion

                            /*Para guardar el recibo en tabla DOCS_CC */
                            #region guardar recibo en DOCS_CC
                            DOCS_CC dOCS = new DOCS_CC();
                            dOCS.TIPO_DOC_ID = 2;
                            dOCS.FORMA_PAGO_ID = rECIBO.FORMA_PAGO_ID;
                            dOCS.USUARIO_ID = 1;
                            dOCS.CLIENTE_ID = rECIBO.CLIENTE_ID;
                            dOCS.NRO_DOC = db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.NRO_RECIBO).FirstOrDefault();
                            dOCS.FECHA_EMISION = R.FECHA_EMISION;
                            dOCS.DESC_DOC = "Pago de Nota de cargo No. " + nOTA_CARGO.NRO_NOTA_CARGO;
                            dOCS.MONTO_DOC = i.MONTO;
                            dOCS.MONTO_PARCIAL = i.MONTO;
                            dOCS.NRO_PAGOS = 1;
                            dOCS.BALANCE = 0;
                            dOCS.NRO_DOC_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == nOTA_CARGO.NRO_NOTA_CARGO).Select(f => f.NRO_DOC).FirstOrDefault();
                            dOCS.ID_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == nOTA_CARGO.NRO_NOTA_CARGO).Select(f => f.DOC_ID).FirstOrDefault();
                            dOCS.FECHA_HORA = DateTime.Now;
                            dOCS.ID_BASE = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == nOTA_CARGO.NRO_NOTA_CARGO).Select(f => f.DOC_ID).FirstOrDefault();
                            db.DOCS_CC.Add(dOCS);
                            db.SaveChanges();
                            #endregion

                            /*Para actualizar NOTA_CARGO en DOCS_CC*/
                            #region actualizar NOTA_CARGO en DOCS_CC
                            DOCS_CC dOC1 = db.DOCS_CC.Find(dOCS.ID_PAGADO);
                            dOC1.NRO_PAGOS = dOC1.NRO_PAGOS + 1;
                            dOC1.BALANCE = dOC1.BALANCE - i.MONTO;
                            dOC1.NRO_DOC_PAGADO = dOCS.NRO_DOC;
                            dOC1.ID_PAGADO = dOCS.DOC_ID;
                            db.Entry(dOC1).State = EntityState.Modified;
                            db.SaveChanges();
                            #endregion
                        }
                    }
                    status = true;
                    return RedirectToAction("Create");
                }
            }
            else
            {
                status = false;
            }
            return RedirectToAction("Create");
            //return new JsonResult { Data = new { status = status } };
        }
    }
}
