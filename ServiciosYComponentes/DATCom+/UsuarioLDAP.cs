using System;
using System.DirectoryServices;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Configuration;


namespace ANSES.Microinformatica.DATComPlus
{

	//El codigo de abjo esta comentado
	#region Interface IUsuarioLDAP
	/// <summary>
	/// Define los métodos para acceder a la clase AdaptadorUsuarioLDAP, 
	/// a los cuales se les van a asignar permisos en COM+
	/// </summary>
	//	[GuidAttribute("E74E30B5-01A4-4ac8-A9E9-2EFEE8EEFB1C")]
	//	public interface IUsuarioLDAP
	//	{
	//		/// <summary>
	//		/// Definicón del método FindByUserName
	//		/// </summary>
	//		/// <param name="userName">Nombre de usuario en el ActiveDirectory</param>
	//		/// <returns>"True" si se encontró el usuario, sinó "false"</returns>
	//		bool FindByUserName(string userName);
	//
	//	}
	#endregion Interface IUsuarioLDAP	


	/// <summary>
	/// Implementa los métodos para buscar propiedades de los usuarios en el 
	/// ActiveDirectory Por ejemplo: "LDAP://anses.gov.ar".
	/// </summary>

	//	[ProgId("AdaptadorUsuarioLDAP")]
	//	[ClassInterface(ClassInterfaceType.None)]
	//	[GuidAttribute("F12B0F4F-6588-4577-9D46-C391B6C1FBBF")]
	//	[Transaction(TransactionOption.Disabled)]
	//	[ComponentAccessControl(true)]
	//	[SecureMethod]
	//: ServicedComponent, IUsuarioLDAP			//A continuacion de la declaracion de la clase
	internal class UsuarioLDAP 
	{

		#region Atributos

		string _IDUsuario				= String.Empty;
		string _ApellidoNombe		= String.Empty ;
		string _CUITPrestador			= String.Empty;
		string _Mail						= String.Empty;
        
		#endregion Atributos
	
		#region Constructores
		
        public UsuarioLDAP()
		{
		}

		#endregion Constructores

		#region Métodos públicos
		/// <summary>
		/// Busca las propiedades de un Usuario según el nombre
		/// </summary>
		/// <param name="userName">Nombre de usuario en el ActiveDirectory</param>
		/// <returns>"True" si se encontró el usuario, sinó "false"</returns>
		public bool FindByUserName(string NombreUsuario)
		{
			try
			{
				string IDUsuario		= ConfigurationSettings.AppSettings["DAT_LDAP_ID"] ;
				string Nombre		= ConfigurationSettings.AppSettings["DAT_LDAP_Nombre"] ;
				string CUITPrestador	= ConfigurationSettings.AppSettings["DAT_LDAP_Prestador"] ;
				string Mail				= ConfigurationSettings.AppSettings["DAT_LDAP_Mail"] ;
				string LDAPPath		= ConfigurationSettings.AppSettings["DAT_LDAP_Dominio"];

				//Verifico la existencia de las entradas que considero vitales, en el Machine.config
				if ( IDUsuario == String.Empty ||
					LDAPPath == String.Empty ||
					CUITPrestador == String.Empty 
					)
				{
				
					throw new Exception(@"No se pudieron obtener todas las entradas ( IDUsuario, LDAPPath, CUITPrestador ) en el archivo de configuración, para buscar el usuario mediante LDAP.");
				}
				else
				{

					#region BUSCO EN EL LDAP LOS DATOS DEL USUARIO
					
					ResultPropertyCollection PropiedadesUsuario = TraerDatosUsuario( NombreUsuario, LDAPPath );

					foreach( string PropNombre in PropiedadesUsuario.PropertyNames )
					{
						
						if ( PropNombre.ToUpper() == IDUsuario.ToUpper() ) { _IDUsuario =PropiedadesUsuario[ PropNombre ][0].ToString(); }

						if ( PropNombre.ToUpper() == Nombre.ToUpper() ) { _ApellidoNombe =PropiedadesUsuario[ PropNombre ][0].ToString(); }

						if ( PropNombre.ToUpper() == CUITPrestador.ToUpper() ) { _CUITPrestador =PropiedadesUsuario[ PropNombre ][0].ToString(); }

						if ( PropNombre.ToUpper() == Mail.ToUpper() ) { _Mail =PropiedadesUsuario[ PropNombre ][0].ToString(); }


					}

					return true;

					#endregion

				}

			}
			catch(Exception Err)
			{
				throw Err;
			}
		}

		#endregion Métodos públicos

		#region Métodos privados

		private ResultPropertyCollection TraerDatosUsuario(string IDUsuario, string ActiveDirectoryPath )
		{
			DirectoryEntry myEntry = new DirectoryEntry( ActiveDirectoryPath );
			DirectorySearcher mySearcher = new DirectorySearcher( myEntry );

			mySearcher.Filter = FormatFilter("user",IDUsuario) ;

			SearchResult mySearchResult = mySearcher.FindOne();
			if (mySearchResult != null)
			{
				return mySearchResult.Properties;
			}
			else
			{
				return null;
			}
		}

		// Formatea la cadena para el filtro
		private string FormatFilter(string objectCategory, string filter)
		{
			String result;
			result = String.Format("(&(objectCategory={0})(name={1}))", objectCategory, filter);
			return result;
		}

		#endregion Métodos privados

		#region Propiedades

		public string IDUsuario
		{
			get	{ return _IDUsuario; }
		}

		public string ApellidoNombe
		{
			get	{ return _ApellidoNombe; }
		}

		public string CUITPrestador
		{
			get	{ return _CUITPrestador; }
		}

		public string Mail
		{
			get	{ return _Mail; }
		}

		#endregion Propiedades

	}



}
