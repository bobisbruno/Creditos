using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Configuration;
using System.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
//using System.Data.SqlClient;

/// <summary>
/// Summary description for ComercializadorWS
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ComercializadorWS : System.Web.Services.WebService
{

  
    public ComercializadorWS()
    {
    }


    [WebMethod(Description = "Retorna un Comercializador por ID Presatador")]
    public List<Comercializador> TraerComercializadoras_xidPrestador(long idPrestador)
    {
        try
        {
            return ComercializadorDAO.TraerComericalizadoras_xidPrestador(idPrestador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Retorna un Comercializador por Cuit")]
    public Comercializador TraerComercializadoras_xCuit(string cuit)
    {
        try
        {
            return ComercializadorDAO.TraerComericalizadoras_xCuit(cuit);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Retorna Comercializadores con su domicilio por ID Presatador")]
    public List<Comercializador> TraerDomiciliosComercializador_T_PrestadorComercializador(long idPrestador, long idComercializador)
    {
        try
        {
            return ComercializadorDAO.TraerDomiciliosComercializador_T_PrestadorComercializador(idPrestador, idComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod(Description = "Retorna Comercializadores con sus domicilios por distinto ID Presatador")]
    public List<Comercializador> TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(long idPrestador, long idComercializador)
    {
        try
        {
            return ComercializadorDAO.TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(idPrestador, idComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public bool DomicilioComercializador_BuscarIgual(string calle, string nro, string piso,
                                                     string dPto, string codPostal)
    {
        try
        {
            return ComercializadorDAO.DomicilioComercializador_BuscarIgual(calle, nro, piso, dPto, codPostal);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public bool DomicilioComercializador_BuscarComercializadorDistintoIDPrestador(long idPrestador, long idDomicilioComercializador)
    {
        try
        {
            return ComercializadorDAO.DomicilioComercializador_BuscarComercializadorDistintoIDPrestador(idPrestador, idDomicilioComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public string Relacion_ComercializadorAPrestador(long idPrestador, Comercializador unComercializador)
    {
        try
        {
            return ComercializadorDAO.Relacion_ComercializadorPrestador_A(idPrestador, unComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public string Relacion_ComercializadorPrestadorMB(long idPrestador, Comercializador unComercializador)
    {
        try
        {
            return ComercializadorDAO.Relacion_ComercializadorPrestadorMB(idPrestador, unComercializador);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string DomicilioComercializador_RelacionDC_A(long idPrestador, Comercializador unComercializador)
    {
        try
        {
            return ComercializadorDAO.DomicilioComercializador_RelacionDC_A(idPrestador, unComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public long DomicilioComercializadorA(long idPrestador, Comercializador unComercializador)
    {
        try
        {
            return ComercializadorDAO.DomicilioComercializadorA(idPrestador, unComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public string Relacion_DomicilioComercializadorPrestadorA(long idPrestador, Comercializador unComercializador)
    {
        try
        {
            return ComercializadorDAO.Relacion_DomicilioComercializadorPrestadorA(idPrestador, unComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public string Relacion_ComercializadorPrestadorDomicilioMB(long idPrestador, long idDomicilioComercializador, Comercializador unComercializador)
    {
        try
        {
            return ComercializadorDAO.Relacion_ComercializadorPrestadorDomicilioMB(idPrestador, idDomicilioComercializador, unComercializador);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    [WebMethod]
    public string Relacion_ComercializadorPrestador_Domicilio_Tasas_B(Int64 idPrestador,
                                                                      Int64 idComercializador,
                                                                      DateTime FechaInicio,
                                                                      DateTime FFin_Baja)
    {
        try
        {
            List<Error> listaError = new List<Error>();
            Prestador unPrestador = new Prestador();

            unPrestador.ID = idPrestador;
            unPrestador.FechaInicio = FechaInicio;

            unPrestador.Comercializadoras = new List<Comercializador>();
            unPrestador.Comercializadoras.Add(new Comercializador(idComercializador,
                                                                  string.Empty, 0, string.Empty, new Estado(), new Auditoria(), string.Empty,
                                                                  FechaInicio,
                                                                  FFin_Baja));
            unPrestador.ArrayComercializadoras = unPrestador.Comercializadoras.ToArray();

            //List<Error> listaError = unPrestador.ValidateRuleSet<Prestador>("ValidaEntidades_ID_Comercializadoras");           
            //List<Error> listaError = unPrestador.Comercializadoras.ValidateRuleSet<Comercializador>("ValidaEntidades_ID_Comercializadoras");            

            string validacionConfig = ConfigurationManager.AppSettings["validacionConfig"].ToString();
            Validator oValidator =  ValidationFactory.CreateValidatorFromConfiguration(typeof(Prestador),
                                                                                      "ValidaEntidades_ID_Comercializadoras",
                                                                                      new FileConfigurationSource(validacionConfig));
            
            //Validation.ValidateFromConfiguration<T>(
            
            bool isvalid = oValidator.Validate(unPrestador.ArrayComercializadoras).IsValid;

            string descripcion = string.Empty;
            if (listaError.Count == 0)
                return "OK";
            else
            {
                foreach (Error itemError in listaError)
                    descripcion += itemError.Descripcion;

                return descripcion;
            }

            //return ComercializadorDAO.Relacion_ComercializadorPrestador_Domicilio_Tasas_B(
            //                                                                    idPrestador,
            //                                                                    idComercializador,
            //                                                                    FechaInicio,
            //                                                                    FFin_Baja);
        }
        catch (Exception err)
        {
            throw err;
        }
    }

}

