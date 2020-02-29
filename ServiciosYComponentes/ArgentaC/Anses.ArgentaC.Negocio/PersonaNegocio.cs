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
    public class PersonaNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonaNegocio).Name);
        public static Persona traerPersona(DatosdePersonaporCuip.RetornoDatosPersonaCuip _personaADP)
        {
            List<Tipo> Errores = new List<Tipo>();
            Persona persona = new Persona();
            try
            {
                if (_personaADP.PersonaCuip != null)
                {
                    Domicilio domicilio = new Domicilio(_personaADP.PersonaCuip.domi_calle,
                                                        _personaADP.PersonaCuip.domi_nro,
                                                        _personaADP.PersonaCuip.domi_piso,
                                                        _personaADP.PersonaCuip.domi_dpto,
                                                        _personaADP.PersonaCuip.domi_cod_postal.ToString(),
                                                        new Tipo(_personaADP.PersonaCuip.domi_cod_pcia.ToString(), ""),
                                                        _personaADP.PersonaCuip.domi_localidad);
                    Contacto contacto = new Contacto(_personaADP.PersonaCuip.email,
                                                     string.Concat(_personaADP.PersonaCuip.telediscado, 
                                                                   _personaADP.PersonaCuip.telefono.ToString()),
                                                     string.Concat(_personaADP.PersonaCuip.telediscado,
                                                                   _personaADP.PersonaCuip.telefono.ToString()));
                    persona = new Persona(  long.Parse(_personaADP.PersonaCuip.cuip),
                                            _personaADP.PersonaCuip.ape_nom,
                                            int.Parse(_personaADP.PersonaCuip.doc_nro),
                                            _personaADP.PersonaCuip.doc_c_tipo,
                                            _personaADP.PersonaCuip.f_naci,
                                            _personaADP.PersonaCuip.sexo,
                                            _personaADP.PersonaCuip.f_falle,
                                            (((_personaADP.PersonaCuip.cod_estcivil == 2) || (_personaADP.PersonaCuip.cod_estcivil == 6)) && ((DateTime.Now.Year - _personaADP.PersonaCuip.f_naci.Year) < 18)),
                                            domicilio,
                                            contacto
                    );
                }
                else
                {
                        
                }
                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona Persona_Guardar(Persona _unaPersona)
        {
            try
            {
                return PersonaDato.Persona_Guardar(_unaPersona);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public static Persona traerAnme(Anses.ArgentaC.Negocio.ExpedienteWS.ExpedienteDTO[] listaExpedientes, Persona persona)
        {
            try
            {
                if (listaExpedientes != null)
                    persona.TieneTramiteAnme = true;
                else
                    persona.TieneTramiteAnme = false;
                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona PrestacionJubilatoria_Traer(decimal nroBeneficio, Anses.ArgentaC.Negocio.RUBConsultas01.ResultadoListaOfRubXBeneficio benef, Persona persona)
        {
            try
            {
                if(benef != null)
                {
                    if (benef.Error != null)
                    {
                        if (benef.Error.CodigoError != 0)
                            log.DebugFormat("El servicio de Rub, metodo obtenerRubxBeneficio, con parametro Beneficio={0} dio error: Codigo={1} ; Mensaje={2} ; DB2NativeError: {3}", nroBeneficio.ToString(), benef.Error.CodigoError, benef.Error.DescripcionMensaje, benef.Error.DB2NativeError);
                        else
                            log.DebugFormat("El servicio de Rub, metodo obtenerRubxBeneficio, con parametro Beneficio={0} se ejecuto sin errores: Codigo={1} ; Mensaje={2} ; DB2NativeError: {3}", nroBeneficio.ToString(), benef.Error.CodigoError, benef.Error.DescripcionMensaje, benef.Error.DB2NativeError);
                    }

                    log.DebugFormat("El servicio de RUB metodo obtenerRubxBeneficio ha devuelto resultados ({0})", benef.TotalOcurrencias);

                    if(benef.Lista != null)
                    {
                        log.DebugFormat("El servicio de RUB metodo obtenerRubxBeneficio ha devuelto prestaciones en lista, un total de ({0})", benef.Lista.Count());
                        foreach (RUBConsultas01.RubXBeneficio r in benef.Lista)
                        {
                            bool seDioDeBaja = string.IsNullOrEmpty(r.FECHBAJABENEF) ? false : ((DateTime.Parse(r.FECHBAJABENEF) != DateTime.MinValue) && (DateTime.Parse(r.FECHBAJABENEF) < DateTime.Now));
                            #region logs
                            log.Debug("Entre al ciclo foreach de beneficios de RUB");
                            log.DebugFormat("La fecha de baja del beneficio actual de RUB es: {0}", r.FECHBAJABENEF);
                            log.DebugFormat("La fecha de baja del beneficio es nula o vacía? {0}", string.IsNullOrEmpty(r.FECHBAJABENEF));
                            log.DebugFormat("La fecha de baja del beneficio es distinta a DateTime.MinValue? {0}", (DateTime.Parse(r.FECHBAJABENEF) != DateTime.MinValue));
                            log.DebugFormat("La fecha actual es: {0}", DateTime.Now);
                            log.DebugFormat("La fecha de baja del beneficio es menor a la fecha actual? {0}", (DateTime.Parse(r.FECHBAJABENEF) < DateTime.Now));
                            log.DebugFormat("Se dio de baja el beneficio actual? {0}", seDioDeBaja);
                            #endregion
                            if (!seDioDeBaja)
                            {
                                log.DebugFormat("Se encontró un beneficio previsional inválido en vigencia ({0})", persona.Cuil);
                                persona.TienePrestacionJubilatoria = true;
                                break;
                            }
                            else
                                log.DebugFormat("Se encontró un beneficio previsional inválido pero ya caducó ({0})", persona.Cuil);
                        }
                    }
                    else
                        log.Debug("El servicio de RUB metodo obtenerRubxBeneficio no ha devuelto prestaciones en lista");
                }
                else
                    log.Debug("El servicio de RUB metodo obtenerRubxBeneficio no ha devuelto resultados");

                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
            finally
            {
                log.DebugFormat("Se ha finalizado el método {0}, para el CUIL {1}, obteniendo prestacion jubilatoria {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, persona.Cuil, persona.TienePrestacionJubilatoria);
            }
        }
        public static Persona obtenerOfertaCredito(Persona unaPersona)
        {
            try
            {
                return ProductoDato.obtenerOfertaCredito(unaPersona);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona traerAltaTemprana(Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana[] _datosAltaTemprana, Persona _persona)
        {
            try
            {
                List<Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana> lista = _datosAltaTemprana.ToList();

                var registrosAT = from registro in lista
                                  where (registro.FechaAltaTemprana > (DateTime.Now.AddDays(-90)) && registro.DescripcionAbreviadaMovimiento == "AT")
                                  group registro by registro.Cuit into x
                                  select x;

                var registrosBT = from registro in lista
                                  where (registro.FechaAltaTemprana > (DateTime.Now.AddDays(-90)) && registro.DescripcionAbreviadaMovimiento == "BT")
                                  group registro by registro.Cuit into x
                                  select x.OrderByDescending(t => t.FechaAltaTemprana);

                if (!registrosAT.Any() && !registrosBT.Any())
                    _persona.TieneAltaTemprana = false;
                else
                {
                    if (!registrosBT.Any())
                        _persona.TieneAltaTemprana = (_persona.IdSistema == 60);
                    else
                        _persona.TieneAltaTemprana = !(registrosBT.ToList().First().First().FechaAltaTemprana > registrosAT.ToList().First().First().FechaAltaTemprana);
                }
                return _persona;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona traerSituacionProcesal(Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado[] _datosProcesales, Persona _persona)
        {
            try
            {
                if (_datosProcesales != null)
                {
                    if (_datosProcesales.Count() > 0)
                        _persona.EsPresoConSentenciaFirme = true;
                    else
                        _persona.EsPresoConSentenciaFirme = false;
                }
                else
                    _persona.EsPresoConSentenciaFirme = false;
                return _persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona traerHuella(Anses.ArgentaC.Negocio.SBAService.CuilStateResponseDto _response, Persona _persona)
        {
            try
            {
                if (_response != null)
                {
                    if (_response.CuilState != null)
                    {
                        if (_response.CuilState.Code == "Enrolled")
                            _persona.TieneHuella = true;
                        else
                            _persona.TieneHuella = false;
                    }
                    else
                        _persona.TieneHuella = false;
                }
                else
                    _persona.TieneHuella = null;
                return _persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona traerDeudaArgenta(bool tieneDeuda, decimal montoDeuda, Persona persona)
        {
            try
            {
                if (tieneDeuda)
                {
                    Deuda deudaArgenta = new Deuda(999, "999", montoDeuda);
                    persona.Deudas.Add(deudaArgenta);
                }
                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona obtenerBancoAgencia(BocaDePagoWS.BancoAgencia[] bancoagencias, Persona _unaPersonaGlobal)
        {
            try
            {
                if (!(bancoagencias == null))
                {
                    if (bancoagencias.ToList().Count > 0)
                    {
                        _unaPersonaGlobal.Banco = bancoagencias.First().Banco;
                        _unaPersonaGlobal.Agencia = bancoagencias.First().Agencia;
                        _unaPersonaGlobal.codBanco = bancoagencias.First().CodigoBanco;
                        _unaPersonaGlobal.codAgencia = bancoagencias.First().CodigoAgencia;
                    }
                }
                else
                {
                    _unaPersonaGlobal.Banco = null;
                    _unaPersonaGlobal.Agencia = null;
                    _unaPersonaGlobal.codBanco = 0;
                    _unaPersonaGlobal.codAgencia = 0;
                }
                return _unaPersonaGlobal;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static bool TieneDeudaEnArgentaC(long cuil)
        {
            try
            {
                return PersonaDato.TieneDeudaEnArgentaC(cuil);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
		public static bool HijoTieneMadrePresaOFallecida(long cuilHijo, long cuilPadre, out long cuilDeLaMadre, out bool? madrePresa, out bool madreFallecida)
        {
            try
            {
                bool madrePresaOFallecida = false;
                madrePresa = null;
                madreFallecida = false;
                cuilDeLaMadre = 0;

                WSPW04.WS_PW04 srv = new WSPW04.WS_PW04();
                log.Debug("Busco relaciones del hijo del titular");
                log.DebugFormat("CuilTitular: {0}", cuilPadre);
                log.DebugFormat("CuilHijo: {0}", cuilHijo);
                WSPW04.ListaPw04 listaRelaciones = srv.ObtenerRelacionesxCuil(cuilHijo.ToString(), 1);

                if (listaRelaciones == null)
                {
                    madrePresaOFallecida = false;
                    log.Debug("El hijo no presenta relaciones al salir del servicio");
                }
                else
                {
                    if (listaRelaciones.Lista != null)
                    {
                        log.Debug("El hijo tiene relaciones al salir del servicio");
                        //List<WSPW04.DatosPw04> lista = new List<WSPW04.DatosPw04>();
                        List<WSPW04.DatosPw04> lst = listaRelaciones.Lista.ToList();

                        if (lst != null)
                        {
                            log.Debug("Recorro los hijos en busqueda del cuil de la madre");
                            int i = 0;
                            foreach (WSPW04.DatosPw04 l in lst)
                            {
                                log.DebugFormat("Hijo Nro {0} tiene cod_Relacion {1}, base {2} y es hijo de cuil distinto del padre {3}", i, l.c_relacion, l.Base, l.cuil_rela != cuilPadre.ToString());
                                if (l.c_relacion == 3 && l.Base == "B" && l.cuil_rela != cuilPadre.ToString())
                                {
                                    log.Debug("Encontre a la madre");
                                    cuilDeLaMadre = long.Parse(l.cuil_rela);
                                    log.DebugFormat("CuilDeLaMadre: {0}", cuilDeLaMadre);
                                }
                                i = i + 1;
                            }
                        }
                        else
                        {
                            madrePresaOFallecida = false;
                            log.Debug("ListaVacia: El hijo no presenta relaciones al salir del servicio");
                            log.DebugFormat("La salida de la funcion {0} es {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, madrePresaOFallecida);
                            return madrePresaOFallecida;
                        }
                    }
                    else
                    {
                        madrePresaOFallecida = false;
                        log.Debug("ListaRelVacia: El hijo no presenta relaciones al salir del servicio");
                        log.DebugFormat("La salida de la funcion {0} es {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, madrePresaOFallecida);
                        return madrePresaOFallecida;
                    }


                    /*
                    Activos_ServiciosComplementariosWs.Activos_ServiciosComplementariosWs srv1 = new Activos_ServiciosComplementariosWs.Activos_ServiciosComplementariosWs();
                    Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado[] datos = srv1.ConsultaCondenadoProcesado(decimal.Parse(cuilDeLaMadre.ToString()));

                    if (datos.Any())
                    {
                        madrePresaOFallecida = true;
                        madrePresa = true;
                    }
                    */

                    if (cuilDeLaMadre != 0)
                    {
                        log.Debug("Voy a buscar los datos de la madre en ADP");
                        DatosdePersonaporCuip.DatosdePersonaporCuip srv2 = new DatosdePersonaporCuip.DatosdePersonaporCuip();
                        DatosdePersonaporCuip.RetornoDatosPersonaCuip adp = srv2.ObtenerPersonaxCUIP(cuilDeLaMadre.ToString());

                        if (adp.PersonaCuip != null)
                        {
                            log.Debug("La madre esta en adp");
                            if (adp.PersonaCuip.f_falle > DateTime.MinValue)
                            {
                                madrePresaOFallecida = true;
                                log.Debug("La madre fallecio");
                                madreFallecida = true;
                            }
                        }
                        else
                        {
                            log.Debug("La madre no esta en adp");
                            madrePresaOFallecida = false;
                        }
                    }
                    else
                    {
                        log.Debug("La madre no pudo ser encontrada en las relaciones del hijo");
                        madrePresaOFallecida = false;
                    }

                }

                log.DebugFormat("La salida de la funcion {0} es {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, madrePresaOFallecida);
                return madrePresaOFallecida;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

		public static bool TieneCodPostalValidoEnPim(int codPostal)
        {
            try
            {
                return PersonaDato.TieneCodPostalValidoEnPim(codPostal);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}
