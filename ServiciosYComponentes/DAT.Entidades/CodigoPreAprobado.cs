using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public enum TipoUsoCodPreAprobado
    {        
        Alta = 0,
        Reposicion = 1,
        Blanqueo = 2
    }
    
    [Serializable]
    public class CodigoPreAprobado
    {        
        public long Cuil { set; get; }
        public string CodigoAValidar { set; get; }
        public long? IdNovedad { set; get; }
        public TipoUsoCodPreAprobado unTipoUso { set; get; }
        public Auditoria UnAuditoria { set; get; }
       
        public CodigoPreAprobado() { }

        public CodigoPreAprobado(long _Cuil, string _CodigoAValidar, long? _IdNovedad, TipoUsoCodPreAprobado _unTipoUso, Auditoria _UnAuditoria) 
        {
            this.Cuil = _Cuil;
            this.CodigoAValidar = _CodigoAValidar;
            this.IdNovedad = _IdNovedad;
            this.unTipoUso = _unTipoUso;
            this.UnAuditoria = _UnAuditoria;
        }

        public CodigoPreAprobado(long _Cuil, string _CodigoAValidar, long? _IdNovedad, TipoUsoCodPreAprobado _unTipoUso)
        {
            this.Cuil = _Cuil;
            this.CodigoAValidar = _CodigoAValidar;
            this.IdNovedad = _IdNovedad;
            this.unTipoUso = _unTipoUso;
        }

        public CodigoPreAprobado(long _Cuil, string _CodigoAValidar)
        {
            this.Cuil = _Cuil;
            this.CodigoAValidar = _CodigoAValidar;            
        }
    }
}
