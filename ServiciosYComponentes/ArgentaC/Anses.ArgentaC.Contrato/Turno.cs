using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Turno
    {
        public long? Id { get; set; }
        public DateTime? Fecha { get; set; }
        public long? IdOficina { get; set; }
        public string RespuestaTurno { get; set; }
        public Turno()
        {
            this.Id = null;
            this.Fecha = null;
            this.IdOficina = null;
            this.RespuestaTurno = "";
        }
        public Turno(long? id, DateTime? fecha, long idOficina, string respuestaTurno)
        {
            this.Id = id;
            this.Fecha = fecha;
            this.IdOficina = idOficina;
            this.RespuestaTurno = respuestaTurno;
        }
    }
}
