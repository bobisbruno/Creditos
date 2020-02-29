using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public enum enum_TipoFecha { CierreAnterior = 1, CierreProximo = 2 }
    
    [Serializable]
    public class FechaCierre
    {
        public DateTime Fecha { get; set; }
        public long Mensual { get; set; }

        public FechaCierre() { }

        public FechaCierre(DateTime _Fecha, long _Mensual)
        {
            Fecha = _Fecha;
            Mensual = _Mensual;
        }
    }
}
