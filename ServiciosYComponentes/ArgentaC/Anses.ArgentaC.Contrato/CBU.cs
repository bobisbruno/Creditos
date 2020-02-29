using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class CBU
    {
        public Tipo Banco { get; set; }
        public Tipo Sucursal { get; set; }
        public string _CBU { get; set; }

        public CBU() 
        {
            this.Banco = new Tipo();
            this.Sucursal = new Tipo();
        }

        public CBU(string id_banco, string banco, string  id_sucursal, string sucursal, string cbu)
        {
            this.Banco = new Tipo(id_banco, banco);
            this.Sucursal = new Tipo(id_sucursal, sucursal);
            this._CBU = cbu;
        }

        public CBU(Tipo banco, Tipo sucursal, string cbu)
        {
            this.Banco = new Tipo();
            this.Sucursal = new Tipo();

            this.Banco = banco;
            this.Sucursal = sucursal;
            this._CBU = cbu;
        }

    }

    [Serializable]
    public class CBUPim
    {
        public bool Respuesta { get; set; }
        public string Mensaje { get; set; }
        public string CBU { get; set; }

        public CBUPim()
        {
        }

        public CBUPim(bool _respuesta, string _mensaje, string _cbu)
        {
            this.Respuesta = _respuesta;
            this.Mensaje = _mensaje;
            this.CBU = _cbu;
        }
    }
}
