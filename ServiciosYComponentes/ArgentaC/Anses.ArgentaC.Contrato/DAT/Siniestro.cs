using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public enum enum_EstadoSiniestro { Todos = 0, Pendiente = 1, Asignado = 2, Presentado = 3, Rechazado = 4, Cobrado = 5 }

    [Serializable]
    public enum enum_TipoSiniestro { Todos = 0, Graciable = 1, NoGraciable = 2 }

    [Serializable]
    public enum enum_TipoPoliza { Todos = 0, AUH = 1, SUAF = 2 }

    [Serializable]
    public class Siniestro
    {
        public long idSiniestro { get; set; }
        public long Cuil { get; set; }
        public string ApellidoYNombre { get; set; }
        public DateTime FechaFallecimiento { get; set; }
        public DateTime FechaNovedadFallecimiento { get; set; }
        public DateTime FechaCredito { get; set; }
        public decimal MontoCredito { get; set; }
        public Int32 CantCuotas { get; set; }
        public decimal MontoAmortizado { get; set; }
        public string Poliza { get; set; }
        public enum_EstadoSiniestro Estado { get; set; }
        public enum_TipoSiniestro Tipo { get; set; }
        public string Operador { get; set; }
        public bool HabilitaAsignar { get; set; }
        public bool HabilitaImpresion { get; set; }
        public bool HabilitaCambiarEstado { get; set; }
        public bool HabilitaSeleccionar { get; set; }
        public bool ResultadoAsignado { get; set; }

        public Siniestro()
        {}

        public Siniestro(long _idSiniestro, long _cuil, string _apellidoYNombre, DateTime _fechaFallecimiento, DateTime _fechaNovedadFallecimiento, DateTime _fechaCredito, 
            decimal _montoCredito, Int32 _cantCuotas, decimal _montoAmortizado, string _poliza, enum_EstadoSiniestro _estadoSiniestro, enum_TipoSiniestro _tipoSiniestro, string _operador,
            bool _habilitaAsignar, bool _habilitaImpresion, bool _habilitaCambiarEstado ,bool _habilitaSeleccionar, bool _resultadoAsignado)
        {
            idSiniestro = _idSiniestro;
            Cuil = _cuil;
            ApellidoYNombre = _apellidoYNombre;
            FechaFallecimiento = _fechaFallecimiento;
            FechaNovedadFallecimiento = _fechaNovedadFallecimiento;
            FechaCredito = _fechaCredito;
            MontoCredito = _montoCredito;
            CantCuotas = _cantCuotas;
            MontoAmortizado = _montoAmortizado;
            Poliza = _poliza;
            Estado = _estadoSiniestro;
            Tipo = _tipoSiniestro;
            Operador = _operador;
            HabilitaAsignar = _habilitaAsignar;
            HabilitaImpresion = _habilitaImpresion;
            HabilitaCambiarEstado = _habilitaCambiarEstado;
            HabilitaSeleccionar = _habilitaSeleccionar;
            ResultadoAsignado = _resultadoAsignado;
        }

        public Siniestro(long id)
        {
            idSiniestro = id;
        }
    }
}
