using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Discapacidad
    {
        public int IdBeneficioAsociado { get; set; }
        public bool esPermanente { get; set; }
        public Tipo Tipo_Discapacidad { get; set; }
        public DateTime? Fecha_Inicio_Discapacidad { get; set; }
        public DateTime? Fecha_Vto_Discapacidad { get; set; }

        public Discapacidad() 
        {
            this.Tipo_Discapacidad = new Tipo();
        }

        public Discapacidad(int _IdBeneficioAsociado, string id_discapacidad, string discapacidad, DateTime? _fecha_Inicio_Discapacidad, DateTime? _fecha_vto_discapacidad) 
        {
            this.IdBeneficioAsociado = _IdBeneficioAsociado;
            this.Tipo_Discapacidad = new Tipo(id_discapacidad, discapacidad);
            this.Fecha_Inicio_Discapacidad = _fecha_Inicio_Discapacidad;
            this.Fecha_Vto_Discapacidad = _fecha_vto_discapacidad;
        }

        public Discapacidad(int _IdBeneficioAsociado, Tipo tipo_discapacidad, DateTime? _fecha_Inicio_Discapacidad, DateTime? _fecha_vto_discapacidad)
        {
            this.IdBeneficioAsociado = _IdBeneficioAsociado;
            this.Tipo_Discapacidad = new Tipo();
            this.Tipo_Discapacidad = tipo_discapacidad;
            this.Fecha_Inicio_Discapacidad = _fecha_Inicio_Discapacidad;
            this.Fecha_Vto_Discapacidad = _fecha_vto_discapacidad;
        }
    }
}
