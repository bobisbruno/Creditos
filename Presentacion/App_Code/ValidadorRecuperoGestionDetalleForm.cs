using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for ValidadorRecuperoGestionDetalleForm
/// </summary>
public class ValidadorRecuperoGestionDetalleForm
{
  	public ValidadorRecuperoGestionDetalleForm()
	{
	}

    public string EjecutarValidacionesParaEnvioARegional(string numeroDeExpediente, int modalidadDePagoIndex, decimal montoTotalARecuperar)
    {
        var errorMessage = string.Empty;
        errorMessage += ExpedienteEstaCaratulado(numeroDeExpediente);
        errorMessage += TieneModalidadDePago(modalidadDePagoIndex);
        errorMessage += EsValidoMontoTotal(montoTotalARecuperar);

        return errorMessage;
    }



    private string ExpedienteEstaCaratulado(string idDeNovedad) 
    {
         return string.IsNullOrEmpty(idDeNovedad)? "El expediente no se encuentra caratulado <br />" : string.Empty;
    }

    private string TieneModalidadDePago(int modalidadDePagoIndex)
    {
        return modalidadDePagoIndex > -1 ? "Debe seleccionar una modalidad de pago" : string.Empty;
    }

    private string EsValidoMontoTotal(decimal montoTotal)
    { 
        decimal valorMinimo = new RecuperoDetalleService().ObtenerMontoMinimoDeRecupero(Constantes.PRESTADOR_ANSES);
        return montoTotal < valorMinimo ? "El monto total a recuperar es menor al monto mínimo <br/>" : string.Empty;
    }





}