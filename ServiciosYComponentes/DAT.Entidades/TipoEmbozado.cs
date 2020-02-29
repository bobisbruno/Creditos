using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public enum enum_TipoEmbozado
    {
        EXPRESS = 1,
        SEMIEXPRESS = 2,
    }

    [Serializable]
    public class TipoEmbozado : IDisposable 
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

        ~TipoEmbozado()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Added Definitions

        public enum_TipoEmbozado IdTipoEmbozado { get; set; }
        public string DescTipoEmbozado;
        public int    CantDiasEntrega;   

        #endregion
      
        public TipoEmbozado() {}

        public TipoEmbozado(enum_TipoEmbozado _IdTipoEmbozado, string _DescTipoEmbozado, int _CantDiasEntrega)
        {
            IdTipoEmbozado = _IdTipoEmbozado;
            DescTipoEmbozado = _DescTipoEmbozado;
            CantDiasEntrega  = _CantDiasEntrega;
        }  
    }
}
