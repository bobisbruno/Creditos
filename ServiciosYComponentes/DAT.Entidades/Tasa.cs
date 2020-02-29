using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Tasa : IDisposable
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

        ~Tasa()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private int id;
        private DateTime? fechainicio;
        private DateTime? fechafin;
        private DateTime? fechainiciovigencia;
        private DateTime? fechaFinVigencia;
        private double tna;
        private double tea;
        private double gastoadministrativo;
        private int? cantcuotas;
        private int? cantcuotasHasta;
        private string lineacredito;
        private int plazo;
        private string observaciones;
        private Prestador prestador;
        private Comercializador comercializador;
        private DateTime? fechaAprobacion;
        private Auditoria unaAuditoria;
        private Auditoria unaAudAprobacion;
        private bool aprobada;

        #region Getters y Setters

        public Auditoria UnaAudAprobacion
        {
            get { return unaAudAprobacion; }
            set { unaAudAprobacion = value; }
        }
        public Auditoria UnaAuditoria
        {
            get { return unaAuditoria; }
            set { unaAuditoria = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime? FechaInicio
        {
            get { return fechainicio; }
            set { fechainicio = value; }
        }
        public DateTime? FechaFin
        {
            get { return fechafin; }
            set { fechafin = value; }
        }
        public DateTime? FechaInicioVigencia
        {
            get { return fechainiciovigencia; }
            set { fechainiciovigencia = value; }
        }
        public DateTime? FechaFinVigencia
        {
            get { return fechaFinVigencia; }
            set { fechaFinVigencia = value; }
        }
        public double TNA
        {
            get { return tna; }
            set { tna = value; }
        }
        public double TEA
        {
            get { return tea; }
            set { tea = value; }
        }

        public double GastoAdministrativo
        {
            get { return gastoadministrativo; }
            set { gastoadministrativo = value; }
        }
        public int? CantCuotas
        {
            get { return cantcuotas; }
            set { cantcuotas = value; }
        }
        public int? CantCuotasHasta
        {
            get { return cantcuotasHasta; }
            set { cantcuotasHasta = value; }
        }
        public string LineaCredito
        {
            get { return lineacredito; }
            set { lineacredito = value; }
        }
        public int Plazo
        {
            get { return plazo; }
            set { plazo = value; }
        }
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }
        //public string UsuarioCarga
        //{
        //    get { return usuariocarga; }
        //    set { usuariocarga = value; }
        //}
        //public DateTime? FUltModificacion
        //{
        //    get { return fultmodificacion; }
        //    set { fultmodificacion = value; }
        //}
        //public string UsuarioAprobacion
        //{
        //    get { return usuarioaprobacion; }
        //    set { usuarioaprobacion = value; }
        //}
        public DateTime? FAprobacion
        {
            get { return fechaAprobacion; }
            set { fechaAprobacion = value; }
        }

        public Prestador Prestador
        {
            get { return prestador; }
            set { prestador = value; }
        }

        public Comercializador Comercializador
        {
            get { return comercializador; }
            set { comercializador = value; }
        }

        public bool Aprobada
        {
            get { return aprobada; }
            set { aprobada = value; }
        }

        #endregion Getters y Setters

        public Tasa()
        { }

        public Tasa(int idTasa, 
                    DateTime? fechaInicio,
                    DateTime? fechaFin, 
                    DateTime? fechaInicioVigencia,
                    DateTime? fFinVigencia,
                    double tna, 
                    double tea,
                    double gastoAdministrativo, 
                    int? cantCuotas, 
                    int? cantCuotasHasta,
                    string lineaCredito, 
                    string observaciones,
                    string usuarioCarga, 
                    DateTime? fUltModificacion,
                    string usuarioAprobacion, 
                    DateTime? fAprobacion,
                    bool aprobada)
        {
            this.ID = idTasa;
            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;
            this.FechaInicioVigencia = fechaInicioVigencia;
            this.FechaFinVigencia = fFinVigencia;
            this.TNA = tna;
            this.TEA = tea;
            this.GastoAdministrativo = gastoAdministrativo;
            this.CantCuotas = cantCuotas;
            this.CantCuotasHasta = cantCuotasHasta;
            this.LineaCredito = lineaCredito;
            this.Observaciones = observaciones;
            this.FAprobacion = fAprobacion;
            this.Aprobada = aprobada;
            this.UnaAuditoria = new Auditoria();
            this.UnaAudAprobacion = new Auditoria();
            this.Prestador = new Prestador();
            this.Comercializador = new Comercializador();

            //this.UsuarioCarga = usuarioCarga;
            //this.FUltModificacion = fUltModificacion;
            //this.UsuarioAprobacion = usuarioAprobacion;
            //this.FAprobacion = fAprobacion;        
        }

        public Tasa(int idTasa,
                    DateTime? fInicio,
                    DateTime? fFin,
                    DateTime? fInicioVigencia,
                    DateTime? fFinVigencia,
                    double tna,
                    double tea,
                    double gastoAdministrativo,
                    int? cantCuotas,
                    int? cantCuotasHasta,
                    string lCredito,
                    string observaciones,
                    DateTime? fAprobacion,
                    bool aprobada,
                    Prestador unPrestador,
                    Comercializador unComercializador,
                    Auditoria unaAuditoria)
        {
            this.ID = idTasa;
            this.FechaInicio = fInicio;
            this.FechaFin = fFin;
            this.FechaInicioVigencia = fInicioVigencia;
            this.FechaFinVigencia = fFinVigencia;
            this.TNA = tna;
            this.TEA = tea;
            this.GastoAdministrativo = gastoAdministrativo;
            this.CantCuotas = cantCuotas;
            this.CantCuotasHasta = cantCuotasHasta;
            this.LineaCredito = lCredito;
            this.Observaciones = observaciones;
            this.FAprobacion = fAprobacion;
            this.UnaAuditoria = unaAuditoria;
            this.Aprobada = aprobada;
            this.UnaAudAprobacion = new Auditoria();
            this.Prestador = unPrestador;
            this.Comercializador = unComercializador;
        }

        public Tasa(int idTasa, 
                    DateTime? fInicio,
                    DateTime? fFin, 
                    DateTime? fInicioVigencia,
                    DateTime? fFinVigencia,
                    double tna,
                    double tea,
                    double gastoAdministrativo, 
                    int? cantCuotas,
                    int? cantCuotasHasta,
                    string lCredito,
                    string observaciones,                    
                    DateTime? fAprobacion,
                    bool aprobada,
                    Prestador unPrestador,
                    Comercializador unComercializador,
                    Auditoria unaAudAprobacion,
                    Auditoria unaAprobacion)
        {
            this.ID = idTasa;
            this.FechaInicio = fInicio;
            this.FechaFin = fFin;
            this.FechaInicioVigencia = fInicioVigencia;
            this.FechaFinVigencia = fFinVigencia;
            this.TNA = tna;
            this.TEA = tea;
            this.GastoAdministrativo = gastoAdministrativo;
            this.CantCuotas = cantCuotas;
            this.CantCuotasHasta = cantCuotasHasta;
            this.LineaCredito = lCredito;
            this.Observaciones = observaciones;
            this.FAprobacion = fAprobacion;
            this.UnaAuditoria = unaAuditoria;
            this.UnaAudAprobacion = unaAudAprobacion;
            this.Aprobada = aprobada;
            this.Prestador = unPrestador; //new Prestador(idPrestador, razonSocialPrestador, cuitPrestador);
            this.Comercializador = unComercializador; //new Comercializador(idcomercializador, razonSocialComercializador, cuitComercializador, "");
        }
    }

    #region Errores de Clase
    [Serializable]
    public class TasaException : System.ApplicationException
    {
        public TasaException(string mensaje)
            : base(mensaje)
        {
        }
    }
    #endregion
}
