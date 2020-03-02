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
    
    public partial class PRODUCTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCTO()
        {
            this.FACTURA_DETALLE = new HashSet<FACTURA_DETALLE>();
        }
    
        public int PRODUCTO_ID { get; set; }
        public Nullable<int> TIPO_PRODUCTO_ID { get; set; }
        public Nullable<int> USUARIO_ID { get; set; }
        public Nullable<int> CODIGO_PRODUCTO { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<decimal> PRECIO1 { get; set; }
        public Nullable<decimal> PRECIO2 { get; set; }
        public Nullable<decimal> PRECIO3 { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_ULTIMA_VENTA { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FACTURA_DETALLE> FACTURA_DETALLE { get; set; }
        public virtual TIPO_PRODUCTO TIPO_PRODUCTO { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}
