using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activos_ServiciosComplementarios.Entidades
{
    [Serializable]
    public class DatosDeConsultaAltaTemprana
    {
        public decimal Cuit { get; set; }
        public decimal Cuil { get; set; }
        public DateTime FechaAltaTemprana { get; set; }
        public string DescripcionAbreviadaMovimiento { get; set; }
        public DateTime? FechaFinRelacionLaboral { get; set; }

    }
}
