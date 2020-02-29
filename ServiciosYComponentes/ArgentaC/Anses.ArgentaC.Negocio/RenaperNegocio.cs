using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anses.ArgentaC.Dato;

namespace Anses.ArgentaC.Negocio
{
    public static class RenaperNegocio
    {
        public static bool Renaper_Habilitado()
        {
            try
            {
                return RenaperDato.Renaper_Habilitado();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
