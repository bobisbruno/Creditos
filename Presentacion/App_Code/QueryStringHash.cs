using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
/*
    
    .::Fueled by $3b@::.

    Version: 02/06/11
   
    About: Esta clase agrega un pokito de seguridad para la utilizacion del query string
           No es la solucion santo grial, solo añade una vuelta de rosca para evitar las inyecciones
    
    Warns: Esta clase solo valida y mantiene integro el query string, NO TODA LA URL.
           Esto puede derivar a ataques del tipo pagina1.aspx?id=nro&FSC=nro2 cuando sean modificados por pagina2.aspx?id=nro&FSC=nro2 o inyeccion previo el querystring
 
    Usage:
    // En cualquier parte de la pagina que se utilize un query string se procede como sigue:
    link.Text = "pagina.aspx?" + QueryStringHash.AttachIntegrityCheckHash("id=nro");
    
    // se verifica en la Master page como sigue:
    if(!string.IsNullOrEmpty(Request.QueryString.ToString()) && !QueryStringHash.GetIntegrityCheck(Request.QueryString.ToString()))
    {
        // error   
    }
        
*/

[Serializable]
public static class QueryStringHash
{
    public const string HashWords = "FSC=";
    public const string HashSessionVariable = "_QueryStringHash_FCS_Random_ID";
    public static void initialize()
    {
        Random rand = new Random();

        // si nunca asigne el random => lo asigno
        if (HttpContext.Current.Session[HashSessionVariable] == null || string.IsNullOrEmpty(HttpContext.Current.Session[HashSessionVariable].ToString()))
            HttpContext.Current.Session[HashSessionVariable] = rand.Next();
    }

    public static bool GetIntegrityCheck(string input)
    {
        initialize();

        input = GetUnencodedFlatString(input);

        int HashOcurrence = input.LastIndexOf(HashWords);

        if (HashOcurrence < 0)
            return false;

        string HashSecuence = input.Substring(HashOcurrence);
        string QueryString = input.Substring(0, HashOcurrence);

        if (QueryString.Length > 0)
        {
            // si tiene querystring => saco el &
            QueryString = QueryString.Remove(QueryString.Length - 1);
        }

        return (HashWords + GetMD5Hash(GetUnencodedFlatString(QueryString) + HttpContext.Current.Session[HashSessionVariable]) == HashSecuence);

    }

    public static string ReturnIntegrityCheckHash(string complete_url)
    {

        initialize();

        string querystring = string.Empty;
        if(complete_url.IndexOf("?") > 0)
            querystring = complete_url.Substring(complete_url.IndexOf("?") + 1);

        return HashWords + GetMD5Hash(GetUnencodedFlatString(querystring) + HttpContext.Current.Session[HashSessionVariable]);
    }

    public static string AttachIntegrityCheckHash(string querystring)
    {
        initialize();
        return querystring + (querystring.Length > 0 ? "&" : "") + HashWords + GetMD5Hash(GetUnencodedFlatString(querystring) + HttpContext.Current.Session[HashSessionVariable]);
    }

    public static string GetEncodedUrlString(string input)
    {
        return System.Web.HttpContext.Current.Server.UrlEncode(input);
    }

    public static string GetUnencodedFlatString(string input)
    {
        string rta = "", str = "";

        str = System.Web.HttpContext.Current.Server.UrlDecode(input);
        while (rta != str)
        {
            rta = str;
            str = System.Web.HttpContext.Current.Server.UrlDecode(input);
        }

        return rta;
    }

    public static string GetMD5Hash(string input)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        string password = s.ToString();
        return password;
    }
}
