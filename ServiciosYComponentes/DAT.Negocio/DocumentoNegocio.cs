using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;

namespace Anses.DAT.Negocio
{
    public class DocumentoNegocio
    {
        public List<Documento> ListarDocumentos(Nullable<int> id)
        {
            return new DocumentoDao().EjecutarDocumento_T(id);
        }

    }
}
