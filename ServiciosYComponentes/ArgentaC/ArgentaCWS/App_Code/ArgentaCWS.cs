using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Negocio;
using log4net;
using System.Threading.Tasks;

/// <summary>
/// ArgentaCWS consume servicios externos, y guarda los datos en un objeto propio del aplicativo para someterlo a validaciones desde la base de datos.
/// </summary>
[WebService(Namespace = "http://ArgentaCWS.Anses.Gov.Ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[Serializable]
public class ArgentaCWS : System.Web.Services.WebService {

    private static readonly ILog log = LogManager.GetLogger(typeof(ArgentaCWS).Name);
    private static Persona unaPersonaGlobal = new Persona();

    public ArgentaCWS() {

        //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Guarda en la capa de datos la información del titular y sus derechohabientes.")]
    public Persona Persona_Guardar(Persona _unaPersonaGlobal)
    {
        try
        {
            return PersonaNegocio.Persona_Guardar(_unaPersonaGlobal);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            _unaPersonaGlobal.Errores.Add(new Mensaje(2, "Ha ocurrido un error en el método Persona_Guardar." + err));
            return _unaPersonaGlobal;
        }
    }

    [WebMethod(Description = "Devuelve los posibles planes de cuotas según la información recopilada.")]
    public Persona CalcularCredito(Persona _unaPersonaGlobal)
    {
        try
        {
            return PersonaNegocio.obtenerOfertaCredito(_unaPersonaGlobal);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Devuelve un plan de cuotas específico que haya elegido el usuario de entre los disponibles.")]
    public Persona CalcularCuotas(Persona persona, Producto producto, decimal monto)
    {
        try
        {
            return CuotaNegocio.calcularCuotas(persona, producto, monto);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Guarda en la capa de datos el plan de cuotas preaprobado y su detalle.")]
    public Persona Preacuerdo_Guardar(Persona _persona, Novedad _novedad)
    {
        try
        {
            return NovedadNegocio.Preacuerdo_Guardar(_persona, _novedad);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Obtiene los preacuerdos de una persona.")]
    public Persona ObtenerNovedades(Persona _persona, enum_TipoEstadoNovedad estado)
    {
        try
        {
            return NovedadNegocio.ObtenerNovedades(_persona, estado);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Obtiene los novedades por cuil, estado u oficina.")]
    public Novedad[] ObtenerNominaNovedades(long? cuil, enum_TipoEstadoNovedad? estado, int? oficina)
    {
        try
        {
            return NovedadNegocio.ObtenerNominaNovedades(cuil, estado, oficina);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Cambia de Estado una Novedad.")]
    public Novedad Novedad_CambiarEstado(Novedad _novedad, enum_TipoEstadoNovedad _idEstadoDestino, long _idProducto, decimal _montoSolicitado)
    {
        try
        {
            return NovedadNegocio.CambiarEstado(_novedad, _idEstadoDestino, _idProducto, _montoSolicitado);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Consulta de Novedades")]
    public List<DatosDeConsultaDeNovedad> NovedadesConsulta(int? idNovedad, long? cuil, int? estado, DateTime? desde, DateTime? hasta)
    {
        try
        {
            return NovedadNegocio.NovedadesConsulta(idNovedad, cuil, estado, desde, hasta);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Obtiene los datos del mutuo de una novedad.")]
    public Mutuo obtenerDatosMutuo(Novedad novedad)
    {
        try
        {
            MutuoNegocio oMutuo = new MutuoNegocio();
            return oMutuo.obtenerDatosMutuo(novedad);
            
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Aprobacion de Creditos")]
    public bool ConfirmarCreditosGenerados(List<Novedad> listaNovedades)
    {
        try
        {
            return NovedadNegocio.ConfirmarCreditosGenerados(listaNovedades);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Puam existe? Si/No")]
    public bool PNC_PUAM_Existe(long cuil)
    {
        try
        {
            return TurnoNegocio.PNC_PUAM_Existe(cuil);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Devuelve la version del mutuo correspondiente a una novedad por grabarse en preacuerdo.")]
    public int obtenerVersionActualMutuo(int idOrigen, DateTime fechaVersion)
    {
        try
        {
            MutuoNegocio oMutuo = new MutuoNegocio();
            return oMutuo.obtenerVersionActualMutuo(idOrigen, fechaVersion);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Valida el DNI en el servicio del RENAPER")]
    public bool ValidarDNI(int DNI, string Sexo, int NroTramiteDNI)
    {
        log.Debug("Ejecuto " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        UVHICreditos.UVHICreditos servicio = new UVHICreditos.UVHICreditos();
            servicio.Url = System.Configuration.ConfigurationManager.AppSettings[servicio.GetType().ToString()];
            servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

        try
        {
            log.Debug("Terminé de ejecutar " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            return servicio.ValidarDNI(DNI, Sexo, NroTramiteDNI);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Devuelve ls lista de sistemas habilitados para solicitar créditos.")]
    public List<Tipo> ObtenerSistemasHabilitados()
    {
        try
        {
            return NovedadNegocio.ObtenerSistemasHabilitados();
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Valida el DNI en el servicio del RENAPER")]
    public bool TieneDeudaEnArgentaC(long cuil)
    {
        try
        {
            return PersonaNegocio.TieneDeudaEnArgentaC(cuil);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Devuelve una respuesta que indica si tiene CBU Pim y el CBU en cuestión.")]
    public bool ObtenerCBUPim(long cuil, out string cbu)
    {
        try
        {
            log.Debug("Invocamos el servicio de Pim");
            PIM.resultadoConsultaCBU res;
            PIM.PIM p = new PIM.PIM();
            res = p.PIM_Consulta(cuil.ToString());
            log.Debug("Pim devolvió resultado");
            if (res.subscriber != null)
                cbu = res.subscriber.CBU;
            else
                cbu = "0";
            log.Debug("El cbu devuelto por el servicio es " + cbu);
            //log.Debug("Pim devolvió: " + (res.codeResult == "0"));
            return (res.codeResult == "0");
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    [WebMethod(Description = "Devuelve una respuesta que indica si tiene CBU Pim y el CBU en cuestión.")]
    public bool TieneCodPostalValidoEnPim(int codPostal)
    {
        try
        {
            return PersonaNegocio.TieneCodPostalValidoEnPim(codPostal);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

	[WebMethod(Description = "Valida que siendo sexo masculino se pueda solicitar crédito")]
    public bool HijoTieneMadrePresaOFallecida(long cuilHijo, long cuilPadre, out long cuilDeLaMadre, out bool? madrePresa, out bool madreFallecida)
    {
        try
        {
            return PersonaNegocio.HijoTieneMadrePresaOFallecida(cuilHijo, cuilPadre, out cuilDeLaMadre, out madrePresa, out madreFallecida);
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
            throw err;
        }
    }

    #region  Parametros_tblDTSVariables
    [WebMethod(Description = "Obtiene si el sitio esta se encuentra habilitado o en mantenimiento.")]
    public bool Parametros_SitioHabilitado()
    {
        return ParametroNegocio.Parametros_SitioHabilitado();
    }

    [WebMethod(Description = "Devuelve si el servicio de turnos está hardcodeado o se está llamando al servicio productivo.")]
    public bool Turno_Hardcoded()
    {
        return TurnoNegocio.Turno_Hardcoded();
    }

    [WebMethod(Description = "Devuelve si esta habilidatada la validación con el Renaper.")]
    public bool Renaper_Habilitado()
    {
        return RenaperNegocio.Renaper_Habilitado();
    }
    #endregion

    #region METODOS ASINCRONOS
    [WebMethod(Description = "Consume los servicios de ADP y UVHI, para poder mostrar los beneficios del titular que solicita el crédito.")]
    public Persona ASYNC_MuestroBeneficios(Persona _unaPersonaGlobal)
    {
        log.Debug("Ejecuto " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        //GLOBAL ------------------
        unaPersonaGlobal = new Persona();
        //*************************
        unaPersonaGlobal.Cuil = _unaPersonaGlobal.Cuil;
        //consumo servicio de ADP y Avila para poder listar los beneficios
        unaPersonaGlobal = ASYNC_Persona_traer(unaPersonaGlobal).Result;
        for (int i = 0; i < unaPersonaGlobal.Errores.Count; i++)
        {
            if (unaPersonaGlobal.Errores[i].ID > 1)
                return unaPersonaGlobal;
        }

        //obtengo la lista de sistemas habilitados
        List<Tipo> sistemas = ObtenerSistemasHabilitados();
        
        //veo si tiene beneficios UVHI
        unaPersonaGlobal = ASYNC_BeneficioUVHI_Traer(unaPersonaGlobal).Result;
        //si hay beneficios UVHI, le digo que entra por ese sistema
        if (unaPersonaGlobal.BeneficiosRelacionados != null)
        {
            if (unaPersonaGlobal.BeneficiosRelacionados.Count > 0)
            {
                unaPersonaGlobal.IdSistema = int.Parse(sistemas.Find(x => x.Descripcion == "Asig por hijo AUH").ID);
            }
        }
        
        //si no tiene beneficios en UVHI, vamos a buscar si tiene beneficios en SUAF
        if (unaPersonaGlobal.BeneficiosRelacionados == null || (unaPersonaGlobal.BeneficiosRelacionados != null && unaPersonaGlobal.BeneficiosRelacionados.Count == 0))
        {
            unaPersonaGlobal = ASYNC_BeneficioSUAF_Traer(unaPersonaGlobal).Result;
            if (unaPersonaGlobal.BeneficiosRelacionados != null)
            {
                if (unaPersonaGlobal.BeneficiosRelacionados.Count > 0)
                {
                    unaPersonaGlobal.IdSistema = int.Parse(sistemas.Find(x => x.Descripcion == "SUAF").ID);
                }
            }
        }

        //traigo banco y agencia de la persona para mostrar en control de beneficios relacionados
        unaPersonaGlobal = ASYNC_BancoAgencia_Traer(unaPersonaGlobal).Result;

        log.Debug("Terminé de ejecutar " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        return unaPersonaGlobal;
    }
    [WebMethod(Description = "Consume los servicios de Anme y Rub")]
    public Persona ASYNC_AnmeYRub(Persona _unaPersonaGlobal)
    {
        log.Debug("Ejecuto " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        //consumo los servicios de Anme y Rub, y guardo la información en la variable global de la persona
        unaPersonaGlobal = ASYNC_TramitesAnme_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_PrestacionJubilatoria_Traer(_unaPersonaGlobal).Result;
        log.Debug("Terminé de ejecutar " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        return unaPersonaGlobal;
    }
    [WebMethod(Description = "Consume los servicios externos y devuelve una persona con todos los datos necesarios para solicitar un crédito.")]
    public Persona ASYNC_TraerResto(Persona _unaPersonaGlobal)
    {
        log.Debug("Ejecuto " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        unaPersonaGlobal = ASYNC_DeudasUVHI_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_Libretas_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_CertificadosCUD_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_SituacionProcesal_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_AltaTemprana_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_Huella_Traer(_unaPersonaGlobal).Result;
        unaPersonaGlobal = ASYNC_DeudaArgenta_Traer(_unaPersonaGlobal).Result;
        //RiesgoBCRA se valida desde la BDD
        log.Debug("Terminé de ejecutar " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        return unaPersonaGlobal;
    }
    //[WebMethod(Description = "Trae una persona de ADP, valida su consistencia y retorna un objeto Persona reducido")]
    public async static Task<Persona> ASYNC_Persona_traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            try
            {
                Persona personaNegocio = new Persona();
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de ADP ObtenerPersonaxCUIP({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                DatosdePersonaporCuip.DatosdePersonaporCuip servicio = new DatosdePersonaporCuip.DatosdePersonaporCuip();
                servicio.Url = System.Configuration.ConfigurationManager.AppSettings[servicio.GetType().ToString()];
                servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
                DatosdePersonaporCuip.RetornoDatosPersonaCuip _personaADP = servicio.ObtenerPersonaxCUIP(_unaPersonaGlobal.Cuil.ToString());
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "ObtenerPersonaxCUIP", tiempo.Elapsed);
                if (_personaADP.PersonaCuip != null)
                {
                    personaNegocio = Anses.ArgentaC.Negocio.PersonaNegocio.traerPersona(
                    (Anses.ArgentaC.Negocio.DatosdePersonaporCuip.RetornoDatosPersonaCuip)reSerializer.reSerialize(_personaADP,
                                            typeof(DatosdePersonaporCuip.RetornoDatosPersonaCuip),
                                            typeof(Anses.ArgentaC.Negocio.DatosdePersonaporCuip.RetornoDatosPersonaCuip),
                                            reSerializer.retNameSpace(typeof(DatosdePersonaporCuip.RetornoDatosPersonaCuip),
                                            servicio.Url)));
                    _unaPersonaGlobal = personaNegocio;
                    _unaPersonaGlobal.Errores.Add(new Mensaje(0, "ADP: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                }
                else
                {
                    _unaPersonaGlobal.Errores.Add(new Mensaje(1, "ADP: No ha pasado los controles: CUIL INEXISTENTE. ASINCRONO"));
                }
                return _unaPersonaGlobal;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "ADP: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de UVHI, y devuelve una persona con sus beneficios UVHI.")]
    public async Task<Persona> ASYNC_BeneficioUVHI_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            UVHICreditos.UVHICreditos servicio = new UVHICreditos.UVHICreditos();
            servicio.Url = System.Configuration.ConfigurationManager.AppSettings[servicio.GetType().ToString()];
            servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            UVHICreditos.CCInfo[] ccdatos;
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de UVHI CuilInformacion({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                ccdatos = servicio.CuilInfo(decimal.Parse(_unaPersonaGlobal.Cuil.ToString()));
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "UVHI CuilInformacion", tiempo.Elapsed);
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "UVHI Beneficios: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                if (ccdatos != null)
                {
                    return Anses.ArgentaC.Negocio.BeneficioNegocio.BeneficioUVHI_Traer(
                (Anses.ArgentaC.Negocio.UVHICreditos.CCInfo[])reSerializer.reSerialize(ccdatos,
                                        typeof(UVHICreditos.CCInfo[]),
                                        typeof(Anses.ArgentaC.Negocio.UVHICreditos.CCInfo[]),
                                        reSerializer.retNameSpace(typeof(UVHICreditos.CCInfo),
                                        servicio.Url)), _unaPersonaGlobal);
                }
                else
                {
                    _unaPersonaGlobal.BeneficiosRelacionados = null;
                    return _unaPersonaGlobal;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "UVHI Beneficios: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de UVHI, y devuelve una persona con sus beneficios UVHI.")]
    public async Task<Persona> ASYNC_BeneficioSUAF_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            UVHICreditos.UVHICreditos servicio = new UVHICreditos.UVHICreditos();
            servicio.Url = System.Configuration.ConfigurationManager.AppSettings[servicio.GetType().ToString()];
            servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            UVHICreditos.CCInfo[] ccdatos;
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de SUAF CuilInfoSuaf({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                ccdatos = servicio.CuilInfoSUAF(decimal.Parse(_unaPersonaGlobal.Cuil.ToString()));
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "SUAF CuilInfoSuaf", tiempo.Elapsed);
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "SUAF Beneficios: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                if (ccdatos != null)
                {
                    return Anses.ArgentaC.Negocio.BeneficioNegocio.BeneficioSUAF_Traer(
                    (Anses.ArgentaC.Negocio.UVHICreditos.CCInfo[])reSerializer.reSerialize(ccdatos,
                                            typeof(UVHICreditos.CCInfo[]),
                                            typeof(Anses.ArgentaC.Negocio.UVHICreditos.CCInfo[]),
                                            reSerializer.retNameSpace(typeof(UVHICreditos.CCInfo),
                                            servicio.Url)), _unaPersonaGlobal);
                }
                else
                {
                    _unaPersonaGlobal.BeneficiosRelacionados = null;
                    return _unaPersonaGlobal;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "SUAF Beneficios: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de ANME, y devuelve una persona con la información sobre si tiene algún trámite iniciado.")]
    public async static Task<Persona> ASYNC_TramitesAnme_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de Anme ConsultaExpedientesPorCuilListaGrupEstadistico({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                ExpedienteWS.ExpedienteWS srv = new ExpedienteWS.ExpedienteWS();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
                ExpedienteWS.CuilDTO cuilDTO = new ExpedienteWS.CuilDTO();
                string cuilParseado = _unaPersonaGlobal.Cuil.ToString();
                cuilDTO.preCuil = cuilParseado.Substring(0, 2);
                cuilDTO.docCuil = cuilParseado.Substring(2, 8);
                cuilDTO.digCuil = cuilParseado.Substring(10, 1);
                string cGgrupoEstad = "2, 0";
                ExpedienteWS.TipoError error = new ExpedienteWS.TipoError();
                ExpedienteWS.ExpedienteDTO[] listaExpedientes = srv.ConsultaExpedientesPorCuilListaGrupEstadistico(cuilDTO, cGgrupoEstad, out error);
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "ConsultaExpedientesPorCuilListaGrupEstadistico", tiempo.Elapsed);

                if (listaExpedientes == null)
                {
                    log.Debug("El servicio de anme no devolvio resultados");
                    _unaPersonaGlobal.TieneTramiteAnme = false;
                }
                else
                {
                    log.Debug("El servicio de anme devolvio resultados y hay que analizar los datos de salida del servicio");
                    _unaPersonaGlobal = Anses.ArgentaC.Negocio.PersonaNegocio.traerAnme(
                                                                                    (Anses.ArgentaC.Negocio.ExpedienteWS.ExpedienteDTO[])reSerializer.reSerialize(listaExpedientes,
                                                                                    typeof(ExpedienteWS.ExpedienteDTO[]),
                                                                                    typeof(Anses.ArgentaC.Negocio.ExpedienteWS.ExpedienteDTO[]),
                                                                                    reSerializer.retNameSpace(typeof(ExpedienteWS.ExpedienteDTO),
                                                                                    srv.Url)), _unaPersonaGlobal);
                }
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "ANME: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return _unaPersonaGlobal;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                //en lugar de disparar la excepcion, devuelvo el objeto nulo
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "ANME: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de Prestaciones Jubilatorias, y devuelve una persona con la información sobre sus jubilaciones y/o pensiones.")]
    public async Task<Persona> ASYNC_PrestacionJubilatoria_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de Rub ObtenerBeneficiosPorCUIP({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                RUBConsultas01.RUBConsultas01 srv = new RUBConsultas01.RUBConsultas01();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
                RUBConsultas01.RUBBeneficiosXCUIP[] prestaciones = srv.ObtenerBeneficiosPorCUIP(_unaPersonaGlobal.Cuil);
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "ObtenerBeneficiosPorCUIP", tiempo.Elapsed);
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "RUB: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                if (prestaciones != null)
                {
                    List<string> beneficiosPrevisionales = new List<string>();
                    beneficiosPrevisionales.Add("42");
                    beneficiosPrevisionales.Add("48");
                    beneficiosPrevisionales.Add("70");
                    beneficiosPrevisionales.Add("71");
                    beneficiosPrevisionales.Add("72");
                    beneficiosPrevisionales.Add("74");
                    beneficiosPrevisionales.Add("77");
                    beneficiosPrevisionales.Add("78");
                    beneficiosPrevisionales.Add("79");
                    beneficiosPrevisionales.Add("90");
                    beneficiosPrevisionales.Add("92");
                    beneficiosPrevisionales.Add("93");

                    if (prestaciones.Count() == 0)
                    {
                        _unaPersonaGlobal.TienePrestacionJubilatoria = false;
                        if (log.IsInfoEnabled)
                            log.DebugFormat("El servicio de Rub no arrojó resultados({0})", _unaPersonaGlobal.Cuil);
                    }
                    else
                    {
                        if (log.IsInfoEnabled)
                            log.DebugFormat("El servicio de Rub devolvió resultados({0})", _unaPersonaGlobal.Cuil);

                        _unaPersonaGlobal.TienePrestacionJubilatoria = false; // valor por defecto... de no ser así se modifica en el siguiente foreach

                        foreach (RUBConsultas01.RUBBeneficiosXCUIP p in prestaciones)
                        {
                            // condicion para abortar la estructura de control foreach cuando se alterara (la primera vez que entra al ciclo no hace nada)
                            if (_unaPersonaGlobal.TienePrestacionJubilatoria == true)
                                break;

                            if (!beneficiosPrevisionales.Contains(p.Beneficio.ToString().Substring(0, 2)))
                            {
                                if (log.IsInfoEnabled)
                                    log.DebugFormat("Se encontró un beneficio previsional con prestacion ({0}) que no cumple con las aceptadas (UVHI)", p.Beneficio.ToString().Substring(0, 2));

                                // obtengo los datos de Rub para el beneficio
                                log.DebugFormat("Ejecuto el servicio de RUB metodo obtenerRubxBeneficio ({0})", p.Beneficio);
                                RUBConsultas01.ResultadoListaOfRubXBeneficio benef = new RUBConsultas01.ResultadoListaOfRubXBeneficio();
                                benef = srv.obtenerRubxBeneficio(p.Beneficio);

                                _unaPersonaGlobal = Anses.ArgentaC.Negocio.PersonaNegocio.PrestacionJubilatoria_Traer(p.Beneficio, (Anses.ArgentaC.Negocio.RUBConsultas01.ResultadoListaOfRubXBeneficio)reSerializer.reSerialize(benef,
                                    typeof(RUBConsultas01.ResultadoListaOfRubXBeneficio),
                                    typeof(Anses.ArgentaC.Negocio.RUBConsultas01.ResultadoListaOfRubXBeneficio),
                                    reSerializer.retNameSpace(typeof(RUBConsultas01.ResultadoListaOfRubXBeneficio),
                                    srv.Url)), _unaPersonaGlobal);
                            }
                            else
                            {
                                // si no se cumple el IF, existe alguna prestacion vigente tal que cumpla con las prestaciones aceptadas (UVHI)
                                // NOTA: No podría pasar que haya prestaciones no jubilatorias y todas ellas estuvieran dadas de baja, pues sino el servicio de UVHICreditos no tendría datos en beneficios relacionados del titular
                                log.DebugFormat("Existe prestacion ({0}) vigente tal que cumple con las aceptadas (UVHI)", p.Beneficio.ToString().Substring(0, 2));
                            }
                        }
                    }
                    return _unaPersonaGlobal;
                }
                else
                {
                    _unaPersonaGlobal.TienePrestacionJubilatoria = false;
                    return _unaPersonaGlobal;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "RUB: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de UVHI, y devuelve una persona con sus deudas (solo en UVHI) con el organismo.")]
    public async Task<Persona> ASYNC_DeudasUVHI_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            UVHICreditos.UVHICreditos srv = new UVHICreditos.UVHICreditos();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            UVHICreditos.Deuda[] deudas;
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de UVHI DeudaTraer({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                deudas = srv.DeudaTraer(decimal.Parse(_unaPersonaGlobal.Cuil.ToString()));
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "UVHI DeudaTraer", tiempo.Elapsed);
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "UVHI Deudas: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return Anses.ArgentaC.Negocio.BeneficioNegocio.DeudasUVHI_Traer(

                  (Anses.ArgentaC.Negocio.UVHICreditos.Deuda[])reSerializer.reSerialize(deudas,
                                       typeof(UVHICreditos.Deuda[]),
                                       typeof(Anses.ArgentaC.Negocio.UVHICreditos.Deuda[]),
                                       reSerializer.retNameSpace(typeof(UVHICreditos.Deuda),
                                       srv.Url)), _unaPersonaGlobal);
                //deudas = new UVHICreditos.Deuda[0];
                //deudas = new UVHICreditos.Deuda[4];
                //deudas[0] = new UVHICreditos.Deuda();
                //deudas[0].SistemaCodigo = 60;
                //deudas[0].Sistema = "ASIG UNIVERSAL HIJO - UVHI";
                //deudas[0].Saldo = decimal.Parse("996,8");
                //deudas[1] = new UVHICreditos.Deuda();
                //deudas[1].SistemaCodigo = 60;
                //deudas[1].Sistema = "ASIG UNIVERSAL HIJO - UVHI";
                //deudas[1].Saldo = decimal.Parse("996,8");
                //deudas[2] = new UVHICreditos.Deuda();
                //deudas[2].SistemaCodigo = 14;
                //deudas[2].Sistema = "SUAF";
                //deudas[2].Saldo = decimal.Parse("996,8");
                //deudas[3] = new UVHICreditos.Deuda();
                //deudas[3].SistemaCodigo = 60;
                //deudas[3].Sistema = "ASIG UNIVERSAL HIJO - UVHI";
                //deudas[3].Saldo = decimal.Parse("996,8");
                //_unaPersonaGlobal.Errores.Add(new Mensaje(1, "UVHI Deudas: El servicio externo no esta siendo consumido. Está HARDCODEADO actualmente."));
                //return Anses.ArgentaC.Negocio.BeneficioNegocio.DeudasUVHI_Traer(

                //  (Anses.ArgentaC.Negocio.UVHICreditos.Deuda[])reSerializer.reSerialize(deudas,
                //                       typeof(UVHICreditos.Deuda[]),
                //                       typeof(Anses.ArgentaC.Negocio.UVHICreditos.Deuda[]),
                //                       reSerializer.retNameSpace(typeof(UVHICreditos.Deuda),
                //                       srv.Url)), _unaPersonaGlobal);


            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "UVHI Deudas: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de Libretas, y devuelve una persona con las libretas de sus beneficios.")]
    public async Task<Persona> ASYNC_Libretas_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            try
            {
                int tamanio = _unaPersonaGlobal.BeneficiosRelacionados.Count;
                string[] cuiles = new string[tamanio];
                for (int i = 0; i < _unaPersonaGlobal.BeneficiosRelacionados.Count; i++)
                    cuiles[i] = _unaPersonaGlobal.BeneficiosRelacionados[i].Cuil.ToString();
                uvLibretaWS.uvLibretaWS srv = new uvLibretaWS.uvLibretaWS();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
                uvLibretaWS.CUILRelacionados[] arraylibretas = srv.LibretasCargadas_TXLote(cuiles);
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "UVHI Libretas: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return Anses.ArgentaC.Negocio.LibretaNegocio.Persona_GuardarLibretas(
                                                                                    (Anses.ArgentaC.Negocio.uvLibretaWS.CUILRelacionados[])reSerializer.reSerialize(arraylibretas,
                                                                                    typeof(uvLibretaWS.CUILRelacionados[]),
                                                                                    typeof(Anses.ArgentaC.Negocio.uvLibretaWS.CUILRelacionados[]),
                                                                                    reSerializer.retNameSpace(typeof(uvLibretaWS.CUILRelacionados),
                                                                                    srv.Url)), _unaPersonaGlobal);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "UVHI Libretas: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de Certificados de Discapacidad, y devuelve una persona con las certificados de sus beneficios.")]
    public async Task<Persona> ASYNC_CertificadosCUD_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            try
            {
                CertificadosWS.CertificadosWS srv = new CertificadosWS.CertificadosWS();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
                foreach (Beneficio b in _unaPersonaGlobal.BeneficiosRelacionados)
                {
                    if (b.EsDiscapacitado) //consultar por el codigo 14 en caso de que no viniera el dato como un bit
                    {
                        //obtengo la lista de certificados
                        if (log.IsInfoEnabled)
                            log.DebugFormat("Ejecuto servicio de Anses.Prissa.Carpeta.Invalidez.Servicio({0})", _unaPersonaGlobal.Cuil);
                        var tiempo = Stopwatch.StartNew();
                        CertificadosWS.Certificados certificados = srv.obtenerCertificadosCudxCuil(b.Cuil.ToString());
                        tiempo.Stop();
                        if (log.IsInfoEnabled)
                            log.InfoFormat("el servicio {0} tardo {1} ", "Anses.Prissa.Carpeta.Invalidez.Servicio", tiempo.Elapsed);
                        if (certificados != null)
                        {
                            //me quedo con uno solo y lo guardo en el beneficio de la persona
                            if (log.IsInfoEnabled)
                                log.DebugFormat("Invoco el metodo Discapacidad_Guardar ya que tiene certificado/s");
                            _unaPersonaGlobal = Anses.ArgentaC.Negocio.DiscapacidadNegocio.Discapacidad_Guardar(
                                                                                                               (Anses.ArgentaC.Negocio.CertificadosWS.Certificados)reSerializer.reSerialize(certificados,
                                                                                                               typeof(CertificadosWS.Certificados),
                                                                                                               typeof(Anses.ArgentaC.Negocio.CertificadosWS.Certificados),
                                                                                                               reSerializer.retNameSpace(typeof(CertificadosWS.Certificados),
                                                                                                               srv.Url)), _unaPersonaGlobal);
                        }

                    }
                }
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "Anses.Prissa.Carpeta.Invalidez.Servicio: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return _unaPersonaGlobal;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "Anses.Prissa.Carpeta.Invalidez.Servicio: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de Presos, y devuelve una persona con la información de su siatuación procesal si la tuviere.")]
    public async Task<Persona> ASYNC_SituacionProcesal_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            Activos_ServiciosComplementariosWs.Activos_ServiciosComplementariosWs srv = new Activos_ServiciosComplementariosWs.Activos_ServiciosComplementariosWs();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado[] datosProcesales = srv.ConsultaCondenadoProcesado(decimal.Parse(_unaPersonaGlobal.Cuil.ToString()));
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "SITUACION PROCESAL: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return Anses.ArgentaC.Negocio.PersonaNegocio.traerSituacionProcesal(
                                                                                   (Anses.ArgentaC.Negocio.Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado[])reSerializer.reSerialize(datosProcesales,
                                                                                   typeof(Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado[]),
                                                                                   typeof(Anses.ArgentaC.Negocio.Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado[]),
                                                                                   reSerializer.retNameSpace(typeof(Activos_ServiciosComplementariosWs.DatosDeConsultaCondenadoProcesado),
                                                                                   srv.Url)), _unaPersonaGlobal);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "ALTA TEMPRANA: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    //[WebMethod(Description = "Consume servicio de Alta Temprana, y devuelve una persona con la información laboral actualizada.")]
    public async Task<Persona> ASYNC_AltaTemprana_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            Activos_ServiciosComplementariosWs.Activos_ServiciosComplementariosWs srv = new Activos_ServiciosComplementariosWs.Activos_ServiciosComplementariosWs();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana[] datosAltaTemprana = srv.ConsultaAltaTemprana(decimal.Parse(_unaPersonaGlobal.Cuil.ToString()));
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "ALTA TEMPRANA: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return Anses.ArgentaC.Negocio.PersonaNegocio.traerAltaTemprana(
                                                                              (Anses.ArgentaC.Negocio.Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana[])reSerializer.reSerialize(datosAltaTemprana,
                                                                              typeof(Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana[]),
                                                                              typeof(Anses.ArgentaC.Negocio.Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana[]),
                                                                              reSerializer.retNameSpace(typeof(Activos_ServiciosComplementariosWs.DatosDeConsultaAltaTemprana),
                                                                              srv.Url)), _unaPersonaGlobal);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "ALTA TEMPRANA: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    public async Task<Persona> ASYNC_Huella_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
        SBAService.SBAService srv = new SBAService.SBAService();
        srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
        srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
        try
        {
            if (log.IsInfoEnabled)
                log.DebugFormat("*** ASYNC - Ejecuto servicio de Huellas GetCuilState({0})", _unaPersonaGlobal.Cuil);
            var tiempo = Stopwatch.StartNew();
            SBAService.CuilStateRequestDto request = new SBAService.CuilStateRequestDto();
            SBAService.DocumentDto requestDocu = new SBAService.DocumentDto();
            requestDocu.DocumentNum = _unaPersonaGlobal.Cuil.ToString();
            requestDocu.Nationality = "ARG";
            requestDocu.TypeId = "CUIL";
            request.Entity = "33637617449";
            request.Document = requestDocu;
            //llamo al servicio
            SBAService.CuilStateResponseDto response = srv.GetCuilState(request);
            tiempo.Stop();
            if (log.IsInfoEnabled)
                log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "GetCuilState", tiempo.Elapsed);
            _unaPersonaGlobal.Errores.Add(new Mensaje(0, "HUELLA: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
            return Anses.ArgentaC.Negocio.PersonaNegocio.traerHuella(
                                                                    (Anses.ArgentaC.Negocio.SBAService.CuilStateResponseDto)reSerializer.reSerialize(response,
                                                                    typeof(SBAService.CuilStateResponseDto),
                                                                    typeof(Anses.ArgentaC.Negocio.SBAService.CuilStateResponseDto),
                                                                    reSerializer.retNameSpace(typeof(SBAService.CuilStateResponseDto),
                                                                    srv.Url)), _unaPersonaGlobal);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "HUELLA: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    public async Task<Persona> ASYNC_DeudaArgenta_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            DeudaArgentaWS.DeudaArgentaWS srv = new DeudaArgentaWS.DeudaArgentaWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de DeudasArgenta BeneficiarioDeuda_Traer({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                bool tieneDeuda = false;
                decimal montoDeuda = 0;
                //llamo al servicio
                tieneDeuda = srv.BeneficiarioDeuda_Traer(_unaPersonaGlobal.Cuil, out montoDeuda);
                tiempo.Stop();
                if (log.IsInfoEnabled)
                    log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "BeneficiarioDeuda_Traer", tiempo.Elapsed);
                _unaPersonaGlobal.Errores.Add(new Mensaje(0, "DeudasArgenta: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                return Anses.ArgentaC.Negocio.PersonaNegocio.traerDeudaArgenta(tieneDeuda, montoDeuda, _unaPersonaGlobal);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "DeudasArgenta: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    public async Task<Persona> ASYNC_BancoAgencia_Traer(Persona _unaPersonaGlobal)
    {
        return await Task.Run(() =>
        {
            BocaDePagoWS.BocaDePagoWS srv = new BocaDePagoWS.BocaDePagoWS();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                if (log.IsInfoEnabled)
                    log.DebugFormat("*** ASYNC - Ejecuto servicio de BocaDePagoWS ConsultaDetallesBancoAgencia({0})", _unaPersonaGlobal.Cuil);
                var tiempo = Stopwatch.StartNew();
                BocaDePagoWS.BancoAgencia[] bancoagencias;
                if (!string.IsNullOrEmpty(_unaPersonaGlobal.CBU))
                {
                    short banco = _unaPersonaGlobal.codBanco;
                    short agencia = _unaPersonaGlobal.codAgencia;
                    //llamo al servicio
                    bancoagencias = srv.ConsultaDetallesBancoAgencia(banco, agencia);
                    tiempo.Stop();
                    if (log.IsInfoEnabled)
                        log.InfoFormat("*** ASYNC - el servicio {0} tardo {1} ", "ConsultaDetallesBancoAgencia", tiempo.Elapsed);
                    _unaPersonaGlobal.Errores.Add(new Mensaje(0, "BocaDePago: El servicio externo esta siendo consumido actualmente. ASINCRONO"));
                    return Anses.ArgentaC.Negocio.PersonaNegocio.obtenerBancoAgencia(
                        (Anses.ArgentaC.Negocio.BocaDePagoWS.BancoAgencia[])
                            reSerializer.reSerialize(bancoagencias,
                                                        typeof(BocaDePagoWS.BancoAgencia[]),
                                                        typeof(Anses.ArgentaC.Negocio.BocaDePagoWS.BancoAgencia[]),
                                                        reSerializer.retNameSpace(typeof(BocaDePagoWS.BancoAgencia), srv.Url)),
                        _unaPersonaGlobal);
                }
                else
                    return _unaPersonaGlobal;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                _unaPersonaGlobal.Errores.Add(new Mensaje(2, "DeudasArgenta: " + err));
                return _unaPersonaGlobal;
            }
        });
    }
    #endregion
}
