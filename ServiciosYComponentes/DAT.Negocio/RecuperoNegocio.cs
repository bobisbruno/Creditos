using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;

namespace Anses.DAT.Negocio
{
    public class RecuperoNegocio
    {
        public List<TipoMotivoRecupero> ListarTipoMotivoRecupero()
        {
            return new TipoMotivoRecuperoDao().Ejecutar_TipoMotivoRecupero_TT();
        }


        public List<TipoEstadoRecupero> ListarTipoEstadoRecupero()
        {
            return new TipoEstadoRecuperoDao().Ejecutar_TipoEstadoRecupero_TT();
        }

        public GestionRecuperoForm ListarRecuperosPorFiltro(FiltroDeRecuperos recuperos)
        {
            return new RecuperoDao().Ejecutar_Recupero_T(recuperos);
        }


        public decimal ObtenerValorMinimoDeRecuperoPorIdPrestador(int idPrestador)
        {
            return new RecuperoDao().Ejecutar_Recupero_ValorMinimo_T(idPrestador);
        }

        public List<ModalidadDePago> ListarModalidadDePago()
        {
            var modalidadDepagoList = new ModalidadDePagoDao().EjecutarTipoModalidadPago_TT();
            if (modalidadDepagoList.Count > 1)
            {
                modalidadDepagoList.Insert(0, new ModalidadDePago(-1, "Seleccione"));
            }
            return modalidadDepagoList;
        }

        public RecuperoDetalleForm ObtenerDatosDeRecuperoPorId(decimal idRecupero, out decimal valorResidualTotal)
        {
            return new RecuperoDao().Ejecutar_Recupero_TXId(idRecupero, out valorResidualTotal);
        }
    }
}
