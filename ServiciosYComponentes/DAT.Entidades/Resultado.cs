using System;
using System.Collections.Generic;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable()]
    public class Resultado<T, TDatoUnico>
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

        ~Resultado()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Métodos
        public bool HuboError
        {
            get
            {
                return Error.HuboError;
            }
        }
        #endregion

        #region Variables miembro
        private Error error;
        private List<T> datos;
        private TDatoUnico datoUnico;
        #endregion

        #region Propiedades
        public Error Error
        {
            get
            {
                if (error == null)
                    error = new Error();
                return error;
            }
            set
            {
                error = value;
            }
        }

        public List<T> Datos
        {
            get
            {
                if (datos == null)
                    datos = new List<T>();
                return datos;
            }
            set
            {
                datos = value;
            }
        }

        public TDatoUnico DatoUnico
        {
            get
            {
                return datoUnico;
            }
            set
            {
                datoUnico = value;
            }
        }
        #endregion
    }

    [Serializable()]
    public class Resultado<T>
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

        ~Resultado()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Métodos
        public bool HuboError
        {
            get
            {
                return Error.HuboError;
            }
        }
        #endregion

        #region Variables miembro
        private Error error;
        private List<T> datos;
        #endregion

        #region Propiedades
        public Error Error
        {
            get
            {
                if (error == null)
                    error = new Error();
                return error;
            }
            set
            {
                error = value;
            }
        }

        public List<T> Datos
        {
            get
            {
                if (datos == null)
                    datos = new List<T>();
                return datos;
            }
            set
            {
                datos = value;
            }
        }

        public bool HayDatos
        {
            get
            {
                return Datos.Count > 0;
            }
        }
        #endregion
    }

    [Serializable()]
    public class ResultadoUnico<TDatoUnico>
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

        ~ResultadoUnico()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Variables miembro
        private Error error;
        private TDatoUnico datoUnico;
        #endregion

        #region Propiedades
        public Error Error
        {
            get
            {
                if (error == null)
                    error = new Error();
                return error;
            }
            set
            {
                error = value;
            }
        }

        public TDatoUnico DatoUnico
        {
            get
            {
                return datoUnico;
            }
            set
            {
                datoUnico = value;
            }
        }

        public bool HuboError
        {
            get
            {
                return Error.HuboError;
            }
        }
        #endregion
    }

    [Serializable()]
    public class ResultadoUnico<TDatoUnico, TDatoUnico2>
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

        ~ResultadoUnico()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Variables miembro
        private Error error;
        private TDatoUnico datoUnico;
        private TDatoUnico2 datoUnico2;
        #endregion

        #region Propiedades
        public Error Error
        {
            get
            {
                if (error == null)
                    error = new Error();
                return error;
            }
            set
            {
                error = value;
            }
        }

        public TDatoUnico DatoUnico
        {
            get
            {
                return datoUnico;
            }
            set
            {
                datoUnico = value;
            }
        }

        public TDatoUnico2 DatoUnico2
        {
            get
            {
                return datoUnico2;
            }
            set
            {
                datoUnico2 = value;
            }
        }

        public bool HuboError
        {
            get
            {
                return Error.HuboError;
            }
        }
        #endregion
    }
}