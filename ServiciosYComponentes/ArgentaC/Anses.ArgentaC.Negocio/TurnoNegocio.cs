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
    public static class TurnoNegocio
    {
        public static bool Turno_Hardcoded()
        {
            try
            {
                return TurnoDato.Turno_Hardcoded();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public static bool PNC_PUAM_Existe(long cuil)
        {
            try
            {
                return TurnoDato.PNC_PUAM_Existe(cuil);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
