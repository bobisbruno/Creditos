using Anses.DAT.Negocio.Servicios;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.DAT.Negocio
{
    [Serializable]
    public class BeneficiarioNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BeneficiarioNegocio).Name);
        public static List<BeneficiarioCBU> BenefeciariosConCBUValidosXCOELSA(Int64 cuil, out string mensaje,out String error)
        {            
            List<BeneficiarioCBU> lstBeneCbuValidos = new List<BeneficiarioCBU>();
            mensaje = error= string.Empty;

            try
            {
                List<BeneficiarioCBU> lstBeneCbu = BeneficiarioDAO.Benefeciarios_CBU_XCuil(cuil, out mensaje);
                
                if (string.IsNullOrEmpty(mensaje))
                {
                   if (lstBeneCbu != null && lstBeneCbu.Any())
                   {
                        bool CodigoRetorno99 = false;
                        

                        int cant = lstBeneCbu.Count;
                        int i = 0;
                        while (i < cant && !CodigoRetorno99)
                        {
                            BeneficiarioCBU items = lstBeneCbu[i];

                            WSValidarCBU.Retorno retorno = InvocaValidarCBU.ValidarCBU(items.Cuil.ToString(), items.CBU);

                            log.Info(string.Format("BenefeciariosConCBUValidosXCOELSA -> ValidarCBU X COELSA RETORNO: CodigoRespuesta: {0} -> CodigoRetorno: {1} -> DescripcionMensaje: {2}-> CUIL:{3}, CBU:{4}",
                                                     retorno.CodigoRespuesta, retorno.CodigoRetorno, retorno.DescripcionMensaje, items.Cuil,items.CBU));

                            if (retorno.CodigoRespuesta.Trim() == "00" && retorno.CodigoRetorno == 0)
                            {
                                lstBeneCbuValidos.Add(items);
                            }
                            else
                            {
                                if (retorno.CodigoRetorno == 99)
                                {
                                    error = "No se pudo realizar la operación.Reintente en otro momento.";
                                    log.Error(string.Format("CodigoError {0}-> CodigoRetorno {1} -> DescripcionMensaje {2}-> CUIL:{3}, CBU:{4}", retorno.CodigoRespuesta, 
                                                            retorno.CodigoRetorno, retorno.DescripcionMensaje, items.Cuil.ToString(), items.CBU));
                                    CodigoRetorno99 = true;
                                }
                            }

                            i = i + 1;
                        }
                    }
                }
            }
            catch (Exception err)
            {
               log.Error(string.Format("{0}->{1}-> CUIL:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), cuil, err.Source, err.Message));
               throw err;
            }
            finally
            {
            }
            return lstBeneCbuValidos;
        }


        public static Boolean ValidarCBU_X_COELSA(string Cuil, string CBU, out string mensaje)
        {
            mensaje = string.Empty;
            Boolean esCBUOK = true;
            try
            {
                WSValidarCBU.Retorno retorno = InvocaValidarCBU.ValidarCBU(Cuil,CBU);

                log.Info(string.Format("ValidarCBU X COELSA RETORNO: CodigoRespuesta: {0} -> CodigoRetorno: {1} -> DescripcionMensaje: {2}-> CUIL:{3}, CBU:{4}", retorno.CodigoRespuesta, retorno.CodigoRetorno, retorno.DescripcionMensaje, Cuil, CBU));
                               
                if (retorno.CodigoRespuesta.Trim() == "00" && retorno.CodigoRetorno == 0)
                {                    
                    mensaje = "CBU validado correctamente.";

                    log.Info(String.Format("CBU Valido ->   DescripcionMensaje {0} ->  Mensaje:{1}",retorno.DescripcionMensaje, mensaje));
                }
                else if (retorno.CodigoRespuesta.Trim() != "00" || retorno.CodigoRetorno != 0)
                {
                    if (retorno.CodigoRetorno == 99)
                    {
                        mensaje = "No se pudo realizar la operación.Reintente en otro momento.";
                        log.Error(string.Format("CodigoRespuesta {0}-> CodigoRetorno {1} -> DescripcionMensaje {2}-> CUIL:{3}, CBU:{4}", retorno.CodigoRespuesta, retorno.CodigoRetorno, retorno.DescripcionMensaje, Cuil, CBU));
                    }
                    else
                    {
                        switch (retorno.CodigoRespuesta.Trim())
                        {
                            case "27":
                                mensaje = retorno.DescripcionMensaje;
                                break;
                            case "99":
                                mensaje = "No se pudo realizar la operación.Reintente en otro momento.";
                                break;
                            case "9999":
                                mensaje = "No se pudo realizar la operación.Reintente en otro momento.";
                                break;
                            default:
                                // 98:Parámetros incorrectos  
                                // 25:CBU No informada
                                // OTROS
                                mensaje = "El CBU ingresado NO es válido.";
                                break;
                        }
                    }

                    log.Info(String.Format("CBU NO Valido -> Mensaje: {0}",mensaje));
                    esCBUOK = false;
                }                

                log.Info(String.Format("FIN RESULTADO: Es CBU Valido:{0} -> Mensaje :{1}",esCBUOK,mensaje));
                
                return esCBUOK;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> CUIL:{2}, CBU:{3} - Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), Cuil,CBU, err.Source, err.Message));
                throw err;
            }
        }

    }
}
