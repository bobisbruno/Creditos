using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Tipo
    {
        public long Id {get;set;}
        public string Descripcion { get; set; }
        public Tipo() { }
        public Tipo(long _id, string _descripcion) {
            this.Id = _id;
            this.Descripcion = _descripcion;
        }

    }

    [Serializable]
    public class TipoRechazoExpediente : Tipo
    {
        public bool PideNroResolucion { get; set; }

        public TipoRechazoExpediente() { }

        public TipoRechazoExpediente(Tipo tipo, bool _pideNroResolucion):
            base (tipo.Id,tipo.Descripcion)
        {
            PideNroResolucion = _pideNroResolucion;
        }

        public TipoRechazoExpediente(Tipo tipo) : base(tipo.Id, tipo.Descripcion)
        {} 
    }
}
