using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Negocio
{
    public static class ParametroNegocio
    {
        public static bool Parametros_SitioHabilitado()
        {
            return ParametroDato.Parametros_SitioHabilitado();
        }

        public static string TblDTSVariables_Buscar(string batch, string variable)
        {
            return ParametroDato.TblDTSVariables_Buscar(batch, variable);
        }
        
    }
}
