using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;
using Anses.Director.Session;
using Anses.ArgentaC;
using Ar.Gov.Anses.Microinformatica;

using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Data;
using System.Security.Cryptography;


namespace Anses.ArgentaC.Comun.Domain
{
    public class Utils : DomainObject
    {
        public static Usuario GetUsuario()
        {
            Usuario usuario = new Usuario();
            usuario.UsuarioDesc = string.Empty;
            usuario.Nombre = string.Empty;
            usuario.Oficina = string.Empty;
            usuario.OficinaDesc = string.Empty;
            usuario.Ip = string.Empty;
            usuario.CueOCuit = string.Empty;
            usuario.Establecimiento = string.Empty;
            try
            {
                IUsuarioToken usuarioEnDirector = new UsuarioToken();
                usuarioEnDirector.ObtenerUsuarioEnWs();
                if (usuarioEnDirector.IdUsuario != null)
                {


                    usuario.UsuarioDesc = usuarioEnDirector.IdUsuario;
                    usuario.Nombre = usuarioEnDirector.Nombre;
                    usuario.Oficina = usuarioEnDirector.Oficina;
                    usuario.OficinaDesc = usuarioEnDirector.OficinaDesc;
                    usuario.Ip = usuarioEnDirector.DirIP;
                    usuario.Grupo = ((Anses.Director.Session.GroupElement)(usuarioEnDirector.Grupos[0])).Name.ToString();

                    usuario.CueOCuit = usuarioEnDirector.Empresa;
                   

                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return usuario;

        }

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
            catch (Exception)
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
    }
}