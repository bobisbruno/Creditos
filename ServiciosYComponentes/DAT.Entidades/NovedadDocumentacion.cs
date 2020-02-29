using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class NovedadDocumentacion
    {
        public long IdNovedad { get; set; }
        public EstadoDocumentacion Estado { get; set; }
        public int? NroCaja { get; set; }
        public string Error { get; set; }

        public Beneficiario unBeneficiario { get; set; }
        public double? Moto_Prestamo { get; set; }
        public int? Cant_Cuotas { get; set; }
        public DateTime? Fecha_Recepcion { get; set; }

        public NovedadDocumentacion() { }
        public NovedadDocumentacion(long _IdNovedad, EstadoDocumentacion _Estado,
                                    int _NroCaja, string _Error)
        {
            IdNovedad = _IdNovedad;
            Estado = _Estado;
            NroCaja = _NroCaja;
            Error = _Error;
        }

        public NovedadDocumentacion(long _IdNovedad, EstadoDocumentacion _Estado, Beneficiario _unBeneficiario,
                                    double? _Moto_Prestamo, DateTime? _Fecha_Recepcion, int _nroCaja, int _Cant_Cuotas)
        {
            IdNovedad = _IdNovedad;
            Estado = _Estado;
            unBeneficiario = _unBeneficiario;
            Moto_Prestamo = _Moto_Prestamo;
            Fecha_Recepcion = _Fecha_Recepcion;
            NroCaja = _nroCaja;
            Cant_Cuotas = _Cant_Cuotas;
        }
    }
}
