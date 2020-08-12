using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class ReporteFacturacion
    {
        public int? CLIENTE_ID { get; set; }
        public int? CODIGO_CLTE { get; set; }
        public string NOMBRE_CLTE { get; set; }
        public int? NRO_FACTURA { get; set; }
        public DateTime? FECHA_EMISION { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public  decimal? TOTAL { get; set; }
        public  bool? ANULADA { get; set; }

    }

    public class ListadoRecibos
    {
        public int? CLIENTE_ID { get; set; }
        public int? CODIGO_CLTE { get; set; }
        public string NOMBRE_CLTE { get; set; }
        public int? NRO_RECIBO { get; set; }
        public DateTime? FECHA_EMISION { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? TOTAL { get; set; }
    }
}