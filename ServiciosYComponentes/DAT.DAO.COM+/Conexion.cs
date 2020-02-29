using System;
using System.Reflection ; 
using System.Configuration ;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes; 

namespace ANSES.Microinformatica.DATComPlus
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
            return new SqlConnection(ConectarString());
		}	

		public string  ConectarString()
		{
            return ConfigurationManager.ConnectionStrings["DAT_V01"].ConnectionString;
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