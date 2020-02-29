using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class CuotaSocial : IDisposable
    {
        #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~CuotaSocial()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion


        private long codconceptoliq;
        private int idtipoconcepto;
        private Decimal valor;
        private Decimal porcentaje;
        private string error;


        public long CodConceptoLiq
        {
            get { return codconceptoliq; }
            set { codconceptoliq = value; }
        }

        public int IDTipoConcepto
        {
            get { return idtipoconcepto; }
            set { idtipoconcepto = value; }
        }

        public Decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

         public Decimal Porcentaje
        {
            get { return porcentaje; }
            set { porcentaje = value; }
        }

         public String Error
        {
            get { return error; }
            set { error = value; }
        }

        

        public CuotaSocial() { }

        public CuotaSocial(long CodConceptoLiq, int IDTipoConcepto, Decimal Valor, Decimal Porcentaje, string Error)
        {
            this.CodConceptoLiq = CodConceptoLiq;
            this.IDTipoConcepto= IDTipoConcepto; 
            this.Valor= Valor; 
            this.Porcentaje= Porcentaje;
            this.Error = Error;
        }
    }
}
