using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;
using log4net;

namespace Anses.ArgentaC.Negocio
{
    public class PrestamoNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonaNegocio).Name);
        public static Persona ObtenerNovedades(Persona _persona)
        {
            try
            {
                return PrestamoDato.ObtenerNovedades(_persona);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
    }
}
