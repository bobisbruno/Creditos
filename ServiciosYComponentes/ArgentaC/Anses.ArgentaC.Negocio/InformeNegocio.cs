using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;
using log4net;

namespace Anses.ArgentaC.Negocio
{
    public class InformeNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonaNegocio).Name);

        public static List<InformeDeDevolucionesCierreDiario> InformeDeDevolucionesCierreDiario_Obtener(DateTime? _FechaDesde, DateTime? _FechaHasta)
        {
            try
            {
                return InformeDato. InformeDeDevolucionesCierreDiario_Obtener(_FechaDesde, _FechaHasta);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List<Mensual> Mensual_Obtener(enum_Proposito proposito)
        {
            try
            {
                return InformeDato.Mensual_Obtener(proposito);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }

        public static List<Concepto> Concepto_Obtener(enum_Proposito proposito)
        {
            try
            {
                return InformeDato.Concepto_Obtener(proposito);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
    }
}
