using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Derecho_Habiente
    {
        public string CUIL { get; set; }
        public string Apellido { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public DateTime? Fecha_Fallecimiento { get; set; }
        public decimal ValorBrutoPrestacion { get; set; }
        public decimal ValorNetoPrestacion { get; set; }
        public int UltimoPeriodoPago { get; set; }
        public Discapacidad Discapacidad { get; set; }

        public Derecho_Habiente() {
            this.Discapacidad = new Discapacidad();
        }

        public Derecho_Habiente(string cuil, string apellido, DateTime fecha_nacimiento, DateTime fecha_fallecimiento, 
            decimal valorBrutoPrestacion, decimal valorNetoPrestacion, int ultimoPeriodoPago, Discapacidad discapacidad)
        {
            this.CUIL = cuil;
            this.Apellido = apellido;
            this.Fecha_Nacimiento = fecha_nacimiento;
            this.Fecha_Fallecimiento = fecha_fallecimiento;
            this.ValorBrutoPrestacion = valorBrutoPrestacion;
            this.ValorNetoPrestacion = valorNetoPrestacion;
            this.UltimoPeriodoPago = ultimoPeriodoPago;
            this.Discapacidad = discapacidad;
        }


    }
}
