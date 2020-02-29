using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class RecuperoDetalle
    {
        public int IdRecupero { get; set; }
        public DateTime FechaDeAltaDeRecupero { get; set; }
        public int IdEstadoRecupero { get; set; }
        public string DescripcionEstadoRecupero { get; set; }
        public DateTime FechaEstadoDeRecupero { get; set; }
        public Decimal Cuil { get; set; }
        public string NumeroDeExpediente { get; set; }
        public Nullable<DateTime> FechaDeCaratulacion { get; set; }
        public Nullable<DateTime> FechaTeoricaDeAudiencia { get; set; }
        public Nullable<Int64> IdUdaiAudiencia { get; set; }
        public string DescripcionUdai { get; set; }
        public Nullable<Int64> IdRegionalAudiencia { get; set; }
        public string DescripcionRegional { get; set; }
        public string OperadorAudiencia { get; set; }
        public string Observaciones { get; set; }
        public bool EnDccYEE { get; set; }
        public bool EnRegional { get; set; }
        public bool FueNotificado { get; set; }
        public bool FueAcordado { get; set; }
        public bool ConcurrioAudiencia { get; set; }
        public bool DesconoceDeuda { get; set; }
        public bool ImposibilidadDePago { get; set; }

    }
}
