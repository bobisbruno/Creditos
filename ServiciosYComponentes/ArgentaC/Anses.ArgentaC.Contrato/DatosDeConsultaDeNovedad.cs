using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class DatosDeConsultaDeNovedad
    {
        public long IdNovedad { get; set; }
        public long Cuil { get; set; }
        public DateTime? FecNovedad { get; set; }
        public DateTime FecAprobacion { get; set; }
        public decimal ImporteTotal { get; set; }
        public string PrimerMensual { get; set; }
        public Tipo EstadoNovedad { get; set; }
        public int IdProducto { get; set; }
        public string Oficina { get; set; }
        public string NombreYApellido { get; set; }
        public string CBU { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public int CantidadDeCuotas { get; set; }
        public string UsuarioAlta { get; set; }
        public string UsuarioSupervision { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public string CodConceptoLiq { get; set; }

        public DatosDeConsultaDeNovedad(long _IdNovedad, long _Cuil, DateTime _FecNovedad, DateTime _FecAprobacion, decimal _ImporteTotal, string _PrimerMensual,
            Tipo _EstadoNovedad, int _IdProducto, string _Oficina, string _NombreYApellido, string _CBU, string _Banco, string _Agencia, int _CantidadDeCuotas,
            string _UsuarioAlta, string _UsuarioSupervision, DateTime _FechaUltimaModificacion, string _CodConceptoLiq)
        {
            this.IdNovedad = _IdNovedad;
            this.Cuil = _Cuil;
            this.FecNovedad = _FecNovedad;
            this.FecAprobacion = _FecAprobacion;
            this.ImporteTotal = _ImporteTotal;
            this.PrimerMensual = _PrimerMensual;
            this.EstadoNovedad = _EstadoNovedad;
            this.IdProducto = _IdProducto;
            this.Oficina = _Oficina;
            this.NombreYApellido = _NombreYApellido;
            this.CBU = _CBU;
            this.Banco = _Banco;
            this.Agencia = _Agencia;
            this.CantidadDeCuotas = _CantidadDeCuotas;
            this.UsuarioAlta = _UsuarioAlta;
            this.UsuarioSupervision = _UsuarioSupervision;
            this.FechaUltimaModificacion = _FechaUltimaModificacion;
            this.CodConceptoLiq = _CodConceptoLiq;
        }

        public DatosDeConsultaDeNovedad() { }

    }
}
