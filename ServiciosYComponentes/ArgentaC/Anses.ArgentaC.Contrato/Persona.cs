using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Persona
    { 
        public long Cuil { get; set; }
        public int? Documento_Tipo { get; set; }
        public int? Documento_Nro { get; set; }
        public string DNI_Nro_Tramite_Renaper { get; set; }
        public string Sexo { get; set; }
        public string NombreApellido { get; set; }
        public DateTime? Fecha_Nacimiento { get; set; }
        public DateTime? Fecha_Fallecimiento { get; set; }
        public DateTime? Fecha_Alta { get; set; }
        public Contacto Contacto { get; set; }
        public Domicilio Domicilio { get; set; }
        public string CBU { get; set; }
        public bool EsCbuPim { get; set; }
        public string Banco { get; set; }
        public short codBanco { get; set; }
        public string Agencia { get; set; }
        public short codAgencia { get; set; }
        public bool? TieneAltaTemprana { get; set; }
        public bool? TienePrestacionJubilatoria { get; set; }
        public bool? TieneTramiteAnme { get; set; }
        public bool? TieneRiesgoBCRA { get; set; }
        public bool? EsPresoConSentenciaFirme { get; set; }
        public bool? EsEmancipado { get; set; }
        public bool? TieneHuella { get; set; }
        public List<Deuda> Deudas { get; set; }
        public List<Beneficio> BeneficiosRelacionados { get; set; }
        public List<Producto> Productos { get; set; }
        public List<Mensaje> Errores { get; set; }
        public bool SalidaPreacuerdoOK { get; set; }
        public List<MensajeBDD> MensajesBDD { get; set; }
        public List<Novedad> Novedades { get; set; }
        public int IdSistema { get; set; }
        public bool? ImposibilidadDeFirmar { get; set; }

        //Constructores
        public Persona()
        {
            this.BeneficiosRelacionados = null;
            this.Deudas = null;
            this.Documento_Nro = null;
            this.Documento_Tipo = null;
            this.DNI_Nro_Tramite_Renaper = null;
            this.Errores = new List<Mensaje>();
            this.EsEmancipado = null;
            this.EsPresoConSentenciaFirme = null;
            this.Fecha_Alta = null;
            this.Fecha_Fallecimiento = null;
            this.Fecha_Nacimiento = null;
            this.NombreApellido = null;
            this.Sexo = null;
            this.TieneAltaTemprana = null;
            this.TienePrestacionJubilatoria = null;
            this.TieneRiesgoBCRA = null;
            this.TieneTramiteAnme = null;
            this.CBU = null;
            this.Banco = null;
            this.Agencia = null;
            this.EsCbuPim = false;
        }

        public Persona(long cuil, string nombreapellido, int? documento_nro, short documento_tipo, string dni_Nro_Tramite_Renaper,
                        DateTime? fecha_nacimiento, String sexo, DateTime? fecha_fallecimiento, DateTime fecha_alta, 
                        bool alta_temprana, bool tienePrestacionJubilatoria, bool tieneTramiteAnme, bool tieneRiesgoBCRA, bool esPresoConSentenciaFirme, 
                        bool esEmancipado, string cbu, string banco, string agencia,
                        List<Deuda> deudas, List<Beneficio> beneficios, List<Producto> productos, List<Novedad> novedades, List<MensajeBDD> _mensajesBDD)
        {
            this.Cuil = cuil;
            this.NombreApellido = nombreapellido;
            this.Documento_Tipo = documento_tipo;
            this.Documento_Nro = documento_nro;
            this.DNI_Nro_Tramite_Renaper = dni_Nro_Tramite_Renaper;
            this.Sexo = sexo;
            this.Fecha_Nacimiento = fecha_nacimiento;
            this.Fecha_Alta = fecha_alta;
            this.TieneAltaTemprana = alta_temprana;
            this.TienePrestacionJubilatoria = tienePrestacionJubilatoria;
            this.TieneTramiteAnme = tieneTramiteAnme;
            this.TieneRiesgoBCRA = tieneRiesgoBCRA;
            this.EsPresoConSentenciaFirme = esPresoConSentenciaFirme;
            this.EsEmancipado = esEmancipado;
            this.CBU = cbu;
            this.Banco = banco;
            this.Agencia = agencia;
            this.Fecha_Fallecimiento = fecha_fallecimiento;
            this.Deudas = deudas;
            this.BeneficiosRelacionados = beneficios;
            this.Productos = productos;
            this.Novedades = novedades;
            this.Errores = new List<Mensaje>();
            this.MensajesBDD = _mensajesBDD;
            this.EsCbuPim = false;
        }

        public Persona(long cuil, string nombreapellido, int? documento_nro, short documento_tipo,
                        DateTime? fecha_nacimiento, String sexo, DateTime? fecha_fallecimiento, bool? esEmancipado,
                        Domicilio domicilio, Contacto contacto)
        {
            this.Cuil = cuil;
            this.NombreApellido = nombreapellido;
            this.Documento_Nro = documento_nro;
            this.Documento_Tipo = documento_tipo;
            this.Fecha_Nacimiento = fecha_nacimiento;
            this.Sexo = sexo;
            this.Fecha_Fallecimiento = fecha_fallecimiento;
            this.EsEmancipado = esEmancipado;
            this.Domicilio = domicilio;
            this.Contacto = contacto;
            this.Errores = new List<Mensaje>();
            this.EsCbuPim = false;
        }
    }
}
