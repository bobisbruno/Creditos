using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// 
/// Version: 29/05/2009
/// 
/// reSerializer: Esta clase tiene como objetivo reserializar objetos retornados por un webservice, 
/// para poder castearlos directamente como objetos de proyectos Entidades.
/// 
/// </summary>
public static class reSerializer
{

    #region FuncionesPropias

    private static Byte[] StringToUTF8ByteArray(String pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        Byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    private static String UTF8ByteArrayToString(Byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        String constructedString = encoding.GetString(characters);
        return (constructedString);
    }
    #endregion

    #region Metodos

    public static string retNameSpace(Type serviceType, string serviceURL)
    {
        try
        {
            ServiceDescriptionReflector sdf = new ServiceDescriptionReflector();
            sdf.Reflect(serviceType, serviceURL);
            string sNs = "";
            if (sdf.ServiceDescriptions.Count > 1)
            {
                sNs = sdf.ServiceDescriptions[1].TargetNamespace;
            }
            return sNs;
        }
        catch
        {
            throw new ApplicationException("Error within retNameSpace of reSerializer - Service:" + serviceURL.ToString());
        }
    }

    public static String SerializeObject(Object pObject, Type pType, string xmlns)
    {
        try
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(pType, xmlns);
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            return null;
        }
    }


    public static Object DeserializeObject(String pXmlizedString, Type pType, string xmlns)
    {
        XmlSerializer xs = new XmlSerializer(pType, xmlns);
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }
    public static object reSerialize(object objFrom, Type typeFrom, Type typeTo, string xmlns)
    {
        return DeserializeObject(
                SerializeObject(objFrom, typeFrom, xmlns),
            typeTo, xmlns);

    }

    #endregion


}
