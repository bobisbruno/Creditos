using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class ConsultaBatch
    {
        #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~ConsultaBatch()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Errores de Clase
        public class CierreException : System.ApplicationException
        {
            public CierreException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion

        public enum enum_ConsultaBatch_NombreConsulta {
            NOVEDADES_DOCUMENTACION,
            NOVEDADES_DOCUMENTACION_V3,
            NOVEDADES_CANCELADAS,
            NOVEDADES_CARATULADAS,
            NOVEDADES_INGRESADAS,
            NOVEDADES_INGRESADAS_FGS,
            NOVEDADES_INGRESADAS_FGS_OPERADOR,
            NOVEDADESLIQUIDADAS,
            NOVEDADES_NOAPLICADAS,
            NOVEDADES_CANCELADASV2,
            NOVEDADES_TARJETATIPO3,
            NOVEDADES_CTACTE_INVENTARIO,
            NOVEDADES_BAJAT_AGRUPADAS,
        }

        public ConsultaBatch()
        {
            IDConsulta = 0;
            IDPrestador = 0;
            FechaPedido = new DateTime();
            OpcionBusqueda = 0;
            CriterioBusqueda = 0;
            PeriodoCons = string.Empty;        
            UnConceptoLiquidacion = new ConceptoLiquidacion();           
            NroBeneficio = 0;
            FechaDesde = new DateTime();
            FechaHasta = new DateTime();
            RutaArchGenerado = string.Empty;
            NomArchGenerado = string.Empty;
            ArchivoGenerado = string.Empty;
            Vigente = false;
            FechaGenera = new DateTime();
            Usuario = string.Empty;
            GeneraBatch = false;
            SoloArgenta = false;
            SoloEntidades = false;
        }

        public ConsultaBatch(Int64 iDConsulta,
                             Int64 iDPrestador,
                             enum_ConsultaBatch_NombreConsulta nombreConsulta,
                             DateTime fechaPedido,
                             byte opcionBusqueda,
                             byte criterioBusqueda,
                             string periodoCons,                             
                             ConceptoLiquidacion unConceptoLiquidacion,                           
                             Int64 nroBeneficio,
                             DateTime fechaDesde,
                             DateTime fechaHasta,
                             string rutaArchGenerado,
                             string nomArchGenerado,
                             string archivoGenerado,
                             bool vigente,
                             DateTime fechaGenera,
                             string usuario,
                             bool generaBatch)
        {
            IDConsulta = iDConsulta;
            IDPrestador = iDPrestador;
            NombreConsulta = nombreConsulta;
            FechaPedido = fechaPedido;
            OpcionBusqueda = opcionBusqueda;
            CriterioBusqueda = criterioBusqueda;
            PeriodoCons = periodoCons;           
            UnConceptoLiquidacion = unConceptoLiquidacion;           
            NroBeneficio = nroBeneficio;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            RutaArchGenerado = rutaArchGenerado;
            NomArchGenerado = nomArchGenerado;
            ArchivoGenerado = archivoGenerado;
            Vigente = vigente;
            FechaGenera = fechaGenera;
            Usuario = usuario;
            GeneraBatch = generaBatch;
        }
       
        #region Entidades

        private Int64 idConsulta;
        private Int64 idPrestador;
        private enum_ConsultaBatch_NombreConsulta nombreConsulta;
        private DateTime? fechaPedido;
        private byte opcionBusqueda;
        private byte criterioBusqueda;
        private string periodoCons;
        private ConceptoLiquidacion unConceptoLiquidacion;     
        private Int64? nroBeneficio;
        private DateTime? fechaDesde;
        private DateTime? fechaHasta;
        private string rutaArchGenerado;
        private string nomArchGenerado;
        private string archivoGenerado;
        private bool vigente;
        private DateTime fechaGenera;
        private string usuario;
        private bool generaBatch;

        public int? NroReporte{get; set;}
        public DateTime? Fecha_Presentacion{get; set;}
        public string Nro_Sucursal{get; set;}
        public int? Tipo_Pago{get; set;} 
        public string Tipo_Pago_Desc{get; set;}
        public string CUIL_Usuario{get; set;}
        public int? IdEstado_Documentacion{get; set;}
        public string IdEstado_Documentacion_Desc { get; set; }
        public string Usuario_Logeado{get; set;}
        public string Perfil { get; set; }
        
        
        #endregion 

        #region Propiedades

        public Int64 IDConsulta { get { return idConsulta; } set { idConsulta = value; } }
        public Int64 IDPrestador { get { return idPrestador; } set { idPrestador = value; } }
        public enum_ConsultaBatch_NombreConsulta NombreConsulta { get { return nombreConsulta; } set { nombreConsulta = value; } }
        public DateTime? FechaPedido { get { return fechaPedido; } set { fechaPedido = value; } }
        public byte OpcionBusqueda { get { return opcionBusqueda; } set { opcionBusqueda = value; } }
        public byte CriterioBusqueda { get { return criterioBusqueda; } set { criterioBusqueda = value; } }
        public string PeriodoCons { get { return periodoCons; } set { periodoCons = value; } }
        public ConceptoLiquidacion UnConceptoLiquidacion { get { return unConceptoLiquidacion; } set { unConceptoLiquidacion = value; } }
        public Int64? NroBeneficio { get { return nroBeneficio; } set { nroBeneficio = value; } }
        public DateTime? FechaDesde { get { return fechaDesde; } set { fechaDesde = value; } }
        public DateTime? FechaHasta { get { return fechaHasta; } set { fechaHasta = value; } }
        public string RutaArchGenerado { get { return rutaArchGenerado; } set { rutaArchGenerado = value; } }
        public string NomArchGenerado { get { return nomArchGenerado; } set { nomArchGenerado = value; } }
        public string ArchivoGenerado { get { return archivoGenerado; } set { archivoGenerado = value; } }

        public bool Vigente { get { return vigente; } set { vigente = value; } }
        public DateTime FechaGenera { get { return fechaGenera; } set { fechaGenera = value; } }
        public string Usuario { get { return usuario; } set { usuario = value; } }
        public bool GeneraBatch { get { return generaBatch; } set { generaBatch = value; } }
        public bool GeneraArchivo { get; set; }
        public bool GeneradoAdmin { get; set; }
        public string Razonprestador { get; set; }
        public long? Idnovedad { get; set; }

        public bool SoloArgenta { get; set; }
        public bool SoloEntidades { get; set; }
        public Provincia Provincia { get; set; }
        public int? CodPostal { get; set; }
        public List<string> Oficinas { get; set; }
        public string Lote { get; set; }
        public string DescEstado { get; set; }
        public string Regional { get; set; }
        public DateTime? FechaCambioEstadoDesde { get; set; }
        public DateTime? FechaCambioEstadoHasta { get; set; }
        public int? Cuotas { get; set; } 

        public Decimal? SaldoAmortizacionDesde { get; set; }
        public Decimal? SaldoAmortizacionHasta { get; set; }

        #endregion
    }
}