using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
//using System.Web.Caching;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.IO;

using System.Xml.Serialization;
using System.Xml;


namespace Anses.ArgentaC.Negocio
{
    

    /// <summary>
    /// Summary description for Utils
    /// </summary>
    /// 
    [Serializable]
    public class Utils
    {
        public static Exception Crear_Exeption(string Mensaje, string Codigo_Error)
        {
            Exception err = new Exception(Mensaje);
            err.Source = Codigo_Error;
            return err;
        }


        #region Convertir a Double - Respetando separador de decimales
        public static double ConvertToDouble(string Valor)
        {
            return double.Parse(RemplazaPuntoXComa(Valor));

        }
        public static string RemplazaPuntoXComa(string Valor)
        {
            string Separador = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            return Valor.Replace(",", Separador).Replace(".", Separador);

        }

        #endregion

        #region Diferencia entre 2 (dos) fechas

        public static int DateDiff(string FechaDesde, string FechaHasta)
        {
            try
            {
                TimeSpan Dif = DateTime.Parse(FechaHasta).Subtract(DateTime.Parse(FechaDesde));
                return Dif.Days;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion

        #region Valida Ingreso de Numeros

        public static bool esNumerico2(string Valor)
        {
            bool ValidoDatos = false;

            try
            {
                if (Valor.Length != 0)
                {
                    long valido = long.Parse(Valor);
                    ValidoDatos = true;
                }
            }
            catch(Exception)
            {
                ValidoDatos = false;
            }

            return ValidoDatos;

            
        }

        public static bool esNumerico(string Valor)
        {
            bool ValidoDatos = false;

            Regex numeros = new Regex(@"\d");

            if (Valor.Length != 0)
            {
                ValidoDatos = numeros.IsMatch(Valor);
            }
            return ValidoDatos;
        }

        #endregion

        #region CUIL
        /// <summary>
        /// Formatea un Numero de CUIL 12-12345678-1
        /// </summary>
        /// <param name="Numero">el Numero de Expdiente a formatear</param>
        /// <param name="PonerGiones">true para ponerle los giones</param>
        /// <returns>Número de Expediente formateado.</returns>
        public static string FormateoCUIL(string Numero, bool PonerGiones)
        {
            string sCUIL = Numero.Replace("-", "");

            if (!PonerGiones)
            {
                return sCUIL;
            }
            else
            {
                if (sCUIL.Length == 11)
                {
                    sCUIL = sCUIL.Substring(0, 2).ToString() + "-" + sCUIL.Substring(2, 8).ToString() +
                            "-" + sCUIL.Substring(10, 1).ToString();
                }
            }
            return sCUIL;
        }

        public static bool ValidaCUIL(string CUIL)
        {

            string patron = @"^\d{11}$";
            Regex re = new Regex(patron);

            bool resp = re.IsMatch(CUIL);

            if (resp)
            {

                string FACTORES = "54327654321";
                double dblSuma = 0;

                if (!(CUIL.Substring(0, 1).ToString() != "3" && CUIL.Substring(0, 1).ToString() != "2"))
                {
                    for (int i = 0; i < 11; i++)
                        dblSuma = dblSuma + int.Parse(CUIL.Substring(i, 1).ToString()) * int.Parse(FACTORES.Substring(i, 1).ToString());
                }
                resp = Math.IEEERemainder(dblSuma, 11) == 0;


            }


            return resp;

        }

        #endregion

        #region Formateo Expediente
        /// <summary>
        /// Formatea un Numero de Expediente 012-12-12345678-1-123-123456
        /// </summary>
        /// <param name="Numero">el Numero de Expdiente a formatear</param>
        /// <param name="PonerGiones">true para ponerle los giones</param>
        /// <returns>Número de Expediente formateado.</returns>
        public static string FormateoExpediente(string Numero, bool PonerGiones)
        {
            string Expediente = Numero.Replace("-", "");

            //viene con guiones
            if (!PonerGiones)
            {
                return Expediente;
            }
            else if (Expediente.Length == 23)
            {
                Expediente = Expediente.Substring(0, 3).ToString() + "-" + Expediente.Substring(3, 2).ToString() +
                             "-" + Expediente.Substring(5, 8).ToString() + "-" + Expediente.Substring(13, 1).ToString() +
                             "-" + Expediente.Substring(14, 3).ToString() + "-" + Expediente.Substring(17, 6).ToString();
            }

            return Expediente;
        }
        #endregion

        #region formato para Error

        public static string ErrorGenerico()
        {
            return "No se pudo realizar la operación<br />Reintente en otro momento";
        }

        public static string FormatoError(string Error)
        {
            return "Errores detectados:<div style='margin-left:20px; margin-bottom:10px'> " + Error + "</div>";
        }

        #endregion

       
        #region Validacion del Formato Mail

        public static bool validaMail(string mail)
        {
            return Regex.IsMatch(mail, @"\b[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b");
        }
        #endregion

       

        #region calcularEdad
        public static Int16 calcularEdad(DateTime fechaNacimiento, DateTime aFecha)
        {
            Int16 _edad;
            DateTime hoy = aFecha;

            _edad = Convert.ToInt16(hoy.Year - fechaNacimiento.Year);
            if (hoy.Month < fechaNacimiento.Month || (hoy.Month == fechaNacimiento.Month && hoy.Day < fechaNacimiento.Day)) _edad--;
            return _edad;
        }

        #endregion
        
        #region FormateoTelefono
        //public static string FormateoTelefono(ArgentaWS.Telefono unTelefono, bool conGuiones)
        //{
        //    if (unTelefono != null && !string.IsNullOrEmpty(unTelefono.Numero))
        //        if (conGuiones)
        //            return "(" + unTelefono.Codigo + ") - " + unTelefono.Prefijo + " - " + unTelefono.Numero;
        //        else
        //            return unTelefono.Codigo + unTelefono.Prefijo + unTelefono.Numero;

        //    else
        //        return "";

        //}
        #endregion
                
              

        #region IsInCache - Abre tablas que seran utilizadas frecuentemente

        /*
        public enum enum_Tablas_InCache
        {
            BANCOS,
            PROVINCIAS

            
        }

        /// <summary>
        /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
        /// </summary>
        /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla
        ///	con un tiempo por default de 10 Minutos.</param>
        /// <returns>Un DataSet</returns>
        public static object IsInCache(enum_Tablas_InCache NombreTabla)
        {
            return IsInCache(NombreTabla, 5);
        }

        /// <summary>
        /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
        /// </summary>
        /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
        /// <param name="TiempoMinCache">Tipo int. Representa el tiempo en Minutos del cache</param>		
        /// <returns>Un DataSet</returns>
        public static object IsInCache(enum_Tablas_InCache NombreTabla, int TiempoMinCache)
        {
            return IsInCache(NombreTabla, TiempoMinCache, null, false);
        }

        /// <summary>
        /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
        /// </summary>
        /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
        /// <param name="Refrescar">Tipo Boolean. Representa si se desea volver a traer los datos desde la base</param>
        /// <returns>Un DataSet</returns>
        public static object IsInCache(enum_Tablas_InCache NombreTabla, bool Refrescar)
        {
            return IsInCache(NombreTabla, 5, null, Refrescar);
        }

        /// <summary>
        /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
        /// </summary>
        /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
        /// <param name="TiempoMinCache">Tipo int. Representa el tiempo en Minutos del cache</param>		
        /// <param name="Refrescar">Tipo Boolean. Representa si se desea volver a traer los datos desde la base</param>
        /// <returns>Un DataSet</returns>
        //public static object IsInCache(string NombreTabla, int TiempoMinCache, Object[] aParam, bool Refrescar)
        public static object IsInCache(enum_Tablas_InCache NombreTabla, int TiempoMinCache, Object[] aParam, bool Refrescar)
        {
            object datos = new object();
            System.Web.HttpContext oContext = System.Web.HttpContext.Current;

            // Elimino del Cache el Item seleccionado para volver a cargarlo.
            if (Refrescar)
            { oContext.Cache.Remove(NombreTabla.ToString()); }

            if (oContext.Cache[NombreTabla.ToString()] != null)
            {
                datos = (object)oContext.Cache[NombreTabla.ToString()];
            }
            else
            {
                //string Valor;
                switch (NombreTabla.ToString())
                {
                    case  "BANCOS":
                        //List<ArgentaWS.Banco> unaListaBancos = InvocaWsDao.Bancos_Traer_Todos();
                       // datos = (object)unaListaBancos;
                        break;


                    case "PROVINCIAS":
                        List<ADPDescripcionesWS.ProvinciaDTO> lst = invoca_ADPDescripciones.Provincias_Traer_Todas();
                         datos = (object)lst;
                        break;

                    
                }

                // Agrego al Cache
                oContext.Cache.Insert(NombreTabla.ToString(), datos, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(TiempoMinCache));
            }

            return datos;
        }

        public static object IsInCache(string NombreTabla, Object[] aParam)
        {
            object datos = new object();

            if (aParam != null && aParam[0] != null)
            {
                try
                {
                    switch (NombreTabla)
                    {                      

                        case "PROVINCIAS":
                            List<ADPDescripcionesWS.ProvinciaDTO> lstProvincias = new List<ADPDescripcionesWS.ProvinciaDTO>();
                            lstProvincias = (List<ADPDescripcionesWS.ProvinciaDTO>)IsInCache(enum_Tablas_InCache.PROVINCIAS, 5, null, false);
                            short unaProvincia = 0;
                            short.TryParse(aParam[0].ToString(), out unaProvincia);
                            datos = lstProvincias;
                            if (datos != null)
                                datos = lstProvincias.Find(r => r.codigoProvincia == unaProvincia);
                            break;
                       
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }




            return datos;
        }

         */
        #endregion  IsInCache - Abre tablas que seran utilizadas frecuentemente

       
        #region HayRepeticionNumerica
        public static bool HayRepeticionNumerica(string Valor)
        {
            long b = 0;
            for (int i = 0; i < Valor.Length; i++)
            {
                b = Valor.LongCount(letra => letra.ToString() == ((char)Valor.ToString()[i]).ToString());
                //verifica que no se repita + del 60% de lo ingresado
                if (b * 100 / Valor.ToString().Length >= 80)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        

        public static string Obtengo_Dias(string Dias)
        {

            string Texto = string.Empty;

            if (Dias != "")
            {
                int valor = int.Parse(Dias);

                Texto = ((valor & 1) > 0) ? "Lun " : "";
                Texto += ((valor & 2) > 0) ? "Mar " : "";
                Texto += ((valor & 4) > 0) ? "Mier " : "";
                Texto += ((valor & 8) > 0) ? "Jue " : "";
                Texto += ((valor & 16) > 0) ? "Vie " : "";
                Texto += ((valor & 32) > 0) ? "Sáb " : "";
                Texto += ((valor & 64) > 0) ? "Dom " : "";
                Texto += ((valor & 128) > 0) ? "Feriados" : "";

                if (Texto.Equals("Lun Mar Mier Jue Vie Sáb Dom Feriados"))
                {                    
                    Texto = "Todos Feriados";
                }
                else if (Texto.Equals("Lun Mar Mier Jue Vie Sáb Dom"))
                    Texto = "Todos";

                

               // if(Texto.Length>0 && )

            }
            return Texto;
        }

        #region Serialize Objects

        public static String SerializeObject(Object pObject)
        {
            try
            {
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(pObject.GetType());
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
                xmlNamespaces.Add(string.Empty, string.Empty);

                xs.Serialize(xmlTextWriter, pObject, xmlNamespaces);

                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        #endregion Serialize Objects

        #region Convertir string en formato yyyyMMdd a datetime
        public static DateTime ConvertirAFecha(string fecha)
        {
            DateTime dt = DateTime.ParseExact(fecha, "yyyyMMdd", CultureInfo.InvariantCulture);
            return dt;
        }
        #endregion


     
       
        #region Devuelve el primer dia del mes actual
        public static DateTime ObtenerPrimeroDelMes()
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, now.Month, 1);
        }
        #endregion

        #region Devuelve el primer dia del mes que recibe como parametro
        public static DateTime ObtenerPrimeroDelMes(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, 1);
        }
        #endregion
    

    }
  
}

