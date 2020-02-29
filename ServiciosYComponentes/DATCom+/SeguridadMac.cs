using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
namespace ANSES.Microinformatica.Lib.Seguridad
{
	/// <summary>
	/// Summary description for SeguridadMac.
	/// </summary>
	public class CrytoNet
	{
		public CrytoNet()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public string MacSHA1(string dato, string clave)
	{		
			
			UTF8Encoding utf = new UTF8Encoding();
				
				byte[] key  = utf.GetBytes(clave.ToCharArray());
			
			HMACSHA1 oMac = new HMACSHA1(key);
			SHA1Managed  oSha512 = new SHA1Managed();
			
				byte[] Dato =  utf.GetBytes(dato.ToCharArray());

				byte[]  Hash = oMac.ComputeHash(Dato);
			//byte[]  Hash =oSha512.ComputeHash(Dato);
			//String MAC = Convert.ToBase64String(Hash);
			String MAC = toHexa(Hash);
			
			return MAC;
			
	}
		public string MacSHA1(DataSet DSdato, string clave)
		{		
			string dato=DSdato.GetXml();
			string MAC= MacSHA1(dato,clave);
			return MAC;
			
		}
		public string Mac3Des(string dato, string clave)
		{		
			
			UTF8Encoding utf = new UTF8Encoding();
			byte[] keyin  = utf.GetBytes(clave.ToCharArray());
			byte[] key = new byte[24];
			for (int i = 0; i <= 23; i++)
			{
				if (i< keyin.Length)
				{key[i]=keyin[i];}
				else
				{key[i]=0;}
			}

		
			
			//HMACSHA1 oMac = new HMACSHA1(key);
			
			MACTripleDES oMac = new MACTripleDES(key);

			
			byte[] Dato =  utf.GetBytes(dato.ToCharArray());

			byte[]  Hash = oMac.ComputeHash(Dato);
			//byte[]  Hash =oSha512.ComputeHash(Dato);
			String MAC = toHexa(Hash);
			return MAC.ToUpper();
			
		}
		public string Mac3Des(DataSet DSdato, string clave)
		{		
			string dato=DSdato.GetXml();
			string MAC= Mac3Des(dato,clave);
			return MAC;
			
		}
		private string toHexa(byte[] arreglo)
		{
			string salida="";

			string strhex="";

			for (int i = 0; i < arreglo.Length; i++)
			{ 
				strhex=arreglo[i].ToString("x");
				salida =salida +strhex.PadLeft(2,'0');
			}

			return salida.ToUpper();

		}



	}
}
