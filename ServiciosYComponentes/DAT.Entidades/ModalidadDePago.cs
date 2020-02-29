using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class ModalidadDePago
    {
        public int Id { get; set; }
        public string Descripcion {get;set;}

        public ModalidadDePago() { }

        public ModalidadDePago(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }
    }
}
