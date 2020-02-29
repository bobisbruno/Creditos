using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.Embozo.Dominio
{
    [Serializable]
    public class Persona
    {
        public long Cuil { get; set; }
        public int TipoDoc { get; set; }
        public string NroDoc { get; set; }
        public string ApellidoNombre { get; set; }
        public string Sexo { get; set; }
        public Domicilio Domicilio { get; set; }
        
       #region constructores

        public Persona() {}

        public Persona(long _Cuil, int _TipoDoc, string _NroDoc, string _ApellidoNombre, string _Sexo, Domicilio _Domicilio)
        {
            Cuil = _Cuil;
            TipoDoc = _TipoDoc;
            NroDoc = _NroDoc;
            ApellidoNombre = _ApellidoNombre;
            Sexo = _Sexo;
            Domicilio = _Domicilio;
        }

        public Persona(long _Cuil, string _ApellidoNombre)
        {
            Cuil = _Cuil;          
            ApellidoNombre = _ApellidoNombre;         
        }

        #endregion
    }
}
