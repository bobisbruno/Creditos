using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Data.Common;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class EntidadPrivada
    {

        public EntidadPrivada()
        {

        }

        public static bool VerDisponible(long idPrestador, long idBeneficiario, double monto, int mensualDesde, int mensualHasta)
        {
            string resultado;
            int cuotas;
            double importe;
            FechaMensual desde = new FechaMensual(mensualDesde);
            FechaMensual hasta = new FechaMensual(mensualHasta);
            //Creo un objeto tipo novedad para reutilizar el codigo ya existente de verificacion de disponibilidad
            NovedadDAO oNov = new NovedadDAO();

            try
            {
                //Valido los datos ingresados
                ValidoDatos(desde, hasta);

                //Saco la cantidad de cuotas por la diferencias de los mensuales, en meses
                cuotas = desde.cantidadDeMesesCon(hasta);
                importe = monto / cuotas;

                //El metodo de verificacion de disponibilidad retorna un string informando el resultado
                resultado = NovedadDAO.CtrolAlcanza(idBeneficiario, importe, idPrestador, 0);

                if (resultado == String.Empty)
                    return true;
                else                
                    return false;                
            }
            catch (NoValida e)
            {
                //RETORNO MENSAJE DE ERROR
                throw e;
            }
        }

        public static String[] AltaDeNovedad(long idPrestador, long idBeneficiario, double monto, 
                                             int mensualDesde, int mensualHasta, string IP, string usuario)
        {
            //string sql = "Novedades_A_EntidadesPrivadas";
            //Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            //DbCommand dbCommand = db.GetStoredProcCommand(sql);
            ////List<Novedad> lstNovedades = new List<Novedad>();
            ////Novedad unaNovedad;
            //DbParameterCollection dbParametros = null;
            
            //FechaMensual desde = new FechaMensual(mensualDesde);
            //FechaMensual hasta = new FechaMensual(mensualHasta);

            //// ME FIJO EL ULTIMO MENSUAL DE INHIBICION CARGADO, CASO DE Q NO TENGA NINGUNO RETORNA 0
            //FechaMensual ultimoHasta = new FechaMensual(UltimoMensualDeInhibicion(idPrestador, idBeneficiario));
            //Settings objCnn = new Settings();
            String[] retorno = new String[2];
            ////string MAC;

            //try
            //{
            //    //VALIDO LOS DATOS INGRESADOS
            //    ValidoDatos(desde, hasta);
            //    if (this.VerDisponible(idPrestador, idBeneficiario, monto, mensualDesde, mensualHasta))
            //    {
            //        //genero la mac ****FALTA*****
            //        objCon = objCnn.Conectar();
            //        objPar[0] = new SqlParameter("@IdPrestador", SqlDbType.BigInt);
            //        objPar[0].Value = idPrestador;
            //        objPar[1] = new SqlParameter("@IdBeneficiario", SqlDbType.BigInt);
            //        objPar[1].Value = idBeneficiario;
            //        objPar[2] = new SqlParameter("@monto", SqlDbType.Decimal);
            //        objPar[2].Value = monto;
            //        objPar[3] = new SqlParameter("@desde", SqlDbType.BigInt);
            //        objPar[3].Value = mensualDesde;
            //        objPar[4] = new SqlParameter("@hasta", SqlDbType.BigInt);
            //        objPar[4].Value = mensualHasta;
            //        objPar[5] = new SqlParameter("@IP", SqlDbType.VarChar, 20);
            //        objPar[5].Value = IP;
            //        objPar[6] = new SqlParameter("@Usuario", SqlDbType.VarChar, 50);
            //        objPar[6].Value = usuario;
            //        objPar[7] = new SqlParameter("@MAC", SqlDbType.VarChar, 100);
            //        objPar[7].Value = "";
            //        objPar[8] = new SqlParameter("@IdNovedad", SqlDbType.BigInt);
            //        objPar[8].Direction = ParameterDirection.Output;
            //        objPar[8].Value = 0;

            //        //DOY DE ALTA EL REGISTRO EN LA BASE DE DATOS
            //        SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "Novedades_A_EntidadesPrivadas", objPar);

            //        //LLAMO AL COMTI PARA LA TRANSACCION CICS SOLO SI EL MENSUAL HASTA ES MAYOR AL ULTIMO MENSUAL CARGADO
            //        if (ultimoHasta.esMenorQue(hasta))
            //        {
            //            //DOY ALTA EN EL COMTI CON EL MENSUAL Q ME VINO POR PARAMETRO

            //        }

            //        //RETORNO EL NUMERO DE TRANSACCION Y LA MAC
            //        retorno[0] = (string)objPar[8].Value.ToString();
            //        retorno[1] = "";
                    return retorno;
            //    }
            //    else
            //    {
            //        throw new NoValida("Afectacion disponible insuficiente");
            //    }

            //}
            //catch (NoValida e)
            //{
            //    //RETORNO MENSAJE DE ERROR
            //    throw e;
            //}
            //finally
            //{
            //    //objCon.Dispose();
            //    //objCnn = null;
            //}
        }

        public static String[] ModificarNovedad(long idPrestador, long nroDeTransaccion, long idBeneficiario, double monto, int mensualDesde, int mensualHasta, string IP, string usuario)
        {
            // La modificacion consiste en dar de baja la transaccion actual en la base de datos y dar un alta con los nuevos datos ingresados
            String[] retorno = new String[2];

            try
            {
                CancelarNovedad(idPrestador, nroDeTransaccion, idBeneficiario, mensualHasta);
                return AltaDeNovedad(idPrestador, idBeneficiario, monto, mensualDesde, mensualHasta, IP, usuario);
            }
            catch (NoValida e)
            {
                //Retorno mensaje de error
                throw e;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public static void CancelarNovedad(long idPrestador, long nroDeTransaccion, long idBeneficiario, int mensualHasta)
        {
            //Settings objCnn = new Settings();
            //SqlConnection objCon = new SqlConnection();
            //SqlParameter[] objPar = new SqlParameter[1];
            //FechaMensual ultimoMensual;
            //FechaMensual hasta = new FechaMensual(mensualHasta);

            //try
            //{
            //    objCon = objCnn.Conectar();
            //    objPar[0] = new SqlParameter("@transaccion", SqlDbType.BigInt);
            //    objPar[0].Value = nroDeTransaccion;

            //    //Doy de baja el registro en la base de datos
            //    SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "Novedades_BAJA_EntidadesPrivadas", objPar);
            //    //Averiguo el ultimo mensual activo cargado en la base, si no hay retorno 0
            //    ultimoMensual = new FechaMensual(this.UltimoMensualDeInhibicion(idPrestador, idBeneficiario));
            //    //Llamo al comti para la transaccion cics solo si hay otra novedad con mensual hasta menor 
            //    if (ultimoMensual.esMenorQue(hasta))
            //    {
            //        //Llamo al comti para der de baja el actual 
            //        //Me fijo si hay un mensual anterior activo cargado en la base
            //        if (!ultimoMensual.esMensualNulo())
            //        {
            //            //Llamo al comti para dar el alta con el mensual ultimomensual, ultimo mensual activo cargado en la base de datos
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    objCon.Dispose();
            //    objCnn = null;

            //}
        }

        public static DataSet BuscoNovedadesDe(long idPrestador, long idBeneficiario)
        {
            Settings objCnn = new Settings();
            SqlConnection objCon = new SqlConnection();
            SqlParameter[] objPar = new SqlParameter[2];
            DataSet novedades = new DataSet();

            //try
            //{
            //    objCon = objCnn.Conectar();
            //    objPar[0] = new SqlParameter("@idPrestador", SqlDbType.BigInt);
            //    objPar[0].Value = idPrestador;
            //    objPar[1] = new SqlParameter("@idBeneficiario", SqlDbType.BigInt);
            //    objPar[1].Value = idBeneficiario;

            //    //Retorno un dataset conteniendo todas las novedades del beneficiario para esta entidad privada
            //    novedades = SqlHelper.ExecuteDataset(objCnn.ConectarString(), CommandType.StoredProcedure, "BuscoNovedad_EntidadesPrivadas", objPar);
                return novedades;
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    objCon.Dispose();
            //    objCnn = null;

            //}
        }
       
        public static DataSet BuscoNovedadesPorFechas(long idPrestador, int mensual)
        {
            Settings objCnn = new Settings();
            SqlConnection objCon = new SqlConnection();
            SqlParameter[] objPar = new SqlParameter[2];
            DataSet novedades = new DataSet();
            FechaMensual fecha = new FechaMensual(mensual);

            //try
            //{
            //    if (fecha.esMensualNulo())
            //    {
            //        throw new NoValida("Mensual incorrecto");
            //    }
            //    objCon = objCnn.Conectar();
            //    objPar[0] = new SqlParameter("@idPrestador", SqlDbType.BigInt);
            //    objPar[0].Value = idPrestador;
            //    objPar[1] = new SqlParameter("@mensual", SqlDbType.Int);
            //    objPar[1].Value = mensual;

            //    //Retorno un dataset conteniendo todas las novedades activas en el mensual del parametro
            //    novedades = SqlHelper.ExecuteDataset(objCnn.ConectarString(), CommandType.StoredProcedure, "BuscoNovedadPorFechas_EntidadesPrivadas", objPar);
                return novedades;
            //}
            //catch (NoValida e)
            //{
            //    //RETORNO MENSAJE DE ERROR
            //    throw e;
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    objCon.Dispose();
            //    objCnn = null;

            //}
        }
   
        public static DataSet BuscoNovedadesPorTipo(long idPrestador, int tipo)
        {
            Settings objCnn = new Settings();
            SqlConnection objCon = new SqlConnection();
            SqlParameter[] objPar = new SqlParameter[1];
            DataSet novedades = new DataSet();

            //try
            //{
            //    objCon = objCnn.Conectar();
            //    objPar[0] = new SqlParameter("@idPrestador", SqlDbType.BigInt);
            //    objPar[0].Value = idPrestador;

            //    switch (tipo)
            //    {
            //        case 0:
            //            //Retorno un dataset conteniendo todas las novedades activas del prestador
            //            novedades = SqlHelper.ExecuteDataset(objCnn.ConectarString(), CommandType.StoredProcedure, "BuscoNovedadPorTipoActivas_EntidadesPrivadas", objPar);
            //            break;
            //        case 1:
            //            //Retorno un dataset conteniendo todas las novedades finalizadas del prestador
            //            novedades = SqlHelper.ExecuteDataset(objCnn.ConectarString(), CommandType.StoredProcedure, "BuscoNovedadPorTipoFinalizadas_EntidadesPrivadas", objPar);
            //            break;
            //        case 2:
            //            //Retorno un dataset conteniendo todas las novedades canceladas del prestador
            //            novedades = SqlHelper.ExecuteDataset(objCnn.ConectarString(), CommandType.StoredProcedure, "BuscoNovedadPorTipoCanceladas_EntidadesPrivadas", objPar);
            //            break;
            //        default:
            //            //LANZO UNA EXCEPCION
            //            throw new Exception("Tipo erroneo");

            //    }
                return novedades;
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    objCon.Dispose();
            //    objCnn = null;

            //}
        }
 
        private static int UltimoMensualDeInhibicion(long idPrestador, long idBeneficiario)
        {
            Settings objCnn = new Settings();
            SqlConnection objCon = new SqlConnection();
            SqlParameter[] objPar = new SqlParameter[3];

            //try
            //{
            //    objCon = objCnn.Conectar();
            //    objPar[0] = new SqlParameter("@idBeneficiario", SqlDbType.BigInt);
            //    objPar[0].Value = idBeneficiario;
            //    objPar[1] = new SqlParameter("@idPrestador", SqlDbType.BigInt);
            //    objPar[1].Value = idPrestador;
            //    objPar[2] = new SqlParameter("@ultimoMensual", SqlDbType.BigInt);
            //    objPar[2].Direction = ParameterDirection.Output;
            //    objPar[2].Value = 0;

            //    // Devuelvo el ultimo mensual activo, cargado en la base de datos para el beneficiario. si no tiene ninguno devuelve el valor 0
            //    SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "UltimoMensual_EntidadesPrivadas", objPar);

                return int.Parse(objPar[2].Value.ToString());

            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    objCon.Dispose();
            //    objCnn = null;

            //}
        }
   
        private static void ValidoDatos(FechaMensual desde, FechaMensual hasta)
        {
            //Me fijo si la fecha desde es menos q la fecha hasta y las fechas tengas meses validos
            if (desde.esMensualNulo())
                throw new NoValida("Fecha desde invalida");
            if (hasta.esMensualNulo())
                throw new NoValida("Fecha hasta invalida");
            if ((!desde.esMenorQue(hasta)))
                throw new NoValida("Rango de mensuales incorrecto");
        }
    }
}

