using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class MarcaPC :IDisposable
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

        ~MarcaPC()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private int idMarca;
        private String descripcion;
        private Auditoria unAuditoria;

        #region Getters y Setters
        public int IdMarca
        {
            get { return idMarca; }
            set { idMarca = value; }
        }
      

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
       

        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }


        #endregion

        public MarcaPC ()
        {
            IdMarca = 0;
            Descripcion = string.Empty;
            UnAuditoria = new Auditoria();
        }

        public MarcaPC( int idMarca, String descripcion) {
            this.IdMarca = idMarca;
            this.Descripcion = descripcion;
            this.unAuditoria = new Auditoria();
        }


        #region Errores de Clase
        public class MarcaPCException : System.ApplicationException
        {
            public MarcaPCException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
 
}
