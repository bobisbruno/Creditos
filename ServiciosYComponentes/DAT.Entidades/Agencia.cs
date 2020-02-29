using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Agencia : ValidaEntidad<Agencia>, IDisposable 
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

        ~Agencia()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private int idAgencia;
        private String descripcion;
        private int? nroLegajo;
        private string nroCuenta;
        private bool habilitada;
        private bool esMayorista;
        private string cuit;


        #region Getters y Setters    
    
        public string Cuit
        {
            get { return cuit; }
            set { cuit = value; }
        }

        public bool EsMayorista
        {
            get { return esMayorista; }
            set { esMayorista = value; }
        }

        public bool Habilitada
        {
            get { return habilitada; }
            set { habilitada = value; }
        }

        public string NroCuenta
        {
            get { return nroCuenta; }
            set { nroCuenta = value; }
        }


        public int? NroLegajo
        {
            get { return nroLegajo; }
            set { nroLegajo = value; }
        }


        public int IdAgencia
        {
            get { return idAgencia; }
            set { idAgencia = value; }
        }

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        #endregion

        public Agencia() 
        {
            IdAgencia = 0;
            Descripcion = string.Empty;  
        }

        public Agencia(int idAgencia, String descripcion)
        {
            this.idAgencia = idAgencia;
            this.Descripcion = descripcion;
        }

        #region Errores de Clase
        public class AgenciaException : System.ApplicationException
        {
            public AgenciaException(string mensaje)
                : base(mensaje)
            {
            }
        }

        #endregion
    }   
 }