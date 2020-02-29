using System;
using System.Reflection ; 
using System.Configuration ;
using System.Data;


namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
	public class Settings
	{
		public Settings()
		{
		}

        static public string RutaArchivo()
		{
            return  ConfigurationManager.AppSettings["DAT_RutaArchivo"];
		}	

		static public int  MaxCantidadRegistros()
		{
            return int.Parse(ConfigurationManager.AppSettings["DAT_MaxCantidadRegistros"]);
		}

        static public string DelimitadorCampo()
		{
            return ConfigurationManager.AppSettings["DAT_DelimitadorCampo"];
		}

        static public string ConectarStringAsinc()
		{
            return ConfigurationManager.AppSettings["DAT_ConnString_Asinc"];
		}

        static public string CodigoSistema()
        {
            return ConfigurationManager.AppSettings["DAT_Codigo_Sistema"];
        }     
	}	
}