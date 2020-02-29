using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FiltroReclamos
/// </summary>
public class FiltroReclamos
{
    public FiltroReclamos()
    {
      
    }

    public int idEstado { get; set; }
    public string FecDesde { get; set; }
    public string FecHasta { get; set; }
    public string Beneficiario { get; set; }
    public string CuilPre { get; set; }
    public string CuilDoc { get; set; }
    public string CuilDig { get; set; }

}
