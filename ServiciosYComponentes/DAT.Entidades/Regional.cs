using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Regional : IDisposable
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

        ~Regional()
        {           
            Dispose(true);
        }
        #endregion
        
        public Int32 IdRegional { get; set; }
        public string Descripcion { get; set; }
        public List<UDAI> Udais { get; set; }

        public Regional() { }
        public Regional(Int32  _IdRegional,string  _Descripcion) {
           IdRegional = _IdRegional;
           Descripcion = _Descripcion;               
        }

        public Regional(Int32  _IdRegional,string  _Descripcion, List<UDAI> _Udais) {
           IdRegional = _IdRegional;
           Descripcion = _Descripcion;    
           Udais = _Udais;
        }
    }
}
