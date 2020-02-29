using System;
using Microsoft.ApplicationBlocks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.ApplicationBlocks.Data;

namespace ANSES.Microinformatica.DATComPlus
{
	[Guid("94F6BC62-A47B-4b17-8643-631FD04842E0")] 
	public interface DAT_IPrestador
	{
		 DataSet TraerPrestador(string logon) ;

	}


	[Guid("737FA09A-FAF5-4787-A326-C2ABDCD28B79")] 
	[ ProgId( "DAT_Prestador" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 10) ]
	[JustInTimeActivation(true)]
	[Transaction(TransactionOption.Disabled)]
	[EventTrackingEnabled ( true )]
	[ComponentAccessControl(true)]
	[SecureMethod]
	public class Prestador:ServicedComponent,DAT_IPrestador
	{
		/*public long IdPrestador;
		public String RazonSocial ;*/
		

		public Prestador()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[SecurityRole("OperadorEntidad")]
		public DataSet  TraerPrestador(string logon) 
		{
			SqlConnection objCon = new SqlConnection();
			Conexion objCnn = new Conexion();	
			
			string RazonSocialPrestador = String.Empty;
			string IDPrestador = String.Empty;

			DataSet _ds = new DataSet();
			DataTable _dt = new DataTable();
			DataRow _drTemp ;
			
			_ds.Tables.Add( _dt );
			DataTable _dtPrestador = new DataTable();

			UsuarioLDAP oLDAP = new UsuarioLDAP();

			try
			{			
				
				oLDAP.FindByUserName( logon );
				
				string[] aPrestadores = oLDAP.CUITPrestador.Split(char.Parse(";"));

				#region Crea Columnas para el DataSet

					_dt.Columns.Add("IDUsuario",typeof(String) );
					_dt.Columns.Add("ApellidoyNombre",typeof(String) );
					_dt.Columns.Add("IDPrestador",typeof(String) );
					_dt.Columns.Add("CUITPrestador",typeof(String) );
					_dt.Columns.Add("RazonSocial",typeof(String) );
					_dt.Columns.Add("Mail",typeof(String) );
					
				#endregion

				foreach ( string sPrestador in aPrestadores )
				{
				
					_dtPrestador = SqlHelper.ExecuteDataset( objCnn.ConectarString(), "Prestador_Traer", sPrestador ).Tables[0];
				
					if ( _dtPrestador.Rows.Count > 0 )
					{
						foreach ( DataRowView _dr in _dtPrestador.DefaultView )
						{

							//							RazonSocialPrestador = _dtPrestador.Rows[0]["RazonSocial"].ToString();
							//							IDPrestador				= _dtPrestador.Rows[0]["IDPrestador"].ToString();

							RazonSocialPrestador = _dr["RazonSocial"].ToString();
							IDPrestador				= _dr["IDPrestador"].ToString();

							if ( RazonSocialPrestador != String.Empty ) 
							{
								#region LLeno el datatable con los registros

								_drTemp = _dt.NewRow(); 
					
								_drTemp ["IDUsuario"]					= oLDAP.IDUsuario			== String.Empty ? String.Empty :oLDAP.IDUsuario;
								_drTemp ["ApellidoyNombre"]			=	oLDAP.ApellidoNombe == String.Empty ? String.Empty :oLDAP.ApellidoNombe ;
								_drTemp ["IDPrestador"]					=	IDPrestador;
								_drTemp ["CUITPrestador"]				= sPrestador	== String.Empty ? String.Empty :sPrestador;
								_drTemp ["RazonSocial"]					= RazonSocialPrestador;
								_drTemp ["Mail"]							= oLDAP.Mail;
								
								_dt.Rows.Add( _drTemp );

							
								#endregion
							}
						}
					
					}

	
				}

				return _ds;

			} 
			catch(Exception err)
			{
				throw new Exception("Error al intentar obtener los datos del o los  prestador(es) " + oLDAP.CUITPrestador + " Buscando en LDAP por -> " + logon + " El error original es : " +  err  );
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
				_ds.Dispose();
				_dtPrestador.Dispose();
			}
		}
	}
}
