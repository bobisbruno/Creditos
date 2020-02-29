using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class BeneficioDisponible
    {
        public long IdBeneficiario { get; set; }
        public bool AfectacionDisponible { get; set; }

        public BeneficioDisponible(){ }
        public BeneficioDisponible(long idBeneficiario, bool afectacionDisponible)
        {
            IdBeneficiario = idBeneficiario;
            AfectacionDisponible = afectacionDisponible;
        }
    }
}
