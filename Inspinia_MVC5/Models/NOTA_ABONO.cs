//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NOTA_ABONO
    {
        public int NOTA_ABONO_ID { get; set; }
        public Nullable<int> USUARIO_ID { get; set; }
        public Nullable<int> CLIENTE_ID { get; set; }
        public Nullable<int> SERIE_DOC_ID { get; set; }
        public Nullable<int> NRO_NOTA_ABONO { get; set; }
        public Nullable<System.DateTime> FECHA_EMISION { get; set; }
        public Nullable<decimal> SUBTOTAL { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public Nullable<bool> ANULADA { get; set; }
        public string CAUSA_ANULADA { get; set; }
        public Nullable<bool> ESTADO_DOC { get; set; }
        public Nullable<decimal> PAGOS { get; set; }
        public Nullable<System.DateTime> FECHA_ACTUALIZADO { get; set; }
        public Nullable<System.DateTime> FECHA_VENCIMIENTO { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual SERIE_DOCUMENTO SERIE_DOCUMENTO { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}
