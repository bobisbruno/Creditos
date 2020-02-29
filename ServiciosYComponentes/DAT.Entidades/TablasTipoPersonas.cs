using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TablaTipoPersona
    {       
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCorta { get; set; }
    }
}
