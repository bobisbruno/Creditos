using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class EstadoReclamo : Estado
    {

        #region Private Get/Set

        /* Almacena Datos para cada reclamo*/
        public DateTime FecCambio { get; set; }
        public string observacion { get; set; }

        /*Configuracion para cada estado de reclamo*/
        public bool TieneObservacion { get; set; }
        public bool PaseAutomatico { get; set; }
        public bool EsFinal { get; set; }
        public bool FecManual { get; set; }
        public string MensajeInfo { get; set; }
        public string Control { get; set; }
        public string ControlTexto { get; set; }
        public int ControlIdModelo { get; set; }
        public int EstadoAnme { get; set; }

        #endregion
    }
}
