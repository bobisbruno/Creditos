using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoDocumentoPresentado
    {
        public Int16 IdTipoDocPresentado {get;set;}
        public string DescTipoDocPresentado { get; set; }
        public bool Habilitado { get; set; }
        public bool HabilitadoWeb { get; set; }
        public bool HabilitadoPreAprobado { get; set; }
        public bool HabilitadoComercio { get; set; }
        public bool HabilitadoMayor75 { get; set; }
        public bool HabilitadoMenor75 { get; set; }
                
        public TipoDocumentoPresentado() { }

        public TipoDocumentoPresentado(Int16 idTipoDocPresentado, string descTipoDocPresentado, bool habilitado,
                                       bool habilitadoWeb, bool habilitadoPreAprobado, bool habilitadoComercio, bool habilitadoMayor75, bool habilitadoMenor75)
        {
            IdTipoDocPresentado = idTipoDocPresentado;
            DescTipoDocPresentado = descTipoDocPresentado;
            Habilitado = habilitado;
            HabilitadoWeb = habilitadoWeb;
            HabilitadoPreAprobado = habilitadoPreAprobado;
            HabilitadoComercio = habilitadoComercio;
            HabilitadoMayor75 = habilitadoMayor75;
            HabilitadoMenor75 = habilitadoMenor75;
        }
    }   
}
