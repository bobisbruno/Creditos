using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;


// token
using Anses.Director.Session;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Ar.Gov.Anses.AUDao
{

    /// <summary>
    /// SeguridadLog es una clase que loguea en tabla de Auditoria
    /// Esta puede ser incluida dentro de otra clase para trabajar de una forma casi transparente
    /// VERSION: 20-11-09
    /// </summary>
    public class SeguridadLog
    {

        #region Propiedades
        private string codigo_sistema;
		private string ip_origen;
		private decimal cuit_organismo;
		private decimal dependencia;
		private string autenticador;
		private string codigo_usuario;
		private string tabla;
		private string tipo_accion;
		private string entorno_ejecucion;
		private string nombre_servicio;
		private string nombre_metodo;
		private DateTime fecha_hora;
        private string datos;
        private SSOToken token;
        private string cfg_database;
        private string cfg_storeproc;
        #endregion

        #region Seters&Geters
        public string Codigo_sistema
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
        public string Ip_origen
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
        public decimal Cuit_organismo
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
        public decimal Dependencia
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
        public string Autenticador
        {
            get
            {
                return autenticador;
            }
            set
            {
                autenticador = value;
            }
        }
        public string Codigo_usuario
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
        public string Tipo_accion
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
        public string Entorno_ejecucion
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
        public string Nombre_servicio
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
        public string Nombre_metodo
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
        public DateTime Fecha_hora
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
        /// <summary>
        /// Trae el token si previamente se ejecuto correctamente el metodo traerDatosToken
        /// </summary>
        public SSOToken Token
        {
            get
            {
                return token;
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
        #endregion

        #region Contructors
        public SeguridadLog(string pcodigo_sistema, string pip_origen, 
                            decimal pcuit_organismo, decimal pdependencia,
                            string pautenticador, string pcodigo_usuario, 
                            string ptabla, string ptipo_accion,
                            string pentorno_ejecucion, string pnombre_servicio, 
                            string pnombre_metodo, DateTime pfecha_hora, 
                            string pdatos)
        {
            Codigo_sistema = pcodigo_sistema;
            Ip_origen = pip_origen;
            Cuit_organismo = pcuit_organismo;
            Dependencia = pdependencia;
            Autenticador = pautenticador;
            Codigo_usuario = pcodigo_usuario;
            Tabla = ptabla;
            Tipo_accion = ptipo_accion;
            Entorno_ejecucion = pentorno_ejecucion;
            Nombre_servicio = pnombre_servicio;
            Nombre_metodo = pnombre_metodo;
            Fecha_hora = pfecha_hora;
            Datos = pdatos;
        }

        public SeguridadLog()
        {
        }
        #endregion

        #region Fxs

        /// <summary>
        /// Intenta obtener el token del soap header del web service
        /// </summary>
        /// <returns></returns>
        public bool traerDatosToken()
        {

            bool bRta = false;
            try
            {
                byte[] data = new byte[Convert.ToInt32(System.Web.HttpContext.Current.Request.InputStream.Length)];
                System.Web.HttpContext.Current.Request.InputStream.Position = 0;
                System.Web.HttpContext.Current.Request.InputStream.Read(data, 0, Convert.ToInt32(System.Web.HttpContext.Current.Request.InputStream.Length));
                UTF8Encoding encoding = new UTF8Encoding();
                string decodedString = encoding.GetString(data);

                // cargo el soap xml
                XmlDataDocument myXmlDocument = new XmlDataDocument();
                myXmlDocument.LoadXml(decodedString);
                XmlNodeList xmlToken = myXmlDocument.GetElementsByTagName("token");

                // genero el token
                SSOEncodedToken encToken = new SSOEncodedToken();
                encToken.Token = xmlToken.Item(0).InnerText;
                token = Credencial.ObtenerCredencialEnWs(encToken);

                bRta = true;
            }
            catch (Exception ex)
            {
                bRta = false;
            }

            return bRta;
        }

        /// <summary>
        /// Guarda la estructura de la clase en la tabla auditoria
        /// </summary>
        /// <param name="usaToken">utiliza los datos obtenidos del token si previamente se ejecuto el metodo traerDatosToken</param>
        /// <param name="usaHttpContext">utiliza el contexto web para sacar automaticamente el nombre del webservice y webmethod</param>
        /// <returns></returns>
        public string guardarLog(bool usaToken, bool usaHttpContext)
        {
            string sRta = "";
            Cfg_Database = "DAT_V01";
            Database db = DatabaseFactory.CreateDatabase(Cfg_Database);
            DbCommand dbCommand = db.GetStoredProcCommand(Cfg_StoreProc);

            try
            {

                if (usaToken)
                {
                    // vacia las variables
                    Ip_origen = "";
                    Dependencia = 0;
                    Codigo_usuario = "";
                    Cuit_organismo = 0;
                    Autenticador = "";
                    decimal tmpDecimal;

                    foreach (InfoElement iElem in Token.Operation.Login.Info)
                    {
                        switch (iElem.Name.ToLower())
                        { 
                            case "ip":
                                // pone el ip
                                Ip_origen = iElem.Value.Trim();
                                break;

                            case "oficina":
                                // pone la dependencia
                                tmpDecimal=0;
                                if (decimal.TryParse(iElem.Value.Trim(), out tmpDecimal))
                                    Dependencia = tmpDecimal;
                                break;
                        }

                    }


                    if (usaHttpContext)
                    {
                        // utiliza el contexto web para sacar automaticamente el nombre del webservice y webmethod
                        Nombre_servicio = HttpContext.Current.Request.Params["PATH_INFO"];
                        Nombre_metodo = HttpContext.Current.Request.Params["HTTP_SOAPACTION"];

                        // saco el formato
                        if (Nombre_servicio.LastIndexOf(".") != -1 && Nombre_servicio.LastIndexOf("/") != -1)
                            Nombre_servicio = Nombre_servicio.Substring(0, Nombre_servicio.LastIndexOf(".")).Substring(Nombre_servicio.LastIndexOf("/") + 1);
                        if (Nombre_metodo.LastIndexOf("\"") != -1 && Nombre_metodo.LastIndexOf("/") != -1)
                            Nombre_metodo = Nombre_metodo.Substring(0, Nombre_metodo.LastIndexOf("\"")).Substring(Nombre_metodo.LastIndexOf("/") + 1);
                    }

                    // pone el usuario
                    Codigo_usuario = Token.Operation.Login.UId;

                    // pone el cuit org
                    tmpDecimal = 0;
                    if (decimal.TryParse(Token.Operation.Login.Entity, out tmpDecimal))
                        Cuit_organismo = tmpDecimal;

                    // pone el autenticador
                    Autenticador = (Token.Operation.Login.AuthenticationMethod.Length > 2 ? Token.Operation.Login.AuthenticationMethod.Substring(0, 2) : Token.Operation.Login.AuthenticationMethod);

                }

                db.AddInParameter(dbCommand, "@codigo_sistema", DbType.String, Codigo_sistema);
                db.AddInParameter(dbCommand, "@ip_origen", DbType.String, Ip_origen);
                db.AddInParameter(dbCommand, "@cuit_organismo", DbType.Decimal, Cuit_organismo);
                db.AddInParameter(dbCommand, "@dependencia", DbType.Decimal, Dependencia);
                db.AddInParameter(dbCommand, "@autenticador", DbType.String, Autenticador);
                db.AddInParameter(dbCommand, "@codigo_usuario", DbType.String, Codigo_usuario);
                db.AddInParameter(dbCommand, "@tabla", DbType.String, Tabla);
                db.AddInParameter(dbCommand, "@tipo_accion", DbType.String, Tipo_accion);
                db.AddInParameter(dbCommand, "@entorno_ejecucion", DbType.String, Entorno_ejecucion);
                db.AddInParameter(dbCommand, "@nombre_servicio", DbType.String, Nombre_servicio);
                db.AddInParameter(dbCommand, "@nombre_metodo", DbType.String, Nombre_metodo);
                db.AddInParameter(dbCommand, "@fecha_hora", DbType.DateTime, Fecha_hora);
                db.AddInParameter(dbCommand, "@datos", DbType.String, Datos);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                sRta = ex.Message;
                //throw ex;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return sRta;

        }

        #endregion
    }
}
