using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class GestionRecuperoForm
    {
        public List<Recupero> RecuperosList { get; set; }
        public int CantidadTotalDeRegistros { get; set; }
    }
}
