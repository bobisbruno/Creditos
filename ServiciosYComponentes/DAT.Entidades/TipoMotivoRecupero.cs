using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
     public class TipoMotivoRecupero
    {
         public int Id {get; set;}
         public string DescripcionMotivoRecupero { get; set; }
         public string Usuario { get; set; }
         public bool Habilitado { get; set; }
         public bool HabilitadoWEB { get; set; }
         public string Ip { get; set; }
         public DateTime FechaUltimaModificacion { get; set; }

    }
}
