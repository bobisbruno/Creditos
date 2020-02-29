using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Producto
    {
        public int Id { get; set; }
        public int CantCuotas { get; set; }
        public decimal  Monto_Minimo { get; set; }
        public decimal Monto_Maximo { get; set; }
        public decimal TNA { get; set; }
        public decimal CFTEA { get; set; }
        public decimal CFTNA { get; set; }
        public bool EsRestrictivo { get; set; }

        public List<Cuota> Cuotas { get; set; }

        public Producto() { }

        public Producto(int id, int cantcuotas, decimal monto_minimo, decimal monto_maximo, decimal tna, decimal cftea, decimal cftna, List<Cuota> cuotas, bool esRestrictivo)
        {
            this.Id = id;
            this.CantCuotas = cantcuotas;
            this.Monto_Minimo = monto_minimo;
            this.Monto_Maximo = monto_maximo;
            this.TNA = tna;
            this.CFTEA = cftea;
            this.CFTNA = cftna;
            this.Cuotas = cuotas;
            this.EsRestrictivo = esRestrictivo;
        }
    }
}
