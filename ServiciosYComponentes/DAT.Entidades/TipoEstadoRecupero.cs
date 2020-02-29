using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoEstadoRecupero
    {
        public int idEstadorecupero { get; set; }
        public string descripcionEstadoRecupero { get; set; }
        public bool EnDCCyEE { get; set; }
        public bool enRegional { get; set; }
        public bool FueNotificado { get; set; }
        public bool Acordado { get; set; }
        public bool EtapaExtrajudicial { get; set; }
        public bool EtapaJudicial { get; set; }
        public bool Habilitado { get; set; }
        public bool HabilitadoWeb { get; set; }
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }

    }
}
