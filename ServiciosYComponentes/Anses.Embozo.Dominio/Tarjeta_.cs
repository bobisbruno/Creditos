using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Anses.Embozo.Dominio
{  
    [Serializable]
    public class Tarjeta  
    {               
         public long NroBeneficiario { get; set; }
         public Persona Persona { get; set; }
         public Udai Udai { get; set; }
         public DateTime FechaAlta { get; set; }
         public DateTime FechaAsignacion { get; set; }
         public string Destino { get; set; }
         public string Mensaje { get; set; }
         public string ParentescoPami { get; set; }
	     public string DigitoVerificador { get; set; }        
        
        
        public Tarjeta() 
        {}

        public Tarjeta() 
        {
            
        }
    }
}
