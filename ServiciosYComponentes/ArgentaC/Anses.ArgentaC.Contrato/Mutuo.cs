using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Mutuo
    {
        public string NombreYApellido { get; set; }
        public long? CuilTomador { get; set; }
        public string DNI_Nro_Tramite_Renaper { get; set; }
        public string CBU { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Provincia { get; set; }
        public string Calle { get; set; }
        public string Altura { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Mail { get; set; }
        public decimal? ImporteTotal { get; set; }
        public int? CantCuotas { get; set; }
        public decimal? CFTEA { get; set; }
        public decimal? TNA { get; set; }
        public DateTime FechaCredito { get; set; }
        public long? IdProducto { get; set; }
        public long? IdNovedad { get; set; }
        public int IdVersionMutuo { get; set; }
        public long? IdEstadoNovedad { get; set; }
        public long? Oficina { get; set; }
        public List<Beneficio> BeneficiosAfectadosMutuo { get; set; }
        public bool ImposibilidadFirma { get; set; }

        public Mutuo()
        {
            this.NombreYApellido = null;
            this.CuilTomador = null;
            this.DNI_Nro_Tramite_Renaper = null;
            this.Banco = null;
            this.Agencia = null;
            this.CBU = null;
            this.Provincia = null;
            this.Calle = null;
            this.Altura = null;
            this.Piso = null;
            this.Depto = null;
            this.CodigoPostal = null;
            this.Localidad = null;
            this.TelefonoFijo = null;
            this.TelefonoCelular = null;
            this.Mail = null;
            this.ImporteTotal = null;
            this.CantCuotas = null;
            this.CFTEA = null;
            this.TNA = null;
            this.FechaCredito = DateTime.MinValue;
            this.IdProducto = null;
            this.IdNovedad = null;
            this.IdVersionMutuo = 0;
            this.IdEstadoNovedad = null;
            this.Oficina = null;
        }

        public Mutuo(string _nombreYApellido, long? _cuilTomador, string _dni_Nro_Tramite_Renaper, string _banco, string _agencia, string _cbu, string _provincia, string _calle, string _altura,
            string _piso, string _depto, string _codigoPostal, string _localidad, string _telefonoFijo, string _telefonoCelular, string _mail,
            decimal? _importeTotal, int? _cantCuotas, decimal? _cftea, decimal? _tna, DateTime _fechaCredito, long? _idProducto, long? _idNovedad, int _idVersionMutuo, long? _idEstadoNovedad, long? _oficina, bool  _imposibilidadFirma)
        {
            this.NombreYApellido = _nombreYApellido;
            this.CuilTomador = _cuilTomador;
            this.DNI_Nro_Tramite_Renaper = _dni_Nro_Tramite_Renaper;
            this.Banco = _banco;
            this.Agencia = _agencia;
            this.CBU = _cbu;
            this.Provincia = _provincia;
            this.Calle = _calle;
            this.Altura = _altura;
            this.Piso = _piso;
            this.Depto = _depto;
            this.CodigoPostal = _codigoPostal;
            this.Localidad = _localidad;
            this.TelefonoFijo = _telefonoFijo;
            this.TelefonoCelular = _telefonoCelular;
            this.Mail = _mail;
            this.ImporteTotal = _importeTotal;
            this.CantCuotas = _cantCuotas;
            this.CFTEA = _cftea;
            this.TNA = _tna;
            this.FechaCredito = _fechaCredito;
            this.IdProducto = _idProducto;
            this.IdNovedad = _idNovedad;
            this.IdVersionMutuo = _idVersionMutuo;
            this.IdEstadoNovedad = _idEstadoNovedad;
            this.Oficina = _oficina;
            this.ImposibilidadFirma = _imposibilidadFirma;
        }
    }
}
