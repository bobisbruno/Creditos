using Activos_ServiciosComplementarios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Activos_ServiciosComplementarios.Dato;

namespace Activos_ServiciosComplementarios.Negocio
{
    public static class ActivosServiciosComplementariosNegocio
    {

        public static List<DatosDeConsultaAltaTemprana> ConsultaAltaTempranaNegocio(decimal cuil)
        {
            return ActivosServiciosComplementariosDAO.ConsultaAltaTemprana(cuil);
        }


        public static List<DatosDeConsultaCondenadoProcesado> ConsultaCondenadoProcesadoNegocio(decimal cuil)
        {
            return ActivosServiciosComplementariosDAO.ConsultaCondenadoProcesado(cuil);
        }

    }
}
