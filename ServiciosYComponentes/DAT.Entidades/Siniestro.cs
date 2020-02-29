using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoEstadoSiniestro
    {
        public int IdEstadoSiniestro { get; set; }
        public string DescripcionEstadoSiniestro { get; set; }      
        public bool Habilitado { get; set; }
        public bool HabilitadoWeb { get; set; }
        public bool SolicitoAsignacion { get; set; }
        public bool SolicitoImpresion { get; set; }
        public bool SolicitoResumen { get; set; }
        public bool FueImpreso { get; set; }
        public bool FuePresentado { get; set; }
        public bool FueCobrado { get; set; }
        public bool EsBaja { get; set; }
        public bool FiltraPorOperador { get; set; }
        public bool DebeTenerIdResumen { get; set; }
        public Usuario Usuario { get; set; }    

        public TipoEstadoSiniestro() { }

        public TipoEstadoSiniestro(int _IdEstadoSiniestro, string _DescripcionEstadoSiniestro, bool _Habilitado, bool _HabilitadoWeb)
        {
            IdEstadoSiniestro = _IdEstadoSiniestro;
            DescripcionEstadoSiniestro = _DescripcionEstadoSiniestro;
            Habilitado = _Habilitado;
            HabilitadoWeb = _HabilitadoWeb;
        }

        public TipoEstadoSiniestro(int _IdEstadoSiniestro, string _DescripcionEstadoSiniestro, bool _SolicitoAsignacion, bool _SolicitoImpresion, bool _SolicitoResumen, bool _FueImpreso, bool _FuePresentado, bool _FueCobrado, bool _EsBaja, bool _FiltraPorOperador, bool _Habilitado, bool _HabilitadoWeb, bool _DebeTenerIdResumen)
        {
            IdEstadoSiniestro = _IdEstadoSiniestro;
            DescripcionEstadoSiniestro = _DescripcionEstadoSiniestro;
            SolicitoAsignacion = _SolicitoAsignacion;
            SolicitoImpresion = _SolicitoImpresion;
            SolicitoResumen = _SolicitoResumen;
            FueImpreso = _FueImpreso;
            FuePresentado = _FuePresentado;
            FueCobrado = _FueCobrado;
            EsBaja = _EsBaja;
            FiltraPorOperador = _FiltraPorOperador;
            Habilitado = _Habilitado;
            HabilitadoWeb = _HabilitadoWeb;
            DebeTenerIdResumen = _DebeTenerIdResumen;
        }

        public TipoEstadoSiniestro(int _IdEstadoSiniestro, string _DescripcionEstadoSiniestro, bool _SolicitoAsignacion, bool _SolicitoImpresion, bool _SolicitoResumen, bool _FueImpreso, bool _FuePresentado, bool _FueCobrado, bool _EsBaja)
        {
            IdEstadoSiniestro = _IdEstadoSiniestro;
            DescripcionEstadoSiniestro = _DescripcionEstadoSiniestro;
            SolicitoAsignacion = _SolicitoAsignacion;
            SolicitoImpresion = _SolicitoImpresion;
            SolicitoResumen = _SolicitoResumen;
            FueImpreso = _FueImpreso;
            FuePresentado = _FuePresentado;
            FueCobrado = _FueCobrado;
            EsBaja = _EsBaja;
        }
    }

    [Serializable]
    public class TipoPolizaSeguro
    {
        public int IdPolizaSeguro { get; set; }
        public string DescripcionPolizaSeguro { get; set; }      

        public TipoPolizaSeguro() { }

        public TipoPolizaSeguro(int _IdPolizaSeguro, string _DescripcionPolizaSeguro)
        {
            IdPolizaSeguro = _IdPolizaSeguro;
            DescripcionPolizaSeguro = _DescripcionPolizaSeguro;
        }
    }

    [Serializable]
    public class NovedadSiniestro
    {
        public long IdSiniestro { get; set; }        
        public long IdNovedad { get; set; }
        public long IdBeneficiario { get; set; }
        public long Cuil { get; set; }
        public string ApellidoNombre { get; set; }
        public DateTime FechaFallecimiento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public bool EsGraciable { get; set; }
        public DateTime FechaNovedad { get; set; }
        public double MontoPrestamo { get; set; }
        public int CantCuotas { get; set; }
        public double ImporteSolicitado { get; set; }        
        public TipoEstadoSiniestro Estado { get; set; }
        public int IdLote { get; set; }
        public DateTime FechaSolicitudCobro { get; set; }       
        public double ImporteCobrado { get; set; }  
        public DateTime FechaCobroFGS { get; set; }
        public string IdOperadorAsignado { get; set; }
        public Usuario Usuario { get; set; }
        public NovedadSiniestroResumen Resumen { get; set; }

        public NovedadSiniestro() { }

        public NovedadSiniestro(long _IdSiniestro, long _IdNovedad, long _Cuil, string _ApellidoNombre, DateTime _FechaFallecimiento, bool _EsGraciable, DateTime _FechaNovedad, double _MontoPrestamo, double _ImporteSolicitado,
                                  TipoEstadoSiniestro _Estado, int _IdLote, DateTime _FechaSolicitudCobro, double _ImporteCobrado, DateTime _FechaCobroFGS, string _IdOperadorAsignado, Usuario _Usuario)
        {
            IdSiniestro= _IdSiniestro;
            IdNovedad= _IdNovedad;
            Cuil = _Cuil;
            ApellidoNombre = _ApellidoNombre;
            FechaFallecimiento = _FechaFallecimiento;
            EsGraciable = _EsGraciable;
            FechaNovedad = _FechaNovedad;
            MontoPrestamo = _MontoPrestamo;
            ImporteSolicitado = _ImporteSolicitado;
            Estado = _Estado;
            IdLote = _IdLote;
            FechaSolicitudCobro = _FechaSolicitudCobro;
            ImporteCobrado = _ImporteCobrado;
            FechaCobroFGS = _FechaCobroFGS;
            IdOperadorAsignado = _IdOperadorAsignado;
            Usuario = _Usuario;
        }
    }
        
    [Serializable]
    public class NovedadSiniestroResumen
    {
        public int IdResumen { get; set; }
        public int IdOrden { get; set; }
        public int IdSiniestro { get; set; }
        public long Cuil { get; set; }       
        public DateTime FechaResumen { get; set; }
        public int CantidadSiniestros { get; set; }
        public DateTime FechaFallecimiento { get; set; }
        public string ApellidoNombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }          
        public long IdBeneficiario { get; set; }
        public long IdNovedad { get; set; }
        public DateTime FechaNovedad { get; set; }
        public double MontoPrestamo { get; set; }
        public int CantCuotas { get; set; }
        public double ImporteSolicitado { get; set; }
        public TipoPolizaSeguro TipoPolizaSeguro { get; set; }
        public Usuario Usuario { get; set; }

        public NovedadSiniestroResumen() { }

        public NovedadSiniestroResumen(int _IdResumen, int _IdOrden, int _IdSiniestro, long _Cuil, DateTime _FechaFallecimiento, string _ApellidoNombre, DateTime _FechaNacimiento,
                                         string _Sexo, long _IdBeneficiario, long _IdNovedad, DateTime _FechaNovedad, double _MontoPrestamo, int _CantCuotas, double _ImporteSolicitado, Usuario _Usuario)
        {
            IdResumen = _IdResumen;
            IdOrden = _IdOrden;
            IdSiniestro= _IdSiniestro;
            Cuil = _Cuil;
            FechaFallecimiento = _FechaFallecimiento;
            ApellidoNombre = _ApellidoNombre;
            FechaNacimiento = _FechaNacimiento;
            Sexo = _Sexo;
            IdBeneficiario = _IdBeneficiario;
            IdNovedad = _IdNovedad;
            FechaNovedad = _FechaNovedad;
            MontoPrestamo = _MontoPrestamo;
            CantCuotas = _CantCuotas;
            ImporteSolicitado = _ImporteSolicitado;
            Usuario = _Usuario;            
        }

        public NovedadSiniestroResumen(int _IdResumen, DateTime _FechaResumen, int _CantidadSiniestros, TipoPolizaSeguro _TipoPolizaSeguro, Usuario _Usuario)
        {
            IdResumen = _IdResumen;
            FechaResumen = _FechaResumen;
            CantidadSiniestros = _CantidadSiniestros;
            TipoPolizaSeguro = _TipoPolizaSeguro;
            Usuario = _Usuario;
        }
    }

    [Serializable]
    public class NovedadSiniestroImpresion
    { 
        public int IdSiniestro { get; set; }
        public int IdResumen { get; set; }
        public int IdDocumentoImpreso { get; set; }     
        public Usuario Usuario { get; set; }

        public NovedadSiniestroImpresion() { }

        public NovedadSiniestroImpresion(int _IdSiniestro, int _IdResumen, int _IdDocumentoImpreso, Usuario _Usuario)
        {
            IdSiniestro = _IdSiniestro;
            IdResumen = _IdResumen;
            IdDocumentoImpreso = _IdDocumentoImpreso;
            Usuario = _Usuario;
        }
    }

    [Serializable]
    public class TipoCuentaBancariaSiniestro
    {
        public string TipoCuenta { get; set; }
        public string CBU { get; set; }     
        public string NumeroCuenta { get; set; }
        public string Banco { get; set; }
        public DateTime FDesde { get; set; }
        public DateTime? FHasta { get; set; }

        public TipoCuentaBancariaSiniestro() { }

        public TipoCuentaBancariaSiniestro(string _TipoCuenta, string _CBU, string _NumeroCuenta, string _Banco, DateTime _FDesde, DateTime? _FHasta)
        {
            TipoCuenta = _TipoCuenta;
            CBU = _CBU;
            NumeroCuenta = _NumeroCuenta;
            Banco = _Banco;
            FDesde = _FDesde;
            FHasta = _FHasta;
        }
    }    
}
