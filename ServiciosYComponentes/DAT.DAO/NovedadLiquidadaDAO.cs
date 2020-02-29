using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class NovedadLiquidadaDAO
    {
        public NovedadLiquidadaDAO()
        { 
        }

        public static NovedadInfoAmpliada Trae(Int64 IdNovedad)
        {
            NovedadInfoAmpliada unNovedadInfoAmpliada = new NovedadInfoAmpliada();

            try
            {
                //TODO: en Construccion

            }
            catch (Exception ex)
            {   
                throw ex;            

            }

            return unNovedadInfoAmpliada; 
        }
   
   }
}
