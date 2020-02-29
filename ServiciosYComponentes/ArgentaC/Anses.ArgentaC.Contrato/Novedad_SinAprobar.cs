using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Novedad_SinAprobar
    {
        public Int64 IdPrestador { set; get; }
        public String RazonSocial { set; get; }
        public String NroSucursal { set; get; }
        public String Denominacion { set; get; }
        public int CantidaSinAprobar { set; get; }
        public DateTime MinimaFecNovedad { set; get; }
        public DateTime MaxFecNovedad { set; get; }
        
        public Novedad_SinAprobar()
        { }

        public Novedad_SinAprobar(Int64 _IdPrestador, String _RazonSocial, string _NroSucursal, string _Denominacion, int _CantidaSinAprobar,
                                  DateTime _MinimaFecNovedad, DateTime _MaxFecNovedad)
        {
            IdPrestador = _IdPrestador;
            RazonSocial = _RazonSocial;
            NroSucursal = _NroSucursal;
            Denominacion = _Denominacion;
            CantidaSinAprobar = _CantidaSinAprobar;
            MinimaFecNovedad = _MinimaFecNovedad;
            MaxFecNovedad = _MaxFecNovedad;
        }
    }
}
