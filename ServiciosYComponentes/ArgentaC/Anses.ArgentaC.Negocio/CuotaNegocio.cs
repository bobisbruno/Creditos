using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;

namespace Anses.ArgentaC.Negocio
{
    public static class CuotaNegocio
    {
        public static Persona calcularCuotas(Persona persona, Producto producto, decimal monto)
        {
            return CuotaDato.calcularCuotas(persona, producto, monto);
        }
    }
}
