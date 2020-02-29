using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anses.ArgentaC.Comun.Domain
{
    [Serializable]
    public class Usuario : DomainObject
    {
        public string UsuarioDesc { get; set; }
        public string Nombre { get; set; }
        public string CueOCuit { get; set; }
        public string Cue { get; set; }
        public string Cuit { get; set; }
        public string Establecimiento { get; set; }
        public string Ip { get; set; }
        public string Oficina { get; set; }
        public string OficinaDesc { get; set; }
        public string Grupo { get; set; }

        public Usuario()
        {

        }

        public Usuario(
            long _id,
            string _UsuarioDesc,
            string _Nombre,
            string _CueOCuit,
            string _Establecimiento,
            string _Ip,
            string _Oficina,
            string _OficinaDesc,
            string _Grupo)
        {
            Id = _id;
            UsuarioDesc = _UsuarioDesc;
            Nombre = _Nombre;
            CueOCuit = _CueOCuit;
            Establecimiento = _Establecimiento;
            Ip = _Ip;
            Oficina = _Oficina;
            OficinaDesc = _OficinaDesc;
            Grupo = _Grupo;
        }

        public void ValidarParaPT()
        {
            //Comprueba que sea un usuario válido para el Piso Tecnologico
            if (string.IsNullOrEmpty(UsuarioDesc) ||
                string.IsNullOrEmpty(Nombre) ||
                string.IsNullOrEmpty(Ip))
            {
                throw new Exception("El Usuario No es válido,Detalles: \n" +
                                    "UsuarioDesc:" + UsuarioDesc +
                                    "\nNombre:" + Nombre +
                                    "\nIp:" + Ip);
            }


        }


    }
}
