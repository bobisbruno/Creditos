using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoReclamo
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

        ~TipoReclamo()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
        private int idTipoReclamo;
        private String descripcion;
        private string leyenda;

        #region Private Get/Set
        public int IdTipoReclamo
        {
            get { return idTipoReclamo; }
            set { idTipoReclamo = value; }
        }
        
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        

        public string Leyenda
        {
            get { return leyenda; }
            set { leyenda = value; }
        }

        #endregion

        public TipoReclamo() { }

        public TipoReclamo(int idTipoReclamo,
            String descripcion) {
                this.idTipoReclamo = idTipoReclamo;
                this.descripcion = descripcion;
        }


    }
}
