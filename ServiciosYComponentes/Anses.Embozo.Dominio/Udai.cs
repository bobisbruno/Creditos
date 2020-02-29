using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.Embozo.Dominio
{
    [Serializable]
    public class Udai
    {
        public int Codigo { get; set; }
        public string Domicilio { get; set; }
        public string CodPostal { get; set; }
        public string Usuario { get; set; }
              
       #region constructores

        public Udai() {}

        public Udai(int _Codigo, string _Domicilio, string _CodPostal, string _Usuario)
        {
            Codigo = _Codigo;
            Domicilio = _Domicilio;
            CodPostal = _CodPostal;
            Usuario = _Usuario;
        }

        #endregion
    }
}
