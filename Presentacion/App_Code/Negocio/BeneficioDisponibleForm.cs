using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Es una representación de los datos que se deben visualizar en la grilla de Beneficios disponibles
/// </summary>
public class BeneficioDisponibleForm
{

    public long IdBenficiario { get; set; }
    public string AfectacionDiponible { get; set; }


    public BeneficioDisponibleForm(long idBeneficiario, bool afectacionDisponible) 
    {
        IdBenficiario = idBeneficiario;
        AfectacionDiponible = afectacionDisponible ?  "Disponible" : "No Disponible";
    }

    public BeneficioDisponibleForm()
    {
    }
}