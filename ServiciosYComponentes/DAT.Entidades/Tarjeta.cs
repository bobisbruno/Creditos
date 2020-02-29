using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
   
    [Serializable]
    public enum enum_TipoDestinoTarjeta 
    {
        
        DomicilioCliente = 1,
	    OficinaCorreo = 2,
	    UDAI = 3,
    }

    [Serializable]
    public enum enum_TipoTarjeta
    {
        Blanca = 1,
        Nominada = 2,
        Carnet = 3,        
    }

    [Serializable] 
    public enum enum_TipoMotivoReemplazo
    {
        EmbosoErroneo= 0,
        Rotura = 1,
        Perdida = 2,
        Robo = 3,
        Desmagnetizacion = 4,
        Otro = 5,
        Unificacion = 6,
    }

    [Serializable]
    public enum enum_TipoMovimientoTarjeta
    {
        ALTA = 'A',
        BAJA = 'B',
        REEMPLAZO = 'R',
        SOLICITUD = 'S',
        RECEPCION ='C',
        ENTREGA = 'E',
        ENTREGA_ACTO = 'M',
        SOLICITUD_EXPRESS = 'X',
        BAJA_EXPRESS = 'Y',
        REINGRESO_FP = 'F',
        RECEPCION_ARCHIVO = 'O',
        SINIESTRADA_ARCHIVO = 'T',
        SOLICITUD_REINGRESOFP = 'P'

    }

    [Serializable]
    public class TipoEstadoTarjeta{

        public byte Codigo {get;set;}
        public string Descripcion { get; set; }
        public bool Habilitante { get; set; }
        public bool Habilitada { get; set; }
        public bool EsBaja { get; set; }
        public bool EsSolicitud { get; set; }
        public bool EsSolicitudNuevaAutomatica { get; set; }
        public bool EsReingresoFlujoPostal { get; set; }
        public String DescEstadoAplicacion { get;  set; }
       

        public TipoEstadoTarjeta() { }

        public TipoEstadoTarjeta(byte _Codigo, string _Descripcion) {
            Codigo = _Codigo;
            Descripcion = _Descripcion;
        }

        public TipoEstadoTarjeta(byte _Codigo, string _Descripcion, bool _Habilitante, bool _Habilitada, bool _EsBaja, bool _EsSolicitud, bool _EsSolicitudNuevaAutomatica, bool _EsReingresoFlujoPostal)
        {
            Codigo = _Codigo;
            Descripcion = _Descripcion;
            Habilitante = _Habilitante;
            Habilitada = _Habilitada;
            EsBaja = _EsBaja;
            EsSolicitud = _EsSolicitud;
            EsSolicitudNuevaAutomatica = _EsSolicitudNuevaAutomatica;
            EsReingresoFlujoPostal = _EsReingresoFlujoPostal;
        }       

        public TipoEstadoTarjeta(String _DescEstadoAplicacion)
        {           
            DescEstadoAplicacion = _DescEstadoAplicacion;
        }
    }

    [Serializable]
    public class TipoOrigenTarjeta
    {
        public int IdOrigen { set; get; }
        public string DescripcionOrigen { set; get; }

        public TipoOrigenTarjeta() { }

        public TipoOrigenTarjeta(int _IdOrigen)
        {
            IdOrigen = _IdOrigen;
        }

        public TipoOrigenTarjeta(int _IdOrigen, string _DescripcionOrigen) 
        {
            IdOrigen = _IdOrigen;
            DescripcionOrigen = _DescripcionOrigen;
        }
    }

    [Serializable]
    public class TipoEstadoPack
    {
        public int IdEstadoPack { set; get; }
        public string DescripcionEstadoPack { set; get; }

        public TipoEstadoPack() { }
        public TipoEstadoPack(int _IdEstadoPack, string _DescripcionEstadoPack)
        {
            IdEstadoPack = _IdEstadoPack;
            DescripcionEstadoPack = _DescripcionEstadoPack;
        }
    }

     [Serializable]
    public class TipoTarjeta
    {
        public enum_TipoTarjeta? IdTipoTarjeta { set; get; }
        public string DescripcionTipoT { set; get; }
        public int IdTipoT { set; get; }
        public Boolean  EsNominada { set; get; }
        public Boolean  PermiteHabilitarPin { set; get; }
        public Boolean  PermiteSolitarEspontanea { set; get; }

        public TipoTarjeta() { }
        
        public TipoTarjeta(enum_TipoTarjeta? _IdTipoTarjeta, string _DescripcionTipoT)
        {
            IdTipoTarjeta = _IdTipoTarjeta;
            DescripcionTipoT = _DescripcionTipoT;
        }

        public TipoTarjeta(int _idTipoT, string _descripcionTipoT, Boolean _esNominada, Boolean _permiteHabilitarPin,
                           Boolean  _permiteSolitarEspontanea)
        {
            this.IdTipoT = _idTipoT;
            this.DescripcionTipoT = _descripcionTipoT;
            this.EsNominada = _esNominada;
            this.PermiteHabilitarPin = _permiteHabilitarPin;
            this.PermiteSolitarEspontanea = _permiteSolitarEspontanea;
            this.IdTipoTarjeta = null;
        }

    }

     [Serializable]
     public class TipoDestinoTarjeta
     {
         public int IdDestino { set; get; }
         public String DescripcionDestino { set; get; }

         public TipoDestinoTarjeta() { }

         public TipoDestinoTarjeta(int _IdDestino, String _DescripcionDestino)
         {
             this.IdDestino = _IdDestino;
             this.DescripcionDestino = _DescripcionDestino;
         }

     
     }

    [Serializable]
    public class TarjetasTransicionEstados
    {
      public int IdEstado {set;get;}
      public string DescEstado {set; get;}
      public int IdDestino {set;get;}
      public int IdEstadoNuevo { set; get;}
      public string DescEstadoNuevo { set; get; }

        public TarjetasTransicionEstados(){}
        public TarjetasTransicionEstados(int _IdEstado, string _DescEstado, int _IdDestino, int _IdEstadoNuevo, string _DescEstadoNuevo) 
        {
            IdEstado = _IdEstado;
            DescEstado = _DescEstado;
            IdDestino = _IdDestino;
            IdEstadoNuevo = _IdEstadoNuevo;
            DescEstadoNuevo = _DescEstadoNuevo;
        }
    }

    [Serializable]
    public class Tarjeta: IDisposable  
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

        ~Tarjeta()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
         		 
        public string Cuil { set; get; }
        public String ApellidoNombre { set; get; }
        public long NroTarjeta { set; get; }
        public DateTime FechaAlta{ set; get; }
        public DateTime FechaNovedad{ set; get; }
        public TipoEstadoTarjeta TipoEstadoTarjeta { set; get; }
        public Int64 IdDomicilio { set; get; }
        public enum_TipoDestinoTarjeta ? TipoDestinoTarjeta { set; get; }
        public string OficinaDestino { set; get; }
        public string DescOficinaDestino { set; get; }
        public enum_TipoMotivoReemplazo ? TipoMotivoReemplazo { set; get; }
        public TipoTarjeta  unTipoTarjeta { set; get; }
        public DateTime? FechaEstimadaEntrega { set; get; }
        public Int64? IdPrestadorOpera { set; get;}
        public Auditoria UnaAuditoria { set; get; }
        public TipoEstadoPack unTipoEstadoPack { set; get; }
        public TipoOrigenTarjeta unTipoOrigenTarjeta { set; get;}
        public bool BlancaPorNominada { set; get; }
        public enum_TipoMovimientoTarjeta? unTipoMovimientoTarjeta { set; get; }
        public String TrackTrace { set; get; }
        public String RecepcionadoPor { set; get; }
        public String Lote { set; get; }
        public TipoDestinoTarjeta unTipoDestinoTarjeta { set; get; }
        public long NroBeneficioPpal { set; get; }
        public long NroCajaArchivo { set; get; }
        public long NroCajaCorreo { set; get; }
        public int PosCajaArchivo { set; get; }
      
        public Tarjeta() 
        {}

        public Tarjeta(string _Cuil, long _NroTarjeta, DateTime _FechaNovedad, TipoEstadoTarjeta _TipoEstadoTarjeta, Int64 _IdDomicilio, 
                       enum_TipoDestinoTarjeta ? _TipoDestinoTarjeta,
                       string _OficinaDestino, enum_TipoMotivoReemplazo ? _TipoMotivoReemplazo, 
                       TipoTarjeta _unTipoTarjeta, DateTime _FechaEstimadaEntrega) 
        {
            NroTarjeta = _NroTarjeta;
            FechaNovedad = _FechaNovedad;
            TipoEstadoTarjeta = _TipoEstadoTarjeta;
            IdDomicilio = _IdDomicilio;
            TipoDestinoTarjeta = _TipoDestinoTarjeta;
            OficinaDestino = _OficinaDestino;
            TipoMotivoReemplazo = _TipoMotivoReemplazo;
            unTipoTarjeta = _unTipoTarjeta;
            FechaEstimadaEntrega = _FechaEstimadaEntrega;
        }

        public Tarjeta(string _Cuil, long _NroTarjeta, string _ApellidoNombre, DateTime _FechaNovedad, TipoEstadoTarjeta  _TipoEstadoTarjeta,
                       Int64 _IdDomicilio, string _OficinaDestino, string _DescOficinaDestino
                       ,enum_TipoDestinoTarjeta ? _TipoDestinoTarjeta,TipoDestinoTarjeta _unTipoDestinoTarjeta, TipoTarjeta _unTipoTarjeta,
                       TipoOrigenTarjeta _unTipoOrigenTarjeta, TipoEstadoPack _unTipoEstadoPack, DateTime _FechaAlta, DateTime? _FechaEstimadaEntrega,
                       String _TrackTrace, String _RecepcionadoPor, String _Lote, long _NroCajaArchivo, long _NroCajaCorreo, int _posCajaArchivo)
        { 
            Cuil = _Cuil;
            NroTarjeta = _NroTarjeta;
            ApellidoNombre = _ApellidoNombre;
            FechaNovedad = _FechaNovedad;
            TipoEstadoTarjeta = _TipoEstadoTarjeta;
            IdDomicilio = _IdDomicilio;
            OficinaDestino = _OficinaDestino;
            DescOficinaDestino = _DescOficinaDestino;
            TipoDestinoTarjeta = _TipoDestinoTarjeta;
            unTipoDestinoTarjeta = _unTipoDestinoTarjeta;
            unTipoTarjeta = _unTipoTarjeta;
            unTipoOrigenTarjeta = _unTipoOrigenTarjeta;
            unTipoEstadoPack = _unTipoEstadoPack;
            FechaAlta = _FechaAlta;
            FechaEstimadaEntrega = _FechaEstimadaEntrega;
            TrackTrace = _TrackTrace;
            RecepcionadoPor = _RecepcionadoPor;
            Lote = _Lote;
            NroCajaArchivo = _NroCajaArchivo;
            NroCajaCorreo = _NroCajaCorreo;
            PosCajaArchivo = _posCajaArchivo;
        }

        public Tarjeta(string _Cuil, long _NroTarjeta, string _ApellidoNombre, DateTime _FechaNovedad, TipoEstadoTarjeta _TipoEstadoTarjeta,
                       Int64 _IdDomicilio, string _OficinaDestino, enum_TipoDestinoTarjeta? _TipoDestinoTarjeta, TipoTarjeta _unTipoTarjeta,
                       TipoOrigenTarjeta _unTipoOrigenTarjeta, TipoEstadoPack _unTipoEstadoPack, DateTime _FechaAlta, DateTime? _FechaEstimadaEntrega                                 
            )
        {
            Cuil = _Cuil;
            NroTarjeta = _NroTarjeta;
            ApellidoNombre = _ApellidoNombre;
            FechaNovedad = _FechaNovedad;
            TipoEstadoTarjeta = _TipoEstadoTarjeta;
            IdDomicilio = _IdDomicilio;
            OficinaDestino = _OficinaDestino;            
            TipoDestinoTarjeta = _TipoDestinoTarjeta;
            unTipoTarjeta = _unTipoTarjeta;
            unTipoOrigenTarjeta = _unTipoOrigenTarjeta;
            unTipoEstadoPack = _unTipoEstadoPack;
            FechaAlta = _FechaAlta;
            FechaEstimadaEntrega = _FechaEstimadaEntrega;           
        }
        

        public Tarjeta(long _NroTarjeta, DateTime _FechaNovedad, TipoEstadoTarjeta _TipoEstadoTarjeta)
        {
            NroTarjeta = _NroTarjeta;
            FechaNovedad = _FechaNovedad;
            TipoEstadoTarjeta = _TipoEstadoTarjeta;
        }
        
        public Tarjeta(string _cuil, string _ApellidoNombre, Int64 _nroTarjeta, DateTime _fechaAlta, DateTime _fechaNovedad, Auditoria _unaAuditoria)
        {
            Cuil = _cuil;
            ApellidoNombre = _ApellidoNombre;
            NroTarjeta = _nroTarjeta;
            FechaAlta = _fechaAlta;
            FechaNovedad = _fechaNovedad;
            UnaAuditoria = _unaAuditoria;            
        }

        //Constructor para EmbozadoAnses
        public Tarjeta(string _Cuil, string _ApellidoNombre, DateTime _FechaNovedad, long _NroBeneficioPpal)
        {
            Cuil = _Cuil;
            ApellidoNombre = _ApellidoNombre;
            FechaNovedad = _FechaNovedad;
            NroBeneficioPpal = _NroBeneficioPpal;
        }

       //Constructor para ReingresoFlujoPostal      
       public Tarjeta(long _NroTarjeta,String _ApellidoNombre, long _NroCajaArchivo, int _PosCajaArchivo, TipoEstadoTarjeta _TipoEstadoTarjeta)
        {
            NroTarjeta = _NroTarjeta;
            ApellidoNombre = _ApellidoNombre;
            NroCajaArchivo = _NroCajaArchivo;
            PosCajaArchivo = _PosCajaArchivo;
            TipoEstadoTarjeta = _TipoEstadoTarjeta;

        }
     }

    [Serializable]
    public class Novedades_TarjetaHistorica
    {
       public long IdNovedad {get;set;}
       public long NroTarjetaAnterior {get;set;}
       public long NroTarjetaNuevo {get;set;}
       public DateTime FReposicion {get;set;}
       public enum_TipoMovimientoTarShop IdTipoMovimientoTarjeta {get;set;}
       public long IdprestadorRepone {get;set;}
       public string OficinaRepone {get;set;}
       public string Usuario {get;set;}
       public DateTime Fultmodificacion {get;set;}

       public Novedades_TarjetaHistorica() { }

       public Novedades_TarjetaHistorica(long idNovedad, long nroTarjetaAnterior, long nroTarjetaNuevo, DateTime fReposicion, enum_TipoMovimientoTarShop idTipoMovimientoTarjeta,
                                         long idprestadorRepone, string oficinaRepone, string usuario,DateTime fultmodificacion)
       {
           IdNovedad = idNovedad;
           NroTarjetaAnterior = nroTarjetaAnterior;
           NroTarjetaNuevo = nroTarjetaNuevo;
           FReposicion = fReposicion;
           IdTipoMovimientoTarjeta = idTipoMovimientoTarjeta;
           IdprestadorRepone = idprestadorRepone;
           OficinaRepone = oficinaRepone;
           Usuario = usuario;
           Fultmodificacion = fultmodificacion;
       }
    }

    [Serializable]
    public class TarjetaHistorica
    { 
      public TipoEstadoTarjeta IdEstadoTarjeta {get;set;}
      public DateTime FNovedad {get;set;}
      public string OficinaDestino { set; get;}
      public enum_TipoDestinoTarjeta? TipoDestinoTarjeta { set; get; }
      public string Usuario {get;set;}
      public string Oficina { set; get; }
      public String TrackTrace { set; get; }
      public String RecepcionadoPor { set; get; }
      public String Lote { set; get; }
    
      public TarjetaHistorica() { }
     
      public TarjetaHistorica(TipoEstadoTarjeta _IdEstadoTarjeta, DateTime _FNovedad, string _OficinaDestino,
                              enum_TipoDestinoTarjeta? _TipoDestinoTarjeta, string _Usuario, string _Oficina,
                              String _TrackTrace, String _RecepcionadoPor, String _Lote ) 
      {
          IdEstadoTarjeta = _IdEstadoTarjeta;
          FNovedad = _FNovedad;
          OficinaDestino = _OficinaDestino;
          TipoDestinoTarjeta = _TipoDestinoTarjeta;
          Usuario = _Usuario;
          Oficina = _Oficina;
          TrackTrace = _TrackTrace;
          RecepcionadoPor = _RecepcionadoPor;
          Lote = _Lote;
      }    
    }

    [Serializable]
    public class TarjetaConsulta
    {
      public String  NombyAp{set;get;}
      public String  DescEstado { set; get; }

      public TarjetaConsulta() { }
      public TarjetaConsulta(String _NombyAp, String _DescEstado) 
      {
          NombyAp = _NombyAp;
          DescEstado = _DescEstado;
      } 
    }

    [Serializable]
    public class TarjetaTurnos
    {
        public bool BeneficioHabilitado { set; get; }
        public string OficinaDestino { set; get; }
        
        public TarjetaTurnos() { }
        public TarjetaTurnos(bool _BeneficioHabilitado, string _OficinaDestino)
        {
            BeneficioHabilitado = _BeneficioHabilitado;
            OficinaDestino = _OficinaDestino;
        }
    }

    [Serializable]
    public class TarjetaTurnosArg
    {
        public String ApellidoNombre { set; get; }
        public long NroTarjeta { set; get; }
        public int IdEstado { set; get; }
        public String DescEstado { set; get; }
        public bool Habilitarturno { set; get; }
        public bool SolicitaNvoDomicilio { set; get; }
        public bool SolicitaCarnet { set; get; }
        public bool EntregadaXEntregar { set; get; }
        public bool ConfirmarNroTarjeta { set; get; }
        public string OficinaDestino { set; get; }
        public TipoOrigenTarjeta TipoOrigenTarjeta { set; get; }   

        public TarjetaTurnosArg() { }

        public TarjetaTurnosArg(String _ApellidoNombre, long _NroTarjeta, int _IdEstado, string _DescEstado, bool _Habilitarturno, bool _SolicitaNvoDomicilio, bool _SolicitaCarnet, bool _EntregadaXEntregar, bool _ConfirmarNroTarjeta, string _OficinaDestino, TipoOrigenTarjeta _TipoOrigenTarjeta)
        {
            ApellidoNombre = _ApellidoNombre;
            NroTarjeta = _NroTarjeta;
            IdEstado = _IdEstado;
            DescEstado = _DescEstado;
            Habilitarturno = _Habilitarturno;
            SolicitaNvoDomicilio = _SolicitaNvoDomicilio;
            SolicitaCarnet = _SolicitaCarnet;
            EntregadaXEntregar = _EntregadaXEntregar;
            ConfirmarNroTarjeta = _ConfirmarNroTarjeta;
            OficinaDestino = _OficinaDestino;
            TipoOrigenTarjeta = _TipoOrigenTarjeta;
        }
    }

    [Serializable]
    public class TarjetaTotalesXEst
    {
        public String DescEstadoAplicacion { set; get; }
        public Int64 Cantidad { set; get; }

        public TarjetaTotalesXEst() { }

        public TarjetaTotalesXEst(String _DescEstadoAplicacion, Int64 _Cantidad)
        {
            DescEstadoAplicacion = _DescEstadoAplicacion;
            Cantidad = _Cantidad ;
        }
    }

    [Serializable]
    public class TarjetasXSucursalEstadoXTipoTarjeta
    { 
     
      public string Cuil { set; get; }
      public String ApellidoNombre { set; get; }
      public long NroTarjeta { set; get; }
      public byte Codigo { get; set; }
      public String DescEstadoAplicacion { get; set; }
      public TipoOrigenTarjeta unTipoOrigenTarjeta { set; get; }
      public DateTime FechaNovedad { set; get; }
      public DateTime FechaAlta { set; get; }
      public String Lote { set; get; }
      public Domicilio unDomicilio { set; get; }
      public String OficinaDestino { set; get; }
      public String DescOficinaDestino { set; get; }
      public String Regional { set; get; }
      

      public TarjetasXSucursalEstadoXTipoTarjeta() { }

      public TarjetasXSucursalEstadoXTipoTarjeta(string _Cuil, String _ApellidoNombre,long _NroTarjeta,
                                                 byte _Codigo,String _DescEstadoAplicacion, TipoOrigenTarjeta _unTipoOrigenTarjeta,
                                                 DateTime _FechaNovedad, DateTime _FechaAlta, String _Lote, Domicilio _unDomicilio,
                                                 String _OficinaDestino, String _DescOficinaDestino, String _Regional)
      {
          this.Cuil = _Cuil;
          this.ApellidoNombre = _ApellidoNombre;
          this.NroTarjeta = _NroTarjeta;
          this.Codigo = _Codigo;
          this.DescEstadoAplicacion = _DescEstadoAplicacion;
          this.unTipoOrigenTarjeta = _unTipoOrigenTarjeta;
          this.FechaNovedad = _FechaNovedad;
          this.FechaAlta = _FechaAlta;
          this.Lote = _Lote;
          this.unDomicilio = _unDomicilio;
          this.OficinaDestino = _OficinaDestino;
          this.DescOficinaDestino = _DescOficinaDestino;
          this.Regional = _Regional;          
      }    
    }

    [Serializable]
    public class TarjetaEmbozado
    {
        public string Cuil { set; get; }
        public string ApellidoNombre { set; get; }
        public long NroTarjeta { set; get; }
        public long BeneficioPrincipal { get; set; }
        public DateTime FechaAltaTarjeta { get; set; }
        public DateTime FechaAltaEmbozado { get; set; }
        public DateTime FechaNovedad { set; get; }
        public int IdEstadoEmbozado { set; get; }
        public string Observaciones { set; get; }
        public Auditoria UnaAuditoria { set; get; }
        
        public TarjetaEmbozado() { }

        public TarjetaEmbozado(string _Cuil, string _ApellidoNombre, long _NroTarjeta, long _BeneficioPrincipal, DateTime _FechaAltaTarjeta,
                               DateTime _FechaAltaEmbozado, DateTime _FechaNovedad, int _IdEstadoEmbozado, string _Observaciones, string _UnaAuditoria)
        {

        }

        public TarjetaEmbozado(string _Cuil, string _ApellidoNombre, long _BeneficioPrincipal, DateTime _FechaNovedad, int _IdEstadoEmbozado)
        {
            Cuil = _Cuil;
            ApellidoNombre = _ApellidoNombre;
            BeneficioPrincipal = _BeneficioPrincipal;
            FechaNovedad = _FechaNovedad;
            IdEstadoEmbozado = _IdEstadoEmbozado;
        }
    }

    [Serializable]
    public class Tarjeta_TAlerta
    {
        public int Recibida { set; get; }
        public int Cantidad { set; get; }

        public Tarjeta_TAlerta() { }

        public Tarjeta_TAlerta(int _Recibida, int _Cantidad) 
        {
            Recibida = _Recibida;
            Cantidad = _Cantidad;
        }
    }

   

}

