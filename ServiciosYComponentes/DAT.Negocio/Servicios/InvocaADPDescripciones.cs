using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Anses.DAT.Negocio.ADPDescripciones;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;

namespace Anses.DAT.Negocio.Servicios
{
    public static class InvocaADPDescripciones
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InvocaADPDescripciones).Name);

        private static ADPDescripciones.ADPDescripciones InstanciaADP()
        {
            try
            {
                ADPDescripciones.ADPDescripciones oServicio = new ADPDescripciones.ADPDescripciones();
                oServicio.Url = System.Configuration.ConfigurationManager.AppSettings["ADPDescripciones.ADPDescripciones"];
                oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
                return oServicio;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "obtenerEstadoGrupoControl", ex.Source, ex.Message));
                throw;
            }
        }

        public static List<TablaTipoPersona> ObtenerEstadoGrupoControl()
        {
            log.Info("Se inicia ejecución del método ObtenerEstadoGrupoControl");

            try
            {               
                ObtenerEstadoGrupoControlResultado resultado = InstanciaADP().ObtenerEstadoGrupoControl();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleEstadoGrupoControl.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return  resultado.DetalleEstadoGrupoControl.ToList()                       
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoEstadoGrupoControl.ToString(),
                            Descripcion = x.nombreEstadoGrupoControl,
                            DescripcionCorta = x.nombreCortoEstadoGrupoControl
                        }).ToList(); 
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerEstadoGrupoControl", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerEstadoGrupoControl");
            }
        }

        public static List<TablaTipoPersona> ObtenerTipoDocumento()
        {
            log.Info("Se inicia ejecución del método ObtenerTipoDocumento");

            try
            {                
                ObtenerTipoDocumentoResultado resultado = InstanciaADP().ObtenerTipoDocumento();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleTipoDocumento.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleTipoDocumento.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.CodigoTipoDocumento.ToString(),
                            Descripcion = x.NombreTipoDocumento,
                            DescripcionCorta = x.NombreCortoTipoDocumento
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerTipoDocumento", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerTipoDocumento");
            }
        }

        public static List<TablaTipoPersona> ObtenerEstadoCivil()
        {
            log.Info("Se inicia ejecución del método ObtenerEstadoCivilPorCodigo");

            try
            {
                ObtenerEstadoCivilResultado resultado = InstanciaADP().ObtenerEstadoCivil();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleEstadoCivil.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleEstadoCivil.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoEstadoCivil.ToString(),
                            Descripcion = x.nombreEstadoCivil,
                            DescripcionCorta = x.nombreCortoEstadoCivil
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerEstadoCivilPorCodigo", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerEstadoCivilPorCodigo");
            }
        }

        public static List<TablaTipoPersona> ObtenerIncapacidad()
        {
            log.Info("Se inicia ejecución del método ObtenerIncapacidad");

            try
            {
                ObtenerIncapacidadResultado resultado = InstanciaADP().ObtenerIncapacidad();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleIncapacidad.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleIncapacidad.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoIncapacidad.ToString(),
                            Descripcion = x.nombreIncapacidad,
                            DescripcionCorta = x.nombreCortoIncapacidad
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerIncapacidad", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerIncapacidad");
            }
        }

        public static List<TablaTipoPersona> ObtenerPais()
        {
            log.Info("Se inicia ejecución del método ObtenerPais");

            try
            {
                ObtenerPaisResultado resultado = InstanciaADP().ObtenerPais();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetallePais.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetallePais.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.CodPais.ToString(),
                            Descripcion = x.NombrePais,
                            DescripcionCorta = x.NombreCortoPais
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerPais", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerPais");
            }
        }

        public static List<TablaTipoPersona> ObtenerComprobanteIngresoPais()
        {
            log.Info("Se inicia ejecución del método ObtenerComprobanteIngresoPais");

            try
            {
                ObtenerComprobanteIngresoPaisResultado resultado = InstanciaADP().ObtenerComprobanteIngresoPais();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleComprobanteIngresoPais.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleComprobanteIngresoPais.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.CodigoComprobanteIngresoPais.ToString(),
                            Descripcion = x.NombreComprobanteIngresoPais,
                            DescripcionCorta = x.NombreCortoComprobanteIngresoPais
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerComprobanteIngresoPais", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerComprobanteIngresoPais");
            }
        }

        public static List<TablaTipoPersona> ObtenerTipoResidencia()
        {
            log.Info("Se inicia ejecución del método ObtenerTipoResidencia");

            try
            {
                ObtenerTipoResidenciaResultado resultado = InstanciaADP().ObtenerTipoResidencia();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleTipoResidencia.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleTipoResidencia.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoTipoResidencia,
                            Descripcion = x.nombreTipoResidencia,
                            DescripcionCorta = x.nombreTipoResidencia, 
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerTipoResidencia", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerTipoResidencia");
            }
        }

        public static List<TablaTipoPersona> ObtenerEstadoFallecimiento()
        {
            log.Info("Se inicia ejecución del método ObtenerEstadoFallecimiento");

            try
            {
                ObtenerEstadoFallecimientoResultado resultado = InstanciaADP().ObtenerEstadoFallecimiento();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleEstadoFallecimiento.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleEstadoFallecimiento.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoEstadoFallecimiento.ToString(),
                            Descripcion = x.nombreEstadoFallecimiento,
                            DescripcionCorta = x.nombreCortoEstadoFallecimiento
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerEstadoFallecimiento", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerEstadoFallecimiento");
            }
        }

        public static List<TablaTipoPersona> ObtenerTipoCuilCuit()
        {
            log.Info("Se inicia ejecución del método ObtenerTipoCuilCuit");

            try
            {
                ObtenerTipoCuilCuitResultado resultado = InstanciaADP().ObtenerTipoCuilCuit();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleTipoCuilCuit.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleTipoCuilCuit.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoTipoCuilCuit,
                            Descripcion = x.nombreTipoCuilCuit,
                            DescripcionCorta = x.nombreTipoCuilCuit
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerTipoCuilCuit", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerTipoCuilCuit");
            }
        }

        public static List<TablaTipoPersona> ObtenerEstadoRespectoAfip()
        {
            log.Info("Se inicia ejecución del método ObtenerEstadoRespectoAfip");

            try
            {
                ObtenerEstadoRespectoAfipResultado resultado = InstanciaADP().ObtenerEstadoRespectoAfip();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleEstadoRespectoAfip.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleEstadoRespectoAfip.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.codigoEstadoRespectoAfip.ToString(),
                            Descripcion = x.nombreEstadoRespectoAfip,
                            DescripcionCorta = x.nombreCortoEstadoRespectoAfip
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerEstadoRespectoAfip", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerEstadoRespectoAfip");
            }
        }

        public static List<TablaTipoPersona> ObtenerProvincia()
        {
            log.Info("Se inicia ejecución del método ObtenerProvincia");

            try
            {
                ObtenerProvinciaResultado resultado = InstanciaADP().ObtenerProvincia();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleProvincia.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleProvincia.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.CodigoProvincia.ToString(),
                            Descripcion = x.NombreProvincia,
                            DescripcionCorta = x.NombreCortoProvincia
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerProvincia", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerProvincia");
            }
        }
        
        public static List<TablaTipoPersona> ObtenerTipoDomicilio()
        {
            log.Info("Se inicia ejecución del método ObtenerTipoDomicilio");

            try
            {
                ObtenerTipoDomicilioResultado resultado = InstanciaADP().ObtenerTipoDomicilio();

                if (resultado == null)
                {
                    throw new Exception("La ejecución del servicio retorno null");
                }

                if (resultado.DetalleTipoDomicilio.Any() == false)
                {
                    throw new Exception(resultado.Error.DescripcionMensaje);
                }

                return resultado.DetalleTipoDomicilio.ToList()
                        .Select(x => new TablaTipoPersona
                        {
                            Codigo = x.CodigoTipoDomicilio.ToString(),
                            Descripcion = x.NombreTipoDomicilio,
                            DescripcionCorta = x.NombreCortoTipoDomicilio
                        }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerTipoDomicilio", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                log.Info("Culmina del método ObtenerTipoDomicilio");
            }
        }
    }
}
