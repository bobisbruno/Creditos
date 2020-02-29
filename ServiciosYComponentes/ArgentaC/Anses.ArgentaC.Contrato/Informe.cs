using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public enum enum_Proposito { FlujoDeFondos = 1, TableroDeCobranzas = 2 ,  CuentaCorriente = 3 }

    [Serializable]
    public class Informe
    {
    }

    [Serializable]
    public class InformeDeDevolucionesCierreDiario  
    {
        public string nombreArchivoCoelsaEnvio { get; set; }
        public int NroInforme { get; set; }
        public string Sistema { get; set; }
        public long CUIL { get; set; }
        public string NombreyApellido { get; set; }
        public decimal ImporteTotal { get; set; }
        public string MotivoDevolucion { get; set; }
        public DateTime FechaDevolucion { get; set; }

        public InformeDeDevolucionesCierreDiario() { }

        public InformeDeDevolucionesCierreDiario(string _nombreArchivoCoelsaEnvio, int _NroInforme, string _Sistema, long _CUIL, string _NombreyApellido, decimal _ImporteTotal, string _MotivoDevolucion, DateTime _FechaDevolucion)
        {
            this.nombreArchivoCoelsaEnvio = _nombreArchivoCoelsaEnvio;
            this.NroInforme = _NroInforme;
            this.Sistema = _Sistema;
            this.CUIL = _CUIL;
            this.NombreyApellido = _NombreyApellido;
            this.ImporteTotal = _ImporteTotal;
            this.MotivoDevolucion = _MotivoDevolucion;
            this.FechaDevolucion = _FechaDevolucion;
        }
    }

    [Serializable]
    public class Mensual
    {
        public int Descripcion { get; set; }
        public Mensual() { }
        public Mensual(int _Descripcion)
        {
            this.Descripcion = _Descripcion;
        }
    }

    [Serializable]
    public class Concepto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public Concepto() { }
        public Concepto(int _codigo, string _descripcion)
        {
            this.Codigo = _codigo;
            this.Descripcion = _descripcion;
        }
    }
}
