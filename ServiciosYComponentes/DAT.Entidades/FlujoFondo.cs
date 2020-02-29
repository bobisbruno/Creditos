using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class FlujoFondo: IDisposable
    {
        public String RazonSocial { get; set; }
        public Int32 mensual { get; set; }
        public Int64 cantCreditos { get; set; }
        public Decimal TotalMontoPrestamo { get; set; }
        public Decimal TotalImporteCuota { get; set; }
        public Decimal TotalGastoAdministrativo { get; set; }
        public Decimal TotalGastoAdmTarjeta { get; set; }
        public Decimal TotalSeguroVida { get; set; }
        public Decimal TotalAmortizacion { get; set; }
        public Decimal TotalInteres { get; set; }
        public int PrimerMensual { get; set; }
        public Decimal TotalInteresCuotaCero { get; set; }

        public FlujoFondo() { }

        public FlujoFondo(String _RazonSocial,
                          Int32 _mensual,
                          Int64 _cantCreditos,
                          Decimal _TotalMontoPrestamo,
                          Decimal _TotalImporteCuota,
                          Decimal _TotalGastoAdministrativo,
                          Decimal _TotalGastoAdmTarjeta,
                          Decimal _TotalSeguroVida,
                          Decimal _TotalAmortizacion,
                          Decimal _TotalInteres,
                          Decimal _TotalInteresCuotaCero) 
        {
            RazonSocial = _RazonSocial;
            mensual = _mensual;
            cantCreditos = _cantCreditos;
            TotalMontoPrestamo = _TotalMontoPrestamo;
            TotalImporteCuota = _TotalImporteCuota;
            TotalGastoAdministrativo = _TotalGastoAdministrativo;
            TotalGastoAdmTarjeta = _TotalGastoAdmTarjeta;
            TotalSeguroVida = _TotalSeguroVida;
            TotalAmortizacion = _TotalAmortizacion;
            TotalInteres = _TotalInteres;
            TotalInteresCuotaCero = _TotalInteresCuotaCero;
        }

        public FlujoFondo(int _primerMensual)
        {
            PrimerMensual = _primerMensual;        
        }

        #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~FlujoFondo()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
    }
}
