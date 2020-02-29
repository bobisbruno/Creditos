using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class InformePendientesDeCobro
    {
        public List<PendientesDeCobro> listaPendientesDeCobro { get; set; }

        public InformePendientesDeCobro()
        {
            listaPendientesDeCobro = new List<PendientesDeCobro>();
        }

        public InformePendientesDeCobro(List<PendientesDeCobro> _lista)
        {
            this.listaPendientesDeCobro = _lista;
        }
    }

    [Serializable]
    public class PendientesDeCobro
    {
        public int Mensual { get; set; }
        public string Motivo { get; set; }
        public int CantCasos { get; set; }
        public decimal Importe { get; set; }

        public PendientesDeCobro()
        { }

        public PendientesDeCobro(int _mensual, string _motivo, int _cantidadDeCasos, decimal _importe)
        {
            this.Mensual = _mensual;
            this.Motivo = _motivo;
            this.CantCasos = _cantidadDeCasos;
            this.Importe = _importe;
        }
    }
}
