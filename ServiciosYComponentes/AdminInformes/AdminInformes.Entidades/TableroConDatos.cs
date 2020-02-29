using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminInformes.Entidades
{
    public class TableroConDatos
    {

        public int idTablero { get; set; }
        public string NombreTablero { get; set; }
        public List<MapeoParametrosTableroQuery> MapaParametros { get; set; }
        public List<Visualizacion> lstVisualizaciones { get; set; }
        public ItemMenuTablero Solicitud { get; set; }
        public List<string> IncludeScripts { get; set; }
        public List<string> lstPaquetesRequeridos { get; set; }
    }


}