using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    public class Beneficio
    {
        public long Cuil { get; set; }
        public decimal NroBeneficio { get; set; }
        public string ApellidoNombre { get; set; }
        public Int16 TipoDoc { get; set; }
        public long NroDoc { get; set; }
        public string Sexo { get; set; }
	    public DateTime FecNacimiento { get; set; }
        public DateTime? FecFallecimiento { get; set; }
        public decimal SueldoBruto { get; set; }
        public decimal SueldoNeto { get; set; }
        public decimal AfectacionDisponible { get; set; }
        public string CBU { get; set; }
        public int CodBanco { get; set; }
        public int CodAgencia { get; set; }
        public decimal PeriodoAlta { get; set; }
        public int CodPrestacion { get; set; }
        public long RelCuil { get; set; }
        public bool EsDiscapacitado { get; set; }
        public bool EsDiscapacitadoPermanente { get; set; }
        public DateTime? FechaVtoDiscapacidad { get; set; }
        public int? AnioUltimaLiquidacion { get; set; }
        public int? AnioUltimaLibretaPresentada { get; set; }
        public decimal? importeEnMutuo  { get; set; }
        public int? CodigoConceptoLiquidacion { get; set; }
        public string DescripcionConceptoLiquidacion  { get; set; }
        public long CuilDeLaMadre { get; set; }
        public bool MadreFallecida { get; set; }
        public bool? MadreCondenada { get; set; }

        public Beneficio()
        {}

        public Beneficio(   string _Cuil, decimal _NroBeneficio, string _ApellidoNombre, Int16 _TipoDoc, long _NroDoc, string _sexo, DateTime _fnacimiento, 
                            DateTime _ffallecimiento, decimal _SueldoBruto, decimal _SueldoNeto, decimal _AfectacionDisponible, string _cbu, int _codBanco, 
                            int _codAgencia, decimal _periodoAlta, int _CodPrestacion, long _RelCuil, bool _esDiscapacitado, bool _EsDiscapacitadoPermanente, 
                            DateTime? _fechaVtoDiscapacidad, int? _AnioUltimaLiquidacion, int? _AnioUltimaLibretaPresentada
                        )
        {
            this.Cuil = Int32.Parse(_Cuil);
            this.NroBeneficio = _NroBeneficio;
            this.ApellidoNombre = _ApellidoNombre;
            this.TipoDoc = _TipoDoc;
            this.NroDoc = _NroDoc;
            this.Sexo = _sexo;
            this.FecNacimiento = _fnacimiento;
            this.FecFallecimiento = _ffallecimiento;
            this.SueldoBruto = _SueldoBruto;
            this.SueldoNeto = _SueldoNeto;
            this.AfectacionDisponible = _AfectacionDisponible;
            this.CBU = _cbu;
            this.CodBanco = _codBanco;
            this.CodAgencia = _codAgencia;
            this.PeriodoAlta = _periodoAlta;
            this.CodPrestacion = _CodPrestacion;
            this.RelCuil = _RelCuil;
            this.EsDiscapacitado = _esDiscapacitado;
            this.EsDiscapacitadoPermanente = _EsDiscapacitadoPermanente;
            this.FechaVtoDiscapacidad = _fechaVtoDiscapacidad;
            this.AnioUltimaLiquidacion = _AnioUltimaLiquidacion;
            this.AnioUltimaLibretaPresentada = _AnioUltimaLibretaPresentada;
        }

        public Beneficio(long _cuil, string _apellidoNombre, decimal? _importeEnMutuo, long _nroBeneficio, DateTime _fechaNacimiento)
        {
            this.Cuil = _cuil;
            this.ApellidoNombre = _apellidoNombre;
            this.importeEnMutuo = _importeEnMutuo;
            this.NroBeneficio = _nroBeneficio;
            this.FecNacimiento = _fechaNacimiento;
        }
    }

}
