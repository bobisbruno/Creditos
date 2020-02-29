using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.Embozo.Dominio
{
    [Serializable]
    public class Usuario
    {
        public string CodigoUsuario { get; set; }
        public string Oficina { get; set; }
        public string IP { get; set; }

       #region constructores

        public Usuario() {}

        public Usuario(string _CodigoUsuario, string _Oficina, string _IP)
        {
            CodigoUsuario = _CodigoUsuario;
            Oficina = _Oficina;
            IP = _IP;
        }

        #endregion
    }
}
