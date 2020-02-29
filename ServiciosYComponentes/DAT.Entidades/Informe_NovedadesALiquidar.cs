using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Informe_NovedadesALiquidar : IDisposable
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

        ~Informe_NovedadesALiquidar()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public long nroInforme
        { get; set; }

        public DateTime Fecha_Informe
        { get; set; }

        public long id_Novedad
        { get; set; }

        public long Cuil
        { get; set; }

        public string Apellido_Nombre
        { get; set; }

        public string CBU
        { get; set; }

        public double Monto_Prestamo
        { get; set; }

        public long id_Beneficiario
        { get; set; }

        public string Leyenda
        { get; set; }
        
        public Informe_NovedadesALiquidar(){}

        public Informe_NovedadesALiquidar(long nroinforme, DateTime fecha_informe, long id_Novedad, long cuil, 
                                          string apellido_nombre, string cbu, double monto_prestamo, 
                                          long id_beneficiario, string leyenda)
        {
            this.nroInforme = nroinforme;
            this.Fecha_Informe = fecha_informe;
            this.id_Novedad = id_Novedad;
            this.Cuil = cuil;
            this.Apellido_Nombre = apellido_nombre.Trim();
            this.CBU = cbu;
            this.Monto_Prestamo = monto_prestamo;
            this.id_Beneficiario = id_beneficiario;
            this.Leyenda = leyenda;
        }


        #region Errores de Clase
        public class InformeException : System.ApplicationException
        {
            public InformeException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}
