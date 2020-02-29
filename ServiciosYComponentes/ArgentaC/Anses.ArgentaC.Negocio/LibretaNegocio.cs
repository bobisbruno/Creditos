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
    public class LibretaNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonaNegocio).Name);
        public static Persona Persona_GuardarLibretas(Anses.ArgentaC.Negocio.uvLibretaWS.CUILRelacionados[] arraylibretas, Persona _unaPersonaGlobal)
        {
            try
            {
                for (int i = 0; i < _unaPersonaGlobal.BeneficiosRelacionados.Count; i++)
                {
                    if (_unaPersonaGlobal.BeneficiosRelacionados[i].Cuil == arraylibretas[i].CUILRelacionado)
                    {
                        _unaPersonaGlobal.BeneficiosRelacionados[i].AnioUltimaLiquidacion = arraylibretas[i].AnioUltimaLiquidacion;
                        _unaPersonaGlobal.BeneficiosRelacionados[i].AnioUltimaLibretaPresentada = arraylibretas[i].AnioUltimaLibreta;
                    }
                }
                return _unaPersonaGlobal;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
    }
}
