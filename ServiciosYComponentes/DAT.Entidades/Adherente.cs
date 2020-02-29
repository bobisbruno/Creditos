using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Adherente : IDisposable
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

        ~Adherente()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private long _CUIL;
        private string _Apellido_Nombre;
        private string _IP;
        private DateTime _FechaUltModificacion;
        private string _Usuario;

        public long CUIL
        {
            get { return _CUIL; }
            set { _CUIL = value; }
        }
        public string Apellido_Nombre
        {
            get { return _Apellido_Nombre; }
            set { _Apellido_Nombre = value; }
        }
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }
        public DateTime FechaUltModificacion
        {
            get { return _FechaUltModificacion; }
            set { _FechaUltModificacion = value; }
        }
        public string Usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }

        public Adherente() { }

        public Adherente(long CUIL, string Apellido_Nombre, string ip, DateTime FechaUltModificacion, string Usuario)
        {
            this.CUIL = CUIL;
            this.Apellido_Nombre = Apellido_Nombre;
            this.IP = ip;
            this.FechaUltModificacion = FechaUltModificacion;
            this.Usuario = Usuario;
        }

    }


    #region Errores de Clase
    [Serializable]
    public class AdherenteException : System.ApplicationException
    {
        public AdherenteException(string mensaje)
            : base(mensaje)
        {
        }
    }

    #endregion
}