using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Prestador : Entidad_Prest_Comer ,IDisposable
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

        ~Prestador()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private string email;
        private int codoficinaanme;
        private string cbu;
        private string codsistema;
        private ConceptoLiquidacion unConceptoLiquidacion;
        private List<Comercializador> comercializadoras;
        private List<ConceptoLiquidacion> unaListaConceptoLiquidacion;

        //private List<TipoConcepto> unaListaTipoConcepto;

        #region Getters y Setters

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public int CodoFicinaANME
        {
            get { return codoficinaanme; }
            set { codoficinaanme = value; }
        }
        public string CBU
        {
            get { return cbu; }
            set { cbu = value; }
        }
        public string CodSistema
        {
            get { return codsistema; }
            set { codsistema = value; }
        }

        public ConceptoLiquidacion UnConceptoLiquidacion
        {
            get { return unConceptoLiquidacion; }
            set { unConceptoLiquidacion = value; }
        }
        public List<Comercializador> Comercializadoras
        {
            get { return comercializadoras; }
            set { comercializadoras = value; }
        }
        public Comercializador[] ArrayComercializadoras
        {
            get
            { //return comercializadoras.ToArray(); codigo anterior generaba error mod sdr 30/3/11
                return comercializadoras == null ? null : comercializadoras.ToArray();
            }
            set { comercializadoras = new List<Comercializador>(value); }
        }
        
        public List<ConceptoLiquidacion> UnaListaConceptoLiquidacion
        {
            get { return unaListaConceptoLiquidacion; }
            set { unaListaConceptoLiquidacion = value; }
        }

        public bool EsNominada { get; set; }
        public bool EsAnses { get; set; }
        public bool EntregaDocumentacionEnFGS { get; set; }
        public bool EsEntidadOficial { get; set; }
        public bool EsComercio { get; set; }

        #endregion Getters y Setters

        #region Constructores

        public Prestador()
        {
            Email = string.Empty;
            CodoFicinaANME = 0;
            CBU = string.Empty;
            CodSistema = string.Empty;
            UnConceptoLiquidacion = new ConceptoLiquidacion();
            Comercializadoras = new List<Comercializador>();
            //UnaListaTipoConcepto = new List<TipoConcepto>();
            UnaListaConceptoLiquidacion = new List<ConceptoLiquidacion>();

            base.Cuit = 0;
            base.FechaInicio = new DateTime();
            base.ID = 0;
            base.IDEstado = 0;
            base.NombreFantasia = string.Empty;
            base.Observaciones = string.Empty;
            base.Tasas = new List<Tasa>();
            base.UnAuditoria = new Auditoria();
            base.UnDomicilio = new Domicilio();
            base.UnEstado = new Estado();
        }

        public Prestador(long id, string RazonSocial, long Cuit, ConceptoLiquidacion unConceptoLiquidacion)
        {
            base.ID = id;
            base.Cuit = Cuit;
            base.RazonSocial = RazonSocial;
            this.UnConceptoLiquidacion = unConceptoLiquidacion;
            this.comercializadoras = new List<Comercializador>();
        }

        public Prestador(long id, string RazonSocial, long Cuit)
        {
            base.ID = id;
            base.Cuit = Cuit;
            base.RazonSocial = RazonSocial;
            this.comercializadoras = new List<Comercializador>();            
         }

        public Prestador(long id, string RazonSocial, long Cuit, bool entregaDocumentacionEnFGS)
        {
            base.ID = id;
            base.Cuit = Cuit;
            base.RazonSocial = RazonSocial;
            this.comercializadoras = new List<Comercializador>();
            this.EntregaDocumentacionEnFGS = entregaDocumentacionEnFGS;
        }

        public Prestador(string codSistema, long id, string razonSocial,
                         long cuit, string email, int codOficinaANME,
                         string observaciones, Estado unEstado,
                         List<ConceptoLiquidacion> unaListaConceptoLiquidacion,
                         bool esNominada, bool esAnses, bool entregaDocumentacionEnFGS,bool esEntidadOficial)
        {
            this.CodSistema = codSistema;
            base.ID = id;
            base.RazonSocial = razonSocial;
            base.Cuit = cuit;
            this.Email = email;
            this.CodoFicinaANME = codOficinaANME;
            base.Observaciones = observaciones;

            base.UnEstado = unEstado;
            this.UnaListaConceptoLiquidacion = unaListaConceptoLiquidacion;
            this.EsAnses = esAnses;
            this.EntregaDocumentacionEnFGS = entregaDocumentacionEnFGS;
            this.EsNominada = esNominada;
            this.EsEntidadOficial = EsEntidadOficial;
            
        }

        public Prestador(long id, string razonSocial, long cuit, string email, int codOficinaANME,
                         string observaciones, Estado unEstado, Auditoria unAuditoria,
                         string nombreFantasia, string cbu, DateTime operaDesde, string codSistema, bool esNominada,
                         bool esAnses, bool entregaDocumentacionEnFGS)
        {
            base.ID = id;
            base.RazonSocial = razonSocial;
            base.Cuit = cuit;
            this.Email = email;
            this.CodoFicinaANME = codOficinaANME;
            base.Observaciones = observaciones;
            base.NombreFantasia = nombreFantasia;
            this.CBU = cbu;
            base.FechaInicio = operaDesde;
            this.CodSistema = codSistema;
            base.UnEstado = unEstado;
            base.UnAuditoria = unAuditoria;
            this.EsNominada = esNominada;
            //base.UnAuditoria.FecUltimaModificacion = FecultModificacion;
            this.comercializadoras = new List<Comercializador>();
            base.Tasas = new List<Tasa>();
            Comercializador[] ArrayComercializadoras = new List<Comercializador>().ToArray();
            this.EsAnses = esAnses;
            this.EntregaDocumentacionEnFGS = entregaDocumentacionEnFGS;
            
        }

        public Prestador(long id, string razonSocial, long cuit, string email, int codOficinaANME,
                        string observaciones, Estado unEstado, Auditoria unAuditoria,
                        string nombreFantasia, string cbu, DateTime operaDesde, string codSistema, bool esNominada,
                        bool esAnses, bool entregaDocumentacionEnFGS, bool esComercio)
        {
            base.ID = id;
            base.RazonSocial = razonSocial;
            base.Cuit = cuit;
            this.Email = email;
            this.CodoFicinaANME = codOficinaANME;
            base.Observaciones = observaciones;
            base.NombreFantasia = nombreFantasia;
            this.CBU = cbu;
            base.FechaInicio = operaDesde;
            this.CodSistema = codSistema;
            base.UnEstado = unEstado;
            base.UnAuditoria = unAuditoria;
            this.EsNominada = esNominada;
            //base.UnAuditoria.FecUltimaModificacion = FecultModificacion;
            this.comercializadoras = new List<Comercializador>();
            base.Tasas = new List<Tasa>();
            Comercializador[] ArrayComercializadoras = new List<Comercializador>().ToArray();
            this.EsAnses = esAnses;
            this.EntregaDocumentacionEnFGS = entregaDocumentacionEnFGS;
            this.EsComercio = esComercio;
        }


        public Prestador(long id)
        {
            this.ID = id;
        }
        #endregion Constructores

        #region Errores de Clase
        public class PrestadorException : System.ApplicationException
        {
            public PrestadorException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}
