using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    public class ContenedorDeCuotas
    {
        public int concepto { get; set; }
        public int cuotas1 { get; set; }
        public int cuotas12 { get; set; }
        public int cuotas24 { get; set; }
        public int cuotas36 { get; set; }
        public int cuotas40 { get; set; }
        public int cuotas48 { get; set; }
        public int cuotas60 { get; set; }
       
        public int total { get; set; }

        public ContenedorDeCuotas() { }

        public ContenedorDeCuotas(int concepto, int cuotas1, int cuotas12, int cuotas24,
                                  int cuotas36, int cuotas40, int cuotas48, int cuotas60)
        {
            this.concepto = concepto;
            this.cuotas1 = cuotas1;
            this.cuotas12 = cuotas12;
            this.cuotas24 = cuotas24;
            this.cuotas36 = cuotas36;
            this.cuotas40 = cuotas40;
            this.cuotas48 = cuotas48;
            this.cuotas60 = cuotas60;

            total = cuotas1 + cuotas12 + cuotas24 + cuotas36 + cuotas40 + cuotas48 + cuotas60;
        }
    }
}
