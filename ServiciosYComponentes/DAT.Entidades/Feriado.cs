using System;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Feriado
    {
        public DateTime Fecha { get; set; }
        public String Usuario { get; set; }
        public String IP { get; set; }
        public String Oficina { get; set; }
        public DateTime FecUltModificacion { get; set; }

        public Feriado()
        { }

        public Feriado(DateTime _Fecha, String _Usuario, String _IP, String _Oficina, DateTime _FecUltModificacion)
        {
            Fecha = _Fecha;
            Usuario = _Usuario;
            IP = _IP;
            Oficina = _Oficina;
            FecUltModificacion = _FecUltModificacion; 
        }

        public Feriado(DateTime _Fecha)
        {
            Fecha = _Fecha;         
        }

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

        ~Feriado()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
    }
}
