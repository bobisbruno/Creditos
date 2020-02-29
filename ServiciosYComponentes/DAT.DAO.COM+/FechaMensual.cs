using System;

namespace ANSES.Microinformatica.DATComPlus
{
	/// <summary>
	/// EL MENSUAL SE GUARDA COMO 2 VARIABLES NUMERICAS POR SEPARADO, AÑO Y MES
	/// </summary>
	public class FechaMensual
	{
		private int anio;
		private int mes;
		public FechaMensual()
		{
			anio=0;
			mes=0;
		}
		public FechaMensual(int mensual)
		{
			anio=mensual / 100;
			mes=mensual % 100;
			if (!this.esFechaValida())
			{
				anio=0;
				mes=0;
			}									 
		}		
		public void setMensual(int mensual)
		{
			anio=mensual / 10;
			mes=mensual % 10;
			if (!this.esFechaValida())
			{
				anio=0;
				mes=0;
			}
		}
		public int getMensual()
		{
			return ((anio*10)+mes);
		}
		public int getAnio()
		{
			return anio;
		}
		public int getMes()
		{
			return mes;
		}
		public bool esFechaValida()
		{
			 return (!((this.getMes()<=0) || (this.getMes()>=13)));			
		}
		public bool esMenorQue(FechaMensual fecha)
		{
			if ((anio<fecha.getAnio()) || ((anio==fecha.getAnio()) && (mes<fecha.getMes())))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool esMensualNulo()
		{
			return ((this.getAnio()==0)&&(this.getMes()==0));
		}
		public int cantidadDeMesesCon(FechaMensual fecha)
		{
			int acum;
			int anioAux;

			if (anio==fecha.getAnio())	
			{
				return (fecha.getMes()-mes+1);
			}
			else
			{
				anioAux= anio+1;
				acum=12-mes+1;
				while (anioAux<=fecha.getAnio())
				{
					if (fecha.getAnio()==anioAux)
					{
						acum=acum+fecha.getMes();
					}
					else
					{
						acum=acum+12;
					}
					anioAux++;
				}
				return acum;
			}
		}
	}
}
