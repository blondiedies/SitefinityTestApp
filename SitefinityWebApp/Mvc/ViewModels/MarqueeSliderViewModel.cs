using DocumentFormat.OpenXml.Office.CoverPageProps;
using Progress.Sitefinity.Renderer.Designers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace SitefinityWebApp.Mvc.ViewModels
{
    public class MarqueeSliderViewModel
    {
        [DisplayName("Valor compra en Menudeo (Bs)")]
        public float MenudeoCompra {  get; set; }
        [DisplayName("Valor venta en Menudeo (Bs)")]
        public float MenudeoVenta {  get; set; }
        [DisplayName("Fecha de tasa en Menudeo")]
        public DateTime MenudeoFecha { get; set; }
        [DisplayName("Valor venta en Mesa de Cambio (Bs)")]
        public float MesaCambioCompra {  get; set; }
        [DisplayName("Valor venta en Mesa de Cambio (Bs)")]
        public float MesaCambioVenta {  get; set; }
        [DisplayName("Fecha de tasa en Mesa de Cambio")]
        public DateTime MesaCambioFecha { get; set; }
        [DisplayName("Valor de 1 Petro en Bs")]
        public float PetroCotizacion { get; set; }

    }
}