using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers
{
    public class RECIBOSController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();
        private int _result;
        

        // GET: /RECIBOS/
        public ActionResult Index()
        {
            var recibo = db.RECIBO.OrderByDescending(r => r.NRO_RECIBO).Include(r => r.CLIENTE)
                .Include(r => r.FORMA_PAGO).Include(r => r.USUARIO);
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
            clienteList.Add(new SelectListItem()
                {Value = "0", Text = "-Elija Cliente-", Selected = Recibo.CLIENTE_ID == 0});
            clienteList.AddRange(db.CLIENTE.Select(r => new SelectListItem()
            {
                Value = r.CLIENTE_ID + "",
                Text = r.CLIENTE_ID + " - " + r.NOMBRE_CLTE,
                Selected = Recibo.CLIENTE_ID == r.CLIENTE_ID
            }));

            //Serie
            var seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem()
                {Value = "0", Text = "-Elija Serie-", Selected = Recibo.SERIE_DOC_ID == 0});
            seriesList.AddRange(db.SERIE_DOCUMENTO.Where(r => r.SERIE_ACTIVO == true && r.TIPO_DOC_ID == 2).Select(r =>
                new SelectListItem()
                {
                    Value = r.SERIE_DOC_ID + "",
                    Text = r.SERIE,
                    Selected = Recibo.SERIE_DOC_ID == r.SERIE_DOC_ID
                }));

            //Seleccionar que va a pagar
            var itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem() {Value = "0", Text = "-Elija que va a pagar-", Selected = true});
            itemList.Add(new SelectListItem() {Value = "1", Text = "Factura"});
            itemList.Add(new SelectListItem() {Value = "3", Text = "Nota de cargo"});
            itemList.Add(new SelectListItem() {Value = "4", Text = "Otro"});
            itemList.Add(new SelectListItem() {Value = "5", Text = "Adelanto"});

            //Forma de pago
            var formaPagoList = new List<SelectListItem>();
            formaPagoList.Add(new SelectListItem()
                {Value = "0", Text = "-Elija Forma de pago-", Selected = Recibo.FORMA_PAGO_ID == 0});
            formaPagoList.AddRange(db.FORMA_PAGO.Select(r => new SelectListItem()
            {
                Value = r.FORMA_PAGO_ID + "",
                Text = r.DESCRIPCION,
                Selected = Recibo.FORMA_PAGO_ID == r.FORMA_PAGO_ID
            }));

            //DOC_ID
            var documentosList = new List<SelectListItem>();
            documentosList.Add(new SelectListItem() {Value = "0", Text = "-Elija documento-", Selected = true});

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
        public ActionResult Create([Bind(Include =
                "RECIBO_ID,USUARIO_ID,CLIENTE_ID,FORMA_PAGO_ID,NRO_RECIBO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADO,CAUSA_ANULADO,DESCRIPCION,NRO_DOC_PAGO,SERIE_DOC_ID")]
            RECIBO rECIBO)
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
        public ActionResult Edit([Bind(Include =
                "RECIBO_ID,USUARIO_ID,CLIENTE_ID,FORMA_PAGO_ID,NRO_RECIBO,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADO,CAUSA_ANULADO,DESCRIPCION,NRO_DOC_PAGO,SERIE_DOC_ID")]
            RECIBO rECIBO)
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
                    var numerorecibo = db.RECIBO.Where(f => f.SERIE_DOC_ID == id.Value)
                        .OrderByDescending(f => f.NRO_RECIBO).Select(f => f.NRO_RECIBO).FirstOrDefault();
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
                documentosList.AddRange(db.DOCS_CC
                    .Where(r => r.TIPO_DOC_ID == id && r.CLIENTE_ID == cliente && r.BALANCE > 0).Select(r =>
                        new SelectListItem()
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
        public ActionResult GuardarRecibo(int CLIENTE_ID, DateTime FECHA_EMISION, int FORMA_PAGO_ID, string NRO_DOC_PAGO,
            int NRO_RECIBO, int SERIE_DOC_ID, decimal TOTAL,  RECIBO_DETALLE[] reciboDet)
        {
            string result = "Error! Recibo no completado!";

           if (NRO_RECIBO != null && CLIENTE_ID != null)
            {
                RECIBO model = new RECIBO();
                model.USUARIO_ID = 1;
                model.CLIENTE_ID = CLIENTE_ID;
                model.FORMA_PAGO_ID = FORMA_PAGO_ID;
                model.NRO_RECIBO = NRO_RECIBO;
                model.FECHA_EMISION = FECHA_EMISION;
                model.SUBTOTAL = TOTAL;
                model.TOTAL = TOTAL;
                model.NRO_DOC_PAGO = NRO_DOC_PAGO;
                model.SERIE_DOC_ID = SERIE_DOC_ID;
                db.RECIBO.Add(model);
                db.SaveChanges();

                foreach (var i in reciboDet)
                {
                    
                    SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString);
                    using (var cmd = new SqlCommand(String.Empty, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (i.TIPO_DOC_ID == 1 || i.TIPO_DOC_ID == 3)
                        {
                            cmd.CommandText =
                                "insert into RECIBO_DETALLE (RECIBO_ID, FACTURA_ID, DESCRIPCION, MONTO, TIPO_DOC_ID, DOC_NRO)" +
                                " values (@RECIBO_ID, @FACTURA_ID, @DESCRIPCION, @MONTO, @TIPO_DOC_ID, @DOC_NRO)";
                            cmd.Parameters.AddWithValue("@RECIBO_ID",
                                db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.RECIBO_ID).FirstOrDefault());
                            cmd.Parameters.AddWithValue("@FACTURA_ID", db.DOCS_CC.Where(d => d.DOC_ID == i.FACTURA_ID).Select(d => d.ID_ORIGEN).FirstOrDefault());
                            cmd.Parameters.AddWithValue("@DESCRIPCION", i.DESCRIPCION);
                            cmd.Parameters.AddWithValue("@MONTO", i.MONTO);
                            cmd.Parameters.AddWithValue("@TIPO_DOC_ID", i.TIPO_DOC_ID);
                            cmd.Parameters.AddWithValue("@DOC_NRO", i.DOC_NRO);
                        }
                        else
                        {
                            cmd.CommandText =
                                "insert into RECIBO_DETALLE (RECIBO_ID, DESCRIPCION, MONTO, TIPO_DOC_ID)" +
                                " values (@RECIBO_ID, @DESCRIPCION, @MONTO, @TIPO_DOC_ID)";
                            cmd.Parameters.AddWithValue("@RECIBO_ID",
                                db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.RECIBO_ID).FirstOrDefault());
                            cmd.Parameters.AddWithValue("@DESCRIPCION", i.DESCRIPCION);
                            cmd.Parameters.AddWithValue("@MONTO", i.MONTO);
                            cmd.Parameters.AddWithValue("@TIPO_DOC_ID", i.TIPO_DOC_ID);
                        }
                        
                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (SqlException e)
                        {
                            result = e.ToString();
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                foreach (var i in reciboDet)
                {
                    #region Cancelan factura
                    if (i.TIPO_DOC_ID == 1) 
                    {
                        /*Actualizo datos de factura*/
                        #region
                        FACTURA fACTURA = db.FACTURA.FirstOrDefault(f => f.NRO_FACTURA == i.DOC_NRO);
                        fACTURA.FECHA_ACTUALIZADO = DateTime.Today;
                        fACTURA.PAGOS = fACTURA.PAGOS.HasValue ? fACTURA.PAGOS + i.MONTO : i.MONTO;
                        if (fACTURA.TOTAL - fACTURA.PAGOS == 0)
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
                        dOCS.FORMA_PAGO_ID = FORMA_PAGO_ID;
                        dOCS.USUARIO_ID = 1;
                        dOCS.CLIENTE_ID = CLIENTE_ID;
                        dOCS.NRO_DOC = NRO_RECIBO; //db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.NRO_RECIBO).FirstOrDefault();
                        dOCS.FECHA_EMISION = FECHA_EMISION;
                        dOCS.DESC_DOC = "Pago de factura No. " + i.DOC_NRO;
                        dOCS.MONTO_DOC = i.MONTO;
                        dOCS.MONTO_PARCIAL = i.MONTO;
                        dOCS.NRO_PAGOS = 1;
                        dOCS.BALANCE = 0;
                        dOCS.NRO_DOC_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == fACTURA.NRO_FACTURA).Select(f => f.NRO_DOC).FirstOrDefault();
                        dOCS.ID_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == fACTURA.NRO_FACTURA).Select(f => f.DOC_ID).FirstOrDefault();
                        dOCS.FECHA_HORA = DateTime.Now;
                        dOCS.ID_BASE = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == fACTURA.NRO_FACTURA).Select(f => f.DOC_ID).FirstOrDefault();
                        dOCS.TIPO = "A";
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
                        dOC1.FECHA_PAGADO = FECHA_EMISION;
                        db.Entry(dOC1).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                    }
                    #endregion

                    #region Cancelan nota de cargo
                    else if (i.TIPO_DOC_ID == 3) 
                    {
                        /*Actualizo datos de nota de cargo*/
                        #region Actualizar nota de cargo
                        NOTA_CARGO nOTA_CARGO = db.NOTA_CARGO.FirstOrDefault(n => n.NRO_NOTA_CARGO == i.DOC_NRO);
                        nOTA_CARGO.FECHA_ACTUALIZADO = DateTime.Today;
                        nOTA_CARGO.PAGOS = nOTA_CARGO.PAGOS.HasValue ? nOTA_CARGO.PAGOS + i.MONTO : i.MONTO;
                        if (nOTA_CARGO.TOTAL - nOTA_CARGO.PAGOS == 0)
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
                        dOCS.FORMA_PAGO_ID = FORMA_PAGO_ID;
                        dOCS.USUARIO_ID = 1;
                        dOCS.CLIENTE_ID = CLIENTE_ID;
                        dOCS.NRO_DOC = NRO_RECIBO; //db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.NRO_RECIBO).FirstOrDefault();
                        dOCS.FECHA_EMISION = FECHA_EMISION;
                        dOCS.DESC_DOC = "Pago de Nota de cargo No. " + nOTA_CARGO.NRO_NOTA_CARGO;
                        dOCS.MONTO_DOC = i.MONTO;
                        dOCS.MONTO_PARCIAL = i.MONTO;
                        dOCS.NRO_PAGOS = 1;
                        dOCS.BALANCE = 0;
                        dOCS.NRO_DOC_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == nOTA_CARGO.NRO_NOTA_CARGO).Select(f => f.NRO_DOC).FirstOrDefault();
                        dOCS.ID_PAGADO = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == nOTA_CARGO.NRO_NOTA_CARGO).Select(f => f.DOC_ID).FirstOrDefault();
                        dOCS.FECHA_HORA = DateTime.Now;
                        dOCS.ID_BASE = db.DOCS_CC.Where(f => f.TIPO_DOC_ID == i.TIPO_DOC_ID && f.NRO_DOC == nOTA_CARGO.NRO_NOTA_CARGO).Select(f => f.DOC_ID).FirstOrDefault();
                        dOCS.TIPO = "A";
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
                        dOC1.FECHA_PAGADO = FECHA_EMISION;
                        db.Entry(dOC1).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion
                    }
                    #endregion

                    #region Cancelan otros
                    else if (i.TIPO_DOC_ID == 4)
                    {
                        #region guardar recibo en DOCS_CC
                        DOCS_CC dOCS = new DOCS_CC();
                        dOCS.TIPO_DOC_ID = 2;
                        dOCS.FORMA_PAGO_ID = FORMA_PAGO_ID;
                        dOCS.USUARIO_ID = 1;
                        dOCS.CLIENTE_ID = CLIENTE_ID;
                        dOCS.NRO_DOC = NRO_RECIBO; //db.RECIBO.OrderByDescending(r => r.RECIBO_ID).Select(r => r.NRO_RECIBO).FirstOrDefault();
                        dOCS.FECHA_EMISION = FECHA_EMISION;
                        dOCS.DESC_DOC = i.DESCRIPCION;
                        dOCS.MONTO_DOC = i.MONTO;
                        dOCS.MONTO_PARCIAL = i.MONTO;
                        dOCS.NRO_PAGOS = 1;
                        dOCS.BALANCE = 0;
                        dOCS.NRO_DOC_PAGADO = null;
                        dOCS.ID_PAGADO = null;
                        dOCS.FECHA_HORA = DateTime.Now;
                        dOCS.ID_BASE = null;
                        dOCS.TIPO = "A";
                        db.DOCS_CC.Add(dOCS);
                        db.SaveChanges();
                        #endregion
                    }
                    #endregion
                }

                result = "Recibo guardado con exito!";

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportesRecibos()
        {
            var clienteList = new List<SelectListItem>();
            clienteList.Add(new SelectListItem() { Value = "0", Text = "-Elija Cliente-" });
            clienteList.AddRange(db.CLIENTE.Select(r => new SelectListItem()
            {
                Value = r.CLIENTE_ID + "",
                Text = r.NOMBRE_CLTE
            }));
            ViewBag.cliente = clienteList;
            return View();
        }

        public PartialViewResult ReporteSaldos(FormCollection collection)
        {
            ViewBag.fecha = DateTime.Today.ToShortDateString();

            //DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string v = "01/" + fechafin.Value.Month + "/" + fechafin.Value.Year;
            DateTime? fechainicio = Convert.ToDateTime(v);

            SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString);
            var command = new SqlCommand("sp_rptSaldos", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@fechainicio", SqlDbType.Date).Value = fechainicio;
            command.Parameters.Add("@fechafin", SqlDbType.Date).Value = fechafin;
            conn.Open();
            DataSet ds = new DataSet();
            using (SqlDataAdapter sda = new SqlDataAdapter(command))
            {
                sda.Fill(ds);
            }
            var TotalAnterior = ds.Tables[0].Select().Sum(t => (decimal) t["Anterior"]);
            var TotalCargo = ds.Tables[0].Select().Sum(t => (decimal)t["Cargo"]);
            var TotalAbono = ds.Tables[0].Select().Sum(t => (decimal)t["Abono"]);
            var TotalActual = ds.Tables[0].Select().Sum(t => (decimal)t["Actual"]);
            conn.Close();

            ViewBag.FechaDesde = fechainicio.Value.ToShortDateString();
            ViewBag.FechaHasta = fechafin.Value.ToShortDateString();
            ViewBag.TotalAnterior = TotalAnterior;
            ViewBag.TotalCargo = TotalCargo;
            ViewBag.TotalAbono = TotalAbono;
            ViewBag.TotalActual = TotalActual;
            return PartialView("ReporteSaldos", ds);
        }

        public PartialViewResult ListadoRecibos (FormCollection collection)
        {
            ViewBag.fecha = DateTime.Today.ToShortDateString();
            DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var consulta = (from r in db.RECIBO
                            join c in db.CLIENTE on r.CLIENTE_ID equals c.CLIENTE_ID
                            orderby r.NRO_RECIBO
                            select new ListadoRecibos
                            {
                                CODIGO_CLTE = c.CODIGO_CLTE,
                                NOMBRE_CLTE = c.NOMBRE_CLTE,
                                NRO_RECIBO = r.NRO_RECIBO,
                                FECHA_EMISION = r.FECHA_EMISION,
                                TOTAL = r.TOTAL
                            });

            if (fechainicio != null && fechafin != null)
            {
                consulta = consulta.Where(r => r.FECHA_EMISION >= fechainicio && r.FECHA_EMISION <= fechafin);
            }

            ViewBag.FechaDesde = fechainicio.Value.ToShortDateString();
            ViewBag.FechaHasta = fechafin.Value.ToShortDateString();

            return PartialView("ListadoRecibos", consulta.ToList());
        }

        public PartialViewResult RecibosPorCliente(FormCollection collection)
        {
            ViewBag.fecha = DateTime.Today.ToShortDateString();
            DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int cliente;

            if (!collection.AllKeys.Contains("cliente") || string.IsNullOrEmpty(collection["cliente"]))
                cliente = 0;
            else
                cliente = Convert.ToInt32(collection["cliente"]);

            var consulta = (from r in db.RECIBO
                            join c in db.CLIENTE on r.CLIENTE_ID equals c.CLIENTE_ID
                            orderby r.NRO_RECIBO
                            select new ListadoRecibos
                            {
                                CLIENTE_ID = c.CLIENTE_ID,
                                CODIGO_CLTE = c.CODIGO_CLTE,
                                NOMBRE_CLTE = c.NOMBRE_CLTE,
                                NRO_RECIBO = r.NRO_RECIBO,
                                FECHA_EMISION = r.FECHA_EMISION,
                                TOTAL = r.TOTAL
                            });

            if (fechainicio != null && fechafin != null)
            {
                consulta = consulta.Where(r => r.FECHA_EMISION >= fechainicio && r.FECHA_EMISION <= fechafin);
            }


            if (cliente != 0)
            {
                consulta = consulta.Where(r => r.CLIENTE_ID == cliente);
            }

            ViewBag.FechaDesde = fechainicio.Value.ToShortDateString();
            ViewBag.FechaHasta = fechafin.Value.ToShortDateString();
            ViewBag.Cliente = db.CLIENTE.Where(c => c.CLIENTE_ID == cliente).Select(c => c.NOMBRE_CLTE)
                .FirstOrDefault();

            return PartialView("RecibosPorCliente", consulta.ToList());
        }

        public PartialViewResult EstadoCuentaPorCliente(FormCollection collection)
        {
            ViewBag.fecha = DateTime.Today.ToShortDateString();
            DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int cliente;

            if (!collection.AllKeys.Contains("cliente") || string.IsNullOrEmpty(collection["cliente"]))
                cliente = 0;
            else
                cliente = Convert.ToInt32(collection["cliente"]);

            var consulta = (from d in db.DOCS_CC
                            join c in db.CLIENTE on d.CLIENTE_ID equals c.CLIENTE_ID
                            orderby d.FECHA_EMISION
                            select new EstadoCuenta
                            {
                                CLIENTE_ID = c.CLIENTE_ID,
                                CODIGO_CLTE = c.CODIGO_CLTE,
                                NOMBRE_CLTE = c.NOMBRE_CLTE,
                                NRO_DOC = d.NRO_DOC,
                                FECHA_EMISION = d.FECHA_EMISION,
                                DESC_DOC = d.DESC_DOC,
                                MONTO = (d.TIPO_DOC_ID == 1 || d.TIPO_DOC_ID == 3 ? d.MONTO_DOC : (d.MONTO_DOC * -1))
                            });

            if (fechainicio != null && fechafin != null)
            {
                consulta = consulta.Where(r => r.FECHA_EMISION >= fechainicio && r.FECHA_EMISION <= fechafin);
            }


            if (cliente != 0)
            {
                consulta = consulta.Where(r => r.CLIENTE_ID == cliente);
            }

            ViewBag.FechaDesde = fechainicio.HasValue ? fechainicio.Value.ToShortDateString() : consulta.OrderBy(c => c.FECHA_EMISION).Select(c => c.FECHA_EMISION).FirstOrDefault().Value.ToShortDateString();
            ViewBag.FechaHasta = fechafin.HasValue ? fechafin.Value.ToShortDateString() : consulta.OrderByDescending(c => c.FECHA_EMISION).Select(c => c.FECHA_EMISION).FirstOrDefault().Value.ToShortDateString();
            ViewBag.Cliente = db.CLIENTE.Where(c => c.CLIENTE_ID == cliente).Select(c => c.NOMBRE_CLTE)
                .FirstOrDefault();

            return PartialView("EstadoCuenta", consulta.ToList());
        }

        public ActionResult AnalisisSaldos(FormCollection collection)
        {
            List<CLIENTE> clientes = db.CLIENTE.ToList();
            return View(clientes);
        }

        public ActionResult DetalleAnalisisSaldos(int clienteID)
        {
            List<DOCS_CC> detalle = db.DOCS_CC.Where(d =>
            d.CLIENTE_ID == clienteID && d.BALANCE > 0 && (d.TIPO_DOC_ID == 1 || d.TIPO_DOC_ID == 3)).ToList();
            return PartialView(detalle);
        }
    }
}
