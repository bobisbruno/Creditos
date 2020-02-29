using System;
using System.Collections.Generic;
using System.Text;


namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class ContactoReclamo : ValidaEntidad<ContactoReclamo>, IDisposable 
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

        ~ContactoReclamo()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Added Definitions

        private string  cuil;
        private string telediscado;
        private string telefono;
        private bool celular; 
        private string mail;
        private Auditoria unaAuditoria;

        #endregion

        public ContactoReclamo() 
        { }

        public string Cuil
        {
            get {return this.cuil; }
            set { this.cuil = value; }
        }
        public string Telediscado
        { 
            get{return this.telediscado;}
            set {this.telediscado=value;}

        }
        public string Telefono
        {
            get { return this.telefono; }
            set { this.telefono = value; }
        }
        public bool Celular
        {
            get { return this.celular; }
            set { this.celular = value; }
        }
        public string Mail
        {
            get { return this.mail; }
            set { this.mail = value; }
        }

        public Auditoria UnaAuditoria
        {
            get { return this.unaAuditoria; }
            set { this.unaAuditoria = value; }
        }


    }
}
