using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class RecuperoDetalleForm
    {
        public List<DatosDeNovedadDeRecupero> NovedadesList { get; set; }
        public List<BeneficioDisponible> BeneficioDisponibleList { get; set; }

        public RecuperoDetalleForm() { }
        public RecuperoDetalleForm(List<DatosDeNovedadDeRecupero> novedadesList, List<BeneficioDisponible> beneficioDisponibleList)
        {
            this.NovedadesList = novedadesList;
            BeneficioDisponibleList = beneficioDisponibleList;
        }

    }
}
