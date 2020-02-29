using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Prestamo
    {
        public decimal Monto_Prestamo { get; set; }
        public int Cantidad_Cuotas { get; set; }
        public decimal Cuota_Total_Mensual { get; set; }
        public decimal TNA { get; set; }
        public decimal Gastos_Administrativos { get; set; }
        public decimal CFTNA { get; set; }
        public decimal CFTEA { get; set; }
        public List<Cuota> Cuotas { get; set; }

        public Prestamo() { }

        public Prestamo(decimal monto_prestamo, int cantidad_cuotas, decimal cuota_total_mensual,
                        decimal tna, decimal gastos_administrativos, decimal cftna, decimal cftea, List<Cuota> cuotas)
        {
            this.Monto_Prestamo = monto_prestamo;
            this.Cantidad_Cuotas = cantidad_cuotas;
            this.Cuota_Total_Mensual = cuota_total_mensual;
            this.TNA = tna;
            this.Gastos_Administrativos = gastos_administrativos;
            this.CFTNA = cftna;
            this.CFTEA = cftea;
            this.Cuotas = cuotas;
        }
    }
}
