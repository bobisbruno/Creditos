using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;

/// <summary>
/// Summary description for AuxiliaresWS
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class AuxiliarWS : System.Web.Services.WebService
{

    public AuxiliarWS()
    {
    }

    [WebMethod(Description = "Trae Estados de Registro por Perfil")]
    public List<Estado> TraerEstadosRegBajaPorPerfil(string perfil, bool esBaja)
    {
        try
        {
            return AuxiliarDAO.TraerEstadosRegBajaPorPerfil(perfil, esBaja);
        }
        catch (Exception err)
        {
            throw new Exception("Error en servicio AuxiliarWS - ERROR: " + err.Message);
        }
    }

    [WebMethod(Description = "Trae Estados de Registro")]
    public List<Estado> TraerEstadosReg(bool bajas)
    {
        try
        {
            return AuxiliarDAO.TraerEstadosReg(bajas);
        }
        catch (Exception err)
        {
            throw new Exception("Error en servicio AuxiliarWS - ERROR: " + err.Message);
        }
    }

    [WebMethod(Description = "Trae Tipos de Domicio")]
    public List<TipoDomicilio> TraerTiposDomicio()
    {
        try
        {
            return AuxiliarDAO.TraerTiposDomicilio();
        }
        catch (Exception err)
        {
            throw new Exception("Error en servicio AuxiliarWS - ERROR: " + err.Message);
        }
    }

    [WebMethod(Description = "Trae Tipos de Concepto")]
    public List<TipoConcepto> TraerTiposConceptos()
    {
        try
        {
            return AuxiliarDAO.TraerTiposConceptos();
        }
        catch (Exception err)
        {
            throw new Exception("Error en servicio AuxiliarWS - ERROR: " + err.Message);
        }
    }

    

    [WebMethod(Description = "Convierte un número a texto TOPE:999.999.999")]
    public string Convertir_Numero_a_Texto(double Valor, bool incluir_centavos)
    {

        string Entero=string.Empty;
        string Centavos = string.Empty;

        string[] aValor = Valor.ToString().Replace(",", ".").Split('.');

        Entero = aValor[0];

        if (aValor.Length > 1 && incluir_centavos)
        {
            Centavos = aValor[1];
        }

        

        string unidades = "";
        string miles = "";
        string millones = "";

        int k = 1;
        string a = "";

        //Entero = Entero.Replace(".", "").Replace(",", "");


        String[] serie = new String[10];
        //Erase serie
        for (int i = 0; i < 10; i++)
        {
            serie[i] = "0";
        }


        //Convierte el Valor en serie
        if (Entero != "0") //y es numero
        {
            int largo = Entero.Length;
            for (int i = 1; i <= largo; i++)
            {
                serie[i] = Left(Right(Entero, i), 1);
            }
        }
        //Convierte el número escrito de a cada tres cifras
        for (k = 1; k <= 9; k = k + 3)
        {
            a = "";
            if (serie[k + 1] != "1")
            {

                switch (serie[k])
                {
                    case "1":
                        a = "uno" + a;
                        break;
                    case "2":
                        a = "dos" + a;
                        break;
                    case "3":
                        a = "tres" + a;
                        break;
                    case "4":
                        a = "cuatro" + a;
                        break;
                    case "5":
                        a = "cinco" + a;
                        break;
                    case "6":
                        a = "seis" + a;
                        break;
                    case "7":
                        a = "siete" + a;
                        break;
                    case "8":
                        a = "ocho" + a;
                        break;
                    case "9":
                        a = "nueve" + a;
                        break;
                }
            }

            if (serie[k + 1] != "1" && serie[k + 1] != "2" && serie[k + 1] != "0" && serie[k] != "0")
            { a = " y " + a; }
            switch (serie[k + 1])
            {
                case "1":
                    {
                        switch (serie[k])
                        {
                            case "0":
                                a = "diez";
                                break;
                            case "1":
                                a = "once" + a;
                                break;
                            case "2":
                                a = "doce" + a;
                                break;
                            case "3":
                                a = "trece" + a;
                                break;
                            case "4":
                                a = "catorce" + a;
                                break;
                            case "5":
                                a = "quince" + a;
                                break;
                            case "6":
                                a = "dieciseis" + a;
                                break;
                            case "7":
                                a = "diecisiete" + a;
                                break;
                            case "8":
                                a = "dieciocho" + a;
                                break;
                            case "9":
                                a = "diecinueve" + a;
                                break;
                        }
                        break;
                    }
                case "2":
                    {
                        switch (serie[k])
                        {
                            case "0":
                                a = "veinte" + a;
                                break;
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                                a = "veinti" + a;
                                break;
                        }
                        break;
                    }
                case "3":
                    a = "treinta" + a;
                    break;
                case "4":
                    a = "cuarenta" + a;
                    break;
                case "5":
                    a = "cincuenta" + a;
                    break;
                case "6":
                    a = "sesenta" + a;
                    break;
                case "7":
                    a = "setenta" + a;
                    break;
                case "8":
                    a = "ochenta" + a;
                    break;
                case "9":
                    a = "noventa" + a;
                    break;
            }


            a = " " + a;
            switch (serie[k + 2])
            {
                case "1":
                    if (serie[k + 1] == "0" && serie[k] == "0")
                    {
                        a = "cien";
                    }
                    else
                    {
                        a = "ciento" + a;
                    }
                    break;
                case "2":
                    a = "doscientos" + a;
                    break;
                case "3":
                    a = "trescientos" + a;
                    break;
                case "4":
                    a = "cuatrocientos" + a;
                    break;
                case "5":
                    a = "quinientos" + a;
                    break;
                case "6":
                    a = "seiscientos" + a;
                    break;
                case "7":
                    a = "setecientos" + a;
                    break;
                case "8":
                    a = "ochocientos" + a;
                    break;
                case "9":
                    a = "novecientos" + a;
                    break;

            }
            if (k == 1) { unidades = a; }
            if (k == 4) { miles = a; }
            if (k == 7) { millones = a; }
        }

        //Ajustes sintacticos
        string palabramillon = "";
        string palabramiles = "";
        if (serie[7] == "1")
        {
            palabramillon = " millón ";
        }
        else
        {
            palabramillon = " millones ";
        }

        if (millones == " ") { palabramillon = ""; }
        if (miles != "") { palabramiles = " mil "; }
        if (miles == " ") { palabramiles = " "; }
        string auxiliar;
        string auxiliar1;
        if (Right(miles, 3) == "uno")
        {
            auxiliar = Left(miles, miles.Length - 3) + "un";
            miles = auxiliar;
        }
        if (Right(millones, 3) == "uno")
        {
            auxiliar1 = Left(millones, millones.Length - 3) + "un";
        }
        string b = "";
        //concatena el número en letras
        b = millones + palabramillon + miles + palabramiles + unidades;

        //elimina blancos sobrantes
        while (Left(b, 1) == " ")
        {
            b = Right(b, b.Length - 1);
        }
        while (Right(b, 1) == " ")
        {
            b = Left(b, b.Length - 1);
        }
        b = Left(b, 1).ToUpper() + Right(b, b.Length - 1).ToLower();

        if (Centavos.Length <= 0)
        {
            Centavos = "00";
        }
        else
        {
            Centavos = Centavos + "00";
            Centavos = Centavos.Substring(0, 2);
        }

        string resultado= string.Empty;

        if(incluir_centavos)
            resultado = b.ToUpper() + " PESOS CON " + Centavos + "/100";
        else
            resultado = b.ToUpper();

        return resultado;
    }


    private string Left(string cadena, int cantidad)
    {
        string resultado = "";
        if (cadena.Trim() != "")
        { resultado = cadena.Substring(0, cantidad); }
        return resultado;
    }

    private string Right(string cadena, int cantidad)
    {
        string resultado = "";
        if (cadena.Trim() != "")
        { resultado = cadena.Substring(cadena.Length - cantidad, cantidad); }
        return resultado;

    }
}







