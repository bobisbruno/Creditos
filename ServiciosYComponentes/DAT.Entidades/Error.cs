using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.XPath;
//using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Error
    {
        #region Variables miembro

        private string codigo = string.Empty;
        private string descripcion = string.Empty;
        private string tipoDeError = string.Empty;
        private string clase = string.Empty;
        private string metodo = string.Empty;
        private string tabla = string.Empty;
        private string descripcionConcatenada = string.Empty;        

        private Collection<object> parametros = new Collection<object>();
        //Agregado
        #endregion

        #region Propiedades

        public string Codigo
        {
            get
            {
                return codigo;
            }
            set
            {
                codigo = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return descripcion;
            }
            set
            {
                descripcion = value;
            }
        }

        public string TipoDeError
        {
            get
            {
                return tipoDeError;
            }
            set
            {
                tipoDeError = value;
            }
        }

        public string Clase
        {
            get
            {
                return clase;
            }
            set
            {
                clase = value;
            }
        }

        public string Metodo
        {
            get
            {
                return metodo;
            }
            set
            {
                Metodo = value;
            }
        }

        public string Tabla
        {
            get
            {
                return tabla;
            }
            set
            {
                tabla = value;
            }
        }        

        public string DescripcionConcatenada
        {
            get
            {

                return descripcionConcatenada;
            }
            set { descripcionConcatenada = value; }

        }

        public bool HuboError
        {
            get
            {
                return string.IsNullOrEmpty(Codigo);
            }
        }

        public Collection<object> Parametros
        {
            get { return parametros; }
            set { parametros = value; }
        }

        #endregion

        #region Constructores

        public Error()
        {
        }

        //Constructor que usado para poder loguear info de debu
        public Error(string codigo,
                     string descripcion,
                     string tipoDeError,
                     string clase,
                     string metodo,
                     string tabla,
                     string descripcionConcatenada,
                     string accion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.Metodo = metodo;
            this.Clase = clase;
            this.TipoDeError = tipoDeError;
            this.Tabla = tabla ;
            this.DescripcionConcatenada = descripcionConcatenada;
            this.Parametros = null;
        }

        public void EstablecerValores(string codigo,
                                      string clase,
                                      string metodo,
                                      Exception eError)
        {
            this.Codigo = codigo;
            this.Clase = clase;
            this.Metodo = metodo;
            this.DescripcionConcatenada = eError.Message + (eError);

            this.Tabla = string.Empty;
            this.Descripcion = string.Empty;
            this.Metodo = string.Empty;
            this.Descripcion = string.Empty;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ErrorHost:-{0} ErrorPC: {1} DescriHost: {2} DescriPC {3}", this.Codigo, this.Codigo, this.Descripcion, this.Descripcion);
        }

    }
}