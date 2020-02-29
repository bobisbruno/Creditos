using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Cierre
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

        ~Cierre()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private String fecCierre = String.Empty;
        private String mensual = String.Empty;
        private DateTime? fecProceso ;
        private DateTime? fecAplicacionPagos;             

        #region Getters y Setters
        public String FecCierre
        {
            get { return fecCierre; }
            set { fecCierre = value; }
        }

        public String Mensual
        {
            get { return mensual; }
            set { mensual = value; }
        }

        public DateTime? FecProceso
        {
            get { return fecProceso; }
            set { fecProceso = value; }
        }

        public DateTime? FecAplicacionPagos
        {
            get { return fecAplicacionPagos; }
            set { fecAplicacionPagos = value; }
        }

        #endregion

        public Cierre()
        {
            FecCierre = String.Empty;
            Mensual = String.Empty;
            FecProceso = new DateTime?();
            FecAplicacionPagos = new DateTime?();
        }

        public Cierre(String fecCierre, String mensual)
        {
            this.fecCierre = fecCierre;
            this.mensual = mensual;
        
        }

        public Cierre(String fecCierre, String mensual, DateTime? fecAplicacionPAgos)
        {
            this.fecCierre = fecCierre;
            this.mensual = mensual;            
            this.fecAplicacionPagos = fecAplicacionPAgos;
        }

        public Cierre(String fecCierre, String mensual, DateTime? fecProceso, DateTime? fecAplicacionPAgos)
        {
            this.fecCierre = fecCierre;
            this.mensual = mensual;
            this.fecProceso = fecProceso;
            this.fecAplicacionPagos = fecAplicacionPAgos;
        }

        #region Errores de Clase
        public class CierreException : System.ApplicationException
        {
            public CierreException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}