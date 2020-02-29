using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoEstado_SC
    {
        public int idEstadoSC { get; set; }
        public string descripcion { get; set; }
        public bool novedadEstaVigente { get; set; }

        public TipoEstado_SC() { }

        public TipoEstado_SC(int _idEstadoSC, string _descripcion, bool _novedadEstaVigente) 
        {
            idEstadoSC = _idEstadoSC;
            descripcion = _descripcion;
            novedadEstaVigente = _novedadEstaVigente;
        }

        public TipoEstado_SC(int _idEstadoSC, string _descripcion )
        {
            idEstadoSC = _idEstadoSC;
            descripcion = _descripcion;            
        }
    }
}
