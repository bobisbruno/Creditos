using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoImagen
    {
        public int IdTipoImagenDW { get; set; }
         public string DescripcionAbrev { get; set; }
         public string Descripcion { get; set; }

         public TipoImagen(){}

         public TipoImagen(int idTipoImagenDW, string descripcionAbrev, string descripcion)
         {
             IdTipoImagenDW = idTipoImagenDW;
             DescripcionAbrev = descripcionAbrev;
             Descripcion = descripcion;
         }
    }

    [Serializable]
    public class DocumentacionScaneada
        {
            public long Idnovedad { get; set; }
            public Byte[] Imagen { get; set; }    
            public long Cuil { get; set; }
            public string IdGralImagen { get; set; }
            public Guid IdImagen { get; set; }
            public TipoImagen TipoImagen { get; set; }
            public string Nombre { get; set; }
            public int Estado { get; set; }
            public DateTime FechaCreacion { get; set; }
                   
            #region Costructores

            public DocumentacionScaneada()
            { }

           #endregion
    }
}
