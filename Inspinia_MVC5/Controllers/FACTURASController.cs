using Inspinia_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class FACTURASController : Controller
    {
        private intersystemerpEntities db = new intersystemerpEntities();

        // GET: /FACTURAS/
        public ActionResult Index()
        {
            DateTime fecha = DateTime.Now;
            var factura = db.FACTURA.OrderByDescending(f => f.FACTURA_ID).Include(f => f.CLIENTE).Include(f => f.SERIE_DOCUMENTO).Include(f => f.USUARIO);
            var total_mes = db.FACTURA.Where(t => t.FECHA_EMISION.Value.Month == fecha.Month && t.FECHA_EMISION.Value.Year == fecha.Year).Sum(t => t.TOTAL);
            var total_anio = db.FACTURA.Where(t => t.FECHA_EMISION.Value.Year == fecha.Year).Sum(t => t.TOTAL).Value;
            var facturas = db.SERIE_DOCUMENTO.Select(f => f.HASTA).FirstOrDefault() - db.FACTURA.OrderByDescending(f => f.NRO_FACTURA).Select(f => f.NRO_FACTURA).FirstOrDefault();

            ViewBag.Total_Anio = total_anio;
            ViewBag.Facturas = facturas;
            ViewBag.Total_Mensual = total_mes;
            return View(factura.ToList());
        }

        // GET: /FACTURAS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURA fACTURA = db.FACTURA.Find(id);
            if (fACTURA == null)
            {
                return HttpNotFound();
            }
            return View(fACTURA);
        }

        // GET: /FACTURAS/Create
        public ActionResult Create()
        {
            FACTURA Factura = new FACTURA();
            //Clientes
            var clienteList = new List<SelectListItem>();
            clienteList.Add(new SelectListItem() { Value = "0", Text = "-Elija Cliente-", Selected = Factura.CLIENTE_ID == 0 });
            clienteList.AddRange(db.CLIENTE.Select(r => new SelectListItem()
            {
                Value = r.CLIENTE_ID + "",
                Text = r.CLIENTE_ID + " - " + r.NOMBRE_CLTE,
                Selected = Factura.CLIENTE_ID == r.CLIENTE_ID
            }));

            //Serie
            var seriesList = new List<SelectListItem>();
            seriesList.Add(new SelectListItem() { Value = "0", Text = "-Elija Serie-", Selected = Factura.SERIE_DOC_ID == 0 });
            seriesList.AddRange(db.SERIE_DOCUMENTO.Where(r => r.SERIE_ACTIVO == true && r.TIPO_DOC_ID == 1).Select(r => new SelectListItem()
            {
                Value = r.SERIE_DOC_ID + "",
                Text = r.SERIE,
                Selected = Factura.SERIE_DOC_ID == r.SERIE_DOC_ID
            }));

            //Tipo Producto
            var productoList = new List<SelectListItem>();
            productoList.Add(new SelectListItem() { Value = "0", Text = "-Elija Producto-", Selected = true });
            productoList.AddRange(db.PRODUCTO.Where(p => p.ACTIVO == true).Select(p => new SelectListItem()
            {
                Value = p.CODIGO_PRODUCTO + "",
                Text = p.NOMBRE,
            }));

            //Recibo
            var recibosList = new List<SelectListItem>();
            recibosList.Add(new SelectListItem() { Value = "0", Text = "-Elija Recibo-", Selected = true });


            ViewBag.RECIBO_ID = recibosList;
            ViewBag.CLIENTE_ID = clienteList;
            ViewBag.SERIE_DOC_ID = seriesList;
            ViewBag.FECHA_EMISION = DateTime.Now.Date.ToShortDateString();
            ViewBag.PRODUCTO = productoList;
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO");
            ViewBag.FECHA_VENCIMIENTO = DateTime.Today.ToShortDateString();
            return View();
        }



        // GET: /FACTURAS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURA fACTURA = db.FACTURA.Find(id);
            if (fACTURA == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", fACTURA.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", fACTURA.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", fACTURA.USUARIO_ID);
            return View(fACTURA);
        }

        // POST: /FACTURAS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FACTURA_ID,USUARIO_ID,CLIENTE_ID,SERIE_DOC_ID,NRO_FACTURA,FECHA_EMISION,SUBTOTAL,TOTAL,ANULADA,CAUSA_ANULADA,ESTADO_DOC,PAGOS,FECHA_ACTUALIZADO, FECHA_VENCIMIENTO")] FACTURA fACTURA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fACTURA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLIENTE_ID = new SelectList(db.CLIENTE, "CLIENTE_ID", "NOMBRE_CLTE", fACTURA.CLIENTE_ID);
            ViewBag.SERIE_DOC_ID = new SelectList(db.SERIE_DOCUMENTO, "SERIE_DOC_ID", "SERIE", fACTURA.SERIE_DOC_ID);
            ViewBag.USUARIO_ID = new SelectList(db.USUARIO, "USUARIO_ID", "NOMBRE_COMPLETO", fACTURA.USUARIO_ID);
            return View(fACTURA);
        }

        // GET: /FACTURAS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTURA fACTURA = db.FACTURA.Find(id);
            if (fACTURA == null)
            {
                return HttpNotFound();
            }
            return View(fACTURA);
        }

        // POST: /FACTURAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FACTURA fACTURA = db.FACTURA.Find(id);
            //db.FACTURA.Remove(fACTURA);
            //db.SaveChanges();
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
        public ActionResult GuardarFactura(DateTime FECHA_EMISION, int SERIE_DOC_ID, int NRO_FACTURA, int CLIENTE_ID, decimal TOTAL, DateTime FECHA_VENCIMIENTO,
           bool APLICAR_RECIBO, int RECIBO_ID, FACTURA_DETALLE[] facturaDetalle)
        {
            string result = "Error! Factura no completado!";

            if (NRO_FACTURA != null)
            {
                FACTURA model = new FACTURA();
                model.USUARIO_ID = 1;
                model.CLIENTE_ID = CLIENTE_ID;
                model.SERIE_DOC_ID = SERIE_DOC_ID;
                model.NRO_FACTURA = NRO_FACTURA;
                model.FECHA_EMISION = FECHA_EMISION;
                model.SUBTOTAL = TOTAL;
                model.TOTAL = TOTAL;
                model.ANULADA = false;
                model.ESTADO_DOC = false;
                model.PAGOS = 0;
                db.FACTURA.Add(model);
                db.SaveChanges();

                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(SERIE_DOC_ID);
                DOCS_CC docs_cc = new DOCS_CC();
                docs_cc.TIPO_DOC_ID = sERIE.TIPO_DOC_ID;
                docs_cc.USUARIO_ID = 1;
                docs_cc.CLIENTE_ID = CLIENTE_ID;
                docs_cc.NRO_DOC = NRO_FACTURA;
                docs_cc.FECHA_EMISION = FECHA_EMISION;
                docs_cc.MONTO_DOC = TOTAL;
                docs_cc.MONTO_PARCIAL = TOTAL;
                docs_cc.FECHA_HORA = DateTime.Now;
                docs_cc.TIPO = "C";
                docs_cc.DESC_DOC = "Factura No. " + NRO_FACTURA;
                docs_cc.FECHA_VENCIMIENTO = FECHA_VENCIMIENTO;
                docs_cc.NRO_PAGOS = 0;
                docs_cc.BALANCE = TOTAL;
                docs_cc.ID_ORIGEN = db.FACTURA.Where(f => f.NRO_FACTURA == NRO_FACTURA).Select(f => f.FACTURA_ID).FirstOrDefault();
                db.DOCS_CC.Add(docs_cc);
                db.SaveChanges();



                foreach (var i in facturaDetalle)
                {
                    FACTURA_DETALLE facDetalle = new FACTURA_DETALLE();
                    facDetalle.FACTURA_ID = docs_cc.ID_ORIGEN;
                    facDetalle.PRODUCTO_ID = i.PRODUCTO_ID;
                    facDetalle.CANTIDAD = i.CANTIDAD;
                    facDetalle.PRECIO_FACTURA = i.PRECIO_FACTURA;
                    facDetalle.DESCRIPCION = i.DESCRIPCION;
                    facDetalle.ANULADA = false;
                    db.FACTURA_DETALLE.Add(facDetalle);
                    db.SaveChanges();

                    PRODUCTO producto = db.PRODUCTO.Find(i.PRODUCTO_ID);
                    producto.FECHA_ULTIMA_VENTA = FECHA_EMISION;
                    db.Entry(producto).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if (APLICAR_RECIBO == true && RECIBO_ID > 0)
                {
                    RECIBO recibo = db.RECIBO.Find(RECIBO_ID);
                    if (recibo.BALANCE - TOTAL > 0)
                    {
                        //Nuevos registros

                        RECIBO_DETALLE reciboDetalle = new RECIBO_DETALLE();
                        reciboDetalle.RECIBO_ID = RECIBO_ID;
                        reciboDetalle.FACTURA_ID = docs_cc.ID_ORIGEN;
                        reciboDetalle.DESCRIPCION = "Pago de factura No." + NRO_FACTURA;
                        reciboDetalle.MONTO = TOTAL;
                        reciboDetalle.TIPO_DOC_ID = docs_cc.TIPO_DOC_ID;
                        reciboDetalle.DOC_NRO = NRO_FACTURA;
                        db.RECIBO_DETALLE.Add(reciboDetalle);
                        db.SaveChanges();

                        DOCS_CC docs_cc2 = new DOCS_CC();
                        docs_cc2.TIPO_DOC_ID = 2;
                        docs_cc2.USUARIO_ID = 1;
                        docs_cc2.CLIENTE_ID = CLIENTE_ID;
                        docs_cc2.NRO_DOC = recibo.NRO_RECIBO;
                        docs_cc2.FECHA_EMISION = FECHA_EMISION;
                        docs_cc2.MONTO_DOC = TOTAL;
                        docs_cc2.MONTO_PARCIAL = TOTAL;
                        docs_cc2.FECHA_HORA = DateTime.Now;
                        docs_cc2.TIPO = "A";
                        docs_cc2.DESC_DOC = "Pago de factura No. " + NRO_FACTURA;
                        docs_cc2.FECHA_VENCIMIENTO = null;
                        docs_cc2.NRO_PAGOS = 0;
                        docs_cc2.BALANCE = 0;
                        docs_cc2.ID_ORIGEN = RECIBO_ID;
                        db.DOCS_CC.Add(docs_cc2);
                        db.SaveChanges();

                        recibo.BALANCE = recibo.BALANCE - TOTAL;
                        db.Entry(recibo).State = EntityState.Modified;
                        db.SaveChanges();

                        //Actualizo registros anteriores
                        RECIBO_DETALLE reciboDetalle2 = db.RECIBO_DETALLE.FirstOrDefault(d => d.RECIBO_ID == RECIBO_ID && d.BALANCE > 0);
                        reciboDetalle2.MONTO = reciboDetalle2.MONTO - TOTAL;
                        reciboDetalle2.BALANCE = reciboDetalle2.BALANCE - TOTAL;
                        db.Entry(reciboDetalle2).State = EntityState.Modified;
                        db.SaveChanges();

                        DOCS_CC docs_cc3 = db.DOCS_CC.FirstOrDefault(d =>
                            d.NRO_DOC == recibo.NRO_RECIBO && d.TIPO_DOC_ID == 2 && d.BALANCE > 0);
                        docs_cc3.MONTO_DOC = docs_cc3.MONTO_DOC - TOTAL;
                        docs_cc3.MONTO_PARCIAL = docs_cc3.MONTO_PARCIAL - TOTAL;
                        docs_cc3.BALANCE = docs_cc3.BALANCE - TOTAL;
                        db.Entry(docs_cc3).State = EntityState.Modified;
                        db.SaveChanges();

                        //Actualizo FACTURA
                        model.PAGOS = TOTAL;
                        model.ESTADO_DOC = true;
                        model.FECHA_ACTUALIZADO = DateTime.Today;
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();

                        //Actualizo factura en DOCS_CC 
                        docs_cc.NRO_PAGOS = docs_cc.NRO_PAGOS + 1;
                        docs_cc.BALANCE = docs_cc.BALANCE - TOTAL;
                        docs_cc.NRO_DOC_PAGADO = recibo.NRO_RECIBO;
                        docs_cc.ID_PAGADO = docs_cc2.DOC_ID;
                        docs_cc.FECHA_PAGADO = FECHA_EMISION;
                        db.Entry(docs_cc).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else if (recibo.BALANCE - TOTAL == 0)
                    {
                        recibo.BALANCE = recibo.BALANCE - TOTAL;
                        db.Entry(recibo).State = EntityState.Modified;
                        db.SaveChanges();

                        RECIBO_DETALLE reciboDetalle = db.RECIBO_DETALLE.FirstOrDefault(d => d.RECIBO_ID == RECIBO_ID && d.BALANCE > 0);
                        reciboDetalle.DESCRIPCION = "Pago de factura No. " + NRO_FACTURA;
                        reciboDetalle.MONTO = reciboDetalle.MONTO - TOTAL;
                        reciboDetalle.BALANCE = reciboDetalle.BALANCE - TOTAL;
                        db.Entry(reciboDetalle).State = EntityState.Modified;
                        db.SaveChanges();

                        DOCS_CC docs_cc2 = db.DOCS_CC.FirstOrDefault(d =>
                            d.NRO_DOC == recibo.NRO_RECIBO && d.TIPO_DOC_ID == 2 && d.BALANCE > 0);
                        docs_cc2.DESC_DOC = "Pago de factura No. " + NRO_FACTURA;
                        docs_cc2.MONTO_DOC = docs_cc2.MONTO_DOC - TOTAL;
                        docs_cc2.MONTO_PARCIAL = docs_cc2.MONTO_PARCIAL - TOTAL;
                        docs_cc2.BALANCE = docs_cc2.BALANCE - TOTAL;
                        db.Entry(docs_cc2).State = EntityState.Modified;
                        db.SaveChanges();

                        //Actualizo FACTURA
                        model.PAGOS = TOTAL;
                        model.ESTADO_DOC = true;
                        model.FECHA_ACTUALIZADO = DateTime.Today;
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();

                        //Actualizo factura en DOCS_CC 
                        docs_cc.NRO_PAGOS = docs_cc.NRO_PAGOS + 1;
                        docs_cc.BALANCE = docs_cc.BALANCE - TOTAL;
                        docs_cc.NRO_DOC_PAGADO = recibo.NRO_RECIBO;
                        docs_cc.ID_PAGADO = docs_cc2.DOC_ID;
                        docs_cc.FECHA_PAGADO = FECHA_EMISION;
                        db.Entry(docs_cc).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                result = "Factura guardada con éxito!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int? GetNumeroFactura(int? id)
        {
            int? factnumero = null;

            if (id.HasValue && id.Value > 0)
            {
                SERIE_DOCUMENTO sERIE = db.SERIE_DOCUMENTO.Find(id);
                if (sERIE.SERIE_ACTIVO.HasValue && sERIE.SERIE_ACTIVO == true)
                {
                    var numerofactura = db.FACTURA.Where(f => f.SERIE_DOC_ID == id.Value).OrderByDescending(f => f.NRO_FACTURA).Select(f => f.NRO_FACTURA).FirstOrDefault();
                    if (numerofactura.HasValue)
                    {
                        factnumero = numerofactura.Value + 1;
                    }
                    else
                    {
                        factnumero = Convert.ToInt32(sERIE.DESDE);
                    }
                    return factnumero;
                }
                else
                {
                    return -1;
                }
            }
            return factnumero;
        }

        [HttpPost]
        public JsonResult GetRecibos(int? id)
        {
            var recibosList = new List<SelectListItem>();
            if (id.HasValue && id > 0)
            {
                recibosList.AddRange(db.RECIBO.Where(r => r.CLIENTE_ID == id && r.BALANCE > 0).Select(
                    r => new SelectListItem()
                    {
                        Value = r.RECIBO_ID + "",
                        Text = r.NRO_RECIBO.ToString(),
                        Selected = true
                    }));
            }
            return Json(recibosList);
        }

        [HttpPost]
        public (string descripcion, decimal? precio) GetDetalleProducto(int? id)
        {

            string descripcion = null;
            decimal? precio = null;

            if (id.HasValue && id.Value > 0)
            {

                PRODUCTO prod = db.PRODUCTO.Find(id);
                descripcion = prod.DESCRIPCION;
                precio = prod.PRECIO1;
            }

            return (descripcion, precio);
        }

        public ActionResult ReportesFacturas()
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

        [HttpPost]
        public PartialViewResult FacturasPorFecha(FormCollection collection)
        {
            DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["FechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            ViewBag.fechaDesde = fechainicio.Value.ToShortDateString();
            ViewBag.fechaHasta = fechafin.Value.ToShortDateString();

            var consulta = (from f in db.FACTURA
                            join c in db.CLIENTE on f.CLIENTE_ID equals c.CLIENTE_ID
                            orderby f.NRO_FACTURA
                            select new ReporteFacturacion
                            {
                                CODIGO_CLTE = c.CODIGO_CLTE,
                                NOMBRE_CLTE = c.NOMBRE_CLTE,
                                NRO_FACTURA = f.NRO_FACTURA,
                                FECHA_EMISION = f.FECHA_EMISION,
                                TOTAL = f.TOTAL,
                                ANULADA = f.ANULADA
                            });

            if (fechainicio != null && fechafin != null)
            {
                consulta = consulta.Where(r => r.FECHA_EMISION >= fechainicio && r.FECHA_EMISION <= fechafin);
            }

            return PartialView("FacturasPorFecha", consulta.ToList());
        }

        [HttpPost]
        public PartialViewResult FacturasAnuladasPorFecha(FormCollection collection)
        {
            DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["FechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            ViewBag.fechaDesde = fechainicio.Value.ToShortDateString();
            ViewBag.fechaHasta = fechafin.Value.ToShortDateString();

            var consulta = (from f in db.FACTURA
                            join c in db.CLIENTE on f.CLIENTE_ID equals c.CLIENTE_ID
                            where f.ANULADA == true
                            select new ReporteFacturacion
                            {
                                CODIGO_CLTE = c.CODIGO_CLTE,
                                NOMBRE_CLTE = c.NOMBRE_CLTE,
                                NRO_FACTURA = f.NRO_FACTURA,
                                FECHA_EMISION = f.FECHA_EMISION,
                                TOTAL = f.TOTAL,
                                ANULADA = f.ANULADA
                            });

            if (fechainicio != null && fechafin != null)
            {
                consulta = consulta.Where(r => r.FECHA_EMISION >= fechainicio && r.FECHA_EMISION <= fechafin);
            }

            return PartialView("FacturasAnuladasPorFecha", consulta.ToList());
        }

        [HttpPost]
        public PartialViewResult FacturasPorCliente(FormCollection collection)
        {
            DateTime? fechainicio = String.IsNullOrEmpty(collection["fechaDesde"]) ? (DateTime?)null : DateTime.ParseExact(collection["FechaDesde"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? fechafin = String.IsNullOrEmpty(collection["fechaHasta"]) ? (DateTime?)null : DateTime.ParseExact(collection["fechaHasta"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int cliente;

            if (!collection.AllKeys.Contains("cliente") || string.IsNullOrEmpty(collection["cliente"]))
                cliente = 0;
            else
                cliente = Convert.ToInt32(collection["cliente"]);

            ViewBag.fechaDesde = fechainicio.Value.ToShortDateString();
            ViewBag.fechaHasta = fechafin.Value.ToShortDateString();
            ViewBag.cliente = db.CLIENTE.Where(c => c.CLIENTE_ID == cliente).Select(c => c.NOMBRE_CLTE)
                .FirstOrDefault();

            var consulta = (from f in db.FACTURA
                            join c in db.CLIENTE on f.CLIENTE_ID equals c.CLIENTE_ID
                            orderby f.NRO_FACTURA
                            select new ReporteFacturacion
                            {
                                CLIENTE_ID = c.CLIENTE_ID,
                                CODIGO_CLTE = c.CODIGO_CLTE,
                                NOMBRE_CLTE = c.NOMBRE_CLTE,
                                NRO_FACTURA = f.NRO_FACTURA,
                                FECHA_EMISION = f.FECHA_EMISION,
                                TOTAL = f.TOTAL,
                                ANULADA = f.ANULADA
                            });

            if (fechainicio != null && fechafin != null)
            {
                consulta = consulta.Where(r => r.FECHA_EMISION >= fechainicio && r.FECHA_EMISION <= fechafin);
            }

            if (cliente != 0)
            {
                consulta = consulta.Where(r => r.CLIENTE_ID == cliente);
            }

            return PartialView("FacturasPorCliente", consulta.ToList());
        }

        public ViewResult ImprimirFactura(int id)
        {
            FACTURA factura = db.FACTURA.Find(id);
            ViewBag.fecha = factura.FECHA_EMISION.Value.ToShortDateString();
            return View(factura);
        }

        public ActionResult InvoicePrint(int id)
        {
            FACTURA factura = db.FACTURA.Find(id);
            return View(factura);
        }

        [HttpPost]
        public ActionResult AnularFactura(int id, FormCollection collection)
        {
            string result = "Error!";
            FACTURA factura = db.FACTURA.Find(id);
            factura.SUBTOTAL = 0;
            factura.TOTAL = 0;
            factura.ANULADA = true;
            factura.CAUSA_ANULADA = collection["CAUSA_ANULADA"];
            factura.ESTADO_DOC = false;
            factura.PAGOS = 0;
            factura.FECHA_ACTUALIZADO = DateTime.Today;
            db.Entry(factura).State = EntityState.Modified;
            db.SaveChanges();

            var docsID = db.DOCS_CC.Where(d => d.NRO_DOC == factura.NRO_FACTURA && d.TIPO_DOC_ID == 1).Select(d => d.DOC_ID).FirstOrDefault();
            DOCS_CC docs = db.DOCS_CC.Find(docsID);
            docs.MONTO_DOC = 0;
            docs.MONTO_PARCIAL = 0;
            docs.NRO_PAGOS = 0;
            docs.BALANCE = 0;
            db.Entry(docs).State = EntityState.Modified;
            db.SaveChanges();

            var facDetalle = from d in db.FACTURA_DETALLE where d.FACTURA_ID == id select d;

            foreach (FACTURA_DETALLE fdDetalle in facDetalle)
            {
                SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString);
                using (var cmd = new SqlCommand(String.Empty, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                        "UPDATE FACTURA_DETALLE SET ANULADA = 1 WHERE DETALLE_ID = @DETALLE_ID";
                    cmd.Parameters.AddWithValue("@DETALLE_ID", fdDetalle.DETALLE_ID);
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

            result = "Factura anulada con éxito!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
