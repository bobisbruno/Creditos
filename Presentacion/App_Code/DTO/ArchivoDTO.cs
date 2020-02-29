using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Archivo
/// </summary>

[Serializable] 
public class ArchivoDTO
{
    public string Nombre {get; set;}
    public string Tipo {get; set;}
    public string Titulo { get; set; }
    public string Datos { get; set; }
    public byte[] DatosByte { get; set; }

    public ArchivoDTO(string _Nombre, string _Tipo, string _Titulo, string _Datos)
    {
        Nombre = _Nombre;
        Titulo = _Titulo;
        Tipo = _Tipo;
        Datos = _Datos;
    }
    public ArchivoDTO(string _Nombre, string _Tipo, string _Titulo, byte[] _DatosByte)
    {
        Nombre = _Nombre;
        Titulo = _Titulo;
        Tipo = _Tipo;
        DatosByte = _DatosByte;
    }

    public ArchivoDTO(string _Tipo, string _Titulo, string _Datos)
    {       
        Titulo = _Titulo;
        Tipo = _Tipo;
        Datos = _Datos;
    }   
}
