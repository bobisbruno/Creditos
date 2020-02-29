using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic;


namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public class Financiera
    {

        public static double CalcularTIR_Variable(
            double dTotal,
            double[] dCuotas,
            int iGracia,
            double guess)
        {
            double[] dValores = new double[dCuotas.Length + iGracia + 1];
            dValores[0] = -dTotal;
            double res = -1;

            try
            {

                for (int i = 0; i < iGracia; i++)
                {
                    dValores[i + 1] = 0.0;
                }
                for (int i = iGracia; i < dCuotas.Length + iGracia; i++)
                {
                    dValores[i + 1] = dCuotas[i - iGracia];
                }

                res = Financial.IRR(ref dValores, guess);

                return res;
            }
            catch (Exception)
            {
                return res;
            }

        }

        public static double CalcularTIR_Fijo(double dTotal,
            double dValorCuota,
            int iGracia,
            int TotalCuotas,
            double guess)
        {
            double[] dValores = new double[TotalCuotas + iGracia + 1];
            dValores[0] = -dTotal;
            double res = -1;

            try
            {

                for (int i = 0; i < iGracia; i++)
                {
                    dValores[i + 1] = 0.0;
                }
                for (int i = iGracia; i < TotalCuotas + iGracia; i++)
                {
                    dValores[i + 1] = dValorCuota;
                }
                
                res = Financial.IRR(ref dValores, guess);

                return res;
            }
            catch (Exception)
            {
                return res;
            }

        }

        public static double CalcularTEA(double TIR)
        {
            return Math.Pow((1 + TIR), 12) - 1;
        }

        public static double CalcularTNA(double TEA)
        {

            double res;

            //NOTA: Esto '0.0833333333333333', deberia ser 1/12 pero me redondea y no me sirve. Por ahora, lo dejo asi.
            res = Math.Pow((1 + TEA), 0.0833333333333333);

            return (res - 1) * 12;
        }

        public static double PrimerAmortizacion(double monto, double TNA, double cantCuotas)
        {
            double TEM = (TNA / 12) / 100;

            return (monto * TEM) / (Math.Pow(1 + TEM, cantCuotas) - 1);

        }

        public static double AmortizacionEnPeriodoX(double monto, double cantCuotas, double TNA, double nroCuota)
        {
            double TEM = (TNA / 12) / 100;
            double PrimerAmort = PrimerAmortizacion(monto, TNA, cantCuotas);
            return PrimerAmort * Math.Pow(1 + TEM, nroCuota - 1);

        }

        public static double InteresEnPeriodoX(double cuotaPura, double amortizacionX)
        {
            return cuotaPura - amortizacionX;
        }

        public static double InteresEnPeriodoX(double monto, double cantCuotas, double TNA, double nroCuota)
        {
            return CuotaPura(monto, cantCuotas, TNA) - AmortizacionEnPeriodoX(monto, cantCuotas, TNA, nroCuota);

        }

        public static double CuotaPura(double monto, double cantCuotas, double TNA)
        {
            double TEM = (TNA / 12) / 100;
            return monto * TEM * Math.Pow((1 + TEM), cantCuotas) / (Math.Pow(1 + TEM, cantCuotas) - 1);

        }


        public static double PrimerInteres(double monto, double TNA)
        {
            return (monto * (TNA / 12)) / 100;
        }

    }
}
