using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class FiltroDeRecuperos
    {
        private Int64 cuil;
        private ComboBoxItem motivo;
        private ComboBoxItem estado;
        private decimal valorResidualDesde;
        private decimal valorResidualHasta;

        public Int64 Cuil
        {
            get { return  cuil; }
            set { cuil = value; }
        }
        public ComboBoxItem Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }
        public ComboBoxItem Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        public decimal ValorResidualDesde
        {
            get { return valorResidualDesde; }
            set { valorResidualDesde = value; }
        }
        public decimal ValorResidualHasta
        {
            get { return valorResidualHasta; }
            set { valorResidualHasta = value; }
        }
    }
}
