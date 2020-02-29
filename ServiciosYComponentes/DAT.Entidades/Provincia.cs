using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Provincia
    {
        private Int16 codProvincia;      
        private String descripcionProvincia;
        
        #region Getters y Setters
        public Int16 CodProvincia
        {
            get { return codProvincia; }
            set { codProvincia = value; }
        }

        public String DescripcionProvincia
        {
            get { return descripcionProvincia; }
            set { descripcionProvincia = value; }
        }
        #endregion

        public Provincia() 
        {
            CodProvincia = 0;
            DescripcionProvincia = string.Empty;  
        }

        public Provincia(Int16 codProvincia, String descripcion)
        {
            this.codProvincia = codProvincia;
            this.descripcionProvincia = descripcion;
        }

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

        ~Provincia()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Errores de Clase
        public class AuditoriaException : System.ApplicationException
        {
            public AuditoriaException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}
