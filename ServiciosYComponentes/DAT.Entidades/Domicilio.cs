using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Domicilio : IDisposable
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

        ~Domicilio()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public Domicilio() 
        {
            IdDomicilio = 0;
            Calle = string.Empty;
            NumeroCalle = string.Empty;
            Piso = string.Empty;
            Departamento = string.Empty; 
            CodigoPostal = string.Empty;
            PrefijoTel = string.Empty;
            NumeroTel = string.Empty;
            Fax = string.Empty; 
            Localidad = string.Empty;  
            FechaInicio = new DateTime();
            FechaFin = new DateTime();
            Observaciones = string.Empty;
            EsSucursal = false;
            Mail = string.Empty;  
            UnTipoDomicilio = new TipoDomicilio();
            UnaProvincia = new Provincia();
            FechaNacimiento = new DateTime();
            Sexo = string.Empty;
            Nacionalidad = 0;
        }

        public Domicilio(long idDomicilio,
                         string calle,
                         string numeroCalle,
                         string piso,
                         string departamento,
                         string codigoPostal,
                         bool esCelular,
                         string prefijoTel,
                         string numeroTel,
                         bool esCelular2,
                         string prefijoTel2,
                         string numeroTel2,
                         string fax,
                         string localidad,
                         Provincia unaProvincia,
                         DateTime fechaInicio,
                         DateTime? fechaFin,
                         string observaciones,
                         bool esSucursal,
                         string mail,
                         TipoDomicilio unTipoDomicilio /*,
                         DateTime fechaNacimiento,
                         string sexo,
                         int nacionalidad*/)
        {
            IdDomicilio = idDomicilio;
            Calle = calle;
            NumeroCalle = numeroCalle;
            Piso = piso;
            Departamento = departamento;
            CodigoPostal = codigoPostal;
            EsCelular = esCelular;
            PrefijoTel = prefijoTel;
            NumeroTel = numeroTel;
            EsCelular2 = esCelular2;
            PrefijoTel2 = prefijoTel2;
            NumeroTel2 = numeroTel2;
            Fax = fax;
            Localidad = localidad;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Observaciones = observaciones;
            EsSucursal = esSucursal;
            Mail = mail;
            UnaProvincia = unaProvincia;
            UnTipoDomicilio = unTipoDomicilio;
            /*FechaNacimiento = fechaNacimiento;
            Sexo = sexo;
            Nacionalidad = nacionalidad;*/
        }


        public Domicilio(long idDomicilio,
                         string calle,
                         string numeroCalle,
                         string piso,
                         string departamento,
                         string codigoPostal,
                         string localidad,
                         Provincia unaProvincia,
                         String _telediscado1, String _telefono1,
                         Boolean _esCelular1, String _telediscado2,
                         String _telefono2, Boolean _esCelular2, String _mail
                        )
        {
            IdDomicilio = idDomicilio;
            Calle = calle;
            NumeroCalle = numeroCalle;
            Piso = piso;
            Departamento = departamento;
            CodigoPostal = codigoPostal;
            Localidad = localidad;
            UnaProvincia = unaProvincia;
            PrefijoTel = _telediscado1;
            NumeroTel = _telefono1;
            EsCelular = _esCelular1;
            PrefijoTel2 = _telediscado2;
            NumeroTel2 = _telefono2;
            EsCelular2 = _esCelular2;
            Mail = _mail;
        }


        public string Mail { get; set; }

        public bool EsSucursal { get; set; }

        public long IdDomicilio { get; set; }

        public string Calle { get; set; }

        public string NumeroCalle { get; set; }

        public string Piso { get; set; }

        public string Departamento { get; set; }

        public string CodigoPostal { get; set; }

        public string PrefijoTel { get; set; }

        public string NumeroTel { get; set; }

        public bool EsCelular { get; set; }

        public string PrefijoTel2 { get; set; }

        public string NumeroTel2 { get; set; }

        public bool EsCelular2 { get; set; }

        public string Fax { get; set; }

        public string Localidad { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string Observaciones { get; set; }

        public Provincia UnaProvincia { get; set; }

        public TipoDomicilio UnTipoDomicilio { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Sexo { get; set; }

        public int Nacionalidad { get; set; }        
    }
}
