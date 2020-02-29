using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ArchivoConDescripcion
/// </summary>
public class ArchivoConFiltrosDeSeleccionDTO: ArchivoDTO
{
    public string DatosFiltrosDeSeleccion { get; set; }
    public ArchivoConFiltrosDeSeleccionDTO(string _Nombre, string _Tipo, string _Titulo, string _Datos, string _DatosFiltrosDeSeleccion)
        : base(_Nombre, _Tipo, _Titulo, _Datos)
	{
        DatosFiltrosDeSeleccion = _DatosFiltrosDeSeleccion;
	}

   
}