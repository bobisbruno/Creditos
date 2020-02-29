using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class CodigoMovimiento
    {
        private Byte codMovimiento;
        private String descripcion;
        private Boolean  habilitado = true;
        private Auditoria unAuditoria;

        #region Get/Set

        public Byte CodMovimiento
        {
            get { return this.codMovimiento; }
            set { this.codMovimiento = value; }
        }

        public string DescTipoConcepto
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
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

        #endregion

        public CodigoMovimiento()
        {
            CodMovimiento = 0;
            DescTipoConcepto = string.Empty;
            Habilitado = false;
            UnAuditoria = new Auditoria();  
        }

        public CodigoMovimiento(Byte codMovimiento, String descripcion)
        {
            this.codMovimiento = codMovimiento;
            this.descripcion = descripcion;
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

        ~CodigoMovimiento()
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
