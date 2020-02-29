using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;


namespace ANSES.Microinformatica.DATComPlus
{
	/// <summary>
	/// Summary description for Utilidades.
	/// </summary>
	internal class Utilidades
	{
		public Utilidades()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region public static string Calculo_MAC(string sDatosMac)
		public static string Calculo_MAC(string sDatosMac)
		{
			string MAC;
			ANSES.Microinformatica.Lib.Seguridad.CrytoNet csCryto = new ANSES.Microinformatica.Lib.Seguridad.CrytoNet();
						
			string clave=ConfigurationSettings.AppSettings["DAT_ClaveMAC"];	//Obtiene la clave para la generacion del Machine.config
						
			MAC = csCryto.MacSHA1(sDatosMac.ToString() ,clave);
			return MAC;
		}
		#endregion	

		#region static string Armo_String_MAC (Object[] datos)
		public static string Armo_String_MAC (Object[] datos)
		{
			StringBuilder sDatosMac = new StringBuilder();
			int longitud = datos.Length;
			for (int i = 0; i < longitud ; i++)
			{
				sDatosMac.Append(datos[i]);
			}
			return sDatosMac.ToString();
		}
		#endregion	

		#region string GeneraNombreArchivo (string nombreConsulta, long prestador, out string rutaArchivo)
		public static string GeneraNombreArchivo (string nombreConsulta, long prestador, out string rutaArchivo)
		{
			try
			{
				rutaArchivo = new Conexion().RutaArchivo();
						
				string nombreArchivo = nombreConsulta+"."+prestador.ToString()+"."+DateTime.Today.ToString("yyyyMMdd")+"."+DateTime.Now.ToString("HHmmss") + ".txt";

				if (!Directory.Exists(rutaArchivo))
					Directory.CreateDirectory(rutaArchivo);

				if (File.Exists(rutaArchivo + nombreArchivo))
					File.Delete(rutaArchivo + nombreArchivo);
				return nombreArchivo;
			}
			catch(Exception err)
			{
				throw err;
			}
		}
		#endregion

		#region void ComprimirArchivo (string rutaArchivo, string nombreArchivo)
		public static void ComprimirArchivo (string rutaArchivo, string nombreArchivo)
		{
			string archivo = rutaArchivo+nombreArchivo;
			string archivoZip = rutaArchivo+nombreArchivo+".zip";

			if (File.Exists(archivo))
			{
				Crc32 crc = new Crc32();
				ZipOutputStream s = new ZipOutputStream(File.Create(archivoZip));
		
				s.SetLevel(6); // 0 - store only to 9 - means best compression
		
				
				FileStream fs = File.OpenRead(archivo);
			
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				ZipEntry entry = new ZipEntry(nombreArchivo);
			
				
				entry.DateTime = DateTime.Now;
							
				entry.Size = fs.Length;
				fs.Close();
			
				crc.Reset();
				crc.Update(buffer);
			
				entry.Crc  = crc.Value;
			
				s.PutNextEntry(entry);
			
				s.Write(buffer, 0, buffer.Length);
			
				s.Finish();
				s.Close();
				
			}

			
		}
		#endregion

		#region static void Borro_Archivo (string rutaArchivo)
		public static void BorrarArchivo (string rutaArchivo, string nombreArchivo)
		{
			
			if (File.Exists(rutaArchivo+nombreArchivo))
			{
				File.Delete(rutaArchivo+nombreArchivo);
			}
		
		}
		#endregion

		#region static void BorrarArchivosMasXDias (string rutaArchivo, byte cantDias
		public static void BorrarArchivosMasXDias (string rutaArchivo, byte cantDias)
		{
			DateTime fecha = DateTime.Today.AddDays(cantDias*-1);
			
			DateTime fechaCreacion;
			String[] archivos = Directory.GetFiles(rutaArchivo);
			string arch;
			foreach (string archivo in archivos) 
			{				
				arch = rutaArchivo+archivo;
				if (File.Exists(arch))
				{
					fechaCreacion = File.GetCreationTime(arch);
					if (fechaCreacion < fecha)
					{
						File.Delete(arch);
					}
				}
			}
		}
			#endregion
	
		}

		
	}

