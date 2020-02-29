using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{

    public enum enum_TipoOperacion
    {
        nuevo = 1,
        modificacion = 2,
        cierre = 3       
    }
    
    [Serializable]
    public class Beneficiario : IDisposable
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

        ~Beneficiario()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private long idBeneficiario;
        private long cuil;
        private int? tipoDoc;
        private string nroDoc;
        private string apellidoNombre;
        private double sueldoBruto;
        private double sueldoParaOblig;
        private double afectacionDisponible;
        private double totObligatoria;
        private double totNovedad;
        private int cantOcurrenciasDisponibles;
        //private Boolean exAfjp;
        private int exAfjp;
        private Estado unEstado;
        private string _cbu = string.Empty;
        private bool _esCbuSocial = false;
        private bool habilitadoSolicitarTarjeta = false;
        private Auditoria unAuditoria;
        private List<Reclamo> reclamos;
        private List<Novedad> novedades;
        public Domicilio unDomicilio;
        private string codbanco;
        private string codagencia;
                                   
        #region Getters y Setters
                
        public long IdBeneficiario
        {
            get { return idBeneficiario; }
            set { idBeneficiario = value; }
        }        
        public long Cuil
        {
            get { return cuil; }
            set { cuil = value; }
        }
        public int? TipoDoc
        {
            get { return tipoDoc; }
            set { tipoDoc = value; }
        }
        public string NroDoc
        {
            get { return nroDoc; }
            set { nroDoc = value; }
        }
        public string CodTipoDoc { set; get; }
        public string ApellidoNombre
        {
            get { return apellidoNombre; }
            set { apellidoNombre = value; }
        }
        public double SueldoBruto
        {
            get { return sueldoBruto; }
            set { sueldoBruto = value; }
        }
        public double SueldoParaOblig
        {
            get { return sueldoParaOblig; }
            set { sueldoParaOblig = value; }
        }
        public double AfectacionDisponible
        {
            get { return afectacionDisponible; }
            set { afectacionDisponible = value; }
        }
        public double TotObligatoria
        {
            get { return totObligatoria; }
            set { totObligatoria = value; }
        }
        public double TotNovedad
        {
            get { return totNovedad; }
            set { totNovedad = value; }
        }
        public int CantOcurrenciasDisp
        {
            get { return cantOcurrenciasDisponibles; }
            set { cantOcurrenciasDisponibles = value; }
        }
        public int ExAFJP
        {
            get { return exAfjp; }
            set { exAfjp = value; }
        }
        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }
        public Estado UnEstado
        {
            get { return unEstado; }
            set { unEstado = value; }
        }
        public List<Reclamo> Lista_Reclamos
        {
            get { return reclamos; }
            set { reclamos = value; }
        }
        public List<Novedad> Lista_Novedades
        {
            get { return novedades; }
            set { novedades = value; }
        }
        public string CBU
        {
            get { return _cbu; }
            set { _cbu = value; }
        }
        public bool es_CBUSocial
        {
            get { return _esCbuSocial; }
            set { _esCbuSocial = value; }
        }
        public string Sexo { get; set; }        
        public bool HabilitadoSolicitarTarjeta
        {
            get { return habilitadoSolicitarTarjeta; }
            set { habilitadoSolicitarTarjeta = value; }
        }
        public DateTime? FVencimiento { get; set; }
        public string codBanco
        {
            get { return codbanco; }
            set { codbanco = value; }
        }
        public string codAgencia
        {
            get { return codagencia; }
            set { codagencia = value; }
        }
        public char? JubiladoPensionado { get; set; }  
        public string LeyAplicada { get; set; } 
        public string ExCaja { get; set; }
        public TipoOrigenBeneficiario TipoOrigenBeneficiario { get; set; }
        public DateTime? FFallecimiento { get; set; }
        public TipoEvaluacionRiesgo TipoEvaluacionRiesgo { get; set; }

        #endregion

        #region Constructores

        public Beneficiario()
        {
            IdBeneficiario = 0;
            Cuil = 0;
            TipoDoc = 0;
            NroDoc = string.Empty;
            ApellidoNombre = string.Empty;
            SueldoBruto = 0;
            SueldoParaOblig = 0;
            AfectacionDisponible = 0;
            TotObligatoria = 0;
            TotNovedad = 0;
            CantOcurrenciasDisp = 0;
            ExAFJP = 0;//false;
            UnAuditoria = new Auditoria();            
        }

        public Beneficiario(long IdBeneficiario, long Cuil, string ApellidoNombre) 
        {
            this.idBeneficiario = IdBeneficiario;
            this.cuil = Cuil;
            this.apellidoNombre = ApellidoNombre;        
        }

        public Beneficiario(long idBeneficiario, long cuil,string nroDocumento , string apellidoNombre)
        {
            this.IdBeneficiario = idBeneficiario;
            this.Cuil = cuil;
            this.NroDoc = nroDocumento; 
            this.ApellidoNombre = apellidoNombre;
        }

        public Beneficiario(long idBeneficiario, long cuil,  int? tipoDoc,string nroDocumento, string apellidoNombre, string sexo, string codTipoDoc)
        {
            this.IdBeneficiario = idBeneficiario;
            this.Cuil = cuil;
            this.TipoDoc = tipoDoc;
            this.NroDoc = nroDocumento;
            this.ApellidoNombre = apellidoNombre;
            this.Sexo = sexo;
            this.CodTipoDoc = codTipoDoc;
            
        }

        public Beneficiario(long IdBeneficiario, long	Cuil, int?	TipoDoc,  string NroDoc,	 string ApellidoNombre,
                            double SueldoBruto, double	SueldoParaOblig, double	AfectacionDisponible,
                            double TotObligatoria, double TotNovedad, int CantOcurrenciasDisp,
                            Auditoria unAuditoria, //string Usuario, DateTime?	FecUltModificacion, 
                            int	_ExAfjp, int idEstado, string DescripcionEstado                           
            ) 
        {
            this.idBeneficiario = IdBeneficiario;
            this.cuil = Cuil;
            this.tipoDoc = TipoDoc;
            this.nroDoc = NroDoc;
            this.apellidoNombre = ApellidoNombre;
            this.sueldoBruto = SueldoBruto;
            this.sueldoParaOblig = SueldoParaOblig;
            this.afectacionDisponible = AfectacionDisponible;
            this.totObligatoria = TotObligatoria;
            this.totNovedad = TotNovedad;
            this.cantOcurrenciasDisponibles = CantOcurrenciasDisp;
            this.unAuditoria = unAuditoria;
            this.exAfjp = _ExAfjp;
            this.unEstado = new Estado(idEstado, DescripcionEstado);
            this.Lista_Reclamos = new List<Reclamo>();
            this.Lista_Novedades = new List<Novedad>();
            
        }

        public Beneficiario(long IdBeneficiario, long Cuil, int? TipoDoc, string NroDoc, string ApellidoNombre,String sexo,
                           double SueldoBruto, double SueldoParaOblig, double AfectacionDisponible,
                           double TotObligatoria, double TotNovedad, int CantOcurrenciasDisp,
                           Auditoria unAuditoria, 
                           int _ExAfjp, int idEstado, string DescripcionEstado
                          , DateTime? fVencimiento, TipoOrigenBeneficiario tipoOrigenBeneficiario)
        {
            this.idBeneficiario = IdBeneficiario;
            this.cuil = Cuil;
            this.tipoDoc = TipoDoc;
            this.nroDoc = NroDoc;
            this.apellidoNombre = ApellidoNombre;
            this.Sexo = sexo;
            this.sueldoBruto = SueldoBruto;
            this.sueldoParaOblig = SueldoParaOblig;
            this.afectacionDisponible = AfectacionDisponible;
            this.totObligatoria = TotObligatoria;
            this.totNovedad = TotNovedad;
            this.cantOcurrenciasDisponibles = CantOcurrenciasDisp;
            this.unAuditoria = unAuditoria;
            this.exAfjp = _ExAfjp;
            this.unEstado = new Estado(idEstado, DescripcionEstado);
            this.Lista_Reclamos = new List<Reclamo>();
            this.Lista_Novedades = new List<Novedad>();
            this.FVencimiento = fVencimiento;
            this.TipoOrigenBeneficiario = tipoOrigenBeneficiario;
        }


        public Beneficiario(long IdBeneficiario, long Cuil, int? TipoDoc, string NroDoc, string ApellidoNombre, String sexo,
                           double SueldoBruto, double SueldoParaOblig, double AfectacionDisponible,
                           double TotObligatoria, double TotNovedad, int CantOcurrenciasDisp,
                           Auditoria unAuditoria,
                           int _ExAfjp, int idEstado, string DescripcionEstado,
                           DateTime? fVencimiento, string cbu, bool es_CBUSocial, char? _JubiladoPensionado,
                           string _LeyAplicada, string _ExCaja, TipoOrigenBeneficiario _TipoOrigenBeneficiario,TipoEvaluacionRiesgo _TipoEvaluacionRiesgo)
        {
            this.idBeneficiario = IdBeneficiario;
            this.cuil = Cuil;
            this.tipoDoc = TipoDoc;
            this.nroDoc = NroDoc;
            this.apellidoNombre = ApellidoNombre;
            this.Sexo = sexo;
            this.sueldoBruto = SueldoBruto;
            this.sueldoParaOblig = SueldoParaOblig;
            this.afectacionDisponible = AfectacionDisponible;
            this.totObligatoria = TotObligatoria;
            this.totNovedad = TotNovedad;
            this.cantOcurrenciasDisponibles = CantOcurrenciasDisp;
            this.unAuditoria = unAuditoria;
            this.exAfjp = _ExAfjp;
            this.unEstado = new Estado(idEstado, DescripcionEstado);
            this.Lista_Reclamos = new List<Reclamo>();
            this.Lista_Novedades = new List<Novedad>();
            this.FVencimiento = fVencimiento;
            this.CBU = cbu;
            this.es_CBUSocial = es_CBUSocial;
            this.JubiladoPensionado = _JubiladoPensionado;  
            this.LeyAplicada = _LeyAplicada;
            this.ExCaja = _ExCaja;
            this.TipoOrigenBeneficiario = _TipoOrigenBeneficiario;
            this.TipoEvaluacionRiesgo = _TipoEvaluacionRiesgo;
        }


        /*para consulta de Beneficio */
        public Beneficiario(long IdBeneficiario, long Cuil,string ApellidoNombre,
                            double SueldoBruto, double SueldoParaOblig, double AfectacionDisponible,
                            double TotObligatoria, double TotNovedad, int CantOcurrenciasDisp,
                            Estado _unEstado,int? TipoDoc, string codalfa,string NroDoc, string _cbu                            
                            )
        {
            this.idBeneficiario = IdBeneficiario;
            this.cuil = Cuil;
            this.tipoDoc = TipoDoc;
            this.nroDoc = NroDoc;
            this.apellidoNombre = ApellidoNombre;
            this.sueldoBruto = SueldoBruto;
            this.sueldoParaOblig = SueldoParaOblig;
            this.afectacionDisponible = AfectacionDisponible;
            this.totObligatoria = TotObligatoria;
            this.totNovedad = TotNovedad;
            this.cantOcurrenciasDisponibles = CantOcurrenciasDisp;
            this.unEstado = _unEstado;
            this.CBU = _cbu;
        }

       public Beneficiario(long _IdBeneficiario, long _Cuil,string _ApellidoNombre,
                            double _SueldoBruto, double _SueldoParaOblig, double _AfectacionDisponible,
                            double _TotObligatoria, double _TotNovedad, int _CantOcurrenciasDisp,
                            Estado _unEstado,int? _TipoDoc ,string _NroDoc,Auditoria _unAuditoria,
                            int _ExAfjp ,string _cbu, bool _esCbuSocial, Boolean _habilitadoSolicitarTarjeta                            
                            )
        {
            this.idBeneficiario = _IdBeneficiario;
            this.cuil = _Cuil;
            this.apellidoNombre = _ApellidoNombre;
            this.sueldoBruto = _SueldoBruto;
            this.sueldoParaOblig = _SueldoParaOblig;
            this.afectacionDisponible = _AfectacionDisponible;
            this.totObligatoria = _TotObligatoria;
            this.totNovedad = _TotNovedad;
            this.cantOcurrenciasDisponibles = _CantOcurrenciasDisp;
            this.unEstado = _unEstado;
            this.tipoDoc = _TipoDoc;
            this.nroDoc = _NroDoc;
            this.unAuditoria = _unAuditoria;
            this.exAfjp = _ExAfjp;
            this.CBU = _cbu;
            this.es_CBUSocial = _esCbuSocial;
            this.habilitadoSolicitarTarjeta = _habilitadoSolicitarTarjeta;
        }

        #endregion      
    }
    
    #region Errores de Clase
    [Serializable]
    public class BeneficiarioException : System.ApplicationException
    {
        public BeneficiarioException(string mensaje)
            : base(mensaje)
        {
        }
    }
    #endregion

    [Serializable]
    public class BeneficioBloqueado
    {
        public long IdBeneficiario {get; set;}
        public long Cuil { get; set;}
        private string ApellidoNombre {get;set;}
        public DateTime FecInicio {get; set;}
        public DateTime ? FecFin { get; set; }
        public string Origen { get; set;}
        public string Usuario { get; set;}
        public DateTime ? FecUltModificacion { get; set;}
        public string EntradaCAP { get; set; }
        public Int16 C_Pcia { get; set; }
        public string D_Pcia { get; set; }/* Nuevo*/
        public string Causa { get; set; }
        public string Juez { get; set; }
        public string Secretaria { get; set; }
        public string Actuacion { get; set; }
        public DateTime FecNotificacion { get; set; }
        public string NroNota { get; set; }
        public string Firmante { get; set; }
        public string Observaciones { get; set; }
        public string IP { get; set; }
        public string Oficina{ get; set; }
        public string NroNotaBajaBloqueo { get; set; }
        public string UsuarioBajaBloqueo { get; set; }
        public string OficinaBajaBloqueo { get; set; }
        public DateTime ? FProcesoBajaBloqueo { get; set; }
        public string IpcierreBajaBloqueo { get; set; }
        public string NroExpedienteBajaBloqueo { get; set;}
        
        #region Constructores
        public BeneficioBloqueado()
        { 
        
        }       
       
        public BeneficioBloqueado(long _IdBeneficiario,
                                  long _Cuil, 
                                  string _ApellidoNombre, 
                                  DateTime _FecInicio,
                                  DateTime?  _FecFin,
                                  string _Origen,
                                  string _EntradaCAP,
                                  Int16 _C_Pcia,
                                  string _Causa,
                                  string _Juez,
                                  string _Secretaria,
                                  string _Actuacion,
                                  DateTime _FecNotificacion,
                                  string _NroNota,
                                  string _Firmante,
                                  string _Observaciones,
                                  string _Usuario,
                                  DateTime _FecUltModificacion,
                                  string _IP,
                                  string _Oficina,
                                  string _NroNotaBajaBloqueo,  
                                  string _UsuarioBajaBloqueo,  
                                  string _OficinaBajaBloqueo, 
                                  DateTime ? _FProcesoBajaBloqueo,
                                  string _IpcierreBajaBloqueo,
                                  string _NroExpedienteBajaBloqueo)
        {
            IdBeneficiario = _IdBeneficiario;
            Cuil = _Cuil;
            ApellidoNombre = _ApellidoNombre;
            FecInicio = _FecInicio;
            FecFin = _FecFin;
            Origen = _Origen;
            EntradaCAP = _EntradaCAP;
            C_Pcia = _C_Pcia;
            Causa = _Causa;
            Juez = _Juez;
            Secretaria = _Secretaria;
            Actuacion = _Actuacion;
            FecNotificacion = _FecNotificacion;
            NroNota = _NroNota;
            Firmante = _Firmante;
            Observaciones = _Observaciones;
            Usuario = _Usuario;
            FecUltModificacion = _FecUltModificacion;
            IP = _IP;
            Oficina = _Oficina;
            NroNotaBajaBloqueo = _NroNotaBajaBloqueo;
            UsuarioBajaBloqueo = _UsuarioBajaBloqueo;
            OficinaBajaBloqueo = _OficinaBajaBloqueo;
            FProcesoBajaBloqueo = _FProcesoBajaBloqueo;
            IpcierreBajaBloqueo = _IpcierreBajaBloqueo;
            NroExpedienteBajaBloqueo = _NroExpedienteBajaBloqueo;

        }

        public BeneficioBloqueado(long _IdBeneficiario,
                                  DateTime _FecInicio,
                                  DateTime? _FecFin,
                                  string _D_Pcia,
                                  string _Origen,
                                  string _Causa,                     
                                  string _Juez,
                                  string _Secretaria,                      
                                  string _Actuacion,
                                  DateTime _FecNotificacion, 
                                  string _Observaciones,                                  
                                  string _EntradaCAP,
                                  string _NroNota,
                                  string _Firmante                                 
                                  )
        {
            IdBeneficiario = _IdBeneficiario;
            FecInicio = _FecInicio;
            FecFin = _FecFin;
            D_Pcia = _D_Pcia;
            Origen = _Origen;
            Causa = _Causa;
            Juez = _Juez;
            Secretaria = _Secretaria;
            Actuacion = _Actuacion;
            FecNotificacion = _FecNotificacion;
            Observaciones = _Observaciones;
            EntradaCAP = _EntradaCAP;
            NroNota = _NroNota;
            Firmante = _Firmante;            
        }

        #endregion

    }



    [Serializable]
    public class Inhibiciones{
                
        public long IdBeneficiario { get; set; }
        public long IdPrestador { get; set; }/**/
        public long Cuit { get; set;}
        public string RazonSocial { get; set; } 
        public Int64 CodConceptoLiq { get; set; }/**/
        public string DescConceptoLiq { get; set; }
        public long Cuil { get; set; }
        public string CodSistema { get; set; }
        private string ApellidoNombre { get; set; }
        public DateTime FecInicio{ get; set; }
        public DateTime? FecFin { get; set; }
        public Int16 C_Pcia { get; set; }
        public String DescPcia { get; set; }
        public string Origen { get; set; }
        public string Juez { get; set; }
        public string Secretaria { get; set; }
        public string Actuacion { get; set; }
        public DateTime FecNotificacion { get; set; }
        public string Observaciones { get; set; }
        public string EntradaCAP { get; set; }
        public string NroNota { get; set; }
        public string Firmante { get; set; }
        public string Causa { get; set; }
        public string Usuario { get; set; }
        public DateTime FecUltModificacion{ get; set; }
        public string IP{ get; set; }
        public string Oficina { get; set; }
        public string NroNotaBajaIn { get; set; }
        public string UsuarioBajaIn { get; set; }
        public string OficinaBajaIn { get; set; }
        public DateTime? FProcesoBajaIn { get; set; }
        public string IpcierreBajaIn { get; set; }
        public string NroExpedienteBajaIn { get; set; }
        
        #region Constructores
        public Inhibiciones()
        { 
        }

        public Inhibiciones( long _IdBeneficiario,
                                    long _IdPrestador,
                                    string _RazonSocial,
                                    Int64 _CodConceptoLiq,
                                    long _Cuil, 
                                    string _ApellidoNombre,
                                    DateTime _FecInicio,
                                    DateTime ? _FecFin,
                                    string _Origen,
                                    string _EntradaCAP,
                                    Int16 _C_Pcia,
                                    String _DescPcia,
                                    string _Causa,
                                    string _Juez,
                                    string _Secretaria,
                                    string _Actuacion,
                                    DateTime _FecNotificacion,
                                    string _NroNota,
                                    string _Firmante,
                                    string _Observaciones,
                                    string _Usuario,
                                    DateTime _FecUltModificacion,
                                    string _IP,
                                    string  _Oficina,
                                    string _NroNotaBajaIn,
                                    string _UsuarioBajaIn,
                                    string _OficinaBajaIn,
                                    DateTime? _FProcesoBajaIn,
                                    string _IpcierreBajaIn,
                                    string _NroExpedienteBajaIn)
        {

            IdBeneficiario = _IdBeneficiario;
            IdPrestador = _IdPrestador;
            RazonSocial = _RazonSocial;
            CodConceptoLiq = _CodConceptoLiq;
            Cuil = _Cuil;
            ApellidoNombre = _ApellidoNombre;
            FecInicio = _FecInicio;
            FecFin = _FecFin;
            Origen = _Origen;
            EntradaCAP = _EntradaCAP;
            C_Pcia = _C_Pcia;
            DescPcia = _DescPcia;
            Causa = _Causa;
            Juez = _Juez;
            Secretaria = _Secretaria;
            Actuacion = _Actuacion;
            FecNotificacion = _FecNotificacion;
            NroNota = _NroNota;
            Firmante = _Firmante;
            Observaciones = _Observaciones;
            Usuario = _Usuario;
            FecUltModificacion = _FecUltModificacion;
            IP = _IP;
            Oficina = _Oficina;
            NroNotaBajaIn = _NroNotaBajaIn;
            UsuarioBajaIn = _UsuarioBajaIn;
            OficinaBajaIn = _OficinaBajaIn;
            FProcesoBajaIn = _FProcesoBajaIn;
            IpcierreBajaIn = _IpcierreBajaIn;
            NroExpedienteBajaIn = _NroExpedienteBajaIn;
        }
       
        public Inhibiciones(long _IdBeneficiario,
                            DateTime _FecInicio,
                            DateTime? _FecFin,
                            Int64 _CodConceptoLiq, 
                            string _DescConceptoLiq,        
                            string _RazonSocial,
                            long _Cuit,
                            string _CodSistema,
                            String _DescPcia,
                            string _Origen,
                            string _Causa,
                            string _Juez,
                            string _Secretaria,
                            string _Actuacion,
                            DateTime _FecNotificacion,
                            string _Observaciones,
                            string _EntradaCAP,
                            string _NroNota,
                            string _Firmante                              
                            )
        {
            IdBeneficiario = _IdBeneficiario;
            FecInicio = _FecInicio;
            FecFin = _FecFin;
            CodConceptoLiq = _CodConceptoLiq;
            DescConceptoLiq = _DescConceptoLiq;
            RazonSocial = _RazonSocial;
            Cuit = _Cuit;
            CodSistema = _CodSistema;
            DescPcia = _DescPcia;
            Origen = _Origen;
            Causa = _Causa;
            Juez = _Juez;
            Secretaria = _Secretaria;
            Actuacion = _Actuacion;
            FecNotificacion = _FecNotificacion;
            Observaciones = _Observaciones; 
            EntradaCAP = _EntradaCAP;
            NroNota = _NroNota;
            Firmante = _Firmante;                       
        }        

        #endregion

    }

    [Serializable]
    public class ConceptoAplicado
    {
        public long IdNovedad { get; set; }
        public long IdBeneficiario { get; set; }
        public Int64 CodConceptoLiq { get; set; }
        public String DescConceptoLiq { get; set; }
        public String RazonSocial { get; set; }
        public long Cuit { get; set; }
        public String CodSistema     { get; set; }
        public Double ImporteTotal   { get; set; }
        public Int16  CantCuotas     { get; set; }
        public Double Porcentaje    { get; set; }
        public Double MontoPrestamo   { get; set; }
        public TipoConcepto unTipoConcepto { get; set;} 

        #region
        public ConceptoAplicado()
        { 
        }
        
        public ConceptoAplicado(long _IdNovedad,
                                long _IdBeneficiario,
                                Int64 _CodConceptoLiq,
                                String _DescConceptoLiq,
                                String _RazonSocial,
                                long _Cuit,
                                String _CodSistema,
                                Double _ImporteTotal, 
                                Int16 _CantCuotas,    
                                Double _Porcentaje,   
                                Double _MontoPrestamo,
                                TipoConcepto _unTipoConcepto)
        {
            this.IdNovedad = _IdNovedad;
            this.IdBeneficiario = _IdBeneficiario;
            this.CodConceptoLiq = _CodConceptoLiq;
            this.DescConceptoLiq = _DescConceptoLiq;
            this.RazonSocial = _RazonSocial;
            this.Cuit = _Cuit;
            this.CodSistema = _CodSistema;
            this.ImporteTotal = _ImporteTotal;
            this.CantCuotas = _CantCuotas;
            this.Porcentaje = _Porcentaje;
            this.MontoPrestamo = _MontoPrestamo;
            this.unTipoConcepto = _unTipoConcepto;
        }

        #endregion
    }
    [Serializable]
    public class TodoDelBeneficio
    {
        public Beneficiario unBeneficiario { get; set; }
        public List<Inhibiciones> inhibiciones { get; set; }
        public List<ConceptoAplicado> conceptoAplicados { get; set; }
        public BeneficioBloqueado unBeneficioBloqueado { get; set; }
        
        public TodoDelBeneficio()
        { }

        public TodoDelBeneficio(Beneficiario _unBeneficiario, 
                                List<Inhibiciones> _inhibiciones, 
                                BeneficioBloqueado unBeneficioBloqueado,
                                List<ConceptoAplicado> _conceptoAplicados
                                )
        {
            this.unBeneficiario = _unBeneficiario;
            this.inhibiciones = _inhibiciones;
            this.conceptoAplicados = _conceptoAplicados;
            this.unBeneficioBloqueado = unBeneficioBloqueado;
         }
    }

    [Serializable]
    public class BeneficiarioCBU: Beneficiario
    {
        public string denominacionBanco { get; set; }
        public string denominacionAgencia { get; set; }
        public string calle { get; set; }
        public string nro { get; set; }
        public string localidad { get; set; }
        public string codpostal { get; set; }
        public Int32 idprovincia { get; set; }
        public string descripcionProvincia { get; set; }
        public string codigoDeBanco { get; set; }
        public string codigoDeSucursal{ get; set; }
    }

    [Serializable]
    public class Beneficiario_Reducido
    {
        public long IdBeneficiario { get; set; }
        public long Cuil { get; set; }
        public int? TipoDoc { get; set; }
        public string NroDoc { get; set; }
        public string ApellidoNombre { get; set; }           
        public Domicilio UnDomicilio { get; set; }

        public Beneficiario_Reducido() { }

        public Beneficiario_Reducido(long _IdBeneficiario, long _Cuil, int? _TipoDoc,string _NroDoc, string _ApellidoNombre)
        {
            IdBeneficiario = _IdBeneficiario;
            Cuil = _Cuil;
            TipoDoc = _TipoDoc;
            NroDoc = _NroDoc;
            ApellidoNombre = _ApellidoNombre;
        }

        public Beneficiario_Reducido(long _IdBeneficiario, long _Cuil, string _ApellidoNombre)
        {
            IdBeneficiario = _IdBeneficiario;
            Cuil = _Cuil;           
            ApellidoNombre = _ApellidoNombre;
        }   
    }

    [Serializable]
    public class TipoOrigenBeneficiario
    {
        public int IdOrigenBeneficiario { get; set; }
        public string Descripcion { get; set; }
        public bool EsPNC { get; set; }

        public TipoOrigenBeneficiario() { }

        public TipoOrigenBeneficiario(int _IdOrigenBeneficiario, bool _EsPNC)
        {
            IdOrigenBeneficiario = _IdOrigenBeneficiario;
            EsPNC = _EsPNC;
        }

        public TipoOrigenBeneficiario(bool _EsPNC)
        {           
            EsPNC = _EsPNC;
        }
    }

    [Serializable]
    public class TipoEvaluacionRiesgo
    {
        public int CodEvaluacionRiesgo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FDesde { get; set; }
        public DateTime? FHasta { get; set; }
        public bool HabilitaCargaCredito { get; set; }
        public string MensajeMostrar { get; set; }

        public TipoEvaluacionRiesgo() { }

        public TipoEvaluacionRiesgo(int _CodEvaluacionRiesgo,bool _HabilitaCargaCredito, string _MensajeMostrar)
        {
            CodEvaluacionRiesgo = _CodEvaluacionRiesgo;           
            HabilitaCargaCredito = _HabilitaCargaCredito;
            MensajeMostrar = _MensajeMostrar;
        }
    }
}
