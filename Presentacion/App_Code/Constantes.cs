using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Representa un repositorio de constantes utilizadas en el sistema
/// </summary>
/// 


public static class Constantes
{
    public enum Panel
    {
        BUSQUEDA,
        RESULTADO_BUSQUEDA
    }

    public enum TipoDocumentoImpreso
    {
        CARATULA = 1,
        CONSTANCIA_ADP = 2,
        SOLICITUD_CREDITO = 3,
        RESUMEN = 4
    }

    public enum PartesDelBeneficio
    {
        excaja = 0,
        tipo = 1,
        numero = 2,
        coparticipacion = 3
    }
    public const int PRESTADOR_ANSES = 695;

    public const string ADP_FALLECIDO = "FALLECIDO";
    public const string GUION = "-";
    public const string ADP_MARCA_CELULAR = "N";
    public const string OK = "OK";
    public const string NO_OK = "NO_OK";
    public const string TEXTO_SI = "SI";
    public const string TEXTO_NO = "NO";
    public const string CERO = "0";
    public const string DOS = "2";
    public const string SELECCIONE = "- SELECCIONE -";
    public const string TODOS_LOS_OPERADORES = "- TODOS LOS OPERADORES -";
    public const string EXTENSION_PDF = "pdf";
    public const string EXTENSION_EXCEL = "xlsx";
    public const string EXTENSION_TXT= "txt";

    public static readonly Dictionary<Int16, string> ProvinciasCollection = new Dictionary<short, string>()
    {
     {0, "SIN INFORMACION"},
     {1, "CAPITAL FEDERAL"},
     {2, "BUENOS AIRES"},
     {3, "CATAMARCA"},
     {4, "CORDOBA"},
     {5, "CORRIENTES"},
     {6, "ENTRE RIOS"},
     {7, "JUJUY"},
     {8, "LA RIOJA"},
     {9, "MENDOZA"},
     {10,"SALTA"},
     {11,"SAN JUAN"},
     {12,"SAN LUIS"},
     {13,"SANTA FE"},
     {14,"SANTIAGO DEL ESTERO"},
     {15,"TUCUMAN"},
     {16,"CHACO"},
     {17,"CHUBUT"},
     {18,"FORMOSA"},
     {19,"LA PAMPA"},
     {20,"MISIONES"},
     {21,"NEUQUEN"},
     {22,"RIO NEGRO"},
     {23,"SANTA CRUZ"},
     {24,"TIERRA DEL FUEGO"},
     {99,"DESCONOCIDA"}
    };





}


