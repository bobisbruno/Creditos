using System;
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
[Guid("BA782009-3893-4c26-9770-1AC3AB2236F9")] 	
	public interface DAT_ITipoConcConcLiq
{
	DataSet Traer (long Prestador);
	DataSet TraerTipoServicio (int CodConceptoLiq, short TipoConcepto);
}
	[Guid("50C70A69-74C3-4271-84A3-46B179579E98")] 
	[ ProgId( "DAT_TipoConcConcLiq" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[EventTrackingEnabled ( true )]
	[Transaction(TransactionOption.Disabled)]
	[ComponentAccessControl(true)]
	[SecureMethod]
	public class TipoConcConcLiq : ServicedComponent, DAT_ITipoConcConcLiq
	{
		
		public TipoConcConcLiq()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[SecurityRole("OperadorEntidad")]
		public DataSet Traer (long Prestador)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[1];
	
			try
			{
						
				objPar[0] = new SqlParameter("@Prestador",SqlDbType.BigInt); 
				objPar[0].Value = Prestador;
					
	
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "TipoConc_ConcLiq_TxPrestador", objPar );

				
				return ds;
			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}

		[SecurityRole("OperadorEntidad")]
		public DataSet TraerTipoServicio (int CodConceptoLiq, short TipoConcepto)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[2];
	
			try
			{
				
				objPar[0] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int); 
				objPar[0].Value = CodConceptoLiq;
				objPar[1] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt); 
				objPar[1].Value = TipoConcepto;
					
	
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "RelacionCodLiqXTipoConcXPrestITem_Trae", objPar );

				
				return ds;
			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
	}
}