using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{



    [Serializable]
    public enum enum_TipoEstadoNovedad
    {
        Indeterminado = 0,
        Pre_Acuerdo = 1,
        Prestamo_Generado = 2,
        Prestamo_Supervisado = 3,
        Cancelado_Por_Vto_Turno = 4,
        Cancelado_Por_Inaccion_Sup = 5,
        Cancelado_Por_Modificacion = 6,
        Baja = 7,
        Puesto_En_Observacion = 8,
        Cancelado_Por_Excepcion = 9,
        Acreditado_en_CBU = 10,
        Devolucion_Bancaria = 11,
        Credito_Confirmado_MiAnses = 12,
        Rendido_para_transferencia = 13,
        Cancelado_por_DOA = 14,
        Baja_por_fallecido = 15,
        Baja_por_error_operativo = 16,
        Baja_por_desitimiento = 17,
        Baja_por_Fraude = 18,
        Baja_por_orden_judicial = 19,
        Baja_por_Cancelacion_anticipada = 20
    }

    [Serializable]
    public class Novedad
    {
        public long IdNovedad { get; set; }
        public long Cuil { get; set; }
        public string Nombre { get; set; }
        public long IdProducto { get; set; }
        public decimal ImporteTotal { get; set; }
        public string PrimerMensual { get; set; }
        public int Cantidad_Cuotas { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaNovedad { get; set; }
        public Turno Turno { get; set; }
        public enum_TipoEstadoNovedad Estado { get; set; }
        public Mensaje Mensaje { get; set; }
        public int VersionMutuo { get; set; }
        public string CodConceptoLiq { get; set; }
        public bool ImposibilidadFirma { get; set; }

        public Novedad() { }

        public Novedad(long idNovedad, long cuil, string nombre, long idProducto, decimal importeTotal, string primerMensual, int cantidad_cuotas, 
                        decimal importe, DateTime fecha, Turno turno, enum_TipoEstadoNovedad _estado, int _versionMutuo, bool _imposibilidadFirma)
        {
            this.IdNovedad = idNovedad;
            this.IdProducto = idProducto;
            this.ImporteTotal = importeTotal;
            this.PrimerMensual = primerMensual;
            this.Cantidad_Cuotas = cantidad_cuotas;
            this.Importe = importe;
            this.FechaNovedad = fecha;
            this.Turno = turno;
            this.Estado = _estado;
            this.Cuil = cuil;
            this.Nombre = nombre;
            this.VersionMutuo = _versionMutuo;
            this.ImposibilidadFirma = _imposibilidadFirma;
        }

        public Novedad(long idNovedad, long cuil, string nombre, long idProducto, decimal importeTotal, int cantidad_cuotas, decimal importe, enum_TipoEstadoNovedad _estado, DateTime _fechaNovedad, string _codConceptoLiq)
        {
            this.IdNovedad = idNovedad;
            this.IdProducto = idProducto;
            this.ImporteTotal = importeTotal;
            this.Cantidad_Cuotas = cantidad_cuotas;
            this.Importe = importe;
            this.Estado = _estado;
            this.Cuil = cuil;
            this.Nombre = nombre;
            this.FechaNovedad = _fechaNovedad;
            this.CodConceptoLiq = _codConceptoLiq;
        }

        public Novedad(long idNovedad, long cuil, string nombre, decimal importeTotal, int cantidad_cuotas, enum_TipoEstadoNovedad _estado, DateTime _fecha, string _codConceptoLiq)
        {
            this.IdNovedad = idNovedad;
            this.ImporteTotal = importeTotal;
            this.Cantidad_Cuotas = cantidad_cuotas;
            this.Estado = _estado;
            this.Cuil = cuil;
            this.Nombre = nombre;
            this.FechaNovedad = _fecha;
            this.CodConceptoLiq = _codConceptoLiq;
        }
    }

    #region Novedades Baja Suspension y rehabilitacion

    /**
     * Clase consulta de convedades 
     * para Baja - Suspension - Rehabilitacion
     * **/
    public class NovedadBSR
    {

        public int IdNovedad { get; set; }
        //public DateTime FechaAprobacion { get; set; }
        public long Cuil { get; set; }
        public string ApellidoYNombre { get; set; }
        public string CodigoDescuento { get; set; }
        public int IdEstadoNovedad { get; set; }
        public string EstadoNovedad { get; set; }
        //public decimal ImporteTotal { get; set; }
        //public int CantidadCuotas { get; set; }
        //public decimal MontoPrestamo { get; set; }

        public NovedadBSR()
        {

        }

        //public NovedadBSR(int idNovedad, DateTime fechaAprobacion, string codigoDescuento, int idEstadoNovedad,  string estadoNovedad, decimal importeTotal, int cantidadCuotas, decimal montoPrestamo)
        public NovedadBSR(int idNovedad, string codigoDescuento, int idEstadoNovedad, string estadoNovedad)
        {
            this.IdNovedad = idNovedad;
            //this.FechaAprobacion = fechaAprobacion;
            this.CodigoDescuento = codigoDescuento;
            this.IdEstadoNovedad = idEstadoNovedad;
            this.EstadoNovedad = estadoNovedad;
            //this.ImporteTotal = importeTotal;
            //this.CantidadCuotas = cantidadCuotas;
            //this.MontoPrestamo = montoPrestamo;
        }

    }


    public class ONovedadBSRPre : NovedadBSR
    {
        public DateTime FechaAprobacion { get; set; }
        public decimal ImporteTotal { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal MontoPrestamo { get; set; }

        public ONovedadBSRPre()
        {

        }

        public ONovedadBSRPre(int idNovedad, DateTime fechaAprobacion, long cuil, string nombreyApellido, string codigoDescuento, int idEstadoNovedad, string estadoNovedad, decimal importeTotal, int cantidadCuotas, decimal montoPrestamo)
        {
            IdNovedad = idNovedad;
            FechaAprobacion = fechaAprobacion;
            Cuil = cuil;
            ApellidoYNombre = nombreyApellido;
            CodigoDescuento = codigoDescuento;
            IdEstadoNovedad = idEstadoNovedad;
            EstadoNovedad = estadoNovedad;
            ImporteTotal = importeTotal;
            CantidadCuotas = cantidadCuotas;
            MontoPrestamo = montoPrestamo;
        }

    }


    public class ONovedadBSRPost : NovedadBSR
    {
        public int IdNovedadBaja { get; set; }
        public long cuilTomador { get; set; }
        public string NombreyApellido { get; set; }
        public Int16 CuotasPendiente { get; set; }
        public Int16 CuotasPaga { get; set; }
        public Int16 CuotasImpaga { get; set; }
        public Int16 CuotasEnviadaLiq { get; set; }
        //public decimal MontoCancelacion { get; set; }
        //public decimal? Monto { get; set; }
        //public string FechaCancelacion { get; set; }
        public string Fecha { get; set; }
        public string usuario { get; set; }
        public string oficina { get; set; }
        
        public ONovedadBSRPost()
        {

        }

        public ONovedadBSRPost(
            int idNovedadBaja, int idNovedad, int idEstadoNovedad, string DEstadoNovedad, long CuilTomador, string NombreyApellido, Int16 CuotasPendiente, Int16 CuotasPaga, Int16 CuotasImpaga,
            Int16 CuotasEnviadaLiq, string Fecha, string usuario, string oficina
            )
        {
            IdNovedadBaja = idNovedadBaja;
            IdNovedad = idNovedad;
            IdEstadoNovedad = idEstadoNovedad;
            cuilTomador = CuilTomador;
            this.CuotasEnviadaLiq = CuotasEnviadaLiq;
            this.CuotasImpaga = CuotasImpaga;
            this.CuotasPaga = CuotasPaga;
            this.CuotasPendiente = CuotasPendiente;
            EstadoNovedad = DEstadoNovedad;
            this.Fecha = Fecha;
            
            
            this.NombreyApellido = NombreyApellido;
            this.oficina = oficina;
            this.usuario = usuario;
        }

    }


    public class INovedadBSR 
    {
        public Int32 idNovedad { get; set; }
        public Int32? idEstadoOrigen { get; set; }
        public Int32 idEstadoDestino { get; set; }
        public Int32? idProducto { get; set; }
        
        //public decimal MontoCancelacion { get; set; }
        public decimal? Monto { get; set; }
        //public string FechaCancelacion { get; set; }
        public bool imposibilidadFirma { get; set; }

        public string xml { get; set; }
        public string expediente { get; set; }

        public string motivoSuspension { get; set; }
        public string usuario { get; set; }
        public string oficina { get; set; }
        public string ip { get; set; }

        public INovedadBSR()
        {

        }

        public INovedadBSR(
            Int16 idNovedad , Int16? idEstadoOrigen , Int16 idEstadoDestino , Int16? idProducto , Decimal? MontoSolicitado ,decimal? Monto , bool imposibilidadFirma 
            , string xml , string expediente ,  string motivoSuspension ,  string usuario , string oficina , string ip 
            )
        {
            
            this.expediente = expediente;
            this.idEstadoDestino = idEstadoDestino;
            this.idEstadoOrigen = idEstadoOrigen;
            this.idNovedad = idNovedad;
            this.idProducto = idProducto;
            this.imposibilidadFirma = imposibilidadFirma;
            this.ip = ip;
            this.Monto = Monto;
            this.motivoSuspension = motivoSuspension;
            this.oficina = oficina;
            this.usuario = usuario;
            this.xml = xml;
        }

    }

    [Serializable]
    public enum enum_TipoBSR
    {
        Baja = 0,
        Suspension = 1,
        Rehabilitacion = 2
    }

    public class ONovedadHistoEstados
    {
        public Int32 Tramite { get; set; }

        public Int32 NroCredito { get; set; }

        public Int32 idEstadoNovedad { get; set; }

        public string DescEstado { get; set; }
        public long cuilTomador { get; set; }

        public Int16? CuotasPendiente { get; set; }
        public Int16? CuotasPaga { get; set; }
        public Int16? CuotasImpaga { get; set; }
        public Int16? CuotasEnviadaLiq { get; set; }
        public string Fecha { get; set; }
        public string expediente { get; set; }
        public Int32? Mensual { get; set; }
        public string Motivo { get; set; }
        public string usuario { get; set; }
        public string oficina { get; set; }
        public string ip { get; set; }

        public ONovedadHistoEstados()
        {

        }

        public ONovedadHistoEstados(
            int tramite, int idEstadoNovedad, string DEstadoNovedad, long CuilTomador, int nroCredito, Int16? CuotasPendiente, Int16? CuotasPaga, Int16? CuotasImpaga,
            Int16? CuotasEnviadaLiq, string Fecha, string expediente, Int32? mensual, string motivo, string usuario, string oficina, string ip
            )
        {
            this.NroCredito = nroCredito;
            this.Tramite = tramite;
            this.idEstadoNovedad = idEstadoNovedad;
            this.DescEstado = DEstadoNovedad;
            this.cuilTomador = CuilTomador;
            this.CuotasPendiente = CuotasPendiente;
            this.CuotasPaga = CuotasPaga;
            this.CuotasImpaga = CuotasImpaga;
            this.CuotasEnviadaLiq = CuotasEnviadaLiq;
            this.Fecha = Fecha;
            this.expediente = expediente;
            this.Mensual = mensual;
            this.Motivo = motivo;

            this.oficina = oficina;
            this.usuario = usuario;
            this.ip = ip;
        }

    }

    [Serializable]
    public class NovedadSuspension
    {
        public int? Orden { get; set; }
        public int IdNovedad { get; set; }
        public int IdNovedadSuspension { get; set; }
        public int IdNovedadReactivacion { get; set; }
        public int IdEstadoNovedadSuspension { get; set; }
        public string EstadoNovedadSuspensionDescripcion { get; set; }
        public int IdEstadoNovedadReactivacion { get; set; }
        public string EstadoNovedadReactivacionDescripcion { get; set; }
        public long Cuil { get; set; }
        public decimal MontoPrestamo { get; set; }
        public string Expediente { get; set; }
        public DateTime FechaInicio { get; set; }
        public string MensualSuspension { get; set; }
        public string MotivoSuspension { get; set; }
        public string UsuarioSuspension { get; set; }
        public string OficinaSuspension { get; set; }
        public DateTime FechaReactivacion { get; set; }
        public string MensualReactivacion { get; set; }
        public string MotivoReactivacion { get; set; }
        public string UsuarioReactivacion { get; set; }
        public string OficinaReactivacion { get; set; }

        public NovedadSuspension()
        {
        }

        public NovedadSuspension(int _Orden, int _IdNovedad, int _IdNovedadSuspension, int _idNovedadReactivacion, int _IdEstadoNovedadSuspension, string _EstadoNovedadSuspensionDescripcion, long _Cuil, decimal _MontoPrestamo,
                                    int _IdEstadoNovedadReactivacion, string _EstadoNovedadReactivacionDescripcion,
                                    string _ExpedienteSuspension, DateTime _FechaSuspension, string _MensualSuspension, string _MotivoSuspension, string _UsuarioSuspension, string _OficinaSuspension,
                                    DateTime _FechaReactivacion, string _MensualReactivacion, string _MotivoReactivacion, string _UsuarioReactivacion, string _OficinaReactivacion)
        {
            this.Orden = _Orden;
            this.IdNovedad = _IdNovedad;
            this.IdNovedadSuspension = _IdNovedadSuspension;
            this.IdNovedadReactivacion = _idNovedadReactivacion;
            this.IdEstadoNovedadSuspension = _IdEstadoNovedadSuspension;
            this.EstadoNovedadSuspensionDescripcion = _EstadoNovedadSuspensionDescripcion;
            this.Cuil = _Cuil;
            this.MontoPrestamo = _MontoPrestamo;
            this.Expediente = _ExpedienteSuspension;
            this.FechaInicio = _FechaSuspension;
            this.MensualSuspension = _MensualSuspension;
            this.MotivoSuspension = _MotivoSuspension;
            this.UsuarioSuspension = _UsuarioSuspension;
            this.OficinaSuspension = _OficinaSuspension;
            this.IdEstadoNovedadReactivacion = _IdEstadoNovedadReactivacion;
            this.EstadoNovedadReactivacionDescripcion = _EstadoNovedadReactivacionDescripcion;
            this.FechaReactivacion = _FechaReactivacion;
            this.MensualReactivacion = _MensualReactivacion;
            this.MotivoReactivacion = _MotivoReactivacion;
            this.UsuarioReactivacion = _UsuarioReactivacion;
            this.OficinaReactivacion = _OficinaReactivacion;
        }

        public NovedadSuspension(int _IdNovedad, int _IdNovedadSuspension, int _idNovedadReactivacion, int _IdEstadoNovedadSuspension, string _EstadoNovedadSuspensionDescripcion, long _Cuil, decimal _MontoPrestamo,
                                    int _IdEstadoNovedadReactivacion, string _EstadoNovedadReactivacionDescripcion,
                                    string _ExpedienteSuspension, DateTime _FechaSuspension, string _MensualSuspension, string _MotivoSuspension, string _UsuarioSuspension, string _OficinaSuspension,
                                    DateTime _FechaReactivacion, string _MensualReactivacion, string _MotivoReactivacion, string _UsuarioReactivacion, string _OficinaReactivacion)
        {
            this.IdNovedad = _IdNovedad;
            this.IdNovedadSuspension = _IdNovedadSuspension;
            this.IdNovedadReactivacion = _idNovedadReactivacion;
            this.IdEstadoNovedadSuspension = _IdEstadoNovedadSuspension;
            this.EstadoNovedadSuspensionDescripcion = _EstadoNovedadSuspensionDescripcion;
            this.Cuil = _Cuil;
            this.MontoPrestamo = _MontoPrestamo;
            this.Expediente = _ExpedienteSuspension;
            this.FechaInicio = _FechaSuspension;
            this.MensualSuspension = _MensualSuspension;
            this.MotivoSuspension = _MotivoSuspension;
            this.UsuarioSuspension = _UsuarioSuspension;
            this.OficinaSuspension = _OficinaSuspension;
            this.IdEstadoNovedadReactivacion = _IdEstadoNovedadReactivacion;
            this.EstadoNovedadReactivacionDescripcion = _EstadoNovedadReactivacionDescripcion;
            this.FechaReactivacion = _FechaReactivacion;
            this.MensualReactivacion = _MensualReactivacion;
            this.MotivoReactivacion = _MotivoReactivacion;
            this.UsuarioReactivacion = _UsuarioReactivacion;
            this.OficinaReactivacion = _OficinaReactivacion;
        }

    }


    #endregion Novedades Baja Suspension y rehabilitacion
}
