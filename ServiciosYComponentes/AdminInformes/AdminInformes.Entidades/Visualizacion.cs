using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminInformes.Entidades
{
    public class Visualizacion
    {
        public int idVisualizacion { get; set; }
        public int idTipoVisualizacion { get; set; }
        public string NombreVisualizacion { get; set; }
        public string NombreFuncion { get; set; }
        public string Contenedor { get; set; }
        public string ObjetoGrafico { get; set; }
        public Query Proceso { get; set; }
        public List<string> lstDatasets { get; set; }
        public List<string> lstScriptsRequeridos { get; set; }
        public string Opciones { get; set; }
        public string Paqueterequerido { get; set; }

    }
}