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
    public class DiscapacidadNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BeneficioNegocio).Name);
        public static Persona Discapacidad_Guardar(Anses.ArgentaC.Negocio.CertificadosWS.Certificados certificados, Persona _persona)
        {
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("Selecciono certificado de entre todos los certificados de invalidez ({0})", _persona.Cuil.ToString());

                CertificadosWS.Certificado cert = new CertificadosWS.Certificado();
                cert = SeleccionarCertificado(certificados);
                if (cert != null)
                {
                    //actualizo los datos del beneficiario con los datos del certificado de discapacidad
                    if (cert.permanente == "S")
                    {
                        _persona.BeneficiosRelacionados.Find(x => x.Cuil == cert.cuil).EsDiscapacitadoPermanente = true;
                        _persona.BeneficiosRelacionados.Find(x => x.Cuil == cert.cuil).FechaVtoDiscapacidad = DateTime.MaxValue;
                    }
                    else
                    {
                        _persona.BeneficiosRelacionados.Find(x => x.Cuil == cert.cuil).EsDiscapacitadoPermanente = false;
                        _persona.BeneficiosRelacionados.Find(x => x.Cuil == cert.cuil).FechaVtoDiscapacidad = cert.fechaVtoCud;
                    }
                    if (log.IsInfoEnabled)
                        log.DebugFormat("El certificado de invalidez del cuil {0} tiene ID = {1} ; fechaVto = {2} ; esPermanente = {3}", cert.cuil.ToString(), cert.idCud, cert.fechaVtoCud.ToString(), cert.permanente);
                }
                else
                {
                    log.DebugFormat("El método SeleccionarCertificado no ha devuelto ningun certificado valido");
                }
                return _persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Anses.ArgentaC.Negocio.CertificadosWS.Certificado SeleccionarCertificado(Anses.ArgentaC.Negocio.CertificadosWS.Certificados certificados)
        {
            try
            {
                //si hay un solo certificado que no esta dado de baja y aun no vencio, no miro el idcud, de lo contrario hay que seleccionar de la lista el que corresponda
                if (certificados.certificado.Count() == 1)
                {
                    if (!(certificados.certificado.First().fechaBaja < DateTime.Now) && !(certificados.certificado.First().fechaVtoCud < DateTime.Now))
                    {
                        return certificados.certificado.First();
                    }
                }
                //List<Anses.ArgentaC.Negocio.CertificadosWS.Certificado> unCertificado = new List<CertificadosWS.Certificado>;
                var unCertificado = from unCert in certificados.certificado
                                    where unCert.fechaBaja.ToString() == "01/01/1900 12:00:00 a.m." && !string.IsNullOrEmpty(unCert.idCud)
                                    group unCert by unCert.cuil into x
                                    select x.OrderByDescending(t => t.fechaEmisionCud).FirstOrDefault();
                if (unCertificado.Any())
                    return unCertificado.First();
                else
                    return null;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }

        }
    }
}
