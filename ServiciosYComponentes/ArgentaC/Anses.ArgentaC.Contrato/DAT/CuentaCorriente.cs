using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato.DAT
{
    [Serializable]
    public class CuentaCorriente
    {
        public CuentaCorriente() { }
    }

    [Serializable]
    public class CuentaCorriente_Inventario
    {
        public long CUIL { get; set; }
        public string ApellidoyNombre { get; set; }
        public int Novedad_Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public int Concepto { get; set; }
        public decimal ImporteTotal { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal TNA { get; set; }
        public decimal Amortizado { get; set; }
        public decimal Residual { get; set; }
        public short EstadoNovedad_Id { get; set; }
        public string EstadoNovedad_Descripcion { get; set; }
        public DateTime FechaEstado { get; set; }

        public CuentaCorriente_Inventario(long _CUIL, string _ApellidoyNombre, int _Novedad_Id, DateTime _FechaAlta,
                               int _Concepto, decimal _ImporteTotal, int _CantidadCuotas, decimal _TNA,
                               decimal _Amortizado, decimal _Residual, short _EstadoNovedad_Id,
                               string _EstadoNovedad_Descripcion, DateTime _FechaEstado)
        {
            this.CUIL = _CUIL;
            this.ApellidoyNombre = _ApellidoyNombre;
            this.Novedad_Id = _Novedad_Id;
            this.FechaAlta = _FechaAlta;
            this.Concepto = _Concepto;
            this.ImporteTotal = _ImporteTotal;
            this.CantidadCuotas = _CantidadCuotas;
            this.TNA = _TNA;
            this.Amortizado = _Amortizado;
            this.Residual = _Residual;
            this.EstadoNovedad_Id = _EstadoNovedad_Id;
            this.EstadoNovedad_Descripcion = _EstadoNovedad_Descripcion;
            this.FechaEstado = _FechaEstado;
        }
    }
}
