using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades.EstadosNovedad;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    public enum enum_tipoestadoNovedad
    {
        Pendientedeconfirmacion = 0,
        Normal = 1,
        Transitorio = 2,
        EnviadoalaLiquidacion = 3,
        Liquidado = 4,
        CancelacionCuota = 5,
        BajaPorAreaControldeDescuentos = 6,
        BajaPorModificacionValor = 7,
        BajaIndeterminado = 8,
        BajaNovedadTotal = 9,
        BajaBeneficioBloqueado = 10,
        BajaBeneficio_ConceptoInhibido = 11,
        Baja_Modiftipos1y6periodoanterior = 12,
        LiquidadoEnvioMensualTpos1y6noAfil = 13,
        TransitorioEnvioMensualTpos1y6noAfil = 14,
        EnviadoaLiqEnvMensualTpos1y6noAfil = 15,
        BajaporBeneficioinexistenteenlaliquidacion = 16,
        BajaporinhibiciondelaEntidad = 17,
        BajaporsistemadeReclamos = 18,
        BajaporrechazodelanovedadGciaControl = 19,
        Bajaporincumplimientoplazospresdocumentacion = 20,
        BajaARGENTASolicitadaLuegoDelCierreDiario = 21,
        CorreccionValorCuota = 22
    }


    [Serializable]
    public class Contacto
    {
        public String Telediscado1 { get; set; }
        public String Telefono1 { get; set; }
        public Boolean EsCelular1 { get; set; }
        public String Telediscado2 { get; set; }
        public String Telefono2 { get; set; }
        public Boolean EsCelular2 { get; set; }
        public String Mail { get; set; }

        public Contacto() { }

        public Contacto(String telediscado1, String telefono1,
                        Boolean esCelular1, String telediscado2,
                        String telefono2, Boolean esCelular2, String mail
                        )
        {
            this.Telediscado1 = telediscado1;
            this.Telefono1 = telefono1;
            this.EsCelular1 = esCelular1;
            this.Telediscado2 = telediscado2;
            this.Telefono2 = telefono2;
            this.EsCelular2 = esCelular2;
            this.Mail = mail;
        }
    }
    
    [Serializable]
    public class Novedad : IDisposable
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

        ~Novedad()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
        private long idNovedad;
        private Beneficiario unBeneficiario;
        private Prestador unPrestador;
        private DateTime fechaNovedad;
        private DateTime fechaInforme;
        private Estado unEstadoRegistro;
        private Estado unEstadoNovedad;
        private CodigoMovimiento unCodMovimiento;
        private ConceptoLiquidacion unConceptoLiquidacion;
        private TipoConcepto unTipoConcepto;
        private double importeCuota;
        private double importeTotal;
        private double importeALiquidar;
        private double importeLiquidado;
        private Byte cantidadCuotas;
        private Single porcentaje;
        private string comprobante;
        private string mac;
        private DateTime? fechaImportacion;
        private string primerMensual;
        private string mensualCarga;
        private string ultimaMensualCuota;
        private string mensualCuota;
        private bool stock;
        private int? mensualReenvio;
        private ModeloPC unModeloPC;
        private Auditoria unAuditoria;
        private int? cantidadCuotasRestantes;
        private int? cuotasLiquidadas;
        private int nroCuotaLiquidada;
        private DateTime? fechaBaja;
        private enum_TipoTarjeta? unTipoTarjeta;
        private string generaNominada;
        private DateTime? fEstimadaEntrega;
        private string oficinaDestino;
        private string descOficinaDestino;
        private List<Cuota> unalista_cuotas;
        private List<Adherente> _unaLista_Adhetentes;
        private string usuario;
        public double TNA { get; set; }
        public double TEM { get; set; }
        public double Gasto_Otorgamiento { get; set; }
        public double Gasto_Adm_Mensual { get; set; }
        public double Cuota_Social { get; set; }
        public double CFTEA { get; set; }
        public double CFTNAReal { get; set; }
        public double CFTEAReal { get; set; }
        public double Gasto_AdmM_ensual_Real { get; set; }
        public double TIRReal { get; set; }
        public byte IdEstadoReg { get; set; }
        public long? IdDomicilioBeneficiario { get; set; }
        public string CBU { get; set; }
        public string Otro { get; set; }
        public string Nro_Tarjeta { get; set; }
        public bool HabilitaBaja { get; set; }
        public string Nro_Sucursal { get; set; }
        public string Nro_Ticket { get; set; }
        public DateTime? FReposicion { get; set; }
        public double? SaldoCredito { get; set; }
        public double TotalAmortizado { get; set; }
        public double MontoPrestamo { get; set; }
        public int idItem { get; set; }
        public int CodMotivoAlta { get; set; }
        public int CodOperacion { get; set; }
        public Contacto unContacto { get; set; }
        public int CantCuotasSinLiquidar { get; set; }
        public string ProximoMensualAliq { get; set; }
        public TipoEstado_SC unTipoEstado_SC { get; set; }    
        public int IdEstadoSC { get; set; }
        public string Descripcion { get; set; }
        public decimal SaldoAmortizacion { get; set; }
        public List<EstadoNovedad> EstadosNovedad { get; set; }
        public List<CancelacionAnticipada> CancelacionAnticipada { get; set; }
        public List<SiniestroCobrado> SiniestroCobrado { get; set; }
        public String CodigoBanco { get; set; }
        public String DescripcionBanco { get; set; }
        public String CodigoAgencia { get; set; }
        public String DescripcionAgencia { get; set; }        
        public String OficinaAlta { get; set; }      
        public String UsuarioSuperv { get; set; }
        public DateTime? FechaSuperv { get; set; }
        public String NombreArchivo { get; set; }
        public TipoPolizaSeguro UnTipoPolizaSeguro { get; set; } 

        #region Get/Set

        public long IdNovedad
        {
            get { return this.idNovedad; }
            set { this.idNovedad = value; }
        }
        public Beneficiario UnBeneficiario
        {
            get { return this.unBeneficiario; }
            set { this.unBeneficiario = value; }
        }
        public Prestador UnPrestador
        {
            get { return this.unPrestador; }
            set { this.unPrestador = value; }
        }
        public DateTime FechaNovedad
        {
            get { return this.fechaNovedad; }
            set { this.fechaNovedad = value; }
        }
        public DateTime FechaInforme
        {
            get { return this.fechaInforme; }
            set { this.fechaInforme = value; }
        }
        public Estado UnEstadoReg
        {
            get { return this.unEstadoRegistro; }
            set { this.unEstadoRegistro = value; }
        }
        public Estado UnEstadoNovedad
        {
            get { return this.unEstadoNovedad; }
            set { this.unEstadoNovedad = value; }
        }
        public CodigoMovimiento UnCodMovimiento
        {
            get { return this.unCodMovimiento; }
            set { this.unCodMovimiento = value; }
        }
        public ConceptoLiquidacion UnConceptoLiquidacion
        {
            get { return this.unConceptoLiquidacion; }
            set { this.unConceptoLiquidacion = value; }
        }
        public TipoConcepto UnTipoConcepto
        {
            get { return unTipoConcepto; }
            set { unTipoConcepto = value; }
        }
        public double ImporteCuota
        {
            get { return this.importeCuota; }
            set { this.importeCuota = value; }
        }
        public double ImporteTotal
        {
            get { return this.importeTotal; }
            set { this.importeTotal = value; }
        }
        public double ImporteALiquidar
        {
            get { return this.importeALiquidar; }
            set { this.importeALiquidar = value; }
        }
        public double ImporteLiquidado
        {
            get { return this.importeLiquidado; }
            set { this.importeLiquidado = value; }
        }
        public Byte CantidadCuotas
        {
            get { return this.cantidadCuotas; }
            set { this.cantidadCuotas = value; }
        }
        public Single Porcentaje
        {
            get { return this.porcentaje; }
            set { this.porcentaje = value; }
        }
        public string Comprobante
        {
            get { return this.comprobante; }
            set { this.comprobante = value; }
        }
        public string MAC
        {
            get { return this.mac; }
            set { this.mac = value; }
        }
        public DateTime? FechaImportacion
        {
            get { return this.fechaImportacion; }
            set { this.fechaImportacion = value; }
        }
        public string PrimerMensual
        {
            get { return this.primerMensual; }
            set { this.primerMensual = value; }
        }

        public string MensualCarga
        {
            get { return this.mensualCarga; }
            set { this.mensualCarga = value; }
        }

        public string UltimaMensualCuota
        {
            get { return this.ultimaMensualCuota; }
            set { this.ultimaMensualCuota = value; }
        }

        public string MensualCuota
        {
            get { return this.mensualCuota; }
            set { this.mensualCuota = value; }
        }
        public bool Stock
        {
            get { return this.stock; }
            set { this.stock = value; }
        }
        public int? MensualReenvio
        {
            get { return this.mensualReenvio; }
            set { this.mensualReenvio = value; }
        }
        public ModeloPC UnModeloPC
        {
            get { return unModeloPC; }
            set { unModeloPC = value; }
        }
        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }
        public int? CantidadCuotasRestantes
        {
            get { return this.cantidadCuotasRestantes; }
            set { this.cantidadCuotasRestantes = value; }
        }
        public int? CuotasLiquidadas
        {
            get { return this.cuotasLiquidadas; }
            set { this.cuotasLiquidadas = value; }
        }
        public int NroCuotaLiquidada
        {
            get { return this.nroCuotaLiquidada; }
            set { this.nroCuotaLiquidada = value; }
        }
        public DateTime? FechaBaja
        {
            get { return this.fechaBaja; }
            set { this.fechaBaja = value; }
        }

        public enum_TipoTarjeta? UnTipoTarjeta { get; set; }

        public string GeneraNominada { get; set; }

        public DateTime? FEstimadaEntrega { get; set; }

        public string OficinaDestino { get; set; }

        public string DescOficinaDestino { get; set; }

        public List<Cuota> unaLista_Cuotas
        {
            get { return unalista_cuotas; }
            set { unalista_cuotas = value; }
        }

        public List<Adherente> unaLista_Adhetentes
        {
            get { return _unaLista_Adhetentes; }
            set { _unaLista_Adhetentes = value; }
        }

        public string Usuario
        {
            get { return this.usuario; }
            set { this.usuario = value; }
        }

        public bool GeneraCompImpedimentoFirma { get; set; }
        public List<DocumentacionScaneada> DocumentacionScaneada { get; set; }
    
        #endregion

        public Novedad()
        {
            IdNovedad = 0;
            UnBeneficiario = new Beneficiario();
            UnPrestador = new Prestador();
            FechaNovedad = new DateTime();
            UnEstadoReg = new Estado();
            UnCodMovimiento = new CodigoMovimiento();
            UnConceptoLiquidacion = new ConceptoLiquidacion();
            UnTipoConcepto = new TipoConcepto();
            ImporteTotal = 0;
            CantidadCuotas = 0;
            Porcentaje = 0;
            Comprobante = string.Empty;
            MAC = string.Empty;
            FechaImportacion = new DateTime();
            PrimerMensual = string.Empty;
            Stock = false;
            MensualReenvio = 0;
            UnModeloPC = new ModeloPC();
            UnAuditoria = new Auditoria();
            cantidadCuotasRestantes = 0;
            cuotasLiquidadas = 0;
            unaLista_Cuotas = new List<Cuota>();
            unaLista_Adhetentes = new List<Adherente>();
            unTipoEstado_SC = new TipoEstado_SC();
            EstadosNovedad = new List<EstadoNovedad>();
            CancelacionAnticipada = new List<CancelacionAnticipada>();
            SiniestroCobrado = new List<SiniestroCobrado>();
        }

        public Novedad(long idNovedad, DateTime fechaNovedad,
                       double importeTotal, Byte cantidadCuotas,
                       Single porcentaje, string comprobante,
                       string mac)
        {
            this.IdNovedad = idNovedad;
            this.FechaNovedad = fechaNovedad;
            this.ImporteTotal = importeTotal;
            this.CantidadCuotas = cantidadCuotas;
            this.Porcentaje = porcentaje;
            this.Comprobante = comprobante;
            this.MAC = mac;

            UnBeneficiario = new Beneficiario();
            UnPrestador = new Prestador();
            UnEstadoReg = new Estado();
            unEstadoNovedad = new Estado();
            UnCodMovimiento = new CodigoMovimiento();
            UnConceptoLiquidacion = new ConceptoLiquidacion();
            UnTipoConcepto = new TipoConcepto();
            UnModeloPC = new ModeloPC();
            UnAuditoria = new Auditoria();
            unaLista_Cuotas = new List<Cuota>();
            this.unaLista_Adhetentes = new List<Adherente>();
            EstadosNovedad = new List<EstadoNovedad>();
            CancelacionAnticipada = new List<CancelacionAnticipada>();
            SiniestroCobrado = new List<SiniestroCobrado>();
        }
        
        public Novedad(long idNovedad, DateTime fechaNovedad,
                       double importeTotal, Byte cantidadCuotas,
                       Single porcentaje, string comprobante,
                       string mac, DateTime? fechaImportacion,
                       string primerMensual, bool stock, int? mensualReenvio
                       )
        {
            this.IdNovedad = idNovedad;
            this.FechaNovedad = fechaNovedad;
            this.ImporteTotal = importeTotal;
            this.CantidadCuotas = cantidadCuotas;
            this.Porcentaje = porcentaje;
            this.Comprobante = comprobante;
            this.MAC = mac;
            this.FechaImportacion = !fechaImportacion.HasValue ? null : fechaImportacion;
            this.PrimerMensual = primerMensual;
            this.Stock = stock;
            this.MensualReenvio = mensualReenvio;

            UnBeneficiario = new Beneficiario();
            UnPrestador = new Prestador();
            UnEstadoReg = new Estado();
            unEstadoNovedad = new Estado();
            UnCodMovimiento = new CodigoMovimiento();
            UnConceptoLiquidacion = new ConceptoLiquidacion();
            UnTipoConcepto = new TipoConcepto();
            UnModeloPC = new ModeloPC();
            UnAuditoria = new Auditoria();

        }


        public Novedad(long idNovedad, DateTime fechaNovedad,
                   double importeTotal, Byte cantidadCuotas,
                   Single porcentaje, string comprobante,
                   string mac, DateTime? fechaImportacion,
                   string primerMensual, bool stock, int? mensualReenvio,
                   string usuarioAlta,string oficinaAlta,
                   string usuarioSuperv, DateTime? fechaSuperv, string nombreArchivo)
        {
            this.IdNovedad = idNovedad;
            this.FechaNovedad = fechaNovedad;
            this.ImporteTotal = importeTotal;
            this.CantidadCuotas = cantidadCuotas;
            this.Porcentaje = porcentaje;
            this.Comprobante = comprobante;
            this.MAC = mac;
            this.FechaImportacion = !fechaImportacion.HasValue ? null : fechaImportacion;
            this.PrimerMensual = primerMensual;
            this.Stock = stock;
            this.MensualReenvio = mensualReenvio;
            this.Usuario = usuarioAlta;
            this.OficinaAlta = oficinaAlta;            
            this.UsuarioSuperv = usuarioSuperv;
            this.FechaSuperv = fechaSuperv;
            this.NombreArchivo = nombreArchivo;

            UnBeneficiario = new Beneficiario();
            UnPrestador = new Prestador();
            UnEstadoReg = new Estado();
            unEstadoNovedad = new Estado();
            UnCodMovimiento = new CodigoMovimiento();
            UnConceptoLiquidacion = new ConceptoLiquidacion();
            UnTipoConcepto = new TipoConcepto();
            UnModeloPC = new ModeloPC();
            UnAuditoria = new Auditoria();

        }

        public Novedad(long idNovedad,
                       DateTime fechaNovedad,
                       DateTime? fechaImportacion,
                       double importeCuota,
                       double importeTotal,
                       double importeALiquidar,
                       double importeLiquidado,
                       Byte cantidadCuotas,
                       Single porcentaje,
                       string comprobante,
                       string primerMensual,
                       string mensualCuota,
                       int? mensualReenvio,
                       string mac,
                       bool stock,
                       int? cantidadCuotasRestantes,
                       int? cuotasLiquidadas,
                       int nroCuotaLiquidada,
                       DateTime? fechabaja)
        {
            this.IdNovedad = idNovedad;
            this.FechaNovedad = fechaNovedad;
            this.FechaImportacion = !fechaImportacion.HasValue ? null : fechaImportacion;
            this.ImporteCuota = importeCuota;
            this.ImporteTotal = importeTotal;
            this.ImporteALiquidar = importeALiquidar;
            this.ImporteLiquidado = importeLiquidado;
            this.CantidadCuotas = cantidadCuotas;
            this.CantidadCuotasRestantes = cantidadCuotasRestantes;
            this.Porcentaje = porcentaje;
            this.Comprobante = comprobante;
            this.PrimerMensual = primerMensual;
            this.MensualCuota = primerMensual;
            this.MensualReenvio = !mensualReenvio.HasValue ? null : mensualReenvio;
            this.MAC = mac;
            this.Stock = stock;
            this.CuotasLiquidadas = cuotasLiquidadas;
            this.CantidadCuotasRestantes = cantidadCuotasRestantes;
            this.NroCuotaLiquidada = nroCuotaLiquidada;
            this.FechaBaja = fechaBaja;

            UnBeneficiario = new Beneficiario();
            UnPrestador = new Prestador();
            UnEstadoReg = new Estado();
            UnEstadoNovedad = new Estado();
            UnCodMovimiento = new CodigoMovimiento();
            UnConceptoLiquidacion = new ConceptoLiquidacion();
            UnTipoConcepto = new TipoConcepto();
            UnModeloPC = new ModeloPC();
            UnAuditoria = new Auditoria();
            EstadosNovedad = new List<EstadoNovedad>();
            CancelacionAnticipada = new List<CancelacionAnticipada>();
            SiniestroCobrado = new List<SiniestroCobrado>();
        }

        public Novedad(long idNovedad,
                       DateTime fechaNovedad,
                       DateTime? fechaImportacion,
                       double importeCuota,
                       double importeTotal,
                       double importeALiquidar,
                       double importeLiquidado,
                       Byte cantidadCuotas,
                       Single porcentaje,
                       string comprobante,
                       string primerMensual,
                       string mensualCuota,
                       int? mensualReenvio,
                       string mac,
                       bool stock,
                       int? cantidadCuotasRestantes,
                       int? cuotasLiquidadas,
                       int nroCuotaLiquidada,
                       //DateTime? fechaBaja,
                       Beneficiario unBeneficiario,
                       Prestador unPrestador,
                       Estado unEstadoReg,
                       CodigoMovimiento unCodMovimiento,
                       ConceptoLiquidacion unConceptoLiquidacion,
                       TipoConcepto unTipoConcepto,
                       ModeloPC unModeloPC,
                       Auditoria unAuditoria)
        {
            this.IdNovedad = idNovedad;
            this.FechaNovedad = fechaNovedad;
            this.FechaImportacion = !fechaImportacion.HasValue ? null : fechaImportacion;
            this.ImporteCuota = importeCuota;
            this.ImporteTotal = importeTotal;
            this.ImporteALiquidar = importeALiquidar;
            this.ImporteLiquidado = importeLiquidado;
            this.CantidadCuotas = cantidadCuotas;
            this.CantidadCuotasRestantes = cantidadCuotasRestantes;
            this.Porcentaje = porcentaje;
            this.Comprobante = comprobante;
            this.PrimerMensual = primerMensual;
            this.MensualCuota = primerMensual;
            this.MensualReenvio = !mensualReenvio.HasValue ? null : mensualReenvio;
            this.MAC = mac;
            this.Stock = stock;
            this.CuotasLiquidadas = cuotasLiquidadas;
            this.CantidadCuotasRestantes = cantidadCuotasRestantes;
            this.NroCuotaLiquidada = nroCuotaLiquidada;
            //this.FechaBaja = fechaBaja; 

            UnBeneficiario = unBeneficiario;
            UnPrestador = unPrestador;
            UnEstadoReg = unEstadoReg;
            UnCodMovimiento = unCodMovimiento;
            UnConceptoLiquidacion = unConceptoLiquidacion;
            UnTipoConcepto = unTipoConcepto;
            UnModeloPC = unModeloPC;
            UnAuditoria = unAuditoria;
            EstadosNovedad = new List<EstadoNovedad>();
            CancelacionAnticipada = new List<CancelacionAnticipada>();
            SiniestroCobrado = new List<SiniestroCobrado>();
        }

        public Novedad(long idNovedad,
                       DateTime fechaNovedad,
                       DateTime? fechaImportacion,
                       double importeCuota,
                       double importeTotal,
                       double montoPrestamo,
                       double importeALiquidar,
                       double importeLiquidado,
                       Byte cantidadCuotas,
                       Single porcentaje,
                       string comprobante,
                       string primerMensual,
                       string mensualCuota,
                       int? mensualReenvio,
                       string mac,
                       bool stock,
                       int? cantidadCuotasRestantes,
                       int? cuotasLiquidadas,
                       int nroCuotaLiquidada,
                       DateTime? fechaBaja,
                       Beneficiario unBeneficiario,
                       Prestador unPrestador,
                       Estado unEstadoReg,
                       Estado unEstadoNovedad,
                       CodigoMovimiento unCodMovimiento,
                       ConceptoLiquidacion unConceptoLiquidacion,
                       TipoConcepto unTipoConcepto,
                       ModeloPC unModeloPC,
                       Auditoria unAuditoria)
        {
            this.IdNovedad = idNovedad;
            this.FechaNovedad = fechaNovedad;
            this.FechaImportacion = !fechaImportacion.HasValue ? null : fechaImportacion;
            this.ImporteCuota = importeCuota;
            this.ImporteTotal = importeTotal;
            this.MontoPrestamo = montoPrestamo;
            this.ImporteALiquidar = importeALiquidar;
            this.ImporteLiquidado = importeLiquidado;
            this.CantidadCuotas = cantidadCuotas;
            this.CantidadCuotasRestantes = cantidadCuotasRestantes;
            this.Porcentaje = porcentaje;
            this.Comprobante = comprobante;
            this.PrimerMensual = primerMensual;
            this.MensualCuota = primerMensual;
            this.MensualReenvio = !mensualReenvio.HasValue ? null : mensualReenvio;
            this.MAC = mac;
            this.Stock = stock;
            this.CuotasLiquidadas = cuotasLiquidadas;
            this.CantidadCuotasRestantes = cantidadCuotasRestantes;
            this.NroCuotaLiquidada = nroCuotaLiquidada;
            this.FechaBaja = fechaBaja;

            UnBeneficiario = unBeneficiario;
            UnPrestador = unPrestador;
            UnEstadoReg = unEstadoReg;
            UnEstadoNovedad = unEstadoNovedad;
            UnCodMovimiento = unCodMovimiento;
            UnConceptoLiquidacion = unConceptoLiquidacion;
            UnTipoConcepto = unTipoConcepto;
            UnModeloPC = unModeloPC;
            UnAuditoria = unAuditoria;
            EstadosNovedad = new List<EstadoNovedad>();
            CancelacionAnticipada = new List<CancelacionAnticipada>();
            SiniestroCobrado = new List<SiniestroCobrado>();
        }

        public Novedad(long idNovedad,
                       double importeTotal,
                       double montoPrestamo,
                       Byte cantidadCuotas,
                       Byte cantCuotasSinLiquidar,
                       String proximoMensualAliq,
                       String nroTarjeta,
                       Beneficiario unBeneficiario,
                       Prestador unPrestador,
                       Estado unEstadoReg,
                       ConceptoLiquidacion unConceptoLiquidacion)
        {
            this.IdNovedad = idNovedad;
            this.ImporteTotal = importeTotal;
            this.MontoPrestamo = montoPrestamo;
            this.CantidadCuotas = cantidadCuotas;
            this.CantCuotasSinLiquidar = cantCuotasSinLiquidar;
            this.ProximoMensualAliq = proximoMensualAliq;
            this.Nro_Tarjeta = nroTarjeta;
            this.unBeneficiario = unBeneficiario;
            this.unPrestador = unPrestador;
            this.UnEstadoReg = unEstadoReg;
            this.unConceptoLiquidacion = unConceptoLiquidacion;
            EstadosNovedad = new List<EstadoNovedad>();
            CancelacionAnticipada = new List<CancelacionAnticipada>();
            SiniestroCobrado = new List<SiniestroCobrado>();
        }
        
        #region Errores de Clase
        public class NovedadException : System.ApplicationException
        {
            public NovedadException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion

    }

    [Serializable]
    public class Novedad_Info : Novedad
    {
        public String RazonSocial { get; set; }
        public String DescripcionEstadoReg { get; set; }
        public Int32 CodMovimiento { get; set; }
        public String DescMovimiento { get; set; }
        public Int32 CodConceptoLiq { get; set; }
        public String DescConceptoLiq { get; set; }
        public TipoConcepto TipoConcepto { get; set; }
        public Int32 CantCuotas { get; set; }
        public String NroComprobante { get; set; }
        public String UsuarioAlta { get; set; }
        public Double? CuotaTotalMensual { get; set; }
        public Double? GastoOtorgamiento { get; set; }
        public Double? GastoAdmMensual { get; set; }
        public Int32? IdItem { get; set; }
        public String DescripcionItem { get; set; }
        public String NroFactura { get; set; }
        public String Poliza { get; set; }
        public String NroSocio { get; set; }
        public String OtroServicioPrestado { get; set; }


        public Novedad_Info() { }
        public Novedad_Info(Int64 _IdNovedad,
                      String _RazonSocial,
                      DateTime _fechaNovedad,
                      byte _IdEstadoReg,
                      String _DescripcionEstadoReg,
                      Int32 _CodMovimiento,
                      String _DescMovimiento,
                      Int32 _CodConceptoLiq,
                      String _DescConceptoLiq,
                      TipoConcepto _TipoConcepto,
                      Double _ImporteTotal,
                      Int32 _CantCuotas,
                      Single _Porcentaje,
                      String _NroComprobante,
                      String _MAC,
                      String _UsuarioAlta,
                      String _PrimerMensual,
                      Int32? _MensualReenvio,
                      Double _MontoPrestamo,
                      Double? _CuotaTotalMensual,
                      Double _TNA,
                      Double _TEM,
                      Double? _GastoOtorgamiento,
                      Double? _GastoAdmMensual,
                      Double _CuotaSocial,
                      Double _CFTEA,
                      Double _CFTNAReal,
                      Double _GastoAdmMensualReal,
                      Int32? _IdItem,
                      String _DescripcionItem,
                      String _NroFactura,
                      String _Cbu,
                      String _Otro,
                      String _Poliza,
                      String _NroSocio,
                      String _OtroServicioPrestado,
                      String _NroSucursal,
                      String _NroTarjeta,
                      String _NroTicket)
        {
            IdNovedad = _IdNovedad;
            RazonSocial = _RazonSocial;
            FechaNovedad = _fechaNovedad;
            IdEstadoReg = _IdEstadoReg;
            DescripcionEstadoReg = _DescripcionEstadoReg;
            CodMovimiento = _CodMovimiento;
            DescMovimiento = _DescMovimiento;
            CodConceptoLiq = _CodConceptoLiq;
            DescConceptoLiq = _DescConceptoLiq;
            TipoConcepto = _TipoConcepto;
            ImporteTotal = _ImporteTotal;
            CantCuotas = _CantCuotas;
            Porcentaje = _Porcentaje;
            NroComprobante = _NroComprobante;
            MAC = _MAC;
            UsuarioAlta = _UsuarioAlta;
            PrimerMensual = _PrimerMensual;
            MensualReenvio = _MensualReenvio;
            MontoPrestamo = _MontoPrestamo;
            CuotaTotalMensual = _CuotaTotalMensual;
            TNA = _TNA;
            TEM = _TEM;
            GastoOtorgamiento = _GastoOtorgamiento;
            GastoAdmMensual = _GastoAdmMensual;
            Cuota_Social = _CuotaSocial;
            CFTEA = _CFTEA;
            CFTNAReal = _CFTNAReal;
            Gasto_AdmM_ensual_Real = _GastoAdmMensualReal;
            IdItem = _IdItem;
            DescripcionItem = _DescripcionItem;
            NroFactura = _NroFactura;
            CBU = _Cbu;
            Otro = _Otro;
            Poliza = _Poliza;
            NroSocio = _NroSocio;
            OtroServicioPrestado = _OtroServicioPrestado;
            Nro_Sucursal = _NroSucursal;
            Nro_Tarjeta = _NroTarjeta;
            Nro_Ticket = _NroTicket;

        }


    }

    [Serializable]
    public class Novedad_FGS : Novedad
    {
        public string Estado_Documentacion { get; set; }

        public Novedad_FGS() { }

    }

    [Serializable]
    public enum enum_TipoMovimientoTarShop
    {
        ALTA = 1,
        BAJA = 2,
        REEMPLAZO = 3,
        RECHAZO = 4,
        OTRO = 5
    }

    [Serializable]
    public class Novedad_Tarjeta_Reponer : Novedad
    {
        public enum_TipoMovimientoTarShop? MovimientoTarjeta { get; set; }
        public int? MotivoReemplazo { get; set; }
        public string Error_Reposicion { get; set; }

        public Novedad_Tarjeta_Reponer() { }
        public Novedad_Tarjeta_Reponer(Novedad _Novedad, enum_TipoMovimientoTarShop? _MovimientoTarjeta,
                                       int? _MotivoReemplazo, string _Error_Reposicion)
        {
            this.IdNovedad = _Novedad.IdNovedad;
            this.UnBeneficiario = _Novedad.UnBeneficiario;
            this.Nro_Tarjeta = _Novedad.Nro_Tarjeta;
            this.MovimientoTarjeta = _MovimientoTarjeta;
            this.MotivoReemplazo = _MotivoReemplazo;
            this.Error_Reposicion = _Error_Reposicion;
        }

    }

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


    [Serializable]
    public class Novedad_Afiliaciones
    {
        public Int64 IdPrestador { set; get; }
        public String RazonSocial { set; get; }
        public String CodSistema { set; get; }
        public ConceptoLiquidacion ConceptoLiquidacion { set; get; }
        public TipoConcepto TipoConcepto { set; get; }
        public decimal Importetotal { set; get; }
        public decimal Porcentaje { set; get; }
        public decimal MinPrimerMensual { set; get; }
        public decimal MaxPrimerMensual { set; get; }
        public Int32 Cantidad { set; get; }

        public Novedad_Afiliaciones()
        { }

        public Novedad_Afiliaciones(Int64 _IdPrestador, String _RazonSocial, String _CodSistema, ConceptoLiquidacion _ConceptoLiquidacion, TipoConcepto _TipoConcepto,
                                   decimal _Importetotal, decimal _Porcentaje, decimal _MinPrimerMensual, decimal _MaxPrimerMensual, Int32 _Cantidad)
        {
            IdPrestador = _IdPrestador;
            RazonSocial = _RazonSocial;
            CodSistema = _CodSistema;
            ConceptoLiquidacion = _ConceptoLiquidacion;
            TipoConcepto = _TipoConcepto;
            Importetotal = _Importetotal;
            Porcentaje = _Porcentaje;
            MinPrimerMensual = _MinPrimerMensual;
            MaxPrimerMensual = _MaxPrimerMensual;
            Cantidad = _Cantidad;
        }

    }

    [Serializable]
    public class NovedadesLiq_RepImp_Historico
    {
        public int PeriodoLiq { set; get; }
        public int MensualEmision { set; get; }
        public String TipoLiq { set; get; }
        public string IdentPago { get; set; }
        public string DescIdentPago { get; set; }
        public int? IdEstadoRub { get; set; }
        public string DaEstadoRub { get; set; }
        public string DescEstadoRub { get; set; }
        
        

        public NovedadesLiq_RepImp_Historico() { }

        public NovedadesLiq_RepImp_Historico(int periodoLiq, int mensualEmision, String tipoLiq, string identPago, string descIdentPago,
            int? idEstadoRub, string daEstadoRub, string descEstadoRub )
        {
            this.PeriodoLiq = periodoLiq;
            this.MensualEmision = mensualEmision;
            this.TipoLiq = tipoLiq;
            this.IdentPago = identPago;
            this.DescIdentPago = descIdentPago;
            this.IdEstadoRub = idEstadoRub;
            this.DaEstadoRub = daEstadoRub;
            this.DescEstadoRub = descEstadoRub;

        }
    }


    [Serializable]
    public class Novedades_Suspension
    {
        public long IdNovedad { set; get; }
        public long IdBeneficiario { set; get; }
        public DateTime FSuspension { set; get; }
        public DateTime? FReactivacion { set; get; }
        public String NroExpediente { set; get; }
        public String MotivoSuspension { get; set; }
        public Usuario UsuarioSuspension { get; set; }
        public Usuario UsuarioReactivacion { get; set; }
        public DateTime Fultmodificacion { get; set; }
        public int MensualSuspension { set; get; }
        public int MensualReactivacion { set; get; }
        public String MotivoReactivacion { get; set; }


        public Novedades_Suspension() { }

        public Novedades_Suspension(
                                    long idNovedad,
                                    long idBeneficiario,
                                    DateTime fSuspension,
                                    DateTime? fReactivacion,
                                    String nroExpediente,
                                    String motivoSuspension,
                                    Usuario usuarioSuspension,
                                    Usuario usuarioReactivacion,
                                    DateTime fultmodificacion,
                                    int mensualSuspension,
                                    int mensualReactivacion,
                                    String motivoReactivacion
                                    )
        {
            this.IdNovedad = IdNovedad;
            this.IdBeneficiario = idBeneficiario;
            this.FSuspension = fSuspension;
            this.FReactivacion = fReactivacion;
            this.NroExpediente = nroExpediente;
            this.MotivoSuspension = motivoSuspension;
            this.UsuarioSuspension = usuarioSuspension;
            this.UsuarioReactivacion = usuarioReactivacion;
            this.Fultmodificacion = fultmodificacion;
            this.MensualSuspension = mensualSuspension;
            this.MensualReactivacion = mensualReactivacion;
            this.MotivoReactivacion = motivoReactivacion;
        }
    }

    [Serializable]
    public class Novedades_CTACTE
    {
        public long? idnovedad { get; set; }
        public long? idBeneficiario { get; set; }
        public int? idEstadoSC { get; set; }
        public string DescripcionEstadoSC { get; set; }

        //Para evitar multiples output del servicio
        public string ApellidoNombre { get; set; }
        public string CuilRta { get; set; }

        public Novedades_CTACTE() { }

        public Novedades_CTACTE(long? _idnovedad, long? _idBeneficiario, int? _idEstadoSC, string _DescripcionEstadoSC)
        {
            idnovedad = _idnovedad;
            idBeneficiario = _idBeneficiario;
            idEstadoSC = _idEstadoSC;
            DescripcionEstadoSC = _DescripcionEstadoSC;
        }
    }

    [Serializable]
    public class Novedad_CBU
    {
        public long IdNovedad { get; set; }
        public Prestador UnPrestador { get; set; }
        public DateTime FechaNovedad { get; set; }
        public DateTime FechaInforme { get; set; }
        public string OficinaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public ConceptoLiquidacion UnConceptoLiquidacion { get; set; }
        public TipoConcepto UnTipoConcepto { get; set; }
        public double MontoPrestamo { get; set; }
        public Byte CantidadCuotas { get; set; }
        public string NombreArchivoRtaTS { get; set; }
        public DateTime FechaRechazo { get; set; }
        public string MensajeTS { get; set; }
        public Beneficiario_Reducido UnBeneficiario { get; set; }
        public Boolean Contactado { get; set; }
        public Boolean TieneHistorico { get; set; }

        public Novedad_CBU() { }

        public Novedad_CBU(long _IdNovedad, DateTime _FechaNovedad, DateTime _FechaInforme, string _OficinaAlta, string _UsuarioAlta,
                           double _MontoPrestamo, Byte _CantidadCuotas, string _NombreArchivoRtaTS, DateTime _FechaRechazo,
                           string _MensajeTS, Boolean _Contactado, Boolean _TieneHistorico)
        {
            IdNovedad = _IdNovedad;           
            FechaNovedad = _FechaNovedad;
            FechaInforme = _FechaInforme;
            OficinaAlta = _OficinaAlta;
            UsuarioAlta = _UsuarioAlta;
            MontoPrestamo = _MontoPrestamo;           
            CantidadCuotas = _CantidadCuotas;
            NombreArchivoRtaTS = _NombreArchivoRtaTS;
            FechaRechazo = _FechaRechazo;
            MensajeTS = _MensajeTS;
            Contactado = _Contactado;
            TieneHistorico = _TieneHistorico;
        }
    }

    [Serializable]
    public class NovedadRechazada
    {
        public long Idnovedad { get; set; }
        public DateTime FechaContacto { get; set; }
        public string Observaciones { get; set; }
        public Boolean ContactoSatisfactorio { get; set; }
        public string Usuario { get; set; }
        public string OficinaContacto { get; set; }
        public string Ip { get; set; }

        public NovedadRechazada() { }
    }

    [Serializable]
    public class NovedadNoInformadaXBanco{
       
        public DateTime FechaInforme { get; set; }
        public Int32 CantidadCierre { get; set; }
        public Int32 CantidadSinInformar { get; set; }
        public Int32 CantidadInfOk { get; set; }
        public Int32 CantidadInfRechazos { get; set; }

        public NovedadNoInformadaXBanco() { }

        public NovedadNoInformadaXBanco(DateTime _FechaInforme, Int32 _CantidadCierre, Int32 _CantidadSinInformar, Int32 _CantidadInfOk, Int32 _CantidadInfRechazos) 
        {
            FechaInforme = _FechaInforme;
            CantidadCierre = _CantidadCierre;
            CantidadSinInformar = _CantidadSinInformar;
            CantidadInfOk = _CantidadInfOk;
            CantidadInfRechazos = _CantidadInfRechazos;
        }
    }

    [Serializable]
    public class NovedadMontoPrestamoTotal
    {     
        public double ImportePrimerCuota { get; set; }
        public double MontoPrestamo { get; set; }
        public  Byte  CantidadCuotas { get; set; }
        public string Mensaje { get; set; }

        public NovedadMontoPrestamoTotal() { }

        public NovedadMontoPrestamoTotal(double _ImportePrimerCuota, double _MontoPrestamo, Byte  _CantidadCuotas, string _Mensaje)
        {
            ImportePrimerCuota = _ImportePrimerCuota;
            MontoPrestamo = _MontoPrestamo;
            CantidadCuotas = _CantidadCuotas;
            Mensaje = _Mensaje;
        }
    }   
}
