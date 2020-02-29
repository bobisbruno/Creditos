using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Anses.Embozo.Dominio
{
    [Serializable]
    public enum enum_TipoEstadoEmbozado
    {
        Pendiente = 1,
        BuscaDatosTarjetaAEmbozar = 2,
        RecibeDatosTarjetaAEmbozarOK = 3 ,
        RecibeDatosTarjetaAEmbozarError = 4,
        OficinaEmbozadoraError = 5,
        EnvioImprimirOK = 6,
        EnvioImprimirError =7,
        TarjetaImpresaOK = 8,
        TarjetaEmbozadaOK = 9,
        TarjetaEmbozadaErrorManual = 10
    }

    [Serializable]
    public class Tarjeta: IDisposable  
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

        ~Tarjeta()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion    

        public long NroBeneficiario { get; set; }
        public Persona Persona { get; set; }
        public Udai Udai { get; set; }
        public DateTime FechaNovedad { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string Destino { get; set; }
        public string Motivo { get; set; }
        public string Mensaje { get; set; }
        public string DigitoVerificador { get; set; }
        public string Parentesco { get; set; }
        public string BeneficiarioEmbozado { get; set; }
        public Usuario  Usuario { get; set; }
        public Embozado Embozado { get; set; }    
    }

    [Serializable]
    public class TarjetaEmbozado 
    {      
        public long NroBeneficiario { get; set; }
        public long NroTarjeta { get; set; }
        public Persona Persona { get; set; }     
        public DateTime FechaNovedad { get; set; }
        public Usuario Usuario { get; set; }
        public bool Seleccionado { get; set; }
        public string Observacion { get; set; }

        public TarjetaEmbozado() { }

        public TarjetaEmbozado(long _NroBeneficiario, Persona _Persona, DateTime _FechaNovedad, Usuario _Usuario)
        {
            NroBeneficiario = _NroBeneficiario;
            Persona = _Persona;
            FechaNovedad = _FechaNovedad;
            Usuario = _Usuario;
        }

        public TarjetaEmbozado(long _NroBeneficiario, Persona _Persona, DateTime _FechaNovedad)
        {
            NroBeneficiario = _NroBeneficiario;
            Persona = _Persona;
            FechaNovedad = _FechaNovedad;
        } 
    }
}
