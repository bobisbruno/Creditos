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
    public static class ArchivoNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BeneficioNegocio).Name);
        public static string InformesPreparados_Buscar(int mensual, int? concepto, int? tipoInforme)
        {
            try
            {
                return ArchivoDato.InformesPreparados_Buscar(mensual, concepto, tipoInforme);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
    }
}
