using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Nodo
/// </summary>

[Serializable] 
public class Nodo
{
    public string NodoPadre;
    public string Titulo;
    public string Vinculo;
    public int Nivel;
    //se agrego Id, para que arme menu con key y no colocar en director el nombre excato de la pagina.
    public string Id;
}
