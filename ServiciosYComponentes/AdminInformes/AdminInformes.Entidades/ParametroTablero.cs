using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminInformes.Entidades
{
    public class ParametroTablero
    {
        public int idParametro { get; set; }
        public string NombreParametro { get; set; }
        public string AliasParametro { get; set; }
        public string TipoDeDato { get; set; }
        public int Presicion { get; set; }
        public int Escala { get; set; }
        public string ValorMinimo { get; set; }
        public byte ValMinTipoMetodoObtencion { get; set; }
        public string ValorMaximo { get; set; }
        public byte ValMaxTipoMetodoObtencion { get; set; }
        public string QueryDominio { get; set; }
        public string InterfaseIngreso { get; set; }
        public string ValorActual { get; set; }
    }
}