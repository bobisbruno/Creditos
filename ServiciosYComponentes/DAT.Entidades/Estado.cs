using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Estado : IDisposable
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

        ~Estado()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Added Definitions


        private int idEstado;
        private string descEstado;
        private Boolean habilitado = true;
        private Auditoria unAuditoria;
        
        #endregion

        #region Private Get/Set
        
     

        public int IdEstado
        {
            get { return this.idEstado; }
            set { this.idEstado = value; }
        }

        public string DescEstado
        {
            get { return this.descEstado; }
            set { this.descEstado = value; }
        }

        public Boolean Habilitado
        {
            get { return this.habilitado; }
            set { this.habilitado = value; }
        }

        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }
         
        #endregion

        public Estado()
        {
            IdEstado = 0;
            DescEstado = string.Empty;
            Habilitado = false;
            UnAuditoria = new Auditoria();
        }

        public Estado(int IdEstado, string DescEstado)
        {
            this.idEstado = IdEstado;
            this.descEstado = DescEstado;
        }

        public Estado(int IdEstado, string DescEstado, bool habilitado)
        {
            this.IdEstado = IdEstado;
            this.DescEstado = DescEstado;
            this.Habilitado = habilitado; 
        }

        public Estado(int IdEstado)
        {
            this.idEstado = IdEstado;
            
        }
        #region Errores de Clase
        public class EstadoException : System.ApplicationException
        {
            public EstadoException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }

    [Serializable]
    public class EstadoDocumentacion : Estado
    { 
        public bool VerOnlineCarga {get;set;}
        public bool DebeIngresarCaja {get;set;}
        public bool ApruebaNovedad {get;set;}

        public EstadoDocumentacion() { }
        public EstadoDocumentacion(int _IdEstado, string _DescEstado, bool _habilitado,
                                    bool _VerOnlineCarga, bool _DebeIngresarCaja, bool _ApruebaNovedad):
                                    base (_IdEstado, _DescEstado, _habilitado) {

                VerOnlineCarga = _VerOnlineCarga;
                DebeIngresarCaja = _DebeIngresarCaja;
                ApruebaNovedad = _ApruebaNovedad;
    
        }
    }

    [Serializable]
    public class EstadoCaratulacion : Estado
    {
        public Int16 idEstadoExpediente { get; set; }

        public EstadoCaratulacion() { }
        public EstadoCaratulacion(int _IdEstado, string _DescEstado, bool _habilitado,
                                    Int16 _idEstadoExpediente) :
            base(_IdEstado, _DescEstado, _habilitado)
        {

            idEstadoExpediente = _idEstadoExpediente;
        }
    }


}
