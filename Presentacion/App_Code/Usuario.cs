using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Usuario
/// </summary>
/// 
[Serializable]
public class Usuario : IDisposable
{
    #region Dispose

    private bool disposing;

    public void Dispose()
    {
        // Llamo al método que contiene la lógica
        // para liberar los recursos de esta clase.
        Dispose(true);
    }

    protected virtual void Dispose(bool b)
    {
        // Si no se esta destruyendo ya…
        if (!disposing)
        {
            // La marco como desechada ó desechandose,
            // de forma que no se puede ejecutar este código
            // dos veces.
            disposing = true;

            // Indico al GC que no llame al destructor
            // de esta clase al recolectarla.
            GC.SuppressFinalize(this);

            // … libero los recursos… 
        }
    }

    ~Usuario()
    {
        // Llamo al método que contiene la lógica
        // para liberar los recursos de esta clase.
        Dispose(true);
    }
    #endregion

    public string id_Usuario { get; set; }
    public string Nombre { get; set; }
    public string CUIT_Empresa { get; set; }
    public string id_Empresa { get; set; }
    public string Nombre_Empresa { get; set; }
    public string IP { get; set; }
    public string SucursalDesc { get; set; }
    public string Perfil { get; set; }
    public string Grupo { get; set; }
 
    public Usuario()
    {    }

    public Usuario(string id_Usuario, string Nombre, string CUIT_Empresa, string id_Empresa, string Nombre_Empresa, string IP, string sucursalDesc, 
                   string perfil,string grupo)
    {
        this.id_Usuario = id_Usuario;
        this.Nombre = Nombre;
        this.CUIT_Empresa = CUIT_Empresa; 
        this.id_Empresa = id_Empresa;
        this.Nombre_Empresa = Nombre_Empresa;
        this.IP = IP;
        this.SucursalDesc = sucursalDesc;
        this.Perfil = perfil;
        this.Grupo = grupo;

    }
}