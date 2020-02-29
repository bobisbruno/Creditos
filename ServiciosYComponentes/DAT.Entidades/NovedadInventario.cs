using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class NovedadInventario
    {
        public Int64 cuil { get; set; }
        public Int64 beneficio { get; set; }
        public string apellidoNombre { get; set; }
        public Int32 idnovedad { get; set; }
        public DateTime fechaAlta { get; set; }
        public Int32 codconceptoliq { get; set; }
        public float montoPrestamo { get; set; }
        public Int32 cantCuotas { get; set; }
        public float tna { get; set; }
        public float totAmortizado { get; set; }
        public float totResidual { get; set; }
        public Int32 idestadosc { get; set; }
        public string descripcionDelEstado { get; set; }
        public DateTime fechaCambioEstado { get; set; }
        public Int32 cantCuotasSinLiq { get; set; }

        public NovedadInventario() { }

        public NovedadInventario(Int64 _cuil, Int64 beneficio, string _apellidoNombre, Int32 _idnovedad, DateTime _fechaalta, Int32 _codconceptoliq, float _montoPrestamo,
                                 Int32 _cantCuotas, float _tna, Int32 _idestadosc, string _descripcionDelEstado, DateTime _fechaCambioEstado, float _totAmortizado,
                                 float _totResidual, Int32 _cantCuotasSinLiq) 
        {
            cuil = _cuil;
            this.beneficio = beneficio;
            apellidoNombre = _apellidoNombre;
            idnovedad = _idnovedad;
            fechaAlta = _fechaalta;
            codconceptoliq = _codconceptoliq;
            montoPrestamo = _montoPrestamo;
            cantCuotas = _cantCuotas;
            tna = _tna;
            idestadosc = _idestadosc;
            descripcionDelEstado = _descripcionDelEstado;
            fechaCambioEstado = _fechaCambioEstado;
            totAmortizado = _totAmortizado;
            totResidual = _totResidual;
            cantCuotasSinLiq = _cantCuotasSinLiq;
        }
    }
}
