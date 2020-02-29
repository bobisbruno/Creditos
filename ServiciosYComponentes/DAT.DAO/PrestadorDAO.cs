using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Text.RegularExpressions;
using System.EnterpriseServices;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class PrestadorDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PrestadorDAO).Name); 

        #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~PrestadorDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Componentes Transaccionales

        #region Alta y Modificacion de Prestador

        public string PrestadorAltaMod(string idPrestador, string razonSocial, string cuit, string codSis,
                                       string email, string codofanme, string observacion,
                                       bool habilitado, bool prestadorModificado,
                                       List<ConceptoLiquidacion> listConceptoLiquidacion,
                                       List<TipoConcepto> listTipoConcepto, string usuario)
        {
            string MensajeRetorno = "";
            //string sql = "";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = null;

            //DataTable tblConceptosLiq = new DataTable();
            //DataTable tblTiposConceptos = new DataTable();
            //tblConceptosLiq = dsDatosConcepto.Tables[0];
            //tblTiposConceptos = dsDatosConcepto.Tables[1];

            try
            {
                MensajeRetorno = ValidarDatosPrestador(idPrestador, razonSocial, cuit, codSis, email,
                                                       codofanme, observacion, listConceptoLiquidacion, listTipoConcepto);

                if (idPrestador.ToString() == "0")
                    MensajeRetorno += ValidarCruceCUITCodSistema(cuit, codSis);
                MensajeRetorno += validaExistenciaConceptosAM(listConceptoLiquidacion, listTipoConcepto);

                if (MensajeRetorno.Length == 0)
                {
                    #region Carga de Parametros

                    if (email.Length == 0)
                        email = null;
                    if (codofanme.Length == 0)
                        codofanme = null;
                    if (observacion.Length == 0)
                        observacion = null;
                    //if (habilitado)
                    //   objPar[6].Value = 1;

                    db.AddOutParameter(dbCommand, "@IdPrestador", DbType.Int64, 8);
                    db.AddInParameter(dbCommand, "@RazonSocial", DbType.String, razonSocial);
                    db.AddInParameter(dbCommand, "@cuit", DbType.String, cuit);
                    db.AddInParameter(dbCommand, "@email", DbType.String, email);
                    db.AddInParameter(dbCommand, "@codofanme", DbType.Int32, codofanme);
                    db.AddInParameter(dbCommand, "@observaciones", DbType.String, observacion);
                    //TODO: Ver parametro idEstado
                    db.AddInParameter(dbCommand, "@idestado", DbType.Int16, 0);//idEstado);
                    db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);

                    #endregion Carga de Parametros

                    if (idPrestador.ToString() == "0")
                    {
                        #region Alta del Prestador

                        dbCommand = db.GetStoredProcCommand("Admin_Prestador_Alta");
                        db.ExecuteNonQuery(dbCommand);

                        idPrestador = db.GetParameterValue(dbCommand, "@IdPrestador").ToString();

                        #endregion Alta Prestador
                    }
                    else if (prestadorModificado)
                    {
                        #region Modificacion del Prestador
                        //objPar[0].Value = long.Parse(idPrestador.ToString());

                        db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                        db.AddInParameter(dbCommand, "@RazonSocial", DbType.String, razonSocial);
                        db.AddInParameter(dbCommand, "@cuit", DbType.String, cuit);
                        db.AddInParameter(dbCommand, "@email", DbType.String, email);
                        db.AddInParameter(dbCommand, "@codofanme", DbType.Int32, codofanme);
                        db.AddInParameter(dbCommand, "@observaciones", DbType.String, observacion);
                        db.AddInParameter(dbCommand, "@idestado", DbType.Int16, 0); //idEstado);
                        db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);

                        dbCommand = db.GetStoredProcCommand("Admin_Prestador_Modificacion");
                        db.ExecuteNonQuery(dbCommand);

                        #endregion Modificacion del Prestador
                    }

                    #region Alta Conceptos

                    foreach (ConceptoLiquidacion unConceptoLiquidacion in listConceptoLiquidacion)
                    {
                        //TODO: Ver el filtro
                        //string Filtro = "CodConceptoLiq= '" + oTipoConcepto["CodConceptoLiq"].ToString() + "'";
                        //tblTiposConceptos.DefaultView.RowFilter = Filtro;
                        //tblTiposConceptos.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        //foreach (TipoConcepto oTipoConcepto in listTipoConcepto)
                        //{
                            #region Definicion de Parametros


                            #endregion Definicion de Parametros

                            db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                            db.AddInParameter(dbCommand, "@codConcepto", DbType.Int32, unConceptoLiquidacion.CodConceptoLiq);
                            db.AddInParameter(dbCommand, "@descripcionConcepto", DbType.String, unConceptoLiquidacion.DescConceptoLiq);
                            db.AddInParameter(dbCommand, "@prioridad", DbType.Int16, unConceptoLiquidacion.Prioridad);
                            db.AddInParameter(dbCommand, "@codSidif", DbType.Int32, unConceptoLiquidacion.CodSidif);
                            db.AddInParameter(dbCommand, "@obligatorio", DbType.Boolean, unConceptoLiquidacion.Obligatorio);
                            db.AddInParameter(dbCommand, "@esafil", DbType.Boolean, unConceptoLiquidacion.EsAfiliacion);
                            db.AddInParameter(dbCommand, "@codsistema", DbType.String, unConceptoLiquidacion.CodSistema);
                            db.AddInParameter(dbCommand, "@habonline", DbType.Boolean, unConceptoLiquidacion.HabilitadoOnLine);
                            db.AddInParameter(dbCommand, "@habilitadoConc", DbType.Boolean, unConceptoLiquidacion.Habilitado);
                            //TODO: Ver parametros faltantes - Agregar
                            //db.AddInParameter(dbCommand, "@concalta", DbType.Boolean, concalta);
                            //db.AddInParameter(dbCommand, "@maxdesc", DbType.Decimal, maxdesc);
                            //TODO: Agregar - el tipo concepto va a estar dentro del cod concepto
                            //db.AddInParameter(dbCommand, "@tipoconcepto", DbType.Int16, oTipoConcepto.IdTipoConcepto);
                            //db.AddInParameter(dbCommand, "@habilitadoRel", DbType.Boolean, habilitadoRel);
                            db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);
                            //db.AddInParameter(dbCommand, "@nuevotope", DbType.Boolean, nuevotope);

                            #region Carga de Parametros

                            //if (oTipoConcepto["MaxADescontar"].ToString().Length == 0)
                            //    objPar2[11].Value = 0;
                            //else
                            //{
                            //TODO: Ver que hace aca

                            //System.Text.StringBuilder strTmp = new System.Text.StringBuilder(oTipoConcepto["MaxADescontar"].ToString());
                            //strTmp.Replace(',', '.');
                            //objPar2[11].Value = strTmp.ToString();
                            //strTmp = null;
                            //string MaxDesc = FilaTipo["MaxADescontar"].ToString();
                            //objPar2[11].Value = MaxDesc.ToString();
                            //}

                            //if (bool.Parse(oTipoConcepto["nuevo"].ToString()))
                            //{
                            //    objPar2[10].Value = 1;
                            //    objPar2[15].Value = 1;
                            //}
                            //else
                            //{
                            //    objPar2[10].Value = 0;
                            //    objPar2[15].Value = FilaTipo["modificaTope"].ToString();
                            //}

                            #endregion Carga de Parametros

                            dbCommand = db.GetStoredProcCommand("Admin_Alta_Conceptos_Prestador");
                            db.ExecuteNonQuery(dbCommand);
                        //}
                    }
                    #endregion Alta Conceptos

                }
                else
                    return MensajeRetorno;

            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return MensajeRetorno;
        }

        #region Validaciones

        #region Valida todos los Parametros

        private string ValidarDatosPrestador(string idPrestador, string razonsocial, string cuit,
                                             string codSis, string email, string codofanme, string observacion,
                                             List<ConceptoLiquidacion> listConceptoLiquidacion, List<TipoConcepto> listTipoConcepto)
        {
            //DataTable dtLiq = new DataTable();
            //DataTable dtTipoConc = new DataTable();
            string retMsg = "", errConceptoID;
            //dtLiq = dsDatosConcepto.Tables[0];
            //dtTipoConc = dsDatosConcepto.Tables[1];

            #region Valida Datos Prestador

            #region Valida IDPrestador
            if (idPrestador.ToString() != "0")
            {
                if (idPrestador.Length == 0)
                    retMsg += @"\n No existe parametro ID Prestador";
                else if (!Utilidades.esNumerico(idPrestador.ToString()))
                    retMsg += @"\n ID Prestador no es numerico";
                else if (long.Parse(idPrestador.ToString()) > 2147783647)
                    retMsg += @"\n ID Prestador exede su longitud permitida";
            }
            #endregion Valida IDPrestador

            #region Valida RZ
            if (razonsocial.Length == 0)
                retMsg += @"\n Debe ingresar Razon Social";
            else if (razonsocial.Length > 100)
                retMsg += @"\n Razon Social supera la Longitud permitida";
            #endregion Valida RZ

            #region Vailda CUIT
            if (cuit.Length == 0)
                retMsg += @"\n Debe ingresar CUIT";
            else if (cuit.Length != 11)
                retMsg += @"\n CUIT debe tener 11 Digitos";
            else if (!Utilidades.esNumerico(cuit))
                retMsg += @"\n CUIT no es Numerico";
            #endregion Vailda CUIT

            #region Valida Email
            if (email.Length > 0)
            {
                if (email.Length > 100)
                    retMsg += @"\n E-Mail supera Longitud permitida";
                else if (!Utilidades.ValidaMail(email))
                    retMsg += @"\n La dirección de e-mail no es correcta";
            }
            #endregion Valida Email

            #region Valida CodOfANME
            if (codofanme.ToString().Length > 0)
            {
                if (Utilidades.esNumerico(codofanme.ToString()))
                {
                    if (codofanme.ToString().Length > 8)
                        retMsg += @"\n Codigo de Oficina excede la Longitud permitida";
                }
                else
                    retMsg += @"\n Codigo de Oficina no es numerico";
            }

            #endregion Valida CodOfANME

            #region Valida Cod Sistema
            if (codSis.Length == 0)
                retMsg += @" \nSe requiere un Codígo de Sistema";
            else
            {
                Regex Letras = new Regex(@"^[a-zA-Z]{3}$");

                if (!Letras.IsMatch(codSis))
                {
                    retMsg += @" \nPara el Código de Sistema solo se adimiten 3 letras";
                }
            }
            #endregion Valida Cod Sistema

            #region Valida Observaciones
            if (observacion.Length > 0)
            {
                if (observacion.Length > 255)
                    retMsg += @"\n La Observacion supera Longitud permitida";
            }
            #endregion Valida Observaciones

            #endregion Valida Datos Prestador

            #region Valida Conceptos

            if ((listConceptoLiquidacion.Count == 0) || (listTipoConcepto.Count == 0))
            {
                retMsg += @" \nNo existen Descuentos a Ingresar";
                return retMsg;
            }

            foreach (ConceptoLiquidacion oConLiq in listConceptoLiquidacion)
            {
                errConceptoID = oConLiq.CodConceptoLiq.ToString();

                #region Valida Codigo de Concepto

                if (oConLiq.CodConceptoLiq.ToString().Length == 0)
                    retMsg += @" \nDebe ingresar Codigo de Descuento";
                else if (oConLiq.CodConceptoLiq.ToString().Length != 6)
                    retMsg += @" \nError en " + errConceptoID + " Codigo de Descuento debe ser de 6 Digitos";
                else if (!Utilidades.esNumerico(oConLiq.CodConceptoLiq.ToString()))
                    retMsg += @" \nError en " + errConceptoID + " Codigo de Descuento no es Numerico";
                else
                {
                    #region Valida Descripcion Concepto
                    if (oConLiq.DescConceptoLiq.ToString().Length == 0)
                        retMsg += @" \nError en " + errConceptoID + " Debe Ingresar Descripcion del Descuento";
                    else if (oConLiq.DescConceptoLiq.ToString().Length > 50)
                        retMsg += @" \nError en " + errConceptoID + " Descripcion del Descuento excede los 50 Caracteres";

                    #endregion Valida Descripcion Concepto
                }
                #endregion Valida Codigo de Concepto

                #region Valido Prioridad
                if (oConLiq.Prioridad.ToString().Length > 0)
                {
                    if (oConLiq.Prioridad.ToString().Length != 1)
                        retMsg += @" \nError en " + errConceptoID + " Prioridad erronea ";
                    else if (!Utilidades.esNumerico(oConLiq.Prioridad.ToString()))
                        retMsg += @" \nError en " + errConceptoID + " Prioridad erronea ";
                }
                #endregion Valido Prioridad

                #region Valido Cod Sidif
                if (oConLiq.CodSidif.ToString().Length > 0)
                {
                    if (!Utilidades.esNumerico(oConLiq.CodSidif.ToString()))
                        retMsg += @" \nError en " + errConceptoID + " Codigo SiDif no es Numerico";
                    else if (oConLiq.CodSidif.ToString().Length > 8)
                        retMsg += @" \nError en " + errConceptoID + " Codigo SiDif excede los 8 Digitos";
                }
                #endregion Valido Cod Sidif

                #region Valida Afiliacion
                if ((bool.Parse(oConLiq.EsAfiliacion.ToString()) != true) && (bool.Parse(oConLiq.EsAfiliacion.ToString()) != false))
                    retMsg += @"\n Debe Definir Afiliacion";
                #endregion Valida Afiliacion

                #region Valida Tipos de Concepto X Cod de Concepto
                //TODO: Ver el filtro
                //string Filtro = "CodConceptoLiq= '" + oConLiq.CodConceptoLiq.ToString() + "'";
                //dtTipoConc.DefaultView.RowFilter = Filtro;
                //dtTipoConc.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                if (listTipoConcepto.Count == 0)
                    retMsg += @" \nEl Descuento " + oConLiq.CodConceptoLiq.ToString() + " no posee Tipos asociados";
                else
                {
                    foreach (TipoConcepto oTipoConcepto in listTipoConcepto)
                    {
                        if (oTipoConcepto.IdTipoConcepto.ToString().Length == 0)
                            retMsg += @" \nError en " + errConceptoID + " Debe Definir el Tipo de Descuento";
                        else if (oTipoConcepto.IdTipoConcepto.ToString().Length > 1)
                            retMsg += @" \nError en " + errConceptoID + " Tipo de Descuento no Corresponde";
                        else if (!Utilidades.esNumerico(oTipoConcepto.IdTipoConcepto.ToString()))
                            retMsg += @" \nError en " + errConceptoID + " Tipo de Descuento no es Numerico";

                        if (bool.Parse(oConLiq.EsAfiliacion.ToString())) //es Afiliacion
                        {
                            string MaxDesc = ""; //double.Parse(FilaTipo.MaxADescontar.ToString()).ToString();

                            if ((oTipoConcepto.IdTipoConcepto.ToString() == "1") || (oTipoConcepto.IdTipoConcepto.ToString() == "6"))
                            {
                                //si 1 o 6 (Permanente) debe informar importe correcto

                                if (MaxDesc.Length == 0)
                                    retMsg += @" \nError en " + errConceptoID + " Debe Definir el Tope Maximo a Descontar";

                                else if (MaxDesc.Length > 9)
                                    retMsg += @" \nError en " + errConceptoID + " Tope Maximo no Corresponde";

                                else if (!Utilidades.EsNumerioConDecimales(MaxDesc))
                                {
                                    retMsg += @" \nError en " + errConceptoID + " Tope no es Numerico";
                                }
                                else if (oTipoConcepto.IdTipoConcepto.ToString() == "6") //perm por %
                                {
                                    //MaxDesc = Utilidades.ConvertToDouble(FilaTipo.MaxADescontar.ToString());
                                    if ((Single.Parse(MaxDesc) < 0) || (Single.Parse(MaxDesc) > 40))
                                        retMsg += @" \nEl campo porcentaje debe ser un valor entre 0 y 40.";
                                }
                            }
                            else //otro tipo no corresponde
                            {
                                retMsg += @" \nError en " + errConceptoID + " Tipo de Descuento no Corresponde";
                                //if (FilaTipo.MaxADescontar.ToString().Length != 0) //no corresponde tope
                                //    retMsg += @" \nError en " + errConceptoID + " no corresponde informar Tope";
                            }
                        }
                        else //no Afiliacion
                        {
                            //cualquiertipo sin informar tope
                            //if (FilaTipo.MaxADescontar.ToString().Length != 0)
                            //    retMsg += @" \nError en " + errConceptoID + " no corresponde informar Tope";
                        }
                    }
                }

                #endregion Valida Tipos de Concepto X Cod de Concepto
            }
            #endregion Valida Conceptos

            //Testeo del Error
            if (retMsg.Length > 0)
            {
                return retMsg;
            }

            return retMsg;
        }
        #endregion

        #region Valida Repeticion de Conceptos y Tipos de Liq Altas

        private string validaExistenciaConceptos(List<ConceptoLiquidacion> listConceptosLiquidacion)
        {

            string sql = "Admin_ValidaConceptosExistentes";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string msg = "";

            try
            {
                #region Valido Repeticion en tabla Local

                //int err, i;
                //cantConc = dtLiq.Rows.Count
                int codConceptoLiq = 0;

                if (!listConceptosLiquidacion.Exists(delegate(ConceptoLiquidacion cl1)
                        {
                            return listConceptosLiquidacion.Exists(delegate(ConceptoLiquidacion cl2)
                             {
                                 if (cl1.CodConceptoLiq == cl2.CodConceptoLiq)
                                 {
                                     codConceptoLiq = cl1.CodConceptoLiq;
                                     return true;
                                 }
                                 else
                                     return false;
                             });
                        }
                   ))
                {
                    msg += @"\nEl Descuento " + codConceptoLiq.ToString() + " se Repite";
                    return msg;
                }
                //foreach (ConceptoLiquidacion oConLiq in listConceptosLiquidacion)
                //{
                //    err = 0;
                //    for (i = 0; i < cantConc; i++)
                //    {
                //        if (oConLiq.CodConceptoLiq.ToString() == dtLiq.Rows[i]["CodConceptoLiq"].ToString())
                //        {
                //            err++;
                //        }
                //    }
                //    if (err > 1)
                //    {
                //        msg += @" \nEl Descuento " + fila["CodConceptoLiq"].ToString() + " se Repite";
                //        return msg;
                //    }
                //}

                #endregion Valido Repeticion en tabla Local

                #region Valido Conceptos contra SQL
                foreach (ConceptoLiquidacion oConceptoLiquidacion in listConceptosLiquidacion)
                {
                    db.AddInParameter(dbCommand, "@codConcepto", DbType.Int32, oConceptoLiquidacion.CodConceptoLiq);
                    db.AddOutParameter(dbCommand, "@error", DbType.String, 50);

                    db.ExecuteNonQuery(dbCommand);

                    if (db.GetParameterValue(dbCommand, "@error").ToString().Length > 0)
                        msg += @" \n" + db.GetParameterValue(dbCommand, "@error").ToString();
                }
                return msg;

                #endregion Valido Conceptos contra SQL
            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Valida Repeticion de Conceptos y Tipos de Liq Altas y Modificaciones

        private string validaExistenciaConceptosAM(List<ConceptoLiquidacion> listConceptoLiquidacion, List<TipoConcepto> listTipoConcepto)
        {
            //DataTable dtLiq = new DataTable();
            //DataTable dtTipoConc = new DataTable();
            //dtLiq = dsDatosConcepto.Tables[0];
            //dtTipoConc = dsDatosConcepto.Tables[1];
            //Conexion objCnn = new Conexion();
            //SqlParameter[] objPar = new SqlParameter[2];

            string sql = "Admin_ValidaConceptosExistentes";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string msg = "";
            try
            {
                #region Valido Repeticion en tabla Local
                int cantConc = listConceptoLiquidacion.Count, err;

                foreach (ConceptoLiquidacion oConLiq in listConceptoLiquidacion)
                {
                    err = 0;
                    foreach (ConceptoLiquidacion oConLiq2 in listConceptoLiquidacion)
                    {
                        if (oConLiq.CodConceptoLiq.ToString() == oConLiq2.CodConceptoLiq.ToString())
                        {
                            err++;
                            break;
                        }
                    }
                    if (err > 1)
                    {
                        msg += @" \nEl Descuento " + oConLiq.CodConceptoLiq.ToString() + " se Repite";
                        return msg;
                    }
                    else
                    {
                        int errtipo;
                        //TODO: Ver el filtro
                        //string Filtro = "CodConceptoLiq= '" + oConLiq.CodConceptoLiq.ToString() + "'";
                        //dtTipoConc.DefaultView.RowFilter = Filtro;
                        //dtTipoConc.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                        //int cantTipo = dtTipoConc.DefaultView.Count;

                        foreach (TipoConcepto oTipoConcepto in listTipoConcepto)
                        {
                            errtipo = 0;
                            foreach (TipoConcepto oTipoConcepto2 in listTipoConcepto)
                            {
                                if (oTipoConcepto.IdTipoConcepto == oTipoConcepto2.IdTipoConcepto)
                                {
                                    errtipo++;
                                    break;
                                }
                            }
                            if (errtipo > 1)
                            {
                                msg += @" \nEl Tipo de Descuento " + oTipoConcepto.IdTipoConcepto.ToString() + " se Repite";
                                return msg;
                            }
                        }
                    }
                }


                #endregion Valido Repeticion en tabla Local

                #region Valido Conceptos contra SQL

                foreach (ConceptoLiquidacion oConceptoLiquidacion in listConceptoLiquidacion)
                {
                    bool nuevo = true; //bool.Parse(FilaConc["nuevo"].ToString());
                    if (nuevo)
                    {
                        db.AddInParameter(dbCommand, "@codConcepto", DbType.Int32, oConceptoLiquidacion.CodConceptoLiq);
                        db.AddOutParameter(dbCommand, "@error", DbType.String, 50);

                        db.ExecuteNonQuery(dbCommand);

                        if (db.GetParameterValue(dbCommand, "@error").ToString().Length > 0)
                            msg += @" \n" + db.GetParameterValue(dbCommand, "@error").ToString();
                    }
                }
                return msg;

                #endregion Valido Conceptos contra SQL

            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Valida Repeticion del CUIT y Cod Sistema

        private string ValidarCruceCUITCodSistema(string cuit, string codSis)
        {

            string sql = "Admin_ValidaCuitCodConcExistente";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string msg = "";
            try
            {
                db.AddInParameter(dbCommand, "@cuit", DbType.String, cuit);
                db.AddInParameter(dbCommand, "@codsistema", DbType.String, codSis);
                db.AddOutParameter(dbCommand, "@error", DbType.String, 100);

                db.ExecuteNonQuery(dbCommand);

                if (db.GetParameterValue(dbCommand, "@error").ToString().Length != 0)
                    msg = db.GetParameterValue(dbCommand, "@error").ToString();
                return msg;
            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Alta y Modificacion de Contactos

        public string ContactosAltaMod(string idPrestador, string idDomicilio, string idContacto,
                                       string apellNom, string telPref, string telNro, string fax,
                                       string email, string areaTrabajo, string cargo, string obs, string usuario)
        {
            string sql = "Admin_ContactosPrestador_AM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string Error = ValidarParamContacto(idPrestador, idDomicilio, idContacto, apellNom, telPref, telNro, fax, email, areaTrabajo, cargo, obs);

            if (Error.ToString().Length > 0)
                return Error;
            else
            {
                try
                {
                    #region Alta del Contacto
                    //string user = System.EnterpriseServices.SecurityCallContext.CurrentCall.OriginalCaller.AccountName.Substring((SecurityCallContext.CurrentCall.OriginalCaller.AccountName.IndexOf(@"\") + 1), 7).ToString().ToUpper();
                    db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, long.Parse(idPrestador));
                    db.AddInParameter(dbCommand, "@idDomicilio", DbType.Int32, int.Parse(idDomicilio));
                    db.AddInParameter(dbCommand, "@idcontacto", DbType.Int32, int.Parse(idContacto));
                    db.AddInParameter(dbCommand, "@apnom", DbType.String, apellNom);
                    db.AddInParameter(dbCommand, "@telpref", DbType.String, telPref);
                    db.AddInParameter(dbCommand, "@telnro", DbType.String, telNro);
                    db.AddInParameter(dbCommand, "@fax", DbType.String, fax);
                    db.AddInParameter(dbCommand, "@email", DbType.String, email);
                    db.AddInParameter(dbCommand, "@areatrabajo", DbType.String, areaTrabajo);
                    db.AddInParameter(dbCommand, "@cargo", DbType.String, cargo);
                    db.AddInParameter(dbCommand, "@obs", DbType.String, obs);
                    db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);

                    db.ExecuteNonQuery(dbCommand);

                    #endregion Alta Contacto
                }
                catch (Exception SQLDBException)
                {
                    log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                    throw SQLDBException;
                }
                finally
                {
                    db = null;
                    dbCommand.Dispose();
                }
                return Error;
            }
        }

        #region Validaciones

        private string ValidarParamContacto(string prestador, string id_Domicilio, string id_contacto,
                                            string apellnom, string telPref, string telNro, string fax,
                                            string email, string areatrabajo, string cargo, string obs)
        {
            string Error = "";
            #region Valida Prestador
            if (prestador.ToString().Length == 0)
                Error += @"\n No se Establecio un Codigo de la Entidad";
            else if (!Utilidades.esNumerico(prestador.ToString()))
                Error += @"\n Entidad no es numerico";
            else if (long.Parse(prestador.ToString()) > 4294967296)
                Error += @"\n Entidad excede su Longitud permitida";
            #endregion Valida Prestador

            #region Valida ID Contacto

            if (id_contacto.ToString().Length > 0)
            {
                if (!Utilidades.esNumerico(id_contacto.ToString()))
                    Error += @"\n ID de Contacto no es numerica";
                else if (int.Parse(id_contacto.ToString()) > 65536)
                    Error += @"\n ID de Contacto excede su Longitud permitida";

            }

            #endregion Valida ID Contacto

            #region Valida ID Domicilio

            if (id_Domicilio.ToString().Length == 0)
                Error += @"\n No se Establecio ID de Domicilio";
            else if (!Utilidades.esNumerico(id_Domicilio.ToString()))
                Error += @"\n ID de Domicilio no es numerica";
            else if (int.Parse(id_Domicilio.ToString()) > 65536)
                Error += @"\n ID de Domicilio excede su Longitud permitida";


            #endregion Valida ID Domicilio

            #region Valida Apellido y Nombre

            if (apellnom.ToString().Length > 0)
            {
                if (apellnom.ToString().Length > 100)
                    Error += @"\n Apellido y Nombre excede su Longitud permitida";
            }

            #endregion Valida Apellido y Nombre

            #region Prefijo

            if (telPref.ToString().Length > 0)
            {
                if (telPref.ToString().Length > 5)
                    Error += @"\n Prefijo excede su Longitud permitida";
            }

            #endregion Prefijo

            #region Tel Numero

            if (telNro.ToString().Length > 0)
            {
                if (telNro.ToString().Length > 100)
                    Error += @"\n Telefono excede su Longitud permitida";
            }

            #endregion Tel Numero

            #region Fax

            if (fax.ToString().Length > 0)
            {
                if (fax.ToString().Length > 20)
                    Error += @"\n Fax excede su Longitud permitida";
            }

            #endregion Fax

            #region EMail

            if (email.ToString().Length > 0)
            {
                if (email.ToString().Length > 100)
                    Error += @"\n E-Mail excede su Longitud permitida";
                else if (!Utilidades.ValidaMail(email.ToString()))
                    Error += @"\n E-Mail es Incorrecto";

            }

            #endregion EMail

            #region Area Tr

            if (areatrabajo.ToString().Length > 0)
            {
                if (areatrabajo.ToString().Length > 50)
                    Error += @"\n Area de Trabajo excede su Longitud permitida";
            }

            #endregion Area Tr

            #region Cargo

            if (cargo.ToString().Length > 0)
            {
                if (cargo.ToString().Length > 50)
                    Error += @"\n Cargo excede su Longitud permitida";
            }

            #endregion Cargo

            #region Obs

            if (obs.ToString().Length > 0)
            {
                if (obs.ToString().Length > 250)
                    Error += @"\n Observaciones excede su Longitud permitida";
            }

            #endregion Obs

            return Error;
        }

        #endregion

        #endregion

        #region Alta y Modificacion de Domicilio

        public string DomiciliosAltaMod(string idPrestador, string idDomicilio, string calle,
                                        string nro, string piso, string dpto, string codPcia,
                                        string localidad, string codPostal, string telPref,
                                        string telNro, string fax, string observacion, string idTipoDom, string usuario)
        {
            string sql = "Admin_Domicilio_AM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string Error = ValidarParamDomicilio(idPrestador, idDomicilio, calle, nro, piso, dpto, codPcia, localidad, codPostal, telPref, telNro, fax, observacion, idTipoDom);

            if (Error.ToString().Length > 0)
                return Error;
            else
            {
                try
                {
                    #region Alta Mod del Domicilio

                    if (piso.Length == 0)
                        piso = null;
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@IdDomicilio", DbType.Int16, idDomicilio);
                    db.AddInParameter(dbCommand, "@calle", DbType.String, calle);
                    db.AddInParameter(dbCommand, "@nro", DbType.Int32, nro);
                    db.AddInParameter(dbCommand, "@piso", DbType.Int16, Int16.Parse(piso));
                    db.AddInParameter(dbCommand, "@dpto", DbType.String, dpto);
                    db.AddInParameter(dbCommand, "@cod_pcia", DbType.Int16, codPcia);
                    db.AddInParameter(dbCommand, "@localidad", DbType.String, localidad);
                    db.AddInParameter(dbCommand, "@cod_postal", DbType.String, codPostal);
                    db.AddInParameter(dbCommand, "@tel_prefijo", DbType.String, telPref);
                    db.AddInParameter(dbCommand, "@tel_nro", DbType.String, telNro);
                    db.AddInParameter(dbCommand, "@fax", DbType.String, fax);
                    db.AddInParameter(dbCommand, "@observaciones", DbType.String, observacion);
                    db.AddInParameter(dbCommand, "@Id_tipoDom", DbType.Int16, idTipoDom);
                    db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);

                    db.ExecuteNonQuery(dbCommand);

                    #endregion Alta Mod Domicilio
                }
                catch (Exception SQLDBException)
                {
                    log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                    throw SQLDBException;
                }
                finally
                {
                    db = null;
                    dbCommand.Dispose();
                }

                return Error;
            }
        }

        #region Validaciones

        private string ValidarParamDomicilio(string prestador, string id_Domicilio, string calle, string nro, string piso, string dpto, string codPcia, string localidad, string codpos, string telpref, string telnro, string fax, string obs, string tipodom)
        {
            string Error = "";
            #region Valida Prestador
            if (prestador.ToString().Length == 0)
                Error += @"\n No se Establecio un Codigo de la Entidad";
            else if (!Utilidades.esNumerico(prestador.ToString()))
                Error += @"\n Entidad no es numerico";
            else if (long.Parse(prestador.ToString()) > 4294967296)
                Error += @"\n Entidad excede su Longitud permitida";
            #endregion Valida Prestador

            #region Valida ID Domicilio

            if (id_Domicilio.ToString() != "0")
            {
                if (!Utilidades.esNumerico(id_Domicilio.ToString()))
                    Error += @"\n ID de Domicilio no es numerica";
                else if (int.Parse(id_Domicilio.ToString()) > 65536)
                    Error += @"\n ID de Domicilio excede su Longitud permitida";

            }

            #endregion Valida ID Domicilio

            #region Valida calle

            if (calle.ToString().Length > 0)
            {
                if (calle.ToString().Length > 100)
                    Error += @"\n Calle excede su Longitud permitida";
            }

            #endregion Valida Calle

            #region Valida nro

            if (nro.ToString().Length > 0)
            {
                if (nro.ToString().Length > 5)
                    Error += @"\n Altura excede su Longitud permitida";
                else if (!Utilidades.esNumerico(nro.ToString()))
                    Error += @"\n Altura no es numerica";
            }

            #endregion nro

            #region piso

            if (piso.ToString().Length > 0)
            {
                if (piso.ToString().Length > 2)
                    Error += @"\n El Piso excede su Longitud permitida";
                else if (!Utilidades.esNumerico(piso.ToString()))
                    Error += @"\n El Piso no es numerico";
            }

            #endregion piso

            #region dpto

            if (dpto.ToString().Length > 0)
            {
                if (dpto.ToString().Length > 2)
                    Error += @"\n El Departamento excede su Longitud permitida";

            }

            #endregion dpto

            #region pcia

            if (codPcia.ToString().Length > 0)
            {
                if (!Utilidades.esNumerico(codPcia.ToString()))
                    Error += @"\n Parametro Provincia no es numerico.";
                else if (((int.Parse(codPcia.ToString()) < 1) || (int.Parse(codPcia.ToString())) > 24) && (int.Parse(codPcia.ToString()) != 99))
                    Error += @"\n Parametro Provincia es Invalido.";

            }
            #endregion pcia

            #region Localidad

            if (localidad.ToString().Length > 0)
            {
                if (localidad.ToString().Length > 50)
                    Error += @"\n La Localidad excede su Longitud permitida";

            }

            #endregion

            #region Cod Postal

            if (codpos.ToString().Length > 0)
            {
                if (codpos.ToString().Length > 8)
                    Error += @"\n El Codigo Postal excede su Longitud permitida";

            }

            #endregion

            #region PrefijoTel

            if (telpref.ToString().Length > 0)
            {
                if (telpref.ToString().Length > 5)
                    Error += @"\n El Prefijo excede su Longitud permitida";
                else if (!Utilidades.esNumerico(telpref.ToString()))
                    Error += @"\n El Prefijo no es numerico";

            }

            #endregion

            #region NumeroTel

            if (telnro.ToString().Length > 0)
            {
                if (telnro.ToString().Length > 50)
                    Error += @"\n El Numero Telefonico excede su Longitud permitida";
                else if (!Utilidades.esNumerico(telnro.ToString()))
                    Error += @"\n El Numero Telefonico no es numerico";

            }

            #endregion

            #region fax

            if (fax.ToString().Length > 0)
            {
                if (fax.ToString().Length > 50)
                    Error += @"\n Fax excede su Longitud permitida";
            }

            #endregion Fax

            #region Obs

            if (obs.ToString().Length > 0)
            {
                if (obs.ToString().Length > 250)
                    Error += @"\n Observaciones excede su Longitud permitida";
            }

            #endregion Obs

            #region Tipo Domicilio

            if (tipodom.ToString().Length > 0)
            {
                if (tipodom.ToString().Length > 1)
                    Error += @"\n No Corresponde el Tipo de Domicilio";
                else if (!Utilidades.esNumerico(tipodom.ToString()))
                    Error += @"\n No numerico el codigo de Tipo de Domicilio";
            }

            #endregion Tipo Domicilio

            return Error;
        }

        #endregion

        #endregion

        #region Alta y Modificacion de Convenios

        public string ConveniosAltaMod(string idPrestador, string idConvenio, string nroExpediente,
                                       string matricula, string resolucion, string resolucionDen,
                                       string idEnte, string nroCarpeta, string idEstado, string fecInicio,
                                       string descripcion, string observaciones, string tna,
                                       string tea, string gastosAdm, string usuario)
        {

            string sql = "Admin_Convenios_AM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string msgerror = string.Empty;
            msgerror = validarParamConvenio(idPrestador, idConvenio, nroExpediente, matricula, resolucion, resolucionDen, idEnte, nroCarpeta, idEstado, fecInicio, descripcion, observaciones, tna, tea, gastosAdm);

            if (msgerror.Length > 0)
                return msgerror;
            else
            {
                try
                {
                    //user = System.EnterpriseServices.SecurityCallContext.CurrentCall.OriginalCaller.AccountName.Substring((SecurityCallContext.CurrentCall.OriginalCaller.AccountName.IndexOf(@"\") + 1), 7).ToString().ToUpper();
                    #region Carga de Parametros

                    if (!Utilidades.esNumerico(idEnte))
                        idEnte = null;
                    if (!Utilidades.esNumerico(idEstado.ToString()))
                        idEstado = null;

                    #endregion Carga de Parametros

                    #region Declara Parametros

                    db.AddInParameter(dbCommand, "@idconvenio", DbType.Int64, long.Parse(idConvenio));
                    db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, long.Parse(idPrestador));
                    db.AddInParameter(dbCommand, "@exptenro", DbType.String, nroExpediente);
                    db.AddInParameter(dbCommand, "@matricula", DbType.String, matricula);
                    db.AddInParameter(dbCommand, "@resolucion", DbType.String, resolucion);
                    db.AddInParameter(dbCommand, "@resolucion_den", DbType.String, resolucionDen);
                    db.AddInParameter(dbCommand, "@idente", DbType.Int16, idEnte);
                    db.AddInParameter(dbCommand, "@nrocarpeta", DbType.Int32, int.Parse(nroCarpeta));
                    db.AddInParameter(dbCommand, "@idestado", DbType.Int16, int.Parse(idEstado));
                    db.AddInParameter(dbCommand, "@fecini", DbType.DateTime, DateTime.Parse(fecInicio));
                    db.AddInParameter(dbCommand, "@descripcion", DbType.String, descripcion);
                    db.AddInParameter(dbCommand, "@observaciones", DbType.String, observaciones);
                    db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);
                    db.AddInParameter(dbCommand, "@tna", DbType.String, tna);
                    db.AddInParameter(dbCommand, "@tea", DbType.String, tea);
                    db.AddInParameter(dbCommand, "@ga", DbType.String, gastosAdm);

                    #endregion Declara Parametros

                    db.ExecuteNonQuery(dbCommand);

                }
                catch (Exception SQLDBException)
                {
                    log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                    throw SQLDBException;
                }
                finally
                {
                    db = null;
                    dbCommand.Dispose();
                }
            }

            return msgerror;
        }

        private string validarParamConvenio(string idPrestador, string idConvenio, string nroExpediente,
                                            string matricula, string resolucion, string resolucionDen, string
                                            idEnte, string nroCarpeta, string idEstado, string fecInicio, string
                                            descripcion, string observaciones, string tna, string tea, string gastosAdm)
        {
            string Error = string.Empty;

            #region Prestador
            if (idPrestador.Length == 0)
                Error += @"\n Falta Codigo de Prestador";
            else if (!Utilidades.esNumerico(idPrestador.ToString()))
                Error += @"\n Cod. Prestador no es numerico";
            else if (long.Parse(idPrestador.ToString()) > 2147783647)
                Error += @"\n Cod. Prestador exede su longitud permitida";
            #endregion Prestador

            #region Convenio
            if (idConvenio.Length == 0)
                Error += @"\n Falta Codigo de Convenio";
            else if (int.Parse(idConvenio.ToString()) != 0)
            {
                if (!Utilidades.esNumerico(idConvenio.ToString()))
                    Error += @"\n Cod. Convenio no es numerico";
                else if (long.Parse(idConvenio.ToString()) > 2147783647)
                    Error += @"\n Cod. Convenio exede su longitud permitida";
            }
            #endregion Convenio

            #region Expediente
            if (nroExpediente.ToString().Length == 0)
                Error += @"\n Debe registrar un Expediente";
            else if (nroExpediente.ToString().Length > 17)
                Error += @"\n Expediente exede los 17 Caracteres";
            #endregion Expediente

            #region Matricula
            if (matricula.ToString().Length == 0)
                Error += @"\n Debe Registrar Matricula";
            else if (matricula.ToString().Length > 50)
                Error += @"\n La Matricula exede su longitud permitida";
            #endregion Matricula

            #region Resolucion
            if (resolucion.ToString().Length == 0)
                Error += @"\n Debe Registrar Nro de Resolucion";
            else if (resolucion.ToString().Length > 100)
                Error += @"\n La Resolucion exede su longitud permitida";

            #endregion Resolucion

            #region Resolucion DEN
            if (resolucionDen.ToString().Length == 0)
                Error += @"\n Debe Registrar Nro de Resolucion de Direccion Ejecutiva";
            else if (resolucionDen.ToString().Length > 100)
                Error += @"\n La Resolucion DEN  exede su longitud permitida";

            #endregion Resolucion DEN

            #region Carpeta
            if (nroCarpeta.ToString().Length == 0)
                Error += @"\n Debe Registrar Nro de Carpeta";
            else if (!Utilidades.esNumerico(nroCarpeta.ToString()))
                Error += @"\n El Nro de Carpeta  no es numerico";
            else if ((int.Parse(nroCarpeta.ToString()) > 65536) || (int.Parse(nroCarpeta.ToString()) < 0))
                Error += @"\n El Nro de Carpeta  exede su longitud permitida";

            #endregion Carpeta

            #region Fecha inicio

            if (fecInicio.ToString().Length == 0)
                Error += @"\n Debe Registrar Fecha de Inicio del Convenio";
            else if (fecInicio.ToString().Length > 10)
                Error += @"\n Fecha de Inicio no debe exeder los 10 Caracteres (dd/MM/aaaa)";
            else if (!Utilidades.esFechaValida(fecInicio.ToString()))
                Error += @"\n Fecha de Inicio no es valida";


            #endregion Fecha inicio

            #region Descripcion
            if (descripcion.ToString().Length > 0)
            {
                if (descripcion.ToString().Length > 250)
                    Error += @"\n La Descripcion  exede su longitud permitida";
            }

            #endregion Descripcion

            #region Observacion
            if (observaciones.ToString().Length > 0)
            {
                if (observaciones.ToString().Length > 250)
                    Error += @"\n La Observacion  exede su longitud permitida";
            }

            #endregion Observacion

            #region TNA
            if (tna.ToString().Length > 0)
            {
                if (tna.ToString().Length > 10)
                    Error += @"\n La Tasa Nominal Anual exede su longitud permitida";
                else if (!Utilidades.EsNumerioConDecimales(tna.ToString()))
                    Error += @"\n La Tasa Nominal Anual debe ser Numerica";
            }
            #endregion TNA

            #region TEA
            if (tea.ToString().Length > 0)
            {
                if (tea.ToString().Length > 10)
                    Error += @"\n La Tasa Efectiva Anual exede su longitud permitida";
                else if (!Utilidades.EsNumerioConDecimales(tea.ToString()))
                    Error += @"\n La Tasa Efectiva Anual debe ser Numerica";
            }
            #endregion TEA

            #region Gastos Adm
            if (gastosAdm.ToString().Length > 0)
            {
                if (gastosAdm.ToString().Length > 10)
                    Error += @"\n El Monto de Gastos Administrativos exede su longitud permitida";
                else if (!Utilidades.EsNumerioConDecimales(gastosAdm.ToString()))
                    Error += @"\n El Monto corresp. a los Gastos Administrativos debe ser numerico";
            }
            #endregion

            return Error;
        }

        #endregion

        #region Baja de Contacto

        public string ContactosBaja(string idContacto)
        {
            string sql = "Admin_ContactosPrestador_Baja";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string msg = "";
            #region Valida ID Contacto

            if (idContacto.Length == 0)
                msg = "Debe existir un parametro Contacto";
            else if (!Utilidades.esNumerico(idContacto.ToString()))
                msg = "ID de Contacto no es numerica";
            else if (int.Parse(idContacto.ToString()) > 65536)
                msg = "ID de Contacto exede su Dimension permitida";

            if (msg.Length > 0)
                return msg;

            #endregion Valida ID Contacto
            try
            {
                db.AddInParameter(dbCommand, "@idcontacto", DbType.Int32, idContacto);
                db.AddOutParameter(dbCommand, "@error", DbType.String, 100);
                //objPar[1].Value= string.Empty;

                db.ExecuteNonQuery(dbCommand);

                if (db.GetParameterValue(dbCommand, "@error").ToString().Length != 0)
                    msg = db.GetParameterValue(dbCommand, "@error").ToString();
                return msg;
            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Baja de Domicilio

        public string DomiciliosBaja(string idDomicilio)
        {

            string sql = "Admin_DomiciliosPrestador_Baja";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string msg = "";
            #region Valida ID Domicilio

            if (idDomicilio.Length == 0)
                msg = "Debe existir un parametro Domicilio";
            else if (!Utilidades.esNumerico(idDomicilio.ToString()))
                msg = "ID de Domicilio no es numerica";
            else if (int.Parse(idDomicilio.ToString()) > 65536)
                msg = "ID de Domicilio exede su Dimension permitida";

            if (msg.Length > 0)
                return msg;

            #endregion Valida ID Domicilio

            try
            {

                db.AddInParameter(dbCommand, "@idDomicilio", DbType.Int32, idDomicilio);
                db.AddOutParameter(dbCommand, "@error", DbType.String, 100);

                //objPar[1].Value= string.Empty;

                db.ExecuteNonQuery(dbCommand);

                if (db.GetParameterValue(dbCommand, "@error").ToString().Length != 0)
                    msg = db.GetParameterValue(dbCommand, "@error").ToString();
                return msg;
            }
            catch (Exception SQLDBException)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), SQLDBException.Source, SQLDBException.Message));
                throw SQLDBException;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #endregion

        #region Componentes no transaccionales

        #region Traer Prestador - Busqueda

        public static List<Prestador> TraerPrestadoresAdm(string CodigoSistema, int CodConcLiq, string RazonSocial)
        {
            string sql = "Admin_Prestador_Busca";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            //DbParameterCollection dbParametros = null;

            try
            {
                List<Prestador> unaListaPrestadores = new List<Prestador>();

                db.AddInParameter(dbCommand, "@CodSistema", DbType.String, CodigoSistema);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConcLiq);
                db.AddInParameter(dbCommand, "@RazonSocial", DbType.String, RazonSocial);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        unaListaPrestadores.Add(new Prestador(ds.GetNullableString("CodSistema"),
                                           ds.GetNullableInt64("IDPrestador") == null ? 0 : ds.GetInt64("IDPrestador"),
                                           ds.GetNullableString("RazonSocial"),
                                           ds["Cuit"].Equals(DBNull.Value) ? 0 : long.Parse(ds["cuit"].ToString()),
                                           ds["Email"].Equals(DBNull.Value) ? string.Empty : ds["Email"].ToString(),
                                           ds["CodOficinaAnme"].Equals(DBNull.Value) ? 0 : int.Parse(ds["CodOficinaAnme"].ToString()),
                                           ds["Observaciones"].Equals(DBNull.Value) ? string.Empty : ds["Observaciones"].ToString(),
                                           new Estado(ds["IDEstado"].Equals(DBNull.Value) ? 0 : int.Parse(ds["IDEstado"].ToString())),
                                           ConceptoOPPDAO.Traer_ConceptosLiq_TxPrestador(long.Parse(ds["IDPrestador"].ToString())),
                                           Convert.ToBoolean(ds["esNominada"].ToString()),
                                           Convert.ToBoolean(ds["esAnses"].ToString()),
                                           Convert.ToBoolean(ds["entregaDocumentacionEnFGS"].ToString()),
                                           Convert.ToBoolean(ds["esEntidadOficial"].ToString())));
                    }
                }
                return unaListaPrestadores;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));  
                throw new Exception("Error en PrestadorDAO.TraerPrestadoresAdm", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Tipo concepto liquidación tipo presatador NO FUNCIONA - Este metodo se encuntra en TipoConcConcLiqDAO

        ///// <summary>
        ///// Trae 2 trablas conteniendo los tipos de conceptos para ese prestador
        ///// y los conceptos de liquedacion para ese prestador.
        ///// </summary>
        ///// <param name="_Prestador">_Prestador</param>
        ///// <returns>DataSet con dos tablas</returns>
        //public static List<List<TipoConcepto>> TipoConcConLiqPorPresatador(long idPrestador)
        //{
        //    string sql = "TipoConc_ConcLiq_TxPrestador";
        //    Database db = DatabaseFactory.CreateDatabase("DAT_V01");
        //    DbCommand dbCommand = db.GetStoredProcCommand(sql);
        //    List<List<TipoConcepto>> TiposConcepto = new List<List<TipoConcepto>>();

        //    try
        //    {
        //        db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
        //        using (DataSet dataSet = db.ExecuteDataSet(dbCommand))
        //        {
        //            foreach (DataTable dt in dataSet.Tables)
        //            {
        //                List<TipoConcepto> listTipoConcepto = new List<TipoConcepto>();
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    //ConceptoLiquidacion oConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
        //                    //                                                                   dr["DescConceptoLiq"].ToString(),
        //                    //                                                                   byte.Parse(dr["Prioridad"].ToString()),
        //                    //                                                                   int.Parse(dr["CodSidif"].ToString()),
        //                    //                                                                   bool.Parse(dr["Obligatorio"].ToString()),
        //                    //                                                                   bool.Parse(dr["EsAfiliacion"].ToString()),
        //                    //                                                                   dr["CodSistema"].ToString(),
        //                    //                                                                   bool.Parse(dr["Hab_Online"].ToString()),
        //                    //                                                                   bool.Parse(dr["Habilitado2"].ToString()));

        //                    listTipoConcepto.Add(new TipoConcepto(byte.Parse(dr["TipoConcepto"].ToString()),
        //                                                          dr["DescTipoConcepto"].ToString(),
        //                                                          bool.Parse(dr["Habilitado"].ToString())));

        //                    //DateTime.Parse(dr["FecInicio"].ToString());
        //                    //DateTime.Parse(dr["FecFin"].ToString());
        //                    //decimal.Parse(dr["MaxADescontar"].ToString());
        //                }

        //                TiposConcepto.Add(listTipoConcepto);
        //            }

        //            return TiposConcepto;
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //    finally
        //    {
        //        db = null;
        //        dbCommand.Dispose();
        //    }
        //}
        #endregion
                
        #region Trae Contactos

        public static List<Prestador> ContactosTraer(long idPrestador, int idDomicilio)
        {
            string sql = "Admin_ContactosPrestador_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();
            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idDomicilio", DbType.Int32, idDomicilio);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        oListPrestadores.Add(new Prestador(
                                                dr.GetInt64("IdPrestador"),
                                                dr.GetString("RazonSocial"),
                                                long.Parse(dr.GetString("CUIT"))));

                        //dr.GetNullableInt32("IdContacto");
                        //dr.GetNullableInt32("IDDomicilio");
                        //dr.GetString("ApellidoyNombre");
                        //dr.GetString("Tel_Prefijo");
                        //dr.GetString("Tel_Nro");
                        //dr.GetString("Fax");
                        //dr.GetString("Email");
                        //dr.GetString("AreaTrabajo");
                        //dr.GetString("Cargo");
                        //dr.GetString("Observaciones");                         
                    }
                }
                return oListPrestadores;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Trae Domicilios

        public static List<Domicilio> DomiciliosTraer(long idPrestador)
        {
            string sql = "Admin_DomiciliosPrestador_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Domicilio> oListDomicilio = new List<Domicilio>();

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);

                Domicilio oDomicilio = new Domicilio();
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        oDomicilio.Calle = dr.GetString("Calle");
                        oDomicilio.NumeroCalle = dr.GetString("Nro");
                        oDomicilio.Piso = dr.GetByte("Piso").ToString();
                        oDomicilio.Departamento = dr.GetString("Dpto");
                        oDomicilio.CodigoPostal = dr.GetString("CodPostal");
                        oDomicilio.PrefijoTel = dr.GetString("Tel_Prefijo");
                        oDomicilio.NumeroTel = dr.GetString("Tel_Nro");
                        oDomicilio.Fax = dr.GetString("Fax");
                        oDomicilio.Localidad = dr.GetString("Localidad");
                        oDomicilio.UnaProvincia = new Provincia(dr.GetByte("C_Pcia"), dr.GetString("D_PCIA"));
                        oDomicilio.Observaciones = dr.GetString("Observaciones");
                        oDomicilio.UnTipoDomicilio = new TipoDomicilio(dr.GetByte("ID_TipoDomicilio"), dr.GetString("Descripcion"));

                        oListDomicilio.Add(oDomicilio);

                        #region codigo comentado
                        //dr.GetInt64("IdPrestador"),
                        //dr.GetString("RazonSocial"),
                        //long.Parse(dr.GetString("CUIT"))));

                        //dr.GetInt32("IDDomicilio");
                        //dr.GetString("Calle");
                        //dr.GetNullableInt32("Nro");
                        //
                        //dr.GetString("Dpto");
                        //dr.GetNullableByte("C_Pcia");
                        //dr.GetString("D_PCIA");
                        //dr.GetString("DA_PCIA");
                        //dr.GetString("Localidad");
                        //dr.GetString("CodPostal");
                        //dr.GetString("Tel_Prefijo");
                        //dr.GetString("Tel_Nro");
                        //dr.GetString("Fax");
                        //dr.GetString("Observaciones");
                        //dr.GetNullableByte("ID_TipoDomicilio");
                        //dr.GetString("Descripcion");                       
                        #endregion
                    }
                }

                return oListDomicilio;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Trae Convenios

        public static List<Prestador> ConveniosTraer(long idPrestador)
        {
            string sql = "Admin_Convenios_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        oListPrestadores.Add(new Prestador(
                                                dr.GetInt64("IdPrestador"),
                                                dr.GetString("RazonSocial"),
                                                long.Parse(dr.GetString("CUIT"))));

                        #region codigo comentado
                        //dr.GetInt32("IDDomicilio");
                        //dr.GetString("Calle");
                        //dr.GetNullableInt32("Nro");
                        //dr.GetNullableByte("Piso");
                        //dr.GetString("Dpto");
                        //dr.GetNullableByte("C_Pcia");
                        //dr.GetString("D_PCIA");
                        //dr.GetString("DA_PCIA");
                        //dr.GetString("Localidad");
                        //dr.GetString("CodPostal");
                        //dr.GetString("Tel_Prefijo");
                        //dr.GetString("Tel_Nro");
                        //dr.GetString("Fax");
                        //dr.GetString("Observaciones");
                        //dr.GetNullableByte("ID_TipoDomicilio");
                        //dr.GetString("Descripcion");                   
                        #endregion
                    }
                }

                return oListPrestadores;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Traer Conceptos por Codigo de Sistema

        public static List<Prestador> TraerConceptosPorCodSistema(string codSistema)
        {
            string sql = "Admin_TraeConceptosLiquidadosXCodSistema";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();

            try
            {
                db.AddInParameter(dbCommand, "@CodSistema", DbType.String, codSistema);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        ConceptoLiquidacion oConceptoLiquidacion = new ConceptoLiquidacion(
                        dr.GetInt32("CodConceptoLiq"),
                        dr.GetString("DescConceptoLiq"),
                        0, null, false, false,
                        dr.GetString("CodSistema"),
                        false, false, new TipoConcepto() );

                        oListPrestadores.Add(new Prestador(
                                                dr.GetInt64("IdPrestador"),
                                                dr.GetString("RazonSocial"), 0,
                                                oConceptoLiquidacion));

                    }
                }

                return oListPrestadores;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Traer Conceptos por Codigo de Concepto

        public static List<Prestador> TraerConceptosPorCodConcepto(Int64 codConcepto)
        {
            string sql = "Admin_TraeConceptosLiquidadosXConcepto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();

            try
            {
                db.AddInParameter(dbCommand, "@codconceptoliq", DbType.Int64, codConcepto);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        ConceptoLiquidacion oConceptoLiquidacion = new ConceptoLiquidacion(
                        dr.GetInt32("CodConceptoLiq"),
                        dr.GetString("DescConceptoLiq"),
                        0, null, false, false,
                        dr.GetString("CodSistema"),
                        false, false, new TipoConcepto());

                        oListPrestadores.Add(new Prestador(
                                                dr.GetInt64("IdPrestador"),
                                                dr.GetString("RazonSocial"), 0,
                                                oConceptoLiquidacion));

                    }
                }

                return oListPrestadores;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Trae prestadores

        public static List<Prestador> TraerPrestador(byte orden, long idPrestador)
        {
            string sql = "Admin_Prestadores_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();

            try
            {
                db.AddInParameter(dbCommand, "@orden", DbType.Int16, orden);
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);

                if ((orden >= 2))
                {
                    return oListPrestadores;
                }
                else
                {
                    using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                    {
                        while (dr.Read())
                        {                           
                            Prestador oPrestador = new Prestador(
                                                 Int64.Parse(dr["IdPrestador"].ToString()),
                                                 dr["RazonSocial"].ToString(),
                                                 dr["CUIT"].Equals(DBNull.Value) ? 0 : long.Parse(dr["CUIT"].ToString()),
                                                 dr["Email"].Equals(DBNull.Value) ? "" : dr["Email"].ToString(),
                                                 dr["CodOficinaAnme"].Equals(DBNull.Value) ? 0 : int.Parse(dr["CodOficinaAnme"].ToString()),
                                                 dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                 new Estado(dr["IDEstado"].Equals(DBNull.Value) ? 0 : int.Parse(dr.GetByte("IDEstado").ToString())),
                                                 new Auditoria(),
                                                 string.Empty,
                                                 string.Empty,
                                                 new DateTime(),
                                                 string.Empty,
                                                 Convert.ToBoolean(dr["esNominada"]),
                                                 Convert.ToBoolean(dr["esAnses"]),
                                                 Convert.ToBoolean(dr["entregaDocumentacionEnFGS"]),
                                                 Convert.ToBoolean(dr["esComercio"]) 
                                                  );

                            oPrestador.UnaListaConceptoLiquidacion = ConceptoOPPDAO.Traer_ConceptosLiq_TxPrestador(long.Parse(dr["IDPrestador"].ToString()));                                             

                            oListPrestadores.Add(oPrestador);
                        }
                    }
                    return oListPrestadores;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        
        public static Prestador TraerPrestador_x_CUIT(long CUIT)
        {
            string sql = "Prestador_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            
            Prestador oPrestador = new Prestador();
            
            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, CUIT);

                using (DataTable dt = db.ExecuteDataSet(dbCommand).Tables[0])
                {
                    if (dt.Rows.Count != 1)
                    {
                        throw new Exception("Al ejecutar el SP Prestador_Traer CUIL " + CUIT.ToString() + " trajo mas de un prestador");
                    }
                    else
                    {
                        List<Prestador> lPrestador = new List<Prestador>();
                        lPrestador = TraerPrestador(1, long.Parse(dt.Rows[0]["IDPrestador"].ToString()));
                        oPrestador = lPrestador[0];
                    }

                    dt.Dispose();
                }
                return oPrestador;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                oPrestador.Dispose();
            }
        }

        public static List<Prestador> TraerPrestadorAsociadoACUIL(long CUIL)
        {
            //Falta crear metodo y ver que debe retornar de cada prestador
            string sql = "Prestador_Traer_X_CUIL";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();
       
            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, CUIL);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                    {
                        while (dr.Read())
                        {                           
                            Prestador oPrestador = new Prestador(
                                                 Int64.Parse(dr["IdPrestador"].ToString()),
                                                 dr["RazonSocial"].ToString(),
                                                 dr["CUIT"].Equals(DBNull.Value) ? 0 : long.Parse(dr["CUIT"].ToString()),
                                                 dr["Email"].Equals(DBNull.Value) ? "" : dr["Email"].ToString(),
                                                 dr["CodOficinaAnme"].Equals(DBNull.Value) ? 0 : int.Parse(dr["CodOficinaAnme"].ToString()),
                                                 dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                 new Estado(dr["IDEstado"].Equals(DBNull.Value) ? 0 : int.Parse(dr.GetByte("IDEstado").ToString())),
                                                 new Auditoria(),
                                                 string.Empty,
                                                 string.Empty,
                                                 new DateTime(),
                                                 string.Empty,
                                                 Convert.ToBoolean(dr["esNominada"]),
                                                 Convert.ToBoolean(dr["esAnses"]),
                                                 Convert.ToBoolean(dr["entregaDocumentacionEnFGS"]),
                                                 Convert.ToBoolean(dr["esComercio"]) 
                                                  );

                            oPrestador.UnaListaConceptoLiquidacion = ConceptoOPPDAO.Traer_ConceptosLiq_TxPrestador(long.Parse(dr["IDPrestador"].ToString()));                                             

                            oListPrestadores.Add(oPrestador);
                        }
                    }                    

                return oListPrestadores;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Trae Todos los Conceptos

        public static List<TipoConcepto> TraerConceptosTodo(long idPrestador)
        {
            string sql = "Admin_ConceptosXPrestadorTT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoConcepto> listTipoConcepto = new List<TipoConcepto>();

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //ConceptoLiquidacion oConceptoLiquidacion = new ConceptoLiquidacion(dr.GetInt32("CodConceptoLiq"),
                        //                                                                   dr.GetString("DescConceptoLiq"),
                        //                                                                   0,//(dr.GetNullableByte("Prioridad") == null ?  0 : dr.GetByte("Prioridad")),
                        //                                                                   dr.GetNullableInt32("CodSidif"),
                        //                                                                   dr.GetBoolean("Obligatorio"),
                        //                                                                   dr.GetBoolean("EsAfiliacion"),
                        //                                                                   dr.GetString("CodSistema"),
                        //                                                                   dr.GetBoolean("Hab_Online"),
                        //                                                                   dr.GetBoolean("Habilitado"),
                        //                                                                   new TipoConcepto());

                        listTipoConcepto.Add(new TipoConcepto(0,
                                                              string.Empty,
                                                              false));

                        //dr.GetInt32("CodConceptoLiq");
                        //byte.Parse(dr["TipoConcepto");
                        //dr.GetString("DescTipoConcepto");
                        //dr.GetBoolean("Habilitado");
                        //dr.GetNullableDateTime("FecInicio");
                        //dr.GetNullableDateTime("FecFin");
                        //dr.GetNullableDouble("MaxADescontar");

                    }
                }
                return listTipoConcepto;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Trae prestadores Entrega FGS

        public static List<Prestador> Traer_Prestadores_Entrega_FGS()
        {
            string sql = "Prestadores_Trae_Entrega_FGS";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Prestador> oListPrestadores = new List<Prestador>();

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        Prestador oPrestador = new Prestador(
                                             Int64.Parse(dr["IdPrestador"].ToString()),
                                             dr["RazonSocial"].ToString(),
                                             dr["CUIT"].Equals(DBNull.Value) ? 0 : long.Parse(dr["CUIT"].ToString()),
                                             dr["Email"].Equals(DBNull.Value) ? "" : dr["Email"].ToString(),
                                             dr["CodOficinaAnme"].Equals(DBNull.Value) ? 0 : int.Parse(dr["CodOficinaAnme"].ToString()),
                                             dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                             new Estado(dr["IDEstado"].Equals(DBNull.Value) ? 0 : int.Parse(dr.GetByte("IDEstado").ToString())),
                                             new Auditoria(),
                                             string.Empty,
                                             string.Empty,
                                             new DateTime(),
                                             string.Empty,
                                             Convert.ToBoolean(dr["esNominada"]),
                                             Convert.ToBoolean(dr["esAnses"]),
                                             Convert.ToBoolean(dr["entregaDocumentacionEnFGS"])
                                             );

                        oListPrestadores.Add(oPrestador);
                    }
                }

                return oListPrestadores;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }
        #endregion

        #endregion
    }
}
