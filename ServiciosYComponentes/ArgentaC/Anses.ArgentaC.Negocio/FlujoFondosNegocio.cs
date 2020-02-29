using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anses.ArgentaC.Dato;
using Anses.ArgentaC.Contrato;
using log4net;

namespace Anses.ArgentaC.Negocio
{
    public class FlujoFondosNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonaNegocio).Name);
        public static List<FlujoFondos> FlujoFondos_Obtener(int? _IdSistema, int? _MensualCobranzaDesde, int? _MensualCobranzaHasta)
        {
            try {
                return FlujoFondosDato.FlujoFondos_Obtener(_IdSistema, _MensualCobranzaDesde, _MensualCobranzaHasta);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
    }
}
