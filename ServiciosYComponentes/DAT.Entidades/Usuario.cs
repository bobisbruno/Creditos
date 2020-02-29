using System;
using System.Collections.Generic;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
	[Serializable]
    public class Usuario 
    {
        public string Legajo { get; set; }
        public string ApellidoNombre { get; set; }
        public string OficinaCodigo { get; set; }
        public string OficinaDescripcion { get; set; }
        public string Ip { get; set; }
        public string Grupo { get; set; }

        public Usuario() {}

        public Usuario(           
            string legajo,
            string apellidoNombre,
            string oficinaCodigo,
            string oficinaDescripcion,
            string ip,
            string grupo)
        {          
            Legajo = legajo;
            ApellidoNombre = apellidoNombre;
            OficinaCodigo = oficinaCodigo;
            OficinaDescripcion = oficinaDescripcion;
            Ip=ip;
            Grupo=grupo;
        }

        public Usuario(String legajo, String oficinaCodigo, string iP)
        {
            this.Legajo = legajo;
            this.OficinaCodigo = oficinaCodigo;
            this.Ip = iP;        
        }     
                
        public Usuario(String legajo, String apellidoNombre)
        {
            this.Legajo = legajo;          
            this.ApellidoNombre = apellidoNombre;
        }

        public Usuario(String legajo)
        {
            this.Legajo = legajo;
        }        
    }
}
