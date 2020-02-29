using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Comercializador : Entidad_Prest_Comer, IDisposable 
    {
        public Comercializador() { }

        private DateTime? fechafin;

        public DateTime? FechaFin
        {
            get { return fechafin; }
            set { fechafin = value; }
        }

        public Comercializador(long id, string razonSocial,
                               long cuit, string nombreFantasia)
        {
            base.ID = id;
            base.RazonSocial = razonSocial;
            base.Cuit = cuit;
            base.NombreFantasia = nombreFantasia;
        }

        public Comercializador(long id, string razonSocial, long cuit,
                        string observaciones, Estado unEstado, Auditoria unAuditoria,//int idEstado,  string Usuario, DateTime fecultModificacion,
                        string nombreFantasia, DateTime operaDesde, DateTime? fechaFin)
        {
            base.ID = id;
            base.RazonSocial = razonSocial;
            base.Cuit = cuit;
            base.Observaciones = observaciones;
            /*base.idEstado = idEstado;
            base.Usuario = Usuario;*/
            base.UnEstado = unEstado;
            base.UnAuditoria = unAuditoria;
            base.NombreFantasia = nombreFantasia;
            base.FechaInicio = operaDesde;
            this.FechaFin = fechaFin;
            base.Tasas = new List<Tasa>();
        }

        public Comercializador(long id, string razonSocial, long cuit, string observaciones,
                               Estado unEstado, Auditoria unAuditoria, string nombreFantasia,
                               DateTime operaDesde, DateTime? fechaFin, Domicilio unDomicilio)
        {
            base.ID = id;
            base.RazonSocial = razonSocial;
            base.Cuit = cuit;
            base.Observaciones = observaciones;
            base.UnEstado = unEstado;
            base.UnAuditoria = unAuditoria;
            base.NombreFantasia = nombreFantasia;
            base.FechaInicio = operaDesde;
            this.FechaFin = fechaFin;
            base.Tasas = new List<Tasa>();
            base.UnDomicilio = unDomicilio;
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

        ~Comercializador()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Errores de Clase
        public class ComercializadorException : System.ApplicationException
        {
            public ComercializadorException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}
