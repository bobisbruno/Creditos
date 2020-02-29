using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;

namespace Anses.DAT.Negocio
{
    public class PinLinkNegocio
    {
        public static string PinLink_Alta(PinLink unPinLink, CodigoPreAprobado unCodigoPreAprobado)        
        {
            String mjeRdo = String.Empty;
            try
            {
                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    mjeRdo = PinLinkDAO.PinLink_Alta(unPinLink);
                    if (unCodigoPreAprobado != null)
                        mjeRdo += CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Modificacion(unCodigoPreAprobado);
                    oTransactionScope.Complete();
                }
                return mjeRdo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
   }
}
