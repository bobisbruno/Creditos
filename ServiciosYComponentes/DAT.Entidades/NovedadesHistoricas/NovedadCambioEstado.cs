using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades.NovedadesHistoricas
{
    public class NovedadCambioEstado
    {
        public Int64 IdNovedad { get; set; }
        public DateTime? Fecha { get; set; }
        public Int32 IdEstado { get; set; }
        public string DescripcionEstado { get; set; }
        public string Usuario { get; set; }
        public string IpEstado { get; set; }
        public string Oficina { get; set; }

        public NovedadCambioEstado() { }

        public NovedadCambioEstado(Int64 IdNovedad, DateTime? Fecha, Int32 IdEstado, string DescripcionEstado, string Usuario,  string IpEstado, string Oficina)
        {
            this.IdNovedad = IdNovedad;
            this.Fecha = Fecha;
            this.IdEstado = IdEstado;
            this.DescripcionEstado = DescripcionEstado;
            this.Usuario = Usuario;
            this.IpEstado = IpEstado;
            this.Oficina = Oficina;
        }
    }
}
