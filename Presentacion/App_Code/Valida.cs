using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Configuration;

public class Valida
{
		public Valida()
		{

		}

		#region Validaciones
	
		/*public static string valida_entrada (byte opcion , object[] valor_campo, byte cant_obj_campo)
		{
			string mensaje = String.Empty;
			string DifFecha = String.Empty;

			System.Web.HttpContext _Context = System.Web.HttpContext.Current;
			DataSet _ds = new DataSet();
						
			string Mensual;
			string Desde;
			string Hasta;

			_ds= (DataSet) _Context.Session["Prestador"];
			Mensual= _ds.tables["Cierres"].Rows[0]["Mensual"].ToString();

			switch (opcion)
			{
				case 1:
					//numerico
					if (esNumerico( (TextBox) valor_campo[0])== false)
					{
						mensaje ="Dato No Numérico";									
					}
					break;
				case 2:
					//Cuil
					break;
				case 3:
					//Seleccionado
					int i = 0;

					while (i < cant_obj_campo && mensaje =="" ) 
					{
						if ( int.Parse( valor_campo[i].ToString() ) <= 0 )
						{
							mensaje = "Faltan seleccionar datos necesarios";
						}
						else
						{ 
							i++;
						}
					}
					
					int PosFechaDesde = cant_obj_campo == 2 ? 2 : 1;
					int PosFechaHasta	= cant_obj_campo == 2 ? 3 : 2;

					Desde= DateTime.Parse(valor_campo[ PosFechaDesde ].ToString()).Year.ToString() + DateTime.Parse(valor_campo[ PosFechaDesde ].ToString()).Month.ToString("00");
					Hasta= DateTime.Parse(valor_campo[ PosFechaHasta ].ToString()).Year.ToString() + DateTime.Parse(valor_campo[ PosFechaHasta ].ToString()).Month.ToString("00");
						

					if (Mensual== Desde && Mensual==Hasta  )
					{
						DifFecha = Util.DateDiff( valor_campo[ PosFechaDesde ].ToString() , valor_campo[ PosFechaHasta ].ToString() ) ;			//Calculo la diferencia entre fechas
					
						if ( DifFecha == String.Empty || long.Parse( DifFecha ) > 7 )
						{
							mensaje += String.Format(@"\n El rango de fechas ingresado es incorrecto. Solo es posible consultar con un máximo de {0} dias",7);
						}
					}
					else
					{
						mensaje += String.Format(@"\n Solo es posible consultar un período de fechas comprendidas en el mensual.",7);
					}

					break;
				
				case 4:					//Para Novedades Liquidadas
					
					 i = 0;
										
					while (i < cant_obj_campo && mensaje =="" ) 
					{
						if ( int.Parse( valor_campo[i].ToString() ) <= 0 )
						{
							mensaje = "Faltan seleccionar datos necesarios";
						}
						else
						{ 
							i++;
						}
					}
									
					break;

				
				case 5:
					
					i = 0;

					while (i < cant_obj_campo && mensaje =="" ) 
					{
						if ( valor_campo[i].ToString().Length == 0 )
						{
							mensaje = "Faltan seleccionar datos necesarios";
						}
						else
						{ 
							i++;
						}
					}
					
					string sPosFechaDesde = valor_campo[0].ToString() ;
					string sPosFechaHasta = valor_campo[1].ToString() ;

					Desde= DateTime.Parse(sPosFechaDesde).Year.ToString() + DateTime.Parse(sPosFechaDesde).Month.ToString("00");
					Hasta= DateTime.Parse(sPosFechaHasta).Year.ToString() + DateTime.Parse(sPosFechaHasta).Month.ToString("00");

					if (Mensual== Desde && Mensual==Hasta  )
					{
						DifFecha = Util.DateDiff( sPosFechaDesde  ,  sPosFechaHasta  ) ;			//Calculo la diferencia entre fechas
					
						if ( DifFecha == String.Empty || long.Parse( DifFecha ) > 7 )			// El 7 deberia tomarse del WebConfig.
						{
							mensaje += String.Format(@"\n El rango de fechas ingresado es incorrecto. Solo es posible consultar con un máximo de {0} dias",7);
						}
					}
					else
					{
						mensaje += String.Format(@"\n Solo es posible consultar un período de fechas comprendidas en el mensual.",7);
					}
					
					break;

				default:			
					break;
				
			}

			return mensaje;
		}*/

		public static bool esNumerico ( TextBox Campo )
		{
			bool ValidoDatos=false;

			Regex numeros = new Regex(@"\d");

			if (Campo.Text.Length!=0)
			{
				ValidoDatos =numeros.IsMatch( Campo.Text ) ;
			}
			return ValidoDatos;
		}
		#endregion
	}

