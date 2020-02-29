using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;

namespace Anses.ArgentaC.Negocio
{
    public class FechaCierreNegocio
    {
        public static Anses.ArgentaC.Contrato.FechaCierre traerFechaCierre(enum_TipoFecha _tipoFechaCierre)
        {
            try
            {
                Anses.ArgentaC.Contrato.FechaCierre oFechaCierre = new Anses.ArgentaC.Contrato.FechaCierre();
                oFechaCierre = FechaCierreDato.Buscar(_tipoFechaCierre);
                return oFechaCierre;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
