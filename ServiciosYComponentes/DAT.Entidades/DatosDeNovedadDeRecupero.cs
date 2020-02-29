using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class DatosDeNovedadDeRecupero
    {
        public long IdNovedad{ get; set; }
        public DateTime VinculacionNovedadRecupero{ get; set; }
        public Nullable<DateTime> FechaDeNovedad{ get; set; }
        public Decimal? ValorResidual{ get; set; }
        public int CodigoConceptoLiquidacion{ get; set; }
        public string RazonSocial{ get; set; }
        public Decimal? MontoDelPrestamo{ get; set; }
        public int? CantidadDeCuotas{ get; set; }
        public long? IdBeneficiario{ get; set; }
        public int? PeriodoBajaBeneficiario{ get; set; }
        public int? IdMotivoBajaBeneficiario{ get; set; }
        public string MotivoBajaBeneficiario{ get; set; }
        public string OficinaDeBaja{ get; set; }
        public int? PeriodoDeReactivacion{ get; set; }
        public long IdPrestador { get; set; }
        public int? RecuperaSobreConcepto { get; set; }

        public DatosDeNovedadDeRecupero(){}

    }
}
