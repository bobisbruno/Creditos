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
    public class TableroCobranzaNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TableroCobranzaNegocio).Name);
        public static TableroCobranza TableroCobranza_Obtener(int? _mensual, int? _concepto)
        {
            try
            {
                return TableroCobranzaDato.TableroCobranza_Obtener(_mensual, _concepto);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
    }
}
