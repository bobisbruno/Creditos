using System;
using System.Collections.Generic;
using System.Text;


namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Reclamo: ValidaEntidad<Reclamo> , IDisposable  
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

        ~Reclamo()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Added Definitions

        private long idReclamo;
        private DateTime fechaAltaReclamo;
        private TipoReclamo unTipoReclamo;
        private Novedad unaNovedad;
        private ContactoReclamo unContactoReclamo;        
        private string descripcionReclamo;                
        private string expediente;
        private EstadoReclamo unEstadoReclamo;       
        private Auditoria unaAuditoria;
        private Auditoria unaAuditoriaRespuesta;
        private bool presentoDoc;
        private bool solicitaReintegro;
        

        #endregion

        #region Private Get/Set
        public long IdReclamo
        {
            get { return this.idReclamo; }
            set { this.idReclamo = value; }
        }

        public bool SolicitaReintegro
        {
            get { return this.solicitaReintegro; }
            set { this.solicitaReintegro=value;}
        }

        public Novedad UnaNovedad
        {
            get { return this.unaNovedad; }
            set { this.unaNovedad = value; }
        }

        public DateTime FechaAltaReclamo
        {
            get { return this.fechaAltaReclamo; }
            set { this.fechaAltaReclamo = value; }
        }

     
        public string DescripcionReclamo
        {
            get { return this.descripcionReclamo; }
            set { this.descripcionReclamo = value; }
        }

        public string Expediente
        {
            get { return this.expediente; }
            set { this.expediente = value; }
        }

        public TipoReclamo UnTipoReclamo
        {
            get { return this.unTipoReclamo; }
            set { this.unTipoReclamo = value; }
        }


        public ContactoReclamo UnContactoReclamo
        {
            get { return unContactoReclamo; }
            set { unContactoReclamo = value; }
        }
     
        public bool PresentoDoc
        {
            get { return this.presentoDoc; }
            set { this.presentoDoc = value; }
        }
        public EstadoReclamo UnEstadoReclamo
        {
            get { return unEstadoReclamo; }
            set { unEstadoReclamo = value; }
        }   
        public Auditoria UnaAuditoria
        {
            get { return this.unaAuditoria; }
            set { this.unaAuditoria = value; }
        }

        public Auditoria UnaAuditoriaRespuesta
        {
            get { return this.unaAuditoriaRespuesta; }
            set { this.unaAuditoriaRespuesta = value; }
        }

        #endregion

        public Reclamo() { }

    }
}
