using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.Embozo.Dominio
{
    [Serializable]
    public class TipoError
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }

       #region constructores

        public TipoError() {
            Codigo = 0;
        }

        public TipoError(int _Codigo, string _Mensaje)
        {
            Codigo  = _Codigo;
            Mensaje = _Mensaje;
        }
        #endregion
    }
}
