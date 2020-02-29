using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
   public class Documento
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionAbreviada { get; set; }

        public Documento(int id, string descripcion, string descripcionAbreviada)
        {
            Id = id;
            Descripcion = descripcion;
            DescripcionAbreviada = descripcionAbreviada; ;
        }

        public Documento()
        { }
    }
}
