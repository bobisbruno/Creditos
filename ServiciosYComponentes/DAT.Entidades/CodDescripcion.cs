using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{

    [Serializable]
    public class CodDescripcion
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public CodDescripcion() { }
        public CodDescripcion(int _codigo, string _descripcion) {
            this.Codigo = _codigo;
            this.Descripcion = _descripcion;
        }
    }
}
