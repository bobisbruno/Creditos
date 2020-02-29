using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;

/// <summary>
/// Summary description for CodigoPreAprobacionWS
/// </summary>
[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CodigoPreAprobacionWS : System.Web.Services.WebService
    {

    public CodigoPreAprobacionWS ()
    {
        InitializeComponent();           
    }
    
    #region Component Designer generated code

    //Required by the Web Services Designer 
    private IContainer components = null;

    //<summary>
    //Required method for Designer support - do not modify
    //the contents of this method with the code editor.
    //</summary>
    private void InitializeComponent()
    {
    }

    //<summary>
    //Clean up any resources being used.
    //</summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #endregion


    [WebMethod(Description = "Genera Codigo Pre Aprobacion, y se  registrado en tabla Novedades_CodigoPreAprobacion")]
    public string Novedades_CodigoPreAprobacion_Alta(long Cuil, string Ip, string Usuario)
    {
        return CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Alta(Cuil,Ip,Usuario);
    }

    [WebMethod(Description = "Actualiza Tabla Novedades_CodigoPreAprobacion  si es valido cuil, CodigoAValidar")]
    public string Novedades_CodigoPreAprobacion_Modificacion(CodigoPreAprobado unCodigoPreAprobado)
    {
        return CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Modificacion(unCodigoPreAprobado);
    }

    [WebMethod(Description = "Valida el cuil y  CodigoAValidar en tabla Novedades_CodigoPreAprobacion")]
    public string Novedades_CodigoPreAprobacion_Valida(CodigoPreAprobado unCodigoPreAprobado)
    {
        return CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Valida(unCodigoPreAprobado);
    }



}
