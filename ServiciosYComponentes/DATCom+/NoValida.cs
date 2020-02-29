using System;

namespace ANSES.Microinformatica.DATComPlus
{
	/// <summary>
	/// Summary description for NoValida.
	/// </summary>
	public class NoValida : Exception
	{
		public NoValida (string mensaje): base(mensaje)
		{
		}
	}
}
