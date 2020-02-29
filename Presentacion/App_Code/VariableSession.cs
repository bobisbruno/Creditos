using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ar.Gov.Anses.Microinformatica;
using System.Configuration;
using ANSES.Microinformatica.DAT.Negocio;

/// <summary>
/// Summary description for VariableSession
/// </summary>
public class VariableSession
{

    private static System.Web.SessionState.HttpSessionState Session
    {
        get
        {
            return (System.Web.SessionState.HttpSessionState)HttpContext.Current.Session;
        }
    }

   public static Usuario oUsuario
    {
        get { return (Usuario)Session["TOKEN_Usuario"]; }
        set { Session["TOKEN_Usuario"] = value; }
    }

    public static WSPrestador.Prestador UnPrestador
    {
        get
        {
            if (Session["__unPrestador"] != null)
                return (WSPrestador.Prestador)Session["__unPrestador"];
            else
            {
                return new WSPrestador.Prestador();
            }
        }
        set { Session["__unPrestador"] = value; }
    }

    public static WSComercializador.Comercializador UnComercializador
    {
        get
        {
            if (Session["__unComercializador"] != null)
                return (WSComercializador.Comercializador)Session["__unComercializador"];
            else
                return new WSComercializador.Comercializador();
        }
        set { Session["__unComercializador"] = value; }
    }

    public static string HabIngresoNovedades
    {
        get
        {
            return Session["HabIngresoNovedades"].ToString();
        }
        set
        {
            Session["HabIngresoNovedades"] = value;
        }
    }

    public static WSCierre.Cierre oCierreAnt
    {
        get
        {
            if (Session["CierresA"] == null)
            {
                Session["CierresA"] = Cierre.TraerFechaCierreAnterior();
            }
            return (WSCierre.Cierre)Session["CierresA"];
        }
        set
        {
            Session["CierresA"] = value;
        }
    }

    public static WSCierre.Cierre oCierreProx
    {
        get
        {
            if (Session["CierresP"] == null)
            {
                Session["CierresP"] = Cierre.TraerFechaCierreProx();
            }
            return (WSCierre.Cierre)Session["CierresP"];
        }
        set
        {
            Session["CierresP"] = value;
        }
    }

    public static List<WSSucursales.Regional> oRegionales
    {
        get
        {
            if (Session["Regionales"] == null)
            {
                Session["Regionales"] = Sucursal.RegionalUdaiExternoTraer();
            }
            return (List<WSSucursales.Regional>)Session["Regionales"];
        }
        set
        {
            Session["Regionales"] = value;
        }
    }

    public static string MaxPorcentaje
    {
        get
        {
            if (Session["MaxPorcentaje"] == null)
            {
                Session["MaxPorcentaje"] = Parametros.Parametros_MaxPorcentaje();
            }

            return Session["MaxPorcentaje"].ToString();
        }

        set { Session["MaxPorcentaje"] = value; }
    }

    public static IUsuarioToken UsuarioLogeado
    {
        get
        {
            if (Session["UsuarioLogueado"] == null)
            {
                IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
                oUsuarioEnDirector.ObtenerUsuario();

                Session["UsuarioLogueado"] = oUsuarioEnDirector;
            }
            return (IUsuarioToken)Session["UsuarioLogueado"];
        }
        set
        {
            Session["UsuarioLogueado"] = value;
        }
    }

    public static bool esSoloArgenta
    {
        get
        {
            return DirectorManager.TraerPermiso("esSoloArgenta", "Propiedades").HasValue;
        }
    }

    public static bool esSoloEntidades
    {
        get
        {
            return DirectorManager.TraerPermiso("esSoloEntidades", "Propiedades").HasValue;
        }
    }

    public static bool esControlPrestacional
    {
        get
        {
            return DirectorManager.TraerPermiso("esControlPrestacional", "Propiedades").HasValue;
        }
    }

    public static bool esSupervisorSiniestro
    {
        get
        {
            return DirectorManager.TraerPermiso("esSupervisorSiniestro", "Propiedades").HasValue;
        }
    }

    public static string PaginaInicio
    {
        get
        {
            string aRedirigir = "~/DAIndex.aspx";

            return aRedirigir + (aRedirigir.IndexOf("?") > 0 ? "&" : "?") + QueryStringHash.ReturnIntegrityCheckHash(aRedirigir);
        }
    }

    public static ParametrosWS.Parametros ParametrosSitio
    {
        get
        {
            if (Session["ParametrosSitio"] != null)
                return (ParametrosWS.Parametros)Session["ParametrosSitio"];

            return null;
        }
        set
        {
            Session["ParametrosSitio"] = value;
        }
    }

    public static string Cuil
    {
        get { return Session["Cuil"].ToString(); }
        set { Session["Cuil"] = value; }
    }

    public static decimal IdRecupero
    {
        get { return (decimal)Session["IdRecupero"]; }
        set { Session["IdRecupero"] = value; }
    }

    /*Para mi este no va*/
    public static decimal ValorResidual
    {
        get { return (decimal)Session["ValorResidual"]; }
        set { Session["ValorResidual"] = value; }
    }

    public static List<PrestadorRecupero> PrestadoresRecupero
    {
        get { return (List<PrestadorRecupero>)Session["PrestadoresRecupero"]; }
        set { Session["PrestadoresRecupero"] = value; }
    }
    
    public static ArchivoDTO archivo
    {
        get { return (ArchivoDTO)Session["archivo"]; }
        set { Session["archivo"] = value; }
    }
}


