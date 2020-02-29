using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Cuota
    {
        public long ID_Prestamo { get; set; }
        public int NroCuota { get; set; }
        public decimal Intereses { get; set; }
        public decimal Amortizacion { get; set; }
        public decimal Total_Amortizado { get; set; }
        public decimal Amortizacion_Remamente { get; set; }
        public decimal Cuota_Pura { get; set; }
        public decimal Gastos_Operativos { get; set; }
        public decimal Cuota_Total { get; set; }
        public decimal? Cuota_Pagada { get; set; }
        public decimal? Cuota_Impaga { get; set; }
        public decimal Seguro_De_Vida { get; set; }


        public Cuota() { }

        public Cuota(long id_prestamo, int nrocuota, decimal intereses, decimal amortizacion, decimal total_amortizado,
                        decimal _amortizacion_remanente, decimal cuota_pura, decimal gastos_operativos, 
                        decimal cuota_total, decimal cuota_pagada, decimal cuota_impaga, decimal seguro_de_vida)
        {
            this.ID_Prestamo = id_prestamo;
            this.NroCuota = nrocuota;
            this.Amortizacion = amortizacion;
            this.Intereses = intereses;
            this.Total_Amortizado = total_amortizado;
            this.Amortizacion_Remamente = _amortizacion_remanente;
            this.Cuota_Pura = cuota_pura;
            this.Gastos_Operativos = gastos_operativos;
            this.Cuota_Total = cuota_total;
            this.Cuota_Pagada = cuota_pagada;
            this.Cuota_Impaga = cuota_impaga;
            this.Seguro_De_Vida = seguro_de_vida;
        }
    }

    [Serializable]
    public class Cuota_Baja_Suspension
    {
        public int idCuotas { get; set; }
        public int idNovedad { get; set; } 
        public int IdNovedadDetalle { get; set; }
        public long CuilRelacionado { get; set; }
        public string EstadoCuota { get; set; }
        public byte NroCuota { get; set; }
        public int mensual { get; set; }
        public decimal? montoCuotaTotal { get; set; }
        
        public Cuota_Baja_Suspension() { }

        public Cuota_Baja_Suspension(int idCuotas, int idNovedad, int IdNovedadDetalle, long CuilRelacionado, string EstadoCuota, byte NroCuota, int mensual, decimal? montoCuotaTotal)
        {
            this.CuilRelacionado = CuilRelacionado;
            this.NroCuota = NroCuota;
            this.EstadoCuota = EstadoCuota;
            this.idCuotas = idCuotas;
            this.idNovedad = idNovedad;
            this.IdNovedadDetalle = IdNovedadDetalle;
            this.mensual = mensual;
            this.montoCuotaTotal = montoCuotaTotal;
        }
    }
}
