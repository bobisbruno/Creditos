using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Auditoria : IDisposable
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

        ~Auditoria()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion                    

        #region Errores de Clase
        public class AuditoriaException : System.ApplicationException
        {
            public AuditoriaException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion

        #region Propiedades
        private string codigo_sistema;
        private string cuit_autenticador;
        private decimal cuit_organismo;
        private decimal dependencia;
        private string autenticacion;

        private string usuario;
        private string codigo_usuario;
        private int codigo_oficina;
        private string ip_origen;

        private string tabla;
        private string tipo_accion;
        private string entorno_ejecucion;
        private string nombre_servicio;
        private string nombre_metodo;
        private DateTime fecha_hora;
        private string datos;
        //private SSOToken token;
        private string cfg_database;
        private string cfg_storeproc;
        private DateTime? fecUltimaModificacion;
        #endregion

        #region Seters & Geters

        public int IDOficina
        {
            get
            {
                return codigo_oficina;
            }
            set
            {
                codigo_oficina = value;
            }
        }

        public string CodigoSistema
        {
            get
            {
                return codigo_sistema;
            }
            set
            {
                codigo_sistema = value;
            }
        }

        public string IP
        {
            get
            {
                return ip_origen;
            }
            set
            {
                ip_origen = value;
            }
        }

        public decimal CuitOrganismo
        {
            get
            {
                return cuit_organismo;
            }
            set
            {
                cuit_organismo = value;
            }
        }

        public decimal DependenciaOficina
        {
            get
            {
                return dependencia;
            }
            set
            {
                dependencia = value;
            }
        }

        public string CuitAutenticador
        {
            get
            {
                return cuit_autenticador;
            }
            set
            {
                cuit_autenticador = value;
            }
        }

        public string Autenticacion
        {
            get
            {
                return autenticacion;
            }
            set
            {
                autenticacion = value;
            }
        }

        public string Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                usuario = value;
            }
        }

        public string CodigoUsuario
        {
            get
            {
                return codigo_usuario;
            }
            set
            {
                codigo_usuario = value;
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

        public string TipoAccion
        {
            get
            {
                return tipo_accion;
            }
            set
            {
                tipo_accion = value;
            }
        }

        public string EntornoEjecucion
        {
            get
            {
                return entorno_ejecucion;
            }
            set
            {
                entorno_ejecucion = value;
            }
        }

        public string NombreServicio
        {
            get
            {
                return nombre_servicio;
            }
            set
            {
                nombre_servicio = value;
            }
        }

        public string NombreMetodo
        {
            get
            {
                return nombre_metodo;
            }
            set
            {
                nombre_metodo = value;
            }
        }

        public DateTime FechaHora
        {
            get
            {
                return fecha_hora;
            }
            set
            {
                fecha_hora = value;
            }
        }

        public string Datos
        {
            get
            {
                return datos;
            }
            set
            {
                datos = value;
            }
        }

        public string Cfg_Database
        {
            get
            {
                return cfg_database;
            }
            set
            {
                cfg_database = value;
            }
        }

        public string Cfg_StoreProc
        {
            get
            {
                if (string.IsNullOrEmpty(cfg_storeproc))
                    return "GuardarLog";

                return cfg_storeproc;
            }
            set
            {
                cfg_storeproc = value;
            }
        }
       
        public DateTime? FecUltimaModificacion
        {
            get { return fecUltimaModificacion; }
            set { fecUltimaModificacion = value; }
        }
       
        #endregion

        public Auditoria() 
        {
            CodigoSistema = string.Empty;
            CuitOrganismo = 0;
            CuitAutenticador = string.Empty;
            Autenticacion = string.Empty;
            NombreServicio = string.Empty;
            NombreMetodo = string.Empty;
            Usuario = string.Empty;
            CodigoUsuario = string.Empty;  
            IP = string.Empty;
            IDOficina = 0;

            EntornoEjecucion = string.Empty;
            TipoAccion = string.Empty;
            Tabla = string.Empty;
            FechaHora = new DateTime();
            Datos = string.Empty;
            FecUltimaModificacion = new DateTime?();
        }

        public Auditoria(string usuario)
        {
            Usuario = usuario;

            CodigoSistema = string.Empty;
            CuitOrganismo = 0;
            CuitAutenticador = string.Empty;
            Autenticacion = string.Empty;
            NombreServicio = string.Empty;
            NombreMetodo = string.Empty;            
            IP = string.Empty;
            IDOficina = 0;

            EntornoEjecucion = string.Empty;
            TipoAccion = string.Empty;
            Tabla = string.Empty;
            FechaHora = new DateTime();
            Datos = string.Empty;
            this.FecUltimaModificacion = new DateTime?();
        }

        public Auditoria(string usuario, string ip, DateTime? fecUltimaModificacion)
        {
            this.Usuario = usuario;
            this.IP  = ip;
            this.IDOficina = 0;
            this.FecUltimaModificacion = FecUltimaModificacion;
        }

        public Auditoria(string usuario, string ip, int idOficina, DateTime? fecUltimaModificacion)
        {
            this.Usuario = usuario;
            this.IP = ip;
            this.IDOficina = idOficina;
            this.FecUltimaModificacion = FecUltimaModificacion;            
        }
        
        public Auditoria(string pcodigo_sistema,
                         decimal pcuit_organismo,
                         string pcuit_autenticador,
                         string pmetodo_autenticacion,
                         string pnombre_servicio,
                         string pnombre_metodo,
                         string pcodigo_usuario,
                         string usuario,
                         string pip_origen,
                         int poficina)
        {
            CodigoSistema = pcodigo_sistema;
            CuitOrganismo = pcuit_organismo;
            CuitAutenticador = pcuit_autenticador;
            Autenticacion = pmetodo_autenticacion;
            NombreServicio = pnombre_servicio;
            NombreMetodo = pnombre_metodo;
            Usuario = usuario;
            CodigoUsuario = pcodigo_usuario;
            IP = pip_origen;
            IDOficina = poficina;

            EntornoEjecucion = string.Empty;
            TipoAccion = string.Empty;
            Tabla = string.Empty;
            FechaHora = new DateTime();
            Datos = string.Empty;
        }
    }
}