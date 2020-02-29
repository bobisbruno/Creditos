using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Entidad_Prest_Comer : ValidaEntidad<Entidad_Prest_Comer> 
    {
        //#region Dispose

        //private bool disposing;

        //public void Dispose()
        //{
        //    // Llamo al método que contiene la lógica
        //    // para liberar los recursos de esta clase.
        //    Dispose(true);
        //}

        //protected virtual void Dispose(bool b)
        //{
        //    // Si no se esta destruyendo ya…
        //    if (!disposing)
        //    {
        //        // La marco como desechada ó desechandose,
        //        // de forma que no se puede ejecutar este código
        //        // dos veces.
        //        disposing = true;

        //        // Indico al GC que no llame al destructor
        //        // de esta clase al recolectarla.
        //        GC.SuppressFinalize(this);

        //        // … libero los recursos… 
        //    }
        //}

        //~Entidad_Prest_Comer()
        //{
        //    // Llamo al método que contiene la lógica
        //    // para liberar los recursos de esta clase.
        //    Dispose(true);
        //}
        //#endregion

        private long id;
        private long cuit;
        private string razonsocial;
        private string nombrefantasia;
        private Estado unEstado;        
        private string observaciones;
        private DateTime fechainicio;
        private Auditoria unAuditoria;
        private List<Tasa> tasas;
        private int idEstado;
        private Domicilio unDomicilio;      
      
        
        #region Getters y Setters

        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        public long Cuit
        {
            get { return cuit; }
            set { cuit = value; }
        }
        public string RazonSocial
        {
            get { return razonsocial; }
            set { razonsocial = value; }
        }
        public string NombreFantasia
        {
            get { return nombrefantasia; }
            set { nombrefantasia = value; }
        }
        public Estado UnEstado
        {
            get { return unEstado; }
            set { unEstado = value; }
        }
        public int IDEstado
        {
            get
            {
                if (this.unEstado != null)
                    return this.unEstado.IdEstado;
                else
                    return -1;
                }
            set { idEstado = this.unEstado.IdEstado; }
            
            
        }
        public Auditoria UnAuditoria
        {
            get { return unAuditoria; }
            set { unAuditoria = value; }
        }
           
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }
        public DateTime FechaInicio
        {
            get { return fechainicio; }
            set { fechainicio = value; }
        }
        public List<Tasa> Tasas
        {
            get { return tasas; }
            set { tasas = value; }
        }
        public Domicilio UnDomicilio
        {
            get { return unDomicilio; }
            set { unDomicilio = value; }
        } 

        #endregion Getters y Setters

        public Entidad_Prest_Comer()
        {
            ID = 0;
            Cuit = 0;
            RazonSocial = string.Empty ;
            NombreFantasia = string.Empty;
            UnEstado = new Estado();
            IDEstado = 0;
            UnAuditoria = new Auditoria();
            Observaciones = string.Empty;
            FechaInicio = new DateTime();
            Tasas = new List<Tasa>();
            UnDomicilio = new Domicilio();
        }

        public Entidad_Prest_Comer(long id, string razonSocial)
        {
            ID = id;
            RazonSocial = razonSocial;
        }
    }   
}
