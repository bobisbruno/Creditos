using System;
using System.Runtime.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
	/// <summary>
	/// Summary description for NoValida.
	/// </summary>
    /// 
    [Serializable]
	public class NoValida : Exception
	{
		public NoValida(string mensaje): base(mensaje){}
        public NoValida(string message, ApplicationException inner) : base(message, inner) { }
        protected NoValida(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
