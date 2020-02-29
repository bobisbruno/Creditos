using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class ModeloPC : IDisposable
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

        ~ModeloPC()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion


        private int idModeloPC;
        private MarcaPC unaMarcaPC;        
        private String descripcion;
        private DateTime fechaInicio;
        private DateTime? fechaFin;
        private Auditoria unAuditoria;

        #region Getters y Setters
        public int IdModeloPC
        {
            get { return idModeloPC; }
            set { idModeloPC = value; }
        }
        public MarcaPC UnaMarcaPC
        {
            get { return unaMarcaPC; }
            set { unaMarcaPC = value; }
        }

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public DateTime FechaInicio
        {
            get { return fechaInicio; }
            set { fechaInicio = value; }
        }


        public DateTime? FechaFin
        {
            get { return fechaFin; }
            set { fechaFin = value; }
        }


        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }

        #endregion

        public ModeloPC() 
        {
            IdModeloPC = 0;
            UnaMarcaPC = new MarcaPC();
            Descripcion = string.Empty;
            FechaInicio = new DateTime();
            FechaFin = new DateTime();
            UnAuditoria = new Auditoria();
        }

        public ModeloPC(int idModeloPC, String descripcion, DateTime fechaInicio)
        {
            this.IdModeloPC = idModeloPC;
            this.Descripcion = descripcion;
            this.fechaInicio = fechaInicio;
            this.unaMarcaPC = new MarcaPC();            
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
