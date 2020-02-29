using System;
using System.Reflection ; 
using System.Configuration ;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes; 

namespace ANSES.Microinformatica.DATVPComPlus
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>

	internal class Conexion
	{
		public Conexion()
		{
		}
		public SqlConnection Conectar()
		{
			return new SqlConnection ( ConfigurationSettings.AppSettings["DAT_ConnString"] );
		}	

		public string  ConectarString()
		{
			return  ConfigurationSettings.AppSettings["DAT_ConnString"] ;
		}	

		public string  RutaArchivo()
		{
			return  ConfigurationSettings.AppSettings["DAT_RutaArchivo"] ;
		}	

		public int  MaxCantidadRegistros()
		{
			return  int.Parse(ConfigurationSettings.AppSettings["DAT_MaxCantidadRegistros"]) ;
		}	
		
		public string DelimitadorCampo()
		{
			return  ConfigurationSettings.AppSettings["DAT_DelimitadorCampo"] ;
		}	

		public string  ConectarStringAsinc()
		{
			return  ConfigurationSettings.AppSettings["DAT_ConnString_Asinc"] ;
		}	
		
	}	
}