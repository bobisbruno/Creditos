using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Sucursal : IDisposable
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

        ~Sucursal()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
        
        public long IdPrestador { get; set; }
        public string IdSucursal { get; set; }
        public string Denominacion { get; set; }
        public DateTime Fdesde { get; set; }
        public DateTime? Fhasta { get; set; }
        public short CantDiasHasta { get; set; }

        public Sucursal() { }
        public Sucursal(long _IdPrestador, 
                        string _IdSucursal,
                        string _Denominacion,
                        DateTime _Fdesde,
                        DateTime? _Fhasta,
                        short _CantDiasHasta) {
            this.IdPrestador = _IdPrestador;
            this.IdSucursal = _IdSucursal;
            this.Denominacion = _Denominacion;
            this.Fdesde = _Fdesde;
            this.Fhasta = _Fhasta;
            this.CantDiasHasta = _CantDiasHasta;        
        }

        public Sucursal(long _IdPrestador, string _IdSucursal, String _Denominacion)
        {
            IdPrestador = _IdPrestador;
            IdSucursal = _IdSucursal;
            Denominacion = _Denominacion;
       }

    }

    [Serializable]
    public class UDAI
    {
        public long IdUDAI { set; get;}
        public String UdaiDescripcion { set; get; }
        public String Domicilio { set; get; }
        public string CodigoPostal { get; set; }
        public int IdRegional { get; set; }
        public string Regional { get; set; }
        public string DomicilioRegional { get; set; }
        public string CodigoPostalRegional { get; set; }
        public string ProvinciaRegional { get; set; }

        public UDAI() { }

        public UDAI(long _IdUDAI, String _UdaiDescripcion, String _Domicilio, string _CodigoPostal)
        {
            IdUDAI = _IdUDAI;
            UdaiDescripcion = _UdaiDescripcion;
            Domicilio = _Domicilio;
            CodigoPostal = _CodigoPostal;          
        }

        public UDAI(long _IdUDAI, String _UdaiDescripcion, String _Domicilio, string _CodigoPostal, int _IdRegional)
        {
            IdUDAI = _IdUDAI;
            UdaiDescripcion = _UdaiDescripcion;
            Domicilio = _Domicilio;
            CodigoPostal = _CodigoPostal;
            IdRegional = _IdRegional;
        }

        public UDAI(long _IdUDAI, String _UdaiDescripcion, String _Domicilio, string _CodigoPostal, 
                    int _IdRegional, string _regional, string _domicilioRegional, string _codigoPostalRegional, string _provinciaRegional )
        {
            IdUDAI = _IdUDAI;
            UdaiDescripcion = _UdaiDescripcion;
            Domicilio = _Domicilio;
            CodigoPostal = _CodigoPostal;
            IdRegional = _IdRegional;
            Regional = _regional;
            DomicilioRegional = _domicilioRegional;
            CodigoPostalRegional = _codigoPostalRegional;
            ProvinciaRegional = _provinciaRegional;
        }


    }

    [Serializable]
    public class OficinaEmbozadaExpress
    {
        public string IdOficina { set; get; }
        public string Descripcion { set; get; }
        public string IdOficinaEntrega { set; get; }
        public DateTime HoraCorte { get; set; }
        public TipoEmbozado TipoEmbozado { get; set; }

        public OficinaEmbozadaExpress() { }

        public OficinaEmbozadaExpress(string _IdOficina, string _IdOficinaEntrega, DateTime _HoraCorte, TipoEmbozado _TipoEmbozado)
        {
            IdOficina = _IdOficina;        
            IdOficinaEntrega = _IdOficinaEntrega;
            HoraCorte = _HoraCorte;
            TipoEmbozado = _TipoEmbozado;
        }
    }
}
