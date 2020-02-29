using System;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Summary description for ObjectXMLSerializer
/// </summary>
/// 
[Serializable]
public static class ObjectXMLSerializer
{

    public static string XmlSerialize(object unObjeto)
    {


        if (unObjeto != null)
        {
            // Quito la declaracion de XML porque me queda con secuencias de escape
            XmlWriterSettings wset = new XmlWriterSettings();
            wset.Encoding = Encoding.UTF8;
            wset.OmitXmlDeclaration = true;
            wset.Indent = true;
            


            // Creo un Namespace vacio para que no lo coloque en el objeto deserializado
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("", "");
            ns.Add(String.Empty, String.Empty);


            // creo a XML writer que utiliza los settings anteriores para el XML
            StringBuilder sb = new StringBuilder();
            XmlWriter xmlw = XmlWriter.Create(sb, wset);
            xmlw.WriteRaw("");


            // serializo
            XmlSerializer xmls = new XmlSerializer(unObjeto.GetType());


            //xmls.Serialize(xmlw, unObjeto, ns);
            xmls.Serialize(xmlw, unObjeto, ns);

            xmlw.Flush();



            StringBuilder newSb = new StringBuilder();

            char[] sep2 = new char[] { '\'' };
            foreach (string xml2 in sb.ToString().Split(sep2))
                newSb.Append(xml2);

            ////elimino las comilla y el namespace
            //newSb = newSb.Replace("\"", "").Replace(" xmlns=http://ArgentaCWS.Anses.Gov.Ar/", "");

            return newSb.ToString();
        }
        else
            return "";


    }







}
