using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class SolicitudVamosPaseo :IDisposable
    {
        private long idSolicitud;
        private Beneficiario unBeneficiario;
        private DateTime fIngresoSolicitud;
        private int mensualSolicitud;
        private decimal importeTotal;
        private decimal importeFinanciado;

        private Byte cantCuotas;
        private Agencia agenciaMinorista;
        private Agencia agenciaMayorista;
        private DateTime? fDesde;
        private DateTime? fHasta;
        private Estado unEstado;
        private String nroExpediente;
        private Auditoria unaAuditoria;
        private string destino;
        private bool viajaSolo;
        private string usuario;
        private string oficina;



        #region Getters y Setters

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public string Oficina
        {
            get { return oficina; }
            set { oficina = value; }
        }


        public bool ViajaSolo
        {
            get { return viajaSolo; }
            set {viajaSolo=value;}
        }

        public long IdSolicitud
        {
            get { return idSolicitud; }
            set { idSolicitud = value; }
        }
        
        public Beneficiario UnBeneficiario
        {
            get { return unBeneficiario; }
            set { unBeneficiario = value; }
        }
        
        public DateTime FIngresoSolicitud
        {
            get { return fIngresoSolicitud; }
            set { fIngresoSolicitud = value; }
        }
        
        public int MensualSolicitud
        {
            get { return mensualSolicitud; }
            set { mensualSolicitud = value; }
        }


        public decimal ImporteFinanciado
        {
            get { return importeFinanciado; }
            set { importeFinanciado = value; }
        }

        public decimal ImporteTotal
        {
            get { return importeTotal; }
            set { importeTotal = value; }
        }
        
        public Byte CantCuotas
        {
            get { return cantCuotas; }
            set { cantCuotas = value; }
        }
        
        public Agencia AgenciaMayorista
        {
            get { return agenciaMayorista; }
            set { agenciaMayorista = value; }
        }

        public Agencia AgenciaMinorista
        {
            get { return agenciaMinorista; }
            set { agenciaMinorista = value; }
        }

        public string Destino
        {
            get { return destino; }
            set { destino = value; }
        
        }
        public DateTime? FDesde
        {
            get { return fDesde; }
            set { fDesde = value; }
        }
        
        public DateTime? FHasta
        {
            get { return fHasta; }
            set { fHasta = value; }
        }
        
        public Estado UnEstado
        {
            get { return unEstado; }
            set { unEstado = value; }
        }
        
        public String NroExpediente
        {
            get { return nroExpediente; }
            set { nroExpediente = value; }
        }        

        public Auditoria UnaAuditoria
        {
            get { return unaAuditoria; }
            set { unaAuditoria = value; }
        }
        #endregion

        public SolicitudVamosPaseo() { }

        public SolicitudVamosPaseo( long idSolicitud,DateTime fIngresoSolicitud, int mensualSolicitud,decimal importeTotal,
         Byte cantCuotas,DateTime fDesde,DateTime fHasta,String nroExpediente)
        {
            this.idSolicitud = idSolicitud;
            this.fIngresoSolicitud = fIngresoSolicitud;
            this.mensualSolicitud = mensualSolicitud;
            this.importeTotal = importeTotal;
            this.cantCuotas = cantCuotas;
            this.fDesde = fDesde;
            this.fHasta = fHasta;
            this.nroExpediente = nroExpediente;        
        }

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

        ~SolicitudVamosPaseo()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion   
    }

     #region Errores de Clase
    [Serializable]
    public class SolicitudVamosPaseoException : System.ApplicationException
    {
        public SolicitudVamosPaseoException(string mensaje)
            : base(mensaje)
        {
        }
    }
    #endregion
}
