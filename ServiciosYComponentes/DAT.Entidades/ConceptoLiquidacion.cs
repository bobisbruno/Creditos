using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class ConceptoLiquidacion
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

        ~ConceptoLiquidacion()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Added Definitions
                
        private int codConceptoLiq;
        private String descConceptoLiq;
        private Byte prioridad;
        private int? codSidif;
        private Boolean obligatorio;
        private Boolean esAfiliacion;
        private String codSistema;
        private Boolean habilitadoOnLine;
        private Boolean habilitado;
                
        private TipoConcepto unTipoConcepto;
        private Auditoria unAuditoria;
        private DateTime? fechaInicio;
        private DateTime? fechaFin;
        private decimal? maxADescontar;
        private TipoOrigenBeneficiario tipoOrigenBeneficiario;
        private int idPrestacionTurno;

        #endregion

        #region getter and setters
     
        public int CodConceptoLiq
        {
            get { return codConceptoLiq; }
            set { codConceptoLiq = value; }
        }

        public String DescConceptoLiq
        {
            get { return descConceptoLiq; }
            set { descConceptoLiq = value; }
        }

        public Byte Prioridad
        {
            get { return prioridad; }
            set { prioridad = value; }
        }

        public int? CodSidif
        {
            get { return codSidif; }
            set { codSidif = value; }
        }

        public Boolean Obligatorio
        {
            get { return obligatorio; }
            set { obligatorio = value; }
        }

        public Boolean EsAfiliacion
        {
            get { return esAfiliacion; }
            set { esAfiliacion = value; }
        }

        public String CodSistema
        {
            get { return codSistema; }
            set { codSistema = value; }
        }

        public Boolean HabilitadoOnLine
        {
            get { return habilitadoOnLine; }
            set { habilitadoOnLine = value; }
        }

        public Boolean Habilitado
        {
            get { return habilitado; }
            set { habilitado = value; }
        }

        //public Prestador UnPrestador
        //{
        //    get { return unPrestador; }
        //    set { unPrestador = value; }
        //}

        public TipoConcepto UnTipoConcepto
        {
            get { return unTipoConcepto; }
            set { unTipoConcepto = value; }
        }

        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }
              
        public DateTime? FechaInicio
        {
            get { return fechaInicio; }
            set { fechaInicio = value; }
        }

        public DateTime? FechaFin
        {
            get { return fechaFin; }
            set { fechaFin = value; }
        }

        public decimal? MaxADescontar
        {
            get { return maxADescontar; }
            set { maxADescontar = value; }
        }

        public TipoOrigenBeneficiario TipoOrigenBeneficiario
        {
            get { return tipoOrigenBeneficiario; }
            set { tipoOrigenBeneficiario = value; }
        }

        public int IdPrestacionTurno
        {
            get { return idPrestacionTurno; }
            set { idPrestacionTurno = value; }
        }
        #endregion


        public Boolean EsInundado { get; set; }
        public Boolean EsConceptoAjuste { get; set; }
        public int AjustaSobreConcepto { get; set; }
        public Boolean CodConceptoAjusteResta { get; set; }
        public Boolean EsConceptoRecupero { get; set; }
        public int RecuperaSobreConcepto { get; set; }
        public Boolean HabilitadoPNC { get; set; }
        public Boolean RequiereCBU { get; set; }
        public Boolean Hab_Online { get; set; }


        public ConceptoLiquidacion() 
        {            
            CodConceptoLiq = 0;
            DescConceptoLiq = string.Empty;
            Prioridad = 0;
            CodSidif = null;
            Obligatorio = false;
            EsAfiliacion = false;
            CodSistema = string.Empty;
            HabilitadoOnLine = false;
            Habilitado = false;
            EsInundado = false;
            EsConceptoAjuste = false;
            AjustaSobreConcepto = 0;
            CodConceptoAjusteResta = false;
            EsConceptoRecupero = false;
            RecuperaSobreConcepto = 0;
            HabilitadoPNC = false;
            RequiereCBU = false;
            //UnPrestador = new Prestador();
            UnTipoConcepto = new TipoConcepto();
            UnAuditoria = new Auditoria(); 
        }       

        public ConceptoLiquidacion(int codConceptoLiq, String descConceptoLiq)
        {
            this.CodConceptoLiq = codConceptoLiq;
            this.DescConceptoLiq = descConceptoLiq;
        }

        public ConceptoLiquidacion(int codConceptoLiq, String descConceptoLiq, bool esAfiliacion)
        {
            this.CodConceptoLiq = codConceptoLiq;
            this.DescConceptoLiq = descConceptoLiq;
            this.EsAfiliacion = esAfiliacion;
        }


        public ConceptoLiquidacion(int codConceptoLiq, String descConceptoLiq, TipoConcepto unTipoConcepto)
        {
            this.CodConceptoLiq = codConceptoLiq;
            this.DescConceptoLiq = descConceptoLiq;
            this.UnTipoConcepto = unTipoConcepto; 
        }

        public ConceptoLiquidacion(int codConceptoLiq, String descConceptoLiq, 
                                   Byte prioridad, int? codSidif, Boolean obligatorio,
                                   Boolean esAfiliacion, String codSistema,
                                   Boolean habOnline, Boolean habilitado, TipoConcepto unTipoConcepto,
                                   DateTime? fechaIncio, DateTime? fechaFin, decimal? maxADescontar, 
                                   TipoOrigenBeneficiario tipoOrigenBeneficiario, int idPrestacionTurno)
        {
            CodConceptoLiq = codConceptoLiq;
            DescConceptoLiq = descConceptoLiq;
            Prioridad = prioridad;
            CodSidif = codSidif ;
            Obligatorio = obligatorio;
            EsAfiliacion = esAfiliacion;
            CodSistema = codSistema;
            HabilitadoOnLine = habOnline;
            Habilitado = Habilitado;
            UnTipoConcepto = unTipoConcepto;
            FechaInicio = fechaIncio;
            FechaFin = fechaFin;
            MaxADescontar = maxADescontar;
            TipoOrigenBeneficiario = tipoOrigenBeneficiario;
            IdPrestacionTurno = idPrestacionTurno;
        }

        public ConceptoLiquidacion(int codConceptoLiq, String descConceptoLiq,
                                  Byte prioridad, int? codSidif, Boolean obligatorio,
                                  Boolean esAfiliacion, String codSistema,
                                  Boolean habOnline, Boolean habilitado, TipoConcepto unTipoConcepto)
        {
            CodConceptoLiq = codConceptoLiq;
            DescConceptoLiq = descConceptoLiq;
            Prioridad = prioridad;
            CodSidif = codSidif;
            Obligatorio = obligatorio;
            EsAfiliacion = esAfiliacion;
            CodSistema = codSistema;
            HabilitadoOnLine = habOnline;
            Habilitado = Habilitado;
            UnTipoConcepto = unTipoConcepto;            
        }

        public ConceptoLiquidacion(int codConceptoLiq, String descConceptoLiq, Prestador unPrestador, Boolean _esAfiliacion, Boolean _esInundado
                                   , Boolean _esConceptoAjuste, int _ajustaSobreConcepto, Boolean _codConceptoAjusteResta, Boolean _esConceptoRecupero,
                                   int _recuperaSobreConcepto, Boolean _habilitadoPNC, Boolean _requiereCBU, Boolean _hab_Online, Boolean _habilitado                                   
                                  )
        {
            this.CodConceptoLiq = codConceptoLiq;
            this.DescConceptoLiq = descConceptoLiq;
            //this.unPrestador = unPrestador;
            this.EsAfiliacion = _esAfiliacion;
            this.EsInundado = _esInundado;
            this.EsConceptoAjuste = _esConceptoAjuste;
            this.AjustaSobreConcepto = _ajustaSobreConcepto;
            this.CodConceptoAjusteResta = _codConceptoAjusteResta;
            this.EsConceptoRecupero = _esConceptoRecupero;
            this.RecuperaSobreConcepto = _recuperaSobreConcepto;
            this.HabilitadoPNC = _habilitadoPNC;
            this.RequiereCBU = _requiereCBU;
            this.HabilitadoOnLine = _hab_Online;
            this.Habilitado = _habilitado;
        }
                    

        #region Errores de Clase
        public class ConceptoLiquidacionException : System.ApplicationException
        {
            public ConceptoLiquidacionException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}
