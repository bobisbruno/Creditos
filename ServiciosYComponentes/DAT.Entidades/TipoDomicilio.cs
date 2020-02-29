using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoDomicilio : IDisposable
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

        ~TipoDomicilio()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public TipoDomicilio() 
        {
            IdTipoDomicilio = 0;
            DescTipoDomicilio = string.Empty;  
        }

        public TipoDomicilio(Int16 idTipoDomicilio, string descTipoDomicilio)
        {
            IdTipoDomicilio = idTipoDomicilio;
            DescTipoDomicilio = descTipoDomicilio;
        }

        #region Added Definitions

        private Int16 idTipoDomicilio;
        private string descTipoDomicilio;
        #endregion

        #region Private Get/Set

        public Int16 IdTipoDomicilio
        {
            get { return idTipoDomicilio; }
            set { idTipoDomicilio = value; }
        }

        public string DescTipoDomicilio
        {
            get { return descTipoDomicilio; }
            set { descTipoDomicilio = value; }
        }
        #endregion
    }
}
