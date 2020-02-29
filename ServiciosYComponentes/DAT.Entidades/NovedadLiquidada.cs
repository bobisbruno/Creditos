using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class NovedadLiquidada
    {
        public Int64 IdNovedad { set; get; }
        public int PeriodoLiq { set; get; }
        public Double ? ImporteCuota {set;get;}   
        public Int16? NroCuotaLiq { set; get; }
        public Int16? IdEstadoNov { set; get; }
        public Double? ImporteALiq { set; get; }
        public Double? ImporteLiq { set; get; }
        public Boolean PorcentajeNoCalc { set; get; }
        public String IdMensaje { set; get; }
        public String Mensaje { set; get; }
        public Double? ImporteInteres { set; get; }
        public Double? Amortizacion { set; get; }
        public Double? GastoAdmMensualCalc { set; get; }
        public Double? MontoCuotaTotal { set; get; }
        public Double? TotalAmortizado { set; get; }
        public Double? SeguroVida { set; get; }
        public Double? GastoAdminTarjeta { set; get; }
        public int? MensualEmision { set; get; }
        public string TipoLiq { set; get; }
        public Double? AmortizacionPagado { set; get; }
        public Double? ImporteInteresPagado { set; get; }
        public Double? SeguroVidaPagado { set; get; }
        public Double? GastoAdmMensualPagado { set; get; }
        public Double? GastoAdminTarjetaPagado { set; get; }
        public String IdEntPago { set; get; }
        public TipoConcepto unTipoConcepto { set; get;}
 
        #region Constructores
        public NovedadLiquidada() { }
        public NovedadLiquidada( Int64 _IdNovedad,
                                 int   _PeriodoLiq,
                                 Double ? _ImporteCuota,  
                                 Int16? _NroCuotaLiq,	
                                 Int16? _IdEstadoNov,
                                 Double? _ImporteALiq,
                                 Double? _ImporteLiq,
                                 Boolean _PorcentajeNoCalc,
                                 String _IdMensaje,
                                 String _Mensaje	,
                                 Double? _ImporteInteres,
                                 Double? _Amortizacion,
                                 Double? _GastoAdmMensualCalc,
                                 Double? _MontoCuotaTotal,
                                 Double? _TotalAmortizado,
                                 Double? _SeguroVida,
                                 Double? _GastoAdminTarjeta,
                                 int? _MensualEmision,
                                 string _TipoLiq,
                                 Double? _AmortizacionPagado,
                                 Double? _ImporteInteresPagado,
                                 Double? _SeguroVidaPagado,
                                 Double? _GastoAdmMensualPagado,
                                 Double? _GastoAdminTarjetaPagado ,
                                 String _IdEntPago                                 
                                  )
        {
        
         IdNovedad = _IdNovedad;
         PeriodoLiq = _PeriodoLiq;
         ImporteCuota =_ImporteCuota;  
         NroCuotaLiq = _NroCuotaLiq ;	
         IdEstadoNov = _IdEstadoNov;
         ImporteALiq = _ImporteALiq;
         ImporteLiq = _ImporteLiq;
         PorcentajeNoCalc = _PorcentajeNoCalc;
         IdMensaje = _IdMensaje;
         Mensaje = _Mensaje	;
         ImporteInteres =  _ImporteInteres;
         Amortizacion = _Amortizacion;
         GastoAdmMensualCalc = _GastoAdmMensualCalc  ;
         MontoCuotaTotal = _MontoCuotaTotal;
         TotalAmortizado = _TotalAmortizado;
         SeguroVida = _SeguroVida;
         GastoAdminTarjeta = _GastoAdminTarjeta;
         MensualEmision = _MensualEmision;
         TipoLiq = _TipoLiq;
         AmortizacionPagado = _AmortizacionPagado;
         ImporteInteresPagado = _ImporteInteresPagado;
         SeguroVidaPagado = _SeguroVidaPagado;
         GastoAdmMensualPagado = _GastoAdmMensualPagado;
         GastoAdminTarjetaPagado  = _GastoAdminTarjetaPagado ;
         IdEntPago = _IdEntPago;
         
        }
        #endregion
    }

   

    [Serializable]    
    public class NovedadHistorica
    { 
        public long IdNovedad {set;get;}
        public Int16  IdEstadoReg {set;get;}
        public String DescripcionEstadoReg {set;get;}
        public Int16 CodMovimiento { set; get; }
        public String DescMovimiento { set; get; }
        public int ? Mensual { set; get; }
        public Double? ImporteCuota { set; get; }
        public int ? NroCuotaLiq {set;get;}
        public DateTime  FecUltModificacion {set;get;}
        public String UsuarioBaja {set;get;}
        public Double? ImporteInteres { set; get; }
        public Double? Amortizacion { set; get; }
        public Double? GastoAdmMensualCalc { set; get; }
        public Double? MontoCuotaTotal { set; get; }
        public Double? TotalAmortizado { set; get; }
        public Double? SeguroVida { set; get; }
        public Double? GastoAdminTarjeta { set; get; }         


     #region Constructores

     public NovedadHistorica()
     { 
     }

     public NovedadHistorica(   long _IdNovedad,
                                Int16 _IdEstadoReg,
                                String _DescripcionEstadoReg,
                                Int16 _CodMovimiento,
                                String _DescMovimiento,
                                int? _Mensual,
                                Double? _ImporteCuota,
                                int? _NroCuotaLiq,
                                DateTime _FecUltModificacion,
                                String _UsuarioBaja,
                                Double? _ImporteInteres,
                                Double? _Amortizacion,
                                Double? _GastoAdmMensualCalc,
                                Double? _MontoCuotaTotal,
                                Double? _TotalAmortizado,
                                Double? _SeguroVida,
                                Double? _GastoAdminTarjeta
                           ) 
                   { 
                     
                            IdNovedad = _IdNovedad ;
                            IdEstadoReg = _IdEstadoReg;
                            DescripcionEstadoReg = _DescripcionEstadoReg ;
                            CodMovimiento = _CodMovimiento;
                            DescMovimiento = _DescMovimiento; 
                            Mensual = _Mensual; 
                            ImporteCuota = _ImporteCuota;  
                            NroCuotaLiq = _NroCuotaLiq;
                            FecUltModificacion = _FecUltModificacion;
                            UsuarioBaja = _UsuarioBaja;
                            ImporteInteres = ImporteInteres; 
                            Amortizacion = _Amortizacion; 
                            GastoAdmMensualCalc = _GastoAdmMensualCalc; 
                            MontoCuotaTotal = _MontoCuotaTotal; 
                            TotalAmortizado = _TotalAmortizado; 
                            SeguroVida = _SeguroVida;
                            GastoAdminTarjeta = _GastoAdminTarjeta;      
                           
                   }

     #endregion

    }


    [Serializable]
    public class NovedadInfoAmpliada
    {
        public Novedad_Info unNovedad_Info { set; get; }
        public List<Cuota> Cuotas { set; get; }
        public List<NovedadLiquidada> NovedadedLiquidadas { set; get; }
        public List<NovedadHistorica> NovedadHistoricas { set; get; }

        public NovedadInfoAmpliada()
        {
        }

        public NovedadInfoAmpliada(Novedad_Info _unNovedad_Info, 
                                   List<Cuota> _Cuotas, 
                                   List<NovedadLiquidada> _NovedadedLiquidadas, 
                                   List<NovedadHistorica> _NovedadHistoricas)
        {
            unNovedad_Info = _unNovedad_Info;
            Cuotas = _Cuotas;
            NovedadedLiquidadas = _NovedadedLiquidadas;
            NovedadHistoricas = _NovedadHistoricas;
        }

    }
}
