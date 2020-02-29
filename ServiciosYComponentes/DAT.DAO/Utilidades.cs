using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ANSES.Microinformatica.Lib.Seguridad;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Anses.Director.Session;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    /// <summary>
    /// Summary description for Utilidades.
    /// </summary>
    /// 
    [Serializable]
    internal class Utilidades
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Utilidades).Name);
        public Utilidades()
        {
        }

        #region public static string Calculo_MAC(string sDatosMac)

        public static string Calculo_MAC(string sDatosMac)
        {
            string clave = ConfigurationManager.AppSettings["DAT_ClaveMAC"];	//Obtiene la clave para la generacion del web.config del servicio
            string MAC = CrytoNet.MacSHA1(sDatosMac.ToString(), clave);
            return MAC;
        }
        #endregion

        #region static string Armo_String_MAC (Object[] datos)

        public static string Armo_String_MAC(Object[] datos)
        {
            StringBuilder sDatosMac = new StringBuilder();
            int longitud = datos.Length;
            for (int i = 0; i < longitud; i++)
            {
                sDatosMac.Append(datos[i]);
            }
            return sDatosMac.ToString();
        }
        #endregion

        #region string GeneraNombreArchivo (string nombreConsulta, long prestador, out string rutaArchivo)
        public static string GeneraNombreArchivo(string nombreConsulta, long prestador, out string rutaArchivo)
        {
           try
           {
                //System.IO.Directory.Exists(rutaArchivo)

                rutaArchivo = Settings.RutaArchivo();
                string nombreArchivo = nombreConsulta + "." + prestador.ToString() + "." + DateTime.Today.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss") + ".txt";

                if (!Directory.Exists(rutaArchivo))
                    Directory.CreateDirectory(rutaArchivo);

                if (File.Exists(Path.Combine(rutaArchivo, nombreArchivo)))
                    File.Delete(Path.Combine(rutaArchivo, nombreArchivo));
                return nombreArchivo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region void ComprimirArchivo (string rutaArchivo, string nombreArchivo)
        
        public static void ComprimirArchivo(string rutaArchivo, string nombreArchivo)
        {
            string archivo = Path.Combine(rutaArchivo, nombreArchivo);
            string archivoZip = archivo + ".zip";

            if (File.Exists(archivo))
            {
                FileStream fs = File.OpenRead(archivo);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                Compresor.GZipCompress(archivoZip, buffer);

                fs.Close();
            }

            //if (File.Exists(archivo))
            //{
                //Crc32 crc = new Crc32();
                //ZipOutputStream s = new ZipOutputStream(File.Create(archivoZip));
                //s.SetLevel(6); // 0 - store only to 9 - means best compression

                //FileStream fs = File.OpenRead(archivo);

                //byte[] buffer = new byte[fs.Length];
                //fs.Read(buffer, 0, buffer.Length);
                //ZipEntry entry = new ZipEntry(nombreArchivo);

                //entry.DateTime = DateTime.Now;
                //entry.Size = fs.Length;
                //fs.Close();

                //crc.Reset();
                //crc.Update(buffer);
                //entry.Crc = crc.Value;

                //s.PutNextEntry(entry);
                //s.Write(buffer, 0, buffer.Length);
                //s.Finish();
                //s.Close();
            //}
        }
        #endregion

        #region static void Borro_Archivo (string rutaArchivo)
        public static void BorrarArchivo(string rutaArchivo, string nombreArchivo)
        {
            if (File.Exists(rutaArchivo + nombreArchivo))
                File.Delete(rutaArchivo + nombreArchivo);
        }
        public static void BorrarArchivo(string rutaNombreArchivo)
        {
            if (File.Exists(rutaNombreArchivo))
                File.Delete(rutaNombreArchivo);
        }
        #endregion

        #region static void BorrarArchivosMasXDias (string rutaArchivo, byte cantDias
        public static void BorrarArchivosMasXDias(string rutaArchivo, byte cantDias)
        {
            DateTime fecha = DateTime.Today.AddDays(cantDias * -1);

            DateTime fechaCreacion;
            String[] archivos = Directory.GetFiles(rutaArchivo);
            string arch;
            foreach (string archivo in archivos)
            {
                arch = rutaArchivo + archivo;
                if (File.Exists(arch))
                {
                    fechaCreacion = File.GetCreationTime(arch);
                    if (fechaCreacion < fecha)
                    {
                        File.Delete(arch);
                    }
                }
            }
        }
        #endregion

        #region Valida Fecha

        public static bool EsFecha(string Valor)
        {
            DateTime unFechavalida;
            bool esValido = false;

            if (DateTime.TryParse(Valor, out unFechavalida))
            {
                String Patron = "^\\d{2}/\\d{2}/\\d{4}$";
                Regex ExpresionRegular = new Regex(Patron);
                esValido = ExpresionRegular.IsMatch(Valor);
            }

            return esValido;

            //return DateTime.TryParse(Valor, out unFechavalida);

        }

        public static bool esFechaValida(string Fecha)
        {
            if (Fecha.Length != 10)
                return false;

            string dia = Fecha.Trim().Substring(0, 2);
            string mes = Fecha.Trim().Substring(3, 2);
            string ano = Fecha.Trim().Substring(6, 4);

            if ((dia.Length == 0) || (dia.Length != 2) || (mes.Length == 0) || (mes.Length != 2) || (ano.Length == 0) || (ano.Length != 4))
                return false;
            else if ((!esNumerico(dia)) || (!esNumerico(mes)) || (!esNumerico(ano)))
                return false;

            else if ((int.Parse(mes) > 12) || (int.Parse(mes) < 1))
                return false;

            else if (int.Parse(dia) < 1)
                return false;

            else if ((int.Parse(mes) == 1) || (int.Parse(mes) == 3) || (int.Parse(mes) == 5) || (int.Parse(mes) == 7) || (int.Parse(mes) == 8) || (int.Parse(mes) == 10) || (int.Parse(mes) == 12))
            {
                if (int.Parse(dia) > 31)
                    return false;
            }
            else if ((int.Parse(mes) == 4) || (int.Parse(mes) == 6) || (int.Parse(mes) == 9) || (int.Parse(mes) == 11))
            {
                if (int.Parse(dia) > 30)
                    return false;
            }
            else
            {
                int anio = int.Parse(ano);
                if (((anio % 4) == 0) && (((anio % 100) != 0) || (anio % 400) == 0))
                {
                    if (int.Parse(dia) > 29)
                        return false;
                    else if (int.Parse(dia) > 28)
                        return false;
                }
            }
            return true;
        }


        #endregion Valida Fecha

        #region Valida Ingreso de Numeros

        public static bool esNumerico(string Valor)
        {
            bool ValidoDatos = false;

            Regex numeros = new Regex(@"^\d{1,}$");

            if (Valor.Length != 0)
            {
                ValidoDatos = numeros.IsMatch(Valor);
            }
            return ValidoDatos;
        }

        #endregion

        #region Es Numerio con decimales
        /// <summary>
        /// Controla si es un numero válido con o sin Decimales
        /// </summary>
        /// <param name="Valor"></param>
        /// <returns></returns>
        public static bool EsNumerioConDecimales(string Valor)
        {
            bool ValidoDatos = false;

            Regex numeros = new Regex(@"^\d{1,}\.\d{2}$|^\d{1,}$");

            if (Valor.Length != 0)
            {
                ValidoDatos = numeros.IsMatch(Valor);
            }
            return ValidoDatos;
            //			bool ValidoDatos=false;
            //			string Separador = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ;
            //			Regex numeros = new Regex(@"^\d{1,}\" + Separador + @"\d{2}$|^\d{1,}$");
            //			if ( Valor.Length != 0 )
            //			{
            //				ValidoDatos =numeros.IsMatch( Valor.Replace(",", Separador ).Replace(".",Separador ) ) ;
            //			}
            //			return ValidoDatos;
        }
        #endregion

        #region ValidaMail
        /// <summary>
        /// Controla si es un numero válido con o sin Decimales
        /// </summary>
        /// <param name="Valor"></param>
        /// <returns></returns>
        public static bool ValidaMail(string Valor)
        {
            bool ValidoDatos = false;

            Regex numeros = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})");

            if (Valor.Length != 0)
            {
                ValidoDatos = numeros.IsMatch(Valor);
            }
            return ValidoDatos;
        }

        #endregion

        #region ValidoCUIT - Validacion de CUIL

        public static bool ValidoCUIT(string CUIT)
        {
            string FACTORES = "54327654321";
            double dblSuma = 0;
            bool resul = false;

            if (!(CUIT.Substring(0, 1).ToString() != "3" && CUIT.Substring(0, 1).ToString() != "2"))
            {
                for (int i = 0; i < 11; i++)
                    dblSuma = dblSuma + int.Parse(CUIT.Substring(i, 1).ToString()) * int.Parse(FACTORES.Substring(i, 1).ToString());
            }
            resul = Modulo(dblSuma) == 0;
            return resul;

        }

        #region Modulo - Modulo 11 de un numero
        private static double Modulo(double numero)
        {
            double resul = Math.IEEERemainder(numero, 11);
            return resul;
        }
        #endregion

        #endregion

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
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
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

        public static Usuario GetUsuario()
        {
            Usuario usuario = new Usuario();
            usuario.Legajo = string.Empty;
            usuario.ApellidoNombre = string.Empty;
            usuario.OficinaCodigo = string.Empty;
            usuario.OficinaDescripcion = string.Empty;
            usuario.Ip = string.Empty;
            usuario.Grupo = string.Empty;
            try
            {
                IUsuarioToken usuarioEnDirector = new UsuarioToken();
                usuarioEnDirector.ObtenerUsuarioEnWs();
                if (usuarioEnDirector.IdUsuario != null)
                {
                    usuario.Legajo = usuarioEnDirector.IdUsuario;
                    usuario.ApellidoNombre = usuarioEnDirector.Nombre;
                    usuario.OficinaCodigo = usuarioEnDirector.Oficina;
                    usuario.OficinaDescripcion = usuarioEnDirector.OficinaDesc;
                    usuario.Ip = usuarioEnDirector.DirIP;
                    usuario.Grupo = ((GroupElement)(usuarioEnDirector.Grupos[0])).Name.ToString();
                }
            }
            catch
            {
            }
            return usuario;

        }
        
        public static String Encabezado_Archivo_CTACTE_Inventario()
        {
            string separador = Settings.DelimitadorCampo();
            String Titulo = String.Empty;

            Titulo = "CUIL"+separador+"BENEFICIO"+separador+"APELLIDO NOMBRE"+separador+ "NRO SC"+separador+"FECHA ALTA"+separador+ "COD. CONCEPTO LIQ."+separador+
                      "MONTO PRESTAMO"+separador+"CANT. CUOTAS"+separador+ "TNA"+separador+ "TOT. AMORTIZADO"+separador+ "TOT. RESIDUAL"+separador+ "COD. ESTADO"+separador+
                     "ESTADO DESCRIPCION"+separador+ "FECHA CAMBIO ESTADO"+separador + "CANT. CUOTAS S/LIQUIDAR";

            return Titulo; 
        }
    }
}

