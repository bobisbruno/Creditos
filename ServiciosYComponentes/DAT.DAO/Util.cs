using System;
//using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;
//using System.Web.SessionState;
using System.Text;
using System.Security.Principal;
using System.Security;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
	/// <summary>
	/// Provee de funciones grales. de validacion de datos y administracion.
	/// </summary>
	public class Util
	{
		
		#region Contructores
		public Util()
		{
			//
			// TODO: Add constructor logic here
			//


		}
		#endregion Contructores

		#region Registra Error en Log de Eventos
		/// <summary>
		/// Muestra una popap de error
		/// </summary>
		/// 	

		#region Verifica Existencia del log personalizado
		private static  bool VerificaEntradaLog( string AplicacionNombre )
		{
			bool _Existe = false;
			
			try
			{
				if ( !EventLog.Exists( AplicacionNombre ) )
				{
					EventLog.CreateEventSource( AplicacionNombre , AplicacionNombre );
					
				}

				_Existe  = true;

			}
			catch( SecurityException )
			{
				//Generar Mensaje de Error.
				EventLog.WriteEntry("MICROInfo","No se pudo crear el Log personalizado ",EventLogEntryType.Warning);

			}

			return _Existe;
		}
		#endregion

		#endregion

		#region Set Focus - Da el foco a un control
		public static  void SetFocus(System.Web.UI.Page page, System.Web.UI.Control ctrl )
		{			
			string s = "<script language='javascript'>" +
				       "document.getElementById('" + ctrl.ID + "').focus() </script>";

			if (!page.IsStartupScriptRegistered("focus"))
                page.RegisterStartupScript("focus", s);
		}
		#endregion

		#region Tiene Acceso 
		/// <summary>
		/// Valida el acceso del usuario que ingreso y lo guarda en un 
		/// variable de sesion para Luego validar a que funciones puede
		/// acceder. retorna Verdadero o falso si puede acceder a formulario
		/// </summary>
		//		public static bool TieneAcceso()
		//		{
		//return true;
		//			bool bAcesso = false;
		//			bool bHabilitarSeguridad = false;
		//			string sDominio = String.Empty;
		//
		//			
		//			string Admin = String.Empty;						//Para los grupos en el Web.config
		//			string Operador1 = String.Empty;
		//			string Operador3 = String.Empty;
		//			string Supervisor1 = String.Empty;
		//			string Supervisor4 = String.Empty;
		//
		//			System.Web.HttpContext _Context = System.Web.HttpContext.Current;
		//			WindowsPrincipal UsuarioAutenticado = System.Web.HttpContext.Current.User as WindowsPrincipal;
		//
		//			// Leo desde el Web.Config  para determinar si habilito o no 
		//			// la seguridad
		//			bHabilitarSeguridad = bool.Parse(ConfigurationManager.AppSettings["AplicarSeguridad"].ToString());
		//			sDominio = ConfigurationManager.AppSettings["Dominio"].ToString();
		//
		//			//Tomo los grupos
		//			Admin			=	ConfigurationManager.AppSettings["SISA-Admin"].ToString();
		//			Operador1	= ConfigurationManager.AppSettings["SISA-Operador1"].ToString();
		//			Operador3	= ConfigurationManager.AppSettings["SISA-Operador3"].ToString();
		//			Supervisor1	= ConfigurationManager.AppSettings["SISA-Supervisor1"].ToString();
		//			Supervisor4	= ConfigurationManager.AppSettings["SISA-Supervisor4"].ToString();
		//			
		//			if (bHabilitarSeguridad)
		//			{
		//				if (UsuarioAutenticado.IsInRole( sDominio + @"\" + Admin ))
		//				{
		//					_Context.Session["NIVEL"]="SISA-Admin";
		//				}
		//				else if (UsuarioAutenticado.IsInRole( sDominio+ @"\" +Operador1 ) )
		//				{
		//					_Context.Session["NIVEL"]="SISA-Operador1";
		//				}
		//				else if (UsuarioAutenticado.IsInRole( sDominio+ @"\" + Operador3 ) )
		//				{
		//					_Context.Session["NIVEL"]="SISA-Operador3";
		//				}
		//				else if (UsuarioAutenticado.IsInRole( sDominio+ @"\" + Supervisor1 ) )
		//				{
		//					_Context.Session["NIVEL"]="SISA-Supervisor1";
		//				}
		//				else if (UsuarioAutenticado.IsInRole( sDominio+ @"\" + Supervisor4 ) )
		//				{
		//					_Context.Session["NIVEL"]="SISA-Supervisor4";
		//				}
		//				else
		//				{
		//					_Context.Session["NIVEL"]=String.Empty;
		//			
		//				}
		//
		//
		//				// Obtengo el nombre del formulario al que se esta intentando
		//				// ingresar
		//				string Formulario =_Context.Request.RawUrl.Split(char.Parse("/"))[2].ToUpper();
		//
		//				// Este es el DataSet donde guardo los datos leido del XML.
		//				DataSet _dsAcceso = new DataSet( ).ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\Acceso.config") ;
		//
		//				_dsAcceso.Tables[0].DefaultView.RowFilter= "IDGrupo = '" + _Context.Session["NIVEL"].ToString().ToUpper() +"'" ;
		//				_dsAcceso.Tables[0].DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
		//
		//				if ( _dsAcceso.Tables[0].DefaultView.Count > 0 )
		//				{
		//					DataView _dv	= new DataView( _dsAcceso.Tables[0].DefaultView ) ;
		//					
		//					_Context.Session["Acceso"] = _dv;
		//
		//					_dv.Dispose();
		//
		//					bAcesso=true; 
		//				} 
		//				
		//				//				switch (_Context.Session["NIVEL"].ToString().ToUpper())
		//				//				{
		//				//
		//				//					case "SISA-ADMIN" :
		//				//						if(Formulario == "AUDITORES.ASPX" ||
		//				//							Formulario == "CAPITULO.ASPX" ||
		//				//							Formulario == "ESTADONOTAS.ASPX" ||
		//				//							Formulario == "ESTSEGUIMIENTO.ASPX" ||
		//				//							Formulario == "ORGANISMO.ASPX" ||
		//				//							Formulario == "PAA.ASPX" ||
		//				//							Formulario == "RIESGO.ASPX" ||
		//				//							Formulario == "TIPOAUDITORIA.ASPX" ||
		//				//							Formulario == "TIPODOCUMENTO.ASPX" ||
		//				//							Formulario == "TIPOINFORME.ASPX" ||
		//				//							Formulario == "TEMARIESGO.ASPX" ||
		//				//							Formulario == "TIPOTAREA.ASPX" )
		//				//						{
		//				//							bAcesso = true;
		//				//						}
		//				//						break;
		//				//				
		//				//					case "SISA-OPERADOR1":
		//				//
		//				//						if (Formulario == "INFORME.ASPX" ||
		//				//							Formulario == "CONSULTA.ASPX" )
		//				//						{
		//				//							bAcesso=true;
		//				//						}
		//				//						break;
		//				//
		//				//					case "SISA-OPERADOR3":
		//				//
		//				//						if (Formulario == "INFORME.ASPX" ||
		//				//							Formulario == "CONSULTA.ASPX" )
		//				//						{
		//				//							bAcesso=true;
		//				//						}
		//				//						break;
		//				//
		//				//					case "SISA-SUPERVISOR1":
		//				//					case "SISA-SUPERVISOR4":
		//				//
		//				//						if (Formulario == "INFORME.ASPX" ||
		//				//							Formulario == "CONSULTA.ASPX" ||
		//				//							Formulario == "CONFIRMACION.ASPX" ||							//	de aqui para abajo hay que quitarlo 
		//				//							Formulario == "AUDITORES.ASPX" ||
		//				//							Formulario == "CAPITULO.ASPX" ||
		//				//							Formulario == "ESTADONOTAS.ASPX" ||
		//				//							Formulario == "ESTSEGUIMIENTO.ASPX" ||
		//				//							Formulario == "ORGANISMO.ASPX" ||
		//				//							Formulario == "PAA.ASPX" ||
		//				//							Formulario == "RIESGO.ASPX" ||
		//				//							Formulario == "TIPOAUDITORIA.ASPX" ||
		//				//							Formulario == "TIPODOCUMENTO.ASPX" ||
		//				//							Formulario == "TIPOINFORME.ASPX" ||
		//				//							Formulario == "TEMARIESGO.ASPX" ||
		//				//							Formulario == "TIPOTAREA.ASPX" )
		//				//						{
		//				//							bAcesso=true;
		//				//						}
		//				//						break;
		//				//				}
		//
		//				return bAcesso;
		//			}
		//			else
		//			{
		//				return true;
		//			}
		//		}

		#endregion

		#region LLenar Combo
		/// <summary>
		/// Procedimiento que llena un DropDownList en base a un Tabla especificada
		/// </summary>
		/// <param name="ComboBox">Objeto de tipo DropDowList.</param>
		/// <param name="ObjTabla">Objeto de tipo String que representa el nombre de la tabla.</param>
		public static void LLenarCombo(Object ComboBox,string ObjTabla)
		{
		
			LLenarCombo(ComboBox,ObjTabla,null);

		}
		
		/// <summary>
		/// Procedimiento que llena un DropDownList en base a un Tabla especificada
		/// </summary>
		/// <param name="ComboBox">Objeto de tipo DropDowList.</param>
		/// <param name="ObjTabla">Objeto de tipo String que representa el nombre de la tabla.</param>
		/// <param name="aParam">Array de objetos que para ejecutar los metodos de traer.</param>
		public static void LLenarCombo(Object ComboBox, string ObjTabla,Object[] aParam)
		{
			
			DropDownList _Combo = (DropDownList) ComboBox;

			DataSet _datos = new DataSet();

			System.Web.HttpContext _Context = System.Web.HttpContext.Current;
			_datos = (DataSet) _Context.Session["Prestador"];

			_Combo.Items.Clear();				//Elimina todos los elementos de Combo.

			switch (ObjTabla.ToString().ToUpper())
			{
				case "TIPOCONCEPTO":

					//_datos = IsInCache("TIPOCONCEPTO",3,aParam,false);

					if(_datos.Tables["TipoConcepto"].Rows.Count > 0)
					{
						_Combo.DataSource=_datos; 
						_Combo.DataTextField="DescTipoConcepto";
						_Combo.DataValueField="TipoConcepto";
					}
		
					break;

				case "CONCEPTOOPP":

					//_datos = IsInCache("CONCEPTOOPP",3,aParam,true);

					string filtro = "TipoConcepto = '" + aParam[0] + "'";
					
					_datos.Tables["ConceptoLiquidacion"].DefaultView.RowFilter = filtro;
					_datos.Tables["ConceptoLiquidacion"].DefaultView.RowStateFilter= DataViewRowState.CurrentRows;

					if(_datos.Tables["ConceptoLiquidacion"].DefaultView.Count > 0)
					{
						_Combo.DataSource=_datos.Tables["ConceptoLiquidacion"].DefaultView; 
						_Combo.DataTextField="DescConceptoLiq";
						_Combo.DataValueField="CodConceptoLiq";
						
					}

					break;

				case "CIERRES":

					_datos = IsInCache("CIERRES",3,aParam,false);

					if(_datos.Tables[0].Rows.Count > 0)
					{
						_Combo.DataSource=_datos; 
						_Combo.DataTextField="Mensual";
						_Combo.DataValueField="FecCierre";
					}

					break;

			}

			if(_datos.Tables.Count > 0){_Combo.DataBind(); }

			_Combo.Items.Insert(0,"[ Seleccione ]" );
			_Combo.SelectedIndex = -1;

			_datos.Dispose();
			_Combo.Dispose();

		}

		public static void LLenarCombo(Object ComboBox, string ObjTabla,Object[] aParam, DataSet _datos)
		{
			
			DropDownList _Combo = (DropDownList) ComboBox;

			_Combo.Items.Clear();				//Elimina todos los elementos de Combo.
			string filtro = string.Empty;
			switch (ObjTabla.ToString().ToUpper())
			{
				case "TIPOCONCEPTO":
					/* Si el parametro es:
					 *	 0 trae todo
					 *	 1 todo menos lo indeterminado
					 *	 2 solo indeterminado					 * 
					 * */
					
				switch (byte.Parse(aParam[0].ToString()))
				{
					case 1:
						filtro = "TipoConcepto not in (1,6)" ;

						break;
					case 2:
						filtro = "TipoConcepto in (1,6)" ;
							
						break;
					default:
						break;
				}
				
					_datos.Tables[0].DefaultView.RowFilter = filtro;
					if(_datos.Tables[0].Rows.Count > 0)
					{
						_Combo.DataSource=_datos.Tables[0].DefaultView; 
						_Combo.DataTextField="DescTipoConcepto";
						_Combo.DataValueField="TipoConcepto";
					}
					
					break;

				case "CONCEPTOOPP":
		
					filtro = "TipoConcepto = " + aParam[0];
					
					_datos.Tables[1].DefaultView.RowFilter = filtro;

					
					if(_datos.Tables[0].Rows.Count > 0)
					{
						_Combo.DataSource=_datos.Tables[1].DefaultView; 
						_Combo.DataTextField="DescConceptoLiq";
						_Combo.DataValueField="CodConceptoLiq";
						
					}

					break;
			}
			if(_datos.Tables.Count > 0){_Combo.DataBind(); }

			_Combo.Items.Insert(0,"[ Seleccione ]" );
			_Combo.SelectedIndex = -1;

			_datos.Dispose();
			_Combo.Dispose();

		}


		#endregion LLenar Combo

		#region IsInCache - Abre tablas que seran utilizadas frecuentemente"

		/// <summary>
		/// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
		/// </summary>
		/// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla
		///	con un tiempo por default de 10 Minutos.</param>
		/// <returns>Un DataSet</returns>
		public static DataSet IsInCache(string NombreTabla)
		{
		
			return IsInCache(NombreTabla,5);
		}

		/// <summary>
		/// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
		/// </summary>
		/// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
		/// <param name="TiempoMinCache">Tipo int. Representa el tiempo en Minutos del cache</param>		
		/// <returns>Un DataSet</returns>
		public static DataSet IsInCache(string NombreTabla,int TiempoMinCache)
		{
			return IsInCache(NombreTabla,TiempoMinCache,null,false);
		}

		/// <summary>
		/// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
		/// </summary>
		/// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
		/// <param name="Refrescar">Tipo Boolean. Representa si se desea volver a traer los datos desde la base</param>
		/// <returns>Un DataSet</returns>
		public static DataSet IsInCache(string NombreTabla,bool Refrescar)
		{
			return IsInCache(NombreTabla,5,null,Refrescar);
		}

		/// <summary>
		/// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
		/// </summary>
		/// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
		/// <param name="TiempoMinCache">Tipo int. Representa el tiempo en Minutos del cache</param>		
		/// <param name="Refrescar">Tipo Boolean. Representa si se desea volver a traer los datos desde la base</param>
		/// <returns>Un DataSet</returns>
		public static DataSet IsInCache(string NombreTabla,int TiempoMinCache,Object[] aParam,bool Refrescar)
		{
		
			DataSet _datos = new DataSet();

			System.Web.HttpContext _Context = System.Web.HttpContext.Current;

			// Elimino del Cache el Item seleccionado para volver a cargarlo.
			if ( Refrescar ) { 	_Context.Cache.Remove(NombreTabla); }

			if ( _Context.Cache[NombreTabla] != null ) 
			{
				
				_datos=(DataSet) _Context.Cache[NombreTabla] ;

			} 
			else 
			{

				//string Valor;
				switch (NombreTabla.ToString().ToUpper())
				{

//					case "TIPOCONCEPTO":
//
//						TipoConcConcLiqWS tpcon = new TipoConcConcLiqWS();
//						tpcon.Url= ConfigurationManager.AppSettings["url.Servicio.TipoConcConcLiq"];  
//						tpcon.Credentials=CredentialCache.DefaultCredentials;
//
//						_datos =  tpcon.Traer( long.Parse( aParam[0].ToString() ) );				//Corresponde al ID Prestador
//					
//						tpcon.Dispose();
//
//						break;

						
					
					case "CIERRES":
                        //CierresWS cie = new CierresWS();
                        //cie.Url= ConfigurationManager.AppSettings["url.Servicio.Cierres"];  
                        //cie.Credentials=CredentialCache.DefaultCredentials;

                        //_datos = cie.Trae_Cierres();

						//cie.Dispose();

						break;
				}
				

				// Agrego al Cache
				_Context.Cache.Insert(NombreTabla,_datos,null,Cache.NoAbsoluteExpiration,TimeSpan.FromMinutes(TiempoMinCache));
			
			}

			return _datos;

		
		}
		#endregion  IsInCache - Abre tablas que seran utilizadas frecuentemente

		#region Retorna el Path relativo
		/// <summary>
		/// Este método returna el path relativo 
		/// </summary>
		/// <param name="request">Objeto System.Web.HttpRequest </param>
		/// <returns>El Path relativo</returns>
		/// <example>Util.GetRelativePath( Request )</example>
		public static string GetRelativePath( HttpRequest request )
		{
		
			string sPath = String.Empty;

			try
			{
				if (request.ApplicationPath != "/" ) {  sPath = request.ApplicationPath; }

			}
			catch( Exception err )
			{
				throw err;
			}

			return sPath;
           
		}
		#endregion

		#region validoIP
		public static bool  ValidoIP(string IP)
		{		
			try
			{
				IPAddress ip = IPAddress.Parse(IP);
			}
			catch
			{
				return false;
			}
			return true;
		}


		

		
		#endregion

		#region CleanInput  - Limpia todos los caracteres No-alfanumericos
		public static string CleanInput(string strIn)
		{
			//Reemplaza Caracteres no Alfa Por Blancos. 
			Regex _Patron = new Regex("[^A-Za-z0-9]") ;
			return _Patron.Replace(strIn,"");
			
			//return Regex.Replace(strIn,"[^A-Za-z0-9]","");	
		}
		#endregion

		#region Convertir a Double - Respetando separador de decimales
		public static double ConvertToDouble ( string Valor )
		{
			string Separador = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ;
			return double.Parse( Valor.Replace(",", Separador ).Replace(".",Separador ) );
		}
		#endregion

        #region Diferencia entre 2 (dos) fechas
		public static string DateDiff( string FechaDesde, string FechaHasta )
		{

			try
			{
				TimeSpan Dif =  DateTime.Parse( FechaHasta ).Subtract( DateTime.Parse( FechaDesde ) );
				return Dif.Days.ToString();
			}
			catch ( Exception )
			{
				return "";
			}

		}
		
		
		#endregion

		#region Valida Fecha

        public static bool EsFecha(string Valor)
        {
            DateTime unFechavalida;
            bool esValido = false;

            if (DateTime.TryParse(Valor, out unFechavalida))
            {
                String Patron = "^\\d{2}/\\d{2}/\\d{4}$";
                Regex ExpresionRegular = new Regex(Patron);
                esValido = ExpresionRegular.IsMatch(Valor);
            }

            return esValido;

            //return DateTime.TryParse(Valor, out unFechavalida);

        }

		public static bool esFechaValida( string Fecha )
		{
			if(Fecha.Length != 10)
				return false;

			string dia = Fecha.Trim().Substring(0,2); 
			string mes = Fecha.Trim().Substring(3,2);
			string ano = Fecha.Trim().Substring(6,4);
 
			 if(( dia.Length == 0) || (dia.Length != 2) || (mes.Length == 0) || (mes.Length !=  2) || (ano.Length == 0) || (ano.Length != 4)) 
                 return false;
			 else if ((!esNumerico(dia)) || (!esNumerico(mes)) || (!esNumerico(ano)))  
				return  false;
    
			else if((int.Parse(mes) > 12) || (int.Parse(mes) < 1)) 
				return false;
    
			else if(int.Parse(dia) < 1)
				return false;

			else if ((int.Parse(mes) == 1) || (int.Parse(mes) == 3) || (int.Parse(mes) == 5) || (int.Parse(mes) == 7) || (int.Parse(mes) ==      8) || (int.Parse(mes) == 10) || (int.Parse(mes) == 12)) 
			{
				if(int.Parse(dia) > 31)
					return false;
			}
			else if ((int.Parse(mes) == 4) || (int.Parse(mes) == 6) || (int.Parse(mes) == 9) || (int.Parse(mes) == 11)) 
			{
				if(int.Parse(dia) > 30)
					return false;
			}
			else
			{
				int anio = int.Parse(ano);
				if(((anio % 4) == 0) && ( ((anio % 100) != 0) || (anio % 400) == 0))
				{
					if(int.Parse(dia) > 29)
						return false;
					else if(int.Parse(dia) > 28)
						return false;
				}
			}
			return true;
    	}
		
		
		#endregion Valida Fecha

		#region Valida Ingreso de Numeros

		public static bool esNumerico ( string Valor )
		{
			bool ValidoDatos=false;

			Regex numeros = new Regex(@"^\d{1,}$");

			if ( Valor.Length != 0 )
			{
				ValidoDatos =numeros.IsMatch( Valor ) ;
			}
			return ValidoDatos;
		}

		#endregion

		#region Es Numerio con decimales
		/// <summary>
		/// Controla si es un numero válido con o sin Decimales
		/// </summary>
		/// <param name="Valor"></param>
		/// <returns></returns>
		public static bool EsNumerioConDecimales ( string Valor )
		{
			bool ValidoDatos=false;

			Regex numeros = new Regex(@"^\d{1,}\.\d{2}$|^\d{1,}$");

			if ( Valor.Length != 0 )
			{
				ValidoDatos =numeros.IsMatch( Valor ) ;
			}
			return ValidoDatos;
//			bool ValidoDatos=false;
//
//			string Separador = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator ;
//
//			
//		
//			Regex numeros = new Regex(@"^\d{1,}\" + Separador + @"\d{2}$|^\d{1,}$");
//
//			if ( Valor.Length != 0 )
//			{
//				ValidoDatos =numeros.IsMatch( Valor.Replace(",", Separador ).Replace(".",Separador ) ) ;
//			}
//			return ValidoDatos;
		}

		#endregion
		
		#region ValidaMail
		/// <summary>
		/// Controla si es un numero válido con o sin Decimales
		/// </summary>
		/// <param name="Valor"></param>
		/// <returns></returns>
		public static bool ValidaMail ( string Valor )
		{
			bool ValidoDatos=false;

			Regex numeros = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})");

			if ( Valor.Length != 0 )
			{
				ValidoDatos =numeros.IsMatch( Valor ) ;
			}
			return ValidoDatos;
		}
		

		#endregion

		#region ValidoCUIT - Validacion de CUIL

		public static bool ValidoCUIT (string CUIT)
		{
			string FACTORES= "54327654321";
			double dblSuma=0;
			bool resul =false;

			if (!(CUIT.Substring(0,1).ToString() != "3" && CUIT.Substring(0,1).ToString() != "2"))
			{
				for(int i = 0 ;i<11 ; i++)
					dblSuma = dblSuma + int.Parse(CUIT.Substring(i,1).ToString()) * int.Parse(FACTORES.Substring(i,1).ToString());
			}
			resul = Modulo(dblSuma)==0;
			return resul;

		}

		#region Modulo - Modulo 11 de un numero
		private static double Modulo (double numero)
		{
			double resul = Math.IEEERemainder(numero,11);
			return resul;
		}
		#endregion

		#endregion

		#region Acceso Valido
		public static bool AccesoValido()
		{
			bool bAcesso = false, bHabilitarSeguridad = false;
			string sDominio = "", userControl, userUCA, userUCA_Consulta;

			System.Web.HttpContext _Context = System.Web.HttpContext.Current;
			WindowsPrincipal UsuarioAutenticado = System.Web.HttpContext.Current.User as WindowsPrincipal;
			
			bHabilitarSeguridad = bool.Parse(ConfigurationManager.AppSettings["ActivarSeg"].ToString());

            sDominio = ConfigurationManager.AppSettings["Dominio"].ToString();
            userControl = ConfigurationManager.AppSettings["GrupoControl"].ToString();
            userUCA = ConfigurationManager.AppSettings["GrupoUCA"].ToString();
            userUCA_Consulta = ConfigurationManager.AppSettings["GrupoUCA_Consultas"].ToString();


			if (bHabilitarSeguridad)
			{
                if (UsuarioAutenticado.IsInRole(sDominio + @"\" + userControl))
                {
                    _Context.Session["NIVEL"] = userControl;
                }
                else if (UsuarioAutenticado.IsInRole(sDominio + @"\" + userUCA))
                {
                    _Context.Session["NIVEL"] = userUCA;
                }
                else if (UsuarioAutenticado.IsInRole(sDominio + @"\" + userUCA_Consulta))
                {
                    _Context.Session["NIVEL"] = userUCA_Consulta;
                }
                else
                {
                    return bAcesso;
                }

				string Formulario =_Context.Request.RawUrl.Split(char.Parse("/"))[2].ToUpper();
		
				if( 
					(_Context.Session["NIVEL"].ToString().ToUpper() == userUCA.ToUpper())
					&& (Formulario == "INDEX.ASPX" ||
					Formulario == "BLOQUEOINHIBICIONES.ASPX?OPCION=BLOQUEO" ||
					Formulario == "BLOQUEOINHIBICIONES.ASPX?OPCION=INHIBICION" ||
					//Formulario == "BUSQUEDAPRESTADOR.ASPX" ||
					Formulario == "CONCEPTOSLIQDETALLE.ASPX" ||
					Formulario == "CONTACDIRCAM.ASPX" ||
					Formulario == "CONVENIOS_VAMR.ASPX" ||
					Formulario == "NOVEDADESCANCELADAS.ASPX" ||
					Formulario == "NOVEDADESINGRESADAS.ASPX" ||
					Formulario == "NOVEDADESLIQUIDADAS.ASPX" ||
					Formulario == "NOVEDADESNOAPLICADAS.ASPX" ||
					Formulario == "NOVEDADESXBENEFICIO.ASPX" ||
					Formulario == "NOVEDADES_ALL_X_BENEF.ASPX" ||
					Formulario == "NOVEDADES_CANCELADASXBENEFICIO.ASPX" ||
					Formulario == "PRESTADORAM.ASPX" ||
					Formulario == "CONSULTA_BENEFICIO.ASPX" ||
					Formulario == "BAJA_NOVEDADES_GRAL.ASPX" ||
					Formulario == "CONSULTA_DE_BAJAS.ASPX?OPCION=BAJAGRAL" ||
					Formulario == "TELEFONOS_UTILES.ASPX")
					)
					bAcesso = true;
				else if( 
					(_Context.Session["NIVEL"].ToString().ToUpper() == userControl.ToUpper()) 
					&& (Formulario == "INDEX.ASPX" ||
					Formulario == "CONSULTA_DE_BAJAS.ASPX?OPCION=BAJACONTROL" 
					|| Formulario == "NOVEDADES_BAJA_CONTROL.ASPX") 
					)
					bAcesso = true;
				else if( 
					(_Context.Session["NIVEL"].ToString().ToUpper() == userUCA_Consulta.ToUpper()) 
					&& (Formulario == "INDEX.ASPX" ||
					//Formulario == "BUSQUEDAPRESTADOR.ASPX" ||
					Formulario == "CONCEPTOSLIQDETALLE.ASPX" ||
					Formulario == "NOVEDADES_CANCELADASXBENEFICIO.ASPX" ||
					Formulario == "NOVEDADESCANCELADAS.ASPX" ||
					Formulario == "NOVEDADESINGRESADAS.ASPX" ||
					Formulario == "NOVEDADESLIQUIDADAS.ASPX" ||
					Formulario == "NOVEDADESNOAPLICADAS.ASPX" ||
					Formulario == "NOVEDADESXBENEFICIO.ASPX" ||
					Formulario == "NOVEDADES_ALL_X_BENEF.ASPX" ||
					Formulario == "CONSULTA_BENEFICIO.ASPX" ||
					Formulario == "CONSULTA_DE_BAJAS.ASPX?OPCION=BAJAGRAL" ||
					Formulario == "NOVEDADES_CANCELADASXBENEFICIO.ASPX" ||
					Formulario == "TELEFONOS_UTILES.ASPX")
					)
					bAcesso = true;
				
				return bAcesso;
			}
			else
				return true;
			
		}


		#endregion Acceso Valido       

        #region ToPascal()
        public static string ToPascal(string pValue)
        {
            string wRet = string.Empty;
            if (pValue == string.Empty)
            {
                wRet = string.Empty;
            }
            else
            {
                try
                {
                    string[] aPalabras = Regex.Split(pValue, " ");
                    for (int i = 0; i < aPalabras.Length; i++)
                    {
                        wRet += " " + aPalabras[i].Substring(0, 1).ToUpper() + aPalabras[i].Substring(1, aPalabras[i].Length - 1).ToLower();
                    }
                    wRet = wRet.Trim();
                }
                catch
                {
                    wRet = string.Empty;
                }
            }
            return wRet;
        }

        public static string ToPascal(object objeto)
        {
            string wRet = string.Empty;
            string valor = objeto.ToString();
            if (valor == string.Empty)
            {
                wRet = string.Empty;
            }
            else
            {
                try
                {
                    string[] aPalabras = Regex.Split(valor, " ");
                    for (int i = 0; i < aPalabras.Length; i++)
                    {
                        wRet += " " + aPalabras[i].Substring(0, 1).ToUpper() + aPalabras[i].Substring(1, aPalabras[i].Length - 1).ToLower();
                    }
                    wRet = wRet.Trim();
                }
                catch
                {
                    wRet = string.Empty;
                }
            }
            return wRet;
        }
        #endregion

		public static void LLenarRadioButtonList(Object Radio, string ObjTabla,Object[] aParam, DataSet _datos)
		{
			
			RadioButtonList _Radio = (RadioButtonList) Radio;
  
			_Radio.Items.Clear();				//Elimina todos los elementos de Combo.
			
			string filtro = string.Empty;
			switch (ObjTabla.ToString().ToUpper())
			{
				case "TIPOCONCEPTO":
					/* Si el parametro es:
									 *	 0 trae todo
									 *	 1 todo menos lo indeterminado
									 *	 2 solo indeterminado					 * 
									 * */
					
				switch (byte.Parse(aParam[0].ToString()))
				{
					case 1:
						filtro = "TipoConcepto not in (1,6)" ;
						break;
					case 2:
						filtro = "TipoConcepto in (1,6)" ;
						break;
					default:
						break;
				}
				
					_datos.Tables[0].DefaultView.RowFilter = filtro;
					if(_datos.Tables[0].Rows.Count > 0)
					{
						_Radio.DataSource=_datos.Tables[0].DefaultView; 
						_Radio.DataTextField="DescTipoConcepto";
						_Radio.DataValueField="TipoConcepto";
					}
					break;
			}
		}

	}
}