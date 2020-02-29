using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Anses.DAT.Negocio;
using System.ComponentModel;
using System.Transactions;

/// <summary>
/// Summary description for TarjetaWS
/// </summary>
/// 
namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{

    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TarjetaWS : System.Web.Services.WebService
    {

        public TarjetaWS()
        {}

        [WebMethod]
        public List<Novedad_Tarjeta_Reponer> Traer_Reposicion_X_Beneficiario(string cuil, bool blancaPorNominada, out int codMotivoReponer)
        {           
            return TarjetaDAO.EstadoDeTarjetas_Traer(cuil, blancaPorNominada, out codMotivoReponer);
        }
         [WebMethod]
        public void Reposicion_ImputarCostoTarjeta(string cuil, long nroTarjeta,  int codMotivoReponer)
        {
            TarjetaDAO.Reposicion_ImputarCostoTarjeta(cuil, nroTarjeta, codMotivoReponer);
        }
            
        [WebMethod]
        //Inundacion-->Se agrega parametro CodConcepto = 0, para la ValidoTarjeta
        public string Novedades_ReposicionTarjeta_Alta(List<Novedad_Tarjeta_Reponer> tarjetasAReponer, long cuil, long nuevoNroTarjeta, long idBeneficiario, bool esNominada, string IP, string Usuario, long IdPrestador, string oficina)
        {
            return TarjetaDAO.Novedades_ReposicionTarjeta_Alta(tarjetasAReponer, cuil, nuevoNroTarjeta, idBeneficiario, esNominada, IP, Usuario, IdPrestador, oficina);
        }

        [WebMethod]
        public List<CodDescripcion> TipoMotivoReemplazo_Traer()
        {
            return TarjetaDAO.TipoMotivoReemplazo_Traer();
        }

        [WebMethod]
        //Inundacion-->Se agrega parametro CodConcepto
        public string Tarjetas_Valido(Int64 nroTarjeta, long idbeneficiario, int codConcepto, bool esNominada, enum_TipoMovimientoTarjeta tipoMovimientoTarjeta, out  bool repone, out  int idTipoTarjeta, out bool debeSolicitarNominada, out bool blancaPorNominada, out bool habilitaAltaNovedad)
        {
            return TarjetaDAO.Tarjetas_Valido(nroTarjeta, idbeneficiario, codConcepto, esNominada, tipoMovimientoTarjeta, out  repone, out  idTipoTarjeta, out debeSolicitarNominada, out blancaPorNominada, out habilitaAltaNovedad);
        }

        [WebMethod]
        public List<Tarjeta> Tarjetas_Traer(string cuil, Int64? nroTarjeta)
        {
            return TarjetaDAO.Tarjetas_Traer(cuil,nroTarjeta);
        }
        
        [WebMethod]
        public List<Tarjeta> Tarjetas_Guardar(List<Tarjeta> listaTarjetas)
        {
            return TarjetaDAO.Tarjetas_Guardar(listaTarjetas);
        }

        [WebMethod(Description = "Guarda Tarjeta y Domicilio cuando EsSolicitud == true")]
        public string Tarjetas_GuardarEstadoSolicitud(Tarjeta tarjeta, Domicilio domicilio)
        {
           return TarjetaDAO.Tarjetas_AltaEstadoSolicitud(tarjeta, domicilio);
        }

        [WebMethod]
        public List<Tarjeta> Tarjetas_TXSucursalEstado_Traer(Int64 idPrestador, string oficina, Int16? idEstadoEntrega, DateTime? fDesde,
                                                             DateTime? fHasta, Int16? idOrigen, Int16? idEstadoPack,
                                                             out Int16 total)
        {
            return TarjetaDAO.Tarjetas_TXSucursalEstado_Traer(idPrestador,oficina, idEstadoEntrega, fDesde, fHasta,idOrigen,idEstadoPack,out total);
           
        }

        [WebMethod]
        public void TarjetaNominadaValidacionTurnos(string cuil, out short codRetorno, out string oficina)
        {
            TarjetaDAO.TarjetaNominadaValidacionTurnos(cuil, out codRetorno, out oficina);
        }

        [WebMethod]
        public string Tarjetas_ValidoPorCuil(long cuil, enum_TipoMovimientoTarjeta tipoMovimientoTarjeta, out bool tieneNominada, out bool habilitaAltaExpress)
        {
            return TarjetaDAO.Tarjetas_ValidoPorCuil(cuil, tipoMovimientoTarjeta, out tieneNominada, out habilitaAltaExpress);
        }

        [WebMethod(Description="Trae Tarjetas por estado - fecha")] 
        public List<Tarjeta> Tarjetas_TraerxEstadoFecha(Int64? idPrestador, string oficina, string cuil, int idEstadoReg,
                                                                 DateTime? fechaD, DateTime? fechaH, Int16? idOrigen, Int16? idEstadoPack)
        {
              return TarjetaDAO.Tarjetas_TraerxEstadoFecha(idPrestador, oficina, cuil, idEstadoReg, fechaD, fechaH,idOrigen,idEstadoPack);                   
        }

        [WebMethod(Description = "Traer de la tabla TarjetasTransicionEstados los datos de los estados validos")]
        public List<TarjetasTransicionEstados> TarjetasTransicionEstados_T()
        {
            return TarjetaDAO.TarjetasTransicionEstados_T();
        }

        [WebMethod(Description = "trae Novedades_TarjetaHistorica_Traer")]
        public List<Novedades_TarjetaHistorica> Novedades_TarjetaHistorica_Traer(long nroTarjetaNuevo)
        {
            return TarjetaDAO.Novedades_TarjetaHistorica_Traer(nroTarjetaNuevo);

        }

        [WebMethod(Description ="Trae los Estados Historios del Nro de Tarjeta")]
        public List<TarjetaHistorica> TarjetaHistorica_Traer(Int64 nroTarjeta)
        {
            return TarjetaDAO.TarjetaHistorica_Traer(nroTarjeta);
        }

        [WebMethod(Description = "Trae Nro de Tarjeta Por IdBeneficiario")]
        public long Tarjeta_TraerXIDBeneficiario(long idBeneficiario)
        {
            return TarjetaDAO.Tarjeta_TraerXIDBeneficiario(idBeneficiario);
        }

        [WebMethod(Description = "Trae el Estado de PinLink por cuil, Nro de Tarjeta")]
        public List<PinLink> PinLink_Trae( Int64 cuil,Int64 nroTarjeta)
        {
            return PinLinkDAO.PinLink_Trae(cuil,nroTarjeta);
        }

        [WebMethod(Description="Se realiza el alta de tarjetas en PinLink")]
        public String PinLink_Alta(PinLink unPinLink, CodigoPreAprobado unCodigoPreAprobado)
        {
            return PinLinkNegocio.PinLink_Alta(unPinLink, unCodigoPreAprobado);
        }

        [WebMethod(Description="Se realiza consulta de tarjeta tipo carnet por Cuil")]
        public TarjetaConsulta Tarjeta_TipoCarnet_Traer(Int64 cuil)
        {
            return TarjetaDAO.Tarjeta_TipoCarnet_Traer(cuil);
        }

        [WebMethod(Description = "Consulta tarjeta tipo carnet por Cuil, para turnos para Créditos ARGENTA")]
        public TarjetaTurnos Tarjeta_TipoCarnet_TraerParaTurnos(Int64 cuil)
        {
            return TarjetaDAO.Tarjeta_TipoCarnet_TraerParaTurnos(cuil);
        }

        [WebMethod(Description = "Consulta tarjeta tipo carnet por Cuil, para turnos para Créditos ARGENTA")]
        public TarjetaTurnosArg Tarjeta_TipoCarnet_TraerParaTurnosArg(Int64 cuil)
        {
            return TarjetaDAO.Tarjeta_TipoCarnet_TraerParaTurnos_Arg(cuil);
        }

        [WebMethod(Description = "Trae Tipo de estado de Aplicación desde TipoEstadoTarjeta")]
        public List<TipoEstadoTarjeta> TipoEstadoTarjeta_TraerXEstadosAplicacion()
        {
            return TarjetaDAO.TipoEstadoTarjeta_TT_EstadosAplicacion();
        }

        [WebMethod (Description="Trae lote desde Tarjeta")]
        public List<String> Tarjetas_TraerLotes()
        {
          return TarjetaDAO.Tarjetas_TT_Lotes();
        }

        [WebMethod (Description="Trae los Totales por tipo de Estado de Tarjeta.")]
        public List<TarjetaTotalesXEst> Tarjetas_TraerTotalesXTipoEstado(String descEstadoAplicacion,Int16 idprovincia, Int16 codpostal,List<String> oficinas, DateTime ? fAltaDesde, DateTime ? fAltaHasta, string lote)
        {
            return TarjetaDAO.Tarjetas_T_Totales(descEstadoAplicacion, idprovincia, codpostal, oficinas, fAltaDesde, fAltaHasta, lote);           
        }

       [WebMethod (Description="Trae una lista con los Tipo Tarjeta.")]
       public List<TipoTarjeta> TipoTarjeta_Traer()
       {
          return TarjetaDAO.TipoTarjeta_Traer();
       }

       [WebMethod (Description=" Trae un listado de tarjetas filtrado segun los valores de los parametros, Total de registros y Tope de Registro.")]
       public List<TarjetasXSucursalEstadoXTipoTarjeta> Tarjeta_TraerPorSucEstado_TipoTarjeta(long idPrestador, int idTipoTarjeta, int idEstadoAplicacion, String descEstadoAplicacion, Int16 idProvincia, Int16 codPostal, List<string> oficinas,
                                                                                              DateTime? fAltaDesde, DateTime? fAltaHasta, string lote, bool generaArchivo, bool generaAdmin, bool soloArgenta, bool soloEntidades, string regional,
                                                                                              out Int64 topeRegistros, out Int64 total, out string rutaArchivoSal)     
       {
                                                     
           return TarjetaDAO.Tarjeta_TraerSucEstadoYTipoTarjeta(idPrestador, idTipoTarjeta, idEstadoAplicacion, descEstadoAplicacion, idProvincia, codPostal, oficinas, fAltaDesde, fAltaHasta, lote, generaArchivo, generaAdmin, soloArgenta, soloEntidades, regional, out topeRegistros, out total, out rutaArchivoSal);
       }

       [WebMethod(Description = "Realiza ABM en EmbozadoAnses.")]
       public void EmbozadoAnses_Guardar(TarjetaEmbozado tarjeta)
       {
           TarjetaDAO.EmbozadoAnses_Guardar(tarjeta);
       }

       [WebMethod(Description = "Genera y trae la lista de Tarjeta pendiente de Embozado")]
       public List<TarjetaEmbozado> EmbozadoAnses_GeneraPendientesEmbozado(out int cantTotal, out int cantMostrar, Auditoria auditoria)
       {

           return TarjetaDAO.EmbozadoAnses_GeneraPendientesEmbozado(out cantTotal, out cantMostrar, auditoria);
          
       }  

       [WebMethod (Description="Trae una lista de Tarjeta en EmbozadoAnses según el estado.")]
       public List<TarjetaEmbozado> EmbozadoAnses_TraerXEstado(int idEstado, out int cantTotal, out int cantMostrar)
       {
           return TarjetaDAO.EmbozadoAnses_TraerXEstado(idEstado, out cantTotal, out cantMostrar);
       }

       [WebMethod(Description = "Trae una lista de Tarjeta en EmbozadoAnses según el estado.")]
       public Tarjeta EmbozadoAnses_TraerXCuilEstado(long cuil)
       {
           return TarjetaDAO.EmbozadoAnses_TraerXCuilEstado(cuil);
       }

       [WebMethod(Description = "Trae una lista de Tarjeta en EmbozadoAnses según el estado.")]
       public string EmbozadoAnses_ValidoEscaneo(long cuil, long nroTarjeta)
       {
           return TarjetaDAO.EmbozadoAnses_ValidoEscaneo(cuil, nroTarjeta);
       }

      [WebMethod(Description = "Trae la cantidad de tarjetas según oficina.")]
       public  List<Tarjeta_TAlerta> Tarjeta_TAlerta_Traer(string oficinaDestino, Boolean solReenvioFlujoPostal, Boolean solDestruccion)
       {
           return TarjetaDAO.Tarjeta_TAlerta_Traer(oficinaDestino, solReenvioFlujoPostal, solDestruccion);
       }

      [WebMethod(Description = "Reingreso Flujo Postal de Tarjeta")]
      public List<Tarjeta> Tarjetas_TReingresoFlujoPostal_Traer(string oficinaDestino)
      {
          return TarjetaDAO.Tarjetas_TReingresoFlujoPostal_Traer(oficinaDestino);
      }

    }
}