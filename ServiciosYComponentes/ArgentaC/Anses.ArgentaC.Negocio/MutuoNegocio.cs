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
    public class MutuoNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MutuoNegocio).Name);
        public Mutuo obtenerDatosMutuo(Novedad unaNovedad)
        {
            try
            {
                MutuoDato oMutuo = new MutuoDato();
                return oMutuo.obtenerDatosMutuo(unaNovedad);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public int obtenerVersionActualMutuo(int idOrigen, DateTime fechaVersion)
        {
            try
            {
                MutuoDato oMutuo = new MutuoDato();
                return oMutuo.obtenerVersionActualMutuo(idOrigen, fechaVersion);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return 0;
            }
        }
    }
}
