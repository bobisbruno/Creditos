using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminInformes.Entidades
{
    public class ItemMenuTablero
    {
        public int idTablero { get; set; }
        public string Nombre { get; set; }
        public List<ParametroTablero> Parametros { get; set; }
    }
}