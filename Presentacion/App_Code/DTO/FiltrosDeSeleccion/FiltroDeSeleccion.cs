using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FiltroDeSeleccion
/// </summary>
public class FiltroDeSeleccion
{
    public string NombreFiltro { get; set; }
    public object ValorFiltro { get; set; }
    
    public FiltroDeSeleccion(string _Nombrefiltro, object _ValorFiltro)
    {
        NombreFiltro = _Nombrefiltro;
        ValorFiltro = _ValorFiltro;
    }
}