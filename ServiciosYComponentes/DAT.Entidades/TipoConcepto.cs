using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{    
    [Serializable]
    public class TipoConcepto : IDisposable 
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

        ~TipoConcepto()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Added Definitions

        private short idTipoConcepto;
        private string descTipoConcepto;
        private Boolean  habilitado = true;
        private Auditoria unAuditoria;
        //private ConceptoLiquidacion unConceptoLiquidacion;
        private List<ConceptoLiquidacion> unaListaConceptoLiquidacion; 

        #endregion

        #region Private Get/Set

        public short IdTipoConcepto
        {
            get { return this.idTipoConcepto; }
            set { this.idTipoConcepto = value; }
        }

        public string DescTipoConcepto
        {
            get { return this.descTipoConcepto; }
            set { this.descTipoConcepto = value; }
        }

        public Boolean Habilitado
        {
            get { return this.habilitado; }
            set { this.habilitado = value; }
        }

        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }
        //public ConceptoLiquidacion UnConceptoLiquidacion
        //{
        //    get { return unConceptoLiquidacion; }
        //    set { unConceptoLiquidacion = value; }
        //}
        public List<ConceptoLiquidacion> UnaListaConceptoLiquidacion
        {
            get { return unaListaConceptoLiquidacion; }
            set { unaListaConceptoLiquidacion = value; }
        }
        #endregion

        public TipoConcepto()
        {
            IdTipoConcepto = 0;
            DescTipoConcepto = string.Empty;
            Habilitado = false;
            UnAuditoria = new Auditoria();
            //UnConceptoLiquidacion = new ConceptoLiquidacion();
            UnaListaConceptoLiquidacion = new List<ConceptoLiquidacion>();
        }
        
        public TipoConcepto(short idTipoConcepto, string descTipoConcepto)
        {
            this.IdTipoConcepto = idTipoConcepto;
            this.DescTipoConcepto = descTipoConcepto;
        }

        public TipoConcepto(short idTipoConcepto, string descTipoConcepto, bool habilitado)
        {
            this.IdTipoConcepto = idTipoConcepto;
            this.DescTipoConcepto = descTipoConcepto;
            this.Habilitado = habilitado;
            //UnConceptoLiquidacion = unConceptoLiquidacion;
        }

        public TipoConcepto(short idTipoConcepto, string descTipoConcepto, bool habilitado, List<ConceptoLiquidacion> unaListaConceptoLiquidacion)
        {
            this.IdTipoConcepto = idTipoConcepto;
            this.DescTipoConcepto = descTipoConcepto;
            this.Habilitado = habilitado;
            UnaListaConceptoLiquidacion  = unaListaConceptoLiquidacion;
        }

        #region Errores de Clase
        public class TipoConceptoException : System.ApplicationException
        {
            public TipoConceptoException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion

    }
}
