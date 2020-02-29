using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.Embozo.Dominio
{
    [Serializable]
    public class Embozado
    {
        public int IDPlastico { get; set; }
        public int NroPlastico { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string CVV2 { get; set; }
        public string Sticker { get; set; }
        public string Vigencia { get; set; }

       #region constructores

        public Embozado() {}

        public Embozado(int _IDPlastico, int _NroPlastico, string _Track1,string _Track2, string _CVV2, string _Sticker, string _Vigencia )
        {
            IDPlastico = _IDPlastico;
            NroPlastico = _NroPlastico;
            Track1 = _Track1;
            Track2 = _Track2;
            CVV2 = _CVV2;
            Sticker = _Sticker;
            Vigencia = _Vigencia;
        }

        #endregion
    }
}
