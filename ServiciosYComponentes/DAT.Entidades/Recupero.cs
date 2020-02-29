using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Recupero
    {
        public decimal Cuil { get; set; }
        public string ApellidoYNombre { get; set; }
        public decimal ValorResidual { get; set; }
        public int IdMotivoRecupero { get; set; }
        public string DescripcionMotivoDeRecupero { get; set; }
        public int IdEstadoDeRecupero { get; set; }
        public string DescripcionEstadoDeRecupero { get; set; }
        public DateTime FechaDeEstadoDeRecupero { get; set; }
        public int CantidadDeCreditos { get; set; }
        public decimal IdDeRecupero { get; set; }
    }
}
