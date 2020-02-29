using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class NovedadTotal
    {
        public int NumeroEstado { get; set; }
        public int IdBeneficiario { get; set; }
        public List<ContenedorDeCuotas> ContenedoresDeCuotas { get; set; }
        public string Descripcion { get; set; }
        public int Total1Cuotas { get; set; }
        public int Total12Cuotas { get; set; }
        public int Total24Cuotas{get;set;}
        public int Total36Cuotas { get; set; }
        public int Total40Cuotas{get;set;}
        public int Total48Cuotas{get;set;}
        public int Total60Cuotas { get; set; }

        public int TotalAcumulado { get; set; }


        public NovedadTotal() { }


        public NovedadTotal(int numeroEstado, List<ContenedorDeCuotas> contenedoresDeCuotas)
        {
            this.NumeroEstado = numeroEstado;
            this.ContenedoresDeCuotas = contenedoresDeCuotas;
            foreach (ContenedorDeCuotas contenedorCuotas in contenedoresDeCuotas)
            {
                Total1Cuotas += contenedorCuotas.cuotas1;
                Total12Cuotas += contenedorCuotas.cuotas12;
                Total24Cuotas += contenedorCuotas.cuotas24;
                Total36Cuotas += contenedorCuotas.cuotas36;
                Total40Cuotas += contenedorCuotas.cuotas40;
                Total48Cuotas += contenedorCuotas.cuotas48;
                Total60Cuotas += contenedorCuotas.cuotas60;

                TotalAcumulado += Total1Cuotas + Total12Cuotas + Total24Cuotas + Total36Cuotas + Total40Cuotas + Total48Cuotas + Total60Cuotas;
            }
        }

        public NovedadTotal(int numeroEstado, int IdBeneficiario, string descripcion, int concepto, int cuota1, int cuota12, int cuota24,
                            int cuota36, int cuota40, int cuota48, int cuotas60)
        {
            NumeroEstado = numeroEstado;
            this.IdBeneficiario = IdBeneficiario;
            Descripcion = descripcion;
            ContenedoresDeCuotas = new List<ContenedorDeCuotas>();
            AgregarContenedorDeCuotas(new ContenedorDeCuotas(concepto, cuota1, cuota12, cuota24, cuota36, cuota40, cuota48,cuotas60));
        }

        public void AgregarContenedorDeCuotas(ContenedorDeCuotas contenedorDeCuotas)
        {
            ContenedorDeCuotas contenedorDeCuotasExistente = traerContenedorDeCuotasExistente(ContenedoresDeCuotas, contenedorDeCuotas.concepto);
            if (contenedorDeCuotasExistente != null)
            {
                contenedorDeCuotasExistente.cuotas1 += contenedorDeCuotas.cuotas1;
                contenedorDeCuotasExistente.cuotas12 += contenedorDeCuotas.cuotas12;
                contenedorDeCuotasExistente.cuotas24 += contenedorDeCuotas.cuotas24;
                contenedorDeCuotasExistente.cuotas36 += contenedorDeCuotas.cuotas36;
                contenedorDeCuotasExistente.cuotas40 += contenedorDeCuotas.cuotas40;
                contenedorDeCuotasExistente.cuotas48 += contenedorDeCuotas.cuotas48;
                contenedorDeCuotasExistente.cuotas60 += contenedorDeCuotas.cuotas60;

                contenedorDeCuotasExistente.total += contenedorDeCuotas.cuotas1 + contenedorDeCuotas.cuotas12 + contenedorDeCuotas.cuotas24 +
                                                     contenedorDeCuotas.cuotas36 +  contenedorDeCuotas.cuotas40 + contenedorDeCuotas.cuotas48 +
                                                     contenedorDeCuotas.cuotas60;
            }
            else
            {
                ContenedoresDeCuotas.Add(contenedorDeCuotas);
            }
            Total1Cuotas += contenedorDeCuotas.cuotas1;
            Total12Cuotas += contenedorDeCuotas.cuotas12;
            Total24Cuotas += contenedorDeCuotas.cuotas24;
            Total36Cuotas += contenedorDeCuotas.cuotas36;
            Total40Cuotas += contenedorDeCuotas.cuotas40;
            Total48Cuotas += contenedorDeCuotas.cuotas48;
            Total60Cuotas += contenedorDeCuotas.cuotas60;

            TotalAcumulado += Total1Cuotas + Total12Cuotas + Total24Cuotas + Total36Cuotas + Total40Cuotas + Total48Cuotas + Total60Cuotas;
        }

        public ContenedorDeCuotas traerContenedorDeCuotasExistente(List<ContenedorDeCuotas> contenedoresDeCuota, int concepto)
        {
            return contenedoresDeCuota.Exists(n => n.concepto == concepto) ? contenedoresDeCuota.Find(n => n.concepto == concepto) : null;
        }
    }                         
}                             
