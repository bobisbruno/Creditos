using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminInformes.Entidades

{
    public class Query
    {
        public int idVisualizacion { get; set; }
        public int idQuery { get; set; }
        public string NombreProceso { get; set; }
        public string Servidor { get; set; }
        public string Base { get; set; }
        public List<ParametroTablero> lstParametros { get; set; }
        
    }
}