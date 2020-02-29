using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminInformes.Entidades;
using AdminInformes.AccesoDatos;


namespace AdminInformes.Negocio
{
    public class ConstructorTablero
    {
        public TableroConDatos ObtenerDatosTablero (ItemMenuTablero tbl)
        {
            ObtenerDatosEjecucionTablero dt = new ObtenerDatosEjecucionTablero();
            TableroConDatos tcd = dt.ObtenerTableroConDatos(tbl);


            foreach(Visualizacion v in tcd.lstVisualizaciones)
            {
                foreach (string script in v.lstScriptsRequeridos)
                {
                    if (!tcd.IncludeScripts.Contains(script))
                    {
                        tcd.IncludeScripts.Add(script);
                    }
                }

                if (!tcd.lstPaquetesRequeridos.Contains(v.Paqueterequerido))
                {
                    tcd.lstPaquetesRequeridos.Add(v.Paqueterequerido);
                }
            }

            ObtenerDataset od = new ObtenerDataset();
            od.PonerDatosEnTablero(ref tcd);

            return tcd;
        }

    }
}