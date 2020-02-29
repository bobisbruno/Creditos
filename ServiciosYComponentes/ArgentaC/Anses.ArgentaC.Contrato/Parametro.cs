using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Parametro
    {
        public string batch { get; set; }
        public string Variable { get; set; }
        public string Valor { get; set; }

        public Parametro(string _batch, string _variable, string _valor)
        {
            this.batch = _batch;
            this.Variable = _variable;
            this.Valor = _valor;
        }

        public Parametro()
        { }
    }
}
