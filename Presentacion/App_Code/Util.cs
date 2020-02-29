using System;
using System.Text;
using System.Data;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Diagnostics;
using System.Web.SessionState;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.Globalization;
using System.Web.UI;
using ANSES.Microinformatica.DAT.Negocio;
/// <summary>
/// Provee de funciones grales. de validacion de datos y administracion
/// 
/// </summary>
public class Util
{
    public static string fechaInicio_Dat = "01.01.2005";

    #region Contructores
    public Util()
    {

    }
    #endregion Contructores

    #region Set Focus - Da el foco a un control
    public static void SetFocus(System.Web.UI.Page page, System.Web.UI.Control ctrl)
    {
        string s = "<script language='javascript'>" +
                   "document.getElementById('" + ctrl.ID + "').focus() </script>";

        if (!page.IsStartupScriptRegistered("focus"))
            page.RegisterStartupScript("focus", s);
    }
    #endregion

    #region LLenar Combo
    /// <summary>
    /// Procedimiento que llena un DropDownList en base a un Tabla especificada
    /// </summary>
    /// <param name="ComboBox">Objeto de tipo DropDowList.</param>
    /// <param name="ObjTabla">Objeto de tipo String que representa el nombre de la tabla.</param>
    public static void LLenarCombo(DropDownList comboBox, string ObjTabla)
    {
        switch (ObjTabla.ToUpper())
        {
            case "CRITERIOBUSQUEDA":
                comboBox.Items.Insert(0, new ListItem("[ Seleccione ]", "0"));
                comboBox.Items.Insert(1, new ListItem("Descuento Total", "1"));
                comboBox.Items.Insert(2, new ListItem("Descuento Proporcional", "2"));
                comboBox.Items.Insert(3, new ListItem("No Descontado", "3"));
                break;
            case "CRITERIOFILTRADO":
                comboBox.Items.Insert(0, new ListItem("Sin Filtro", "0"));
                comboBox.Items.Insert(1, new ListItem("Nro Beneficiario", "1"));
                comboBox.Items.Insert(2, new ListItem("Tipo Concepto", "3"));
                comboBox.Items.Insert(3, new ListItem("Concepto", "4"));
                break;
            case "CRITERIOFILTRADO_AGENCIAS":
                //_Combo.Items.Insert(0,new ListItem("Sin Filtro","0") );
                comboBox.Items.Insert(0, new ListItem("Nro Legajo", "1"));
                comboBox.Items.Insert(1, new ListItem("Nombre de Agencia", "2"));
                break;
            case "CRITERIOFILTRADO_CONSNOVEDADES":
                LLenarCombo(comboBox, "CRITERIOFILTRADO");
                comboBox.Items.Insert(4, new ListItem("Entre Fechas", "5"));
                break;
            case "CRITERIOFILTRADO_CONSNOVEDADES_CANCELADAS":
                comboBox.Items.Insert(0, new ListItem("Sin Filtro", "0"));
                comboBox.Items.Insert(1, new ListItem("Nro Beneficiario", "1"));
                comboBox.Items.Insert(2, new ListItem("Nro Novedad", "2"));
                comboBox.Items.Insert(3, new ListItem("Concepto", "4"));
                comboBox.Items.Insert(4, new ListItem("Entre Fechas", "5"));
                break;
            case "CRITERIOFILTRADO_CONSNOVEDADES_CANCELADAS_PRESTACIONAL":
                comboBox.Items.Insert(0, new ListItem("Sin Filtro", "0"));
                comboBox.Items.Insert(1, new ListItem("Nro Beneficiario", "1"));
                comboBox.Items.Insert(2, new ListItem("Nro Novedad", "2"));
                comboBox.Items.Insert(2, new ListItem("Tipo Concepto", "3"));
                comboBox.Items.Insert(3, new ListItem("Concepto", "4"));
                break;
            case "PRESTADOR_FGS":
                List<WSPrestador.Prestador> unaListaPrestadorFGS = new List<WSPrestador.Prestador>(Prestador.Traer_Prestadores_Entrega_FGS());

                if (unaListaPrestadorFGS.Count > 0)
                {
                    comboBox.DataSource = unaListaPrestadorFGS;
                    comboBox.DataTextField = "RazonSocial";
                    comboBox.DataValueField = "ID";
                    comboBox.DataBind();
                    comboBox.Items.FindByValue(ConfigurationManager.AppSettings["IDPrestadorANSES"].ToString()).Selected = true;
                }
                break;
            default:
                LLenarCombo(comboBox, ObjTabla, null);
                break;
        }
    }

    /// <summary>
    /// Procedimiento que llena un DropDownList en base a un Tabla especificada
    /// </summary>
    /// <param name="ComboBox">Objeto de tipo DropDowList.</param>
    /// <param name="ObjTabla">Objeto de tipo String que representa el nombre de la tabla.</param>
    /// <param name="aParam">Array de objetos que para ejecutar los metodos de traer.</param>
    public static void LLenarCombo(DropDownList comboBox, string objTabla, Object[] aParam)
    {
        System.Web.HttpContext oContext = System.Web.HttpContext.Current;

        List<WSTipoConcConcLiq.ConceptoLiquidacion> unaListaTipoConcepto = null;
        comboBox.Items.Clear();	//Elimina todos los elementos de Combo.

        switch (objTabla.ToString().ToUpper())
        {
            case "TIPOCONCEPTO":
                unaListaTipoConcepto = (List<WSTipoConcConcLiq.ConceptoLiquidacion>)IsInCache("TIPOCONCEPTO", 3, aParam, false);

                var groupTipoConcepto = from o in unaListaTipoConcepto
                                        group o by o.UnTipoConcepto.IdTipoConcepto into groupTC
                                        select groupTC.ToList();

                unaListaTipoConcepto = new List<WSTipoConcConcLiq.ConceptoLiquidacion>();
                groupTipoConcepto.ToList().ForEach(delegate(List<WSTipoConcConcLiq.ConceptoLiquidacion> lsttc) { unaListaTipoConcepto.Add(lsttc[0]); });

                if (unaListaTipoConcepto.Count() > 0)
                {
                    comboBox.DataSource = unaListaTipoConcepto;
                    comboBox.DataTextField = "DescTipoConcepto";
                    comboBox.DataValueField = "IdTipoConcepto";
                    comboBox.DataBind();
                }
                break;

            case "CONCEPTOOPP":
                unaListaTipoConcepto = (List<WSTipoConcConcLiq.ConceptoLiquidacion>)IsInCache("CONCEPTOOPP");

                var unGroup = from o in unaListaTipoConcepto
                              where o.UnTipoConcepto.IdTipoConcepto.ToString() == aParam[0].ToString()
                              group o by o.CodConceptoLiq into groupCL
                              select groupCL.ToList();

                List<WSTipoConcConcLiq.ConceptoLiquidacion> unConceptoLiquidacion = new List<WSTipoConcConcLiq.ConceptoLiquidacion>();

                unGroup.ToList().ForEach(delegate(List<WSTipoConcConcLiq.ConceptoLiquidacion> tc) { unConceptoLiquidacion.Add(tc.ToList()[0]); });

                if (unConceptoLiquidacion.Count() > 0)
                {
                    comboBox.DataSource = unConceptoLiquidacion;
                    comboBox.DataTextField = "DescConceptoLiq";
                    comboBox.DataValueField = "CodConceptoLiq";
                    comboBox.DataBind();
                }

                break;

            case "CIERRES":
                List<WSCierre.Cierre> unaListaCierres = new List<WSCierre.Cierre>((WSCierre.Cierre[])IsInCache("CIERRES"));

                if (unaListaCierres.Count > 0)
                {
                    if (int.Parse(aParam[0].ToString()) != 0)
                    {
                        var x = from t in unaListaCierres where t.FecAplicacionPagos != null select t;
                        unaListaCierres = x.ToList();
                    }

                    comboBox.DataSource = unaListaCierres;
                    comboBox.DataTextField = "Mensual";
                    comboBox.DataValueField = "FecCierre";
                    comboBox.DataBind();
                }
                break;           
        }

        comboBox.Items.Insert(0, "[ Seleccione ]");
        if (comboBox.Items.Count == 2) { comboBox.SelectedIndex = 1; }
        if (comboBox.Items.Count > 2) { comboBox.SelectedIndex = -1; }

        comboBox.Dispose();
    }

    public static void LLenarCombo(Object ComboBox, string ObjTabla, Object[] aParam, object _datos)
    {

        DropDownList _Combo = (DropDownList)ComboBox;

        _Combo.Items.Clear();  //Elimina todos los elementos de Combo.
        string filtro = string.Empty;

        List<WSPrestador.ConceptoLiquidacion> unaListaConceptoLiquidacion = null;

        switch (ObjTabla.ToString().ToUpper())
        {
            case "TIPOCONCEPTO":
                #region
                /* Si el parametro es:
                 *	 0 trae todo
                 *	 1 todo menos lo indeterminado
                 *	 2 solo indeterminado		
                 *	 3 solo en cuotas
                 * */

                unaListaConceptoLiquidacion = new List<WSPrestador.ConceptoLiquidacion>((WSPrestador.ConceptoLiquidacion[])_datos);
                var unaListaTipoConcepto = (from o in unaListaConceptoLiquidacion
                                            select new { IdTipoConcepto = o.UnTipoConcepto.IdTipoConcepto, DescTipoConcepto = o.UnTipoConcepto.DescTipoConcepto }).Distinct().ToList();

                switch (byte.Parse(aParam[0].ToString()))
                {
                    case 1:
                        filtro = "TipoConcepto not in (1,6)";
                        unaListaTipoConcepto = (from o in unaListaConceptoLiquidacion
                                                where o.UnTipoConcepto.IdTipoConcepto != 1 && o.UnTipoConcepto.IdTipoConcepto != 6
                                                select new { IdTipoConcepto = o.UnTipoConcepto.IdTipoConcepto, DescTipoConcepto = o.UnTipoConcepto.DescTipoConcepto }).Distinct().ToList();
                        break;
                    case 2:
                        filtro = "TipoConcepto in (1,6)";
                        unaListaTipoConcepto = (from o in unaListaConceptoLiquidacion
                                                where o.UnTipoConcepto.IdTipoConcepto == 1 || o.UnTipoConcepto.IdTipoConcepto == 6
                                                select new { IdTipoConcepto = o.UnTipoConcepto.IdTipoConcepto, DescTipoConcepto = o.UnTipoConcepto.DescTipoConcepto }).Distinct().ToList();

                        break;
                    case 3:
                        filtro = "TipoConcepto = 3 ";
                        unaListaTipoConcepto = (from o in unaListaConceptoLiquidacion
                                                where o.UnTipoConcepto.IdTipoConcepto == 3
                                                select new { IdTipoConcepto = o.UnTipoConcepto.IdTipoConcepto, DescTipoConcepto = o.UnTipoConcepto.DescTipoConcepto }).Distinct().ToList();

                        break;
                    default:
                        break;
                }

                if (unaListaTipoConcepto.Count > 0)
                {
                    _Combo.DataSource = unaListaTipoConcepto;
                    _Combo.DataTextField = "DescTipoConcepto";
                    _Combo.DataValueField = "IdTipoConcepto";
                }

                break;
                #endregion
            case "CONCEPTOOPP":
                #region
                //fga 20090610 Resolucion 257/09
                //filtro = "TipoConcepto = " + aParam[0] + " and CodConceptoLiq not in (" + conceptoPCAbuelito + "," + conceptoCPAT + "," + conceptoVamosPaseo + ")";

                unaListaConceptoLiquidacion = new List<WSPrestador.ConceptoLiquidacion>((WSPrestador.ConceptoLiquidacion[])_datos);

                var listConceptoLiquidacion = (from i in unaListaConceptoLiquidacion
                                               where aParam[0].ToString() == i.UnTipoConcepto.IdTipoConcepto.ToString()
                                               select new { ConceptoLiquidacion = i }.ConceptoLiquidacion).ToList().Distinct();

                if (listConceptoLiquidacion.ToList().Count > 0)
                {
                    _Combo.DataSource = listConceptoLiquidacion.ToList();
                    _Combo.DataTextField = "DescConceptoLiq";
                    _Combo.DataValueField = "CodConceptoLiq";
                }

                break;
                #endregion
            case "CONCEPTOPCABUELITO":
                #region
                //filtro = "TipoConcepto = " + aParam[0] + " and CodConceptoLiq in (" + conceptoPCAbuelito + ")";

                //_datos.tables[1].DefaultView.RowFilter = filtro;

                //if (_datos.tables[0].Rows.Count > 0)
                //{
                //    _Combo.DataSource = _datos.tables[1].DefaultView;
                //    _Combo.DataTextField = "DescConceptoLiq";
                //    _Combo.DataValueField = "CodConceptoLiq";
                //}

                break;
            //FGA 20090610 Resolucion 257/09 - Caja Popular de ahorros de pcia Tucuman
                #endregion
            case "CONCEPTOCPAT":
                #region

                //filtro = "TipoConcepto = " + aParam[0] + " and CodConceptoLiq in (" + conceptoCPAT + ")";

                //_datos.tables[1].DefaultView.RowFilter = filtro;

                //if (_datos.tables[0].Rows.Count > 0)
                //{
                //    _Combo.DataSource = _datos.tables[1].DefaultView;
                //    _Combo.DataTextField = "DescConceptoLiq";
                //    _Combo.DataValueField = "CodConceptoLiq";
                //}

                break;
            //FGA 20100121 - Vamos de paseo
                #endregion
            case "CONCEPTOVAMOSPASEO":
                #region
                //filtro = "TipoConcepto = " + aParam[0] + " and CodConceptoLiq in (" + conceptoVamosPaseo + ")";

                //_datos.tables[1].DefaultView.RowFilter = filtro;

                //if (_datos.tables[0].Rows.Count > 0)
                //{
                //    _Combo.DataSource = _datos.tables[1].DefaultView;
                //    _Combo.DataTextField = "DescConceptoLiq";
                //    _Combo.DataValueField = "CodConceptoLiq";
                //}

                break;
                #endregion
        }

        if (_datos != null) { _Combo.DataBind(); }

        _Combo.Items.Insert(0, "[ Seleccione ]");
        if (_Combo.Items.Count == 2) { _Combo.SelectedIndex = 1; _Combo.Enabled = false; }
        if (_Combo.Items.Count > 2) { _Combo.SelectedIndex = -1; }

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
    public static object IsInCache(string NombreTabla)
    {
        return IsInCache(NombreTabla, 5);
    }

    /// <summary>
    /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
    /// </summary>
    /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
    /// <param name="TiempoMinCache">Tipo int. Representa el tiempo en Minutos del cache</param>		
    /// <returns>Un DataSet</returns>
    public static object IsInCache(string NombreTabla, int TiempoMinCache)
    {
        return IsInCache(NombreTabla, TiempoMinCache, null, false);
    }

    /// <summary>
    /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
    /// </summary>
    /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
    /// <param name="Refrescar">Tipo Boolean. Representa si se desea volver a traer los datos desde la base</param>
    /// <returns>Un DataSet</returns>
    public static object IsInCache(string NombreTabla, bool Refrescar)
    {
        return IsInCache(NombreTabla, 5, null, Refrescar);
    }

    /// <summary>
    /// Permite abrir las tablas, llenar un dataset y colocarlas en el Cache
    /// </summary>
    /// <param name="NombreTabla">Tipo string.Representa el nombre de la tabla</param>
    /// <param name="TiempoMinCache">Tipo int. Representa el tiempo en Minutos del cache</param>		
    /// <param name="Refrescar">Tipo Boolean. Representa si se desea volver a traer los datos desde la base</param>
    /// <returns>Un DataSet</returns>
    public static object IsInCache(string NombreTabla, int TiempoMinCache, Object[] aParam, bool Refrescar)
    {
        object datos = new object();
        System.Web.HttpContext oContext = System.Web.HttpContext.Current;

        // Elimino del Cache el Item seleccionado para volver a cargarlo.
        if (Refrescar)
        { oContext.Cache.Remove(NombreTabla); }

        if (oContext.Cache[NombreTabla] != null)
        {
            datos = (object)oContext.Cache[NombreTabla];
        }
        else
        {
            //string Valor;
            switch (NombreTabla.ToString().ToUpper())
            {
                case "TIPOCONCEPTO":
                case "CONCEPTOOPP":
                    List<WSTipoConcConcLiq.ConceptoLiquidacion> unaListaTipoConcepto = TipoConLiq.Traer_ConceptosLiq_TxPrestador(long.Parse(aParam[0].ToString())); //Corresponde al ID Prestador
                    datos = (object)unaListaTipoConcepto;
                    break;

                case "CIERRES":
                    WSCierre.CierreWS cie = new WSCierre.CierreWS();
                    //cie.Url = ConfigurationManager.AppSettings["url.Servicio.Cierres"];
                    cie.Url = ConfigurationManager.AppSettings["WSCierre.CierreWS"];
                    cie.Credentials = CredentialCache.DefaultCredentials;
                    datos = cie.TraerCierresAnteriores();
                    cie.Dispose();
                    break;
            }

            // Agrego al Cache
            oContext.Cache.Insert(NombreTabla, datos, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(TiempoMinCache));
        }

        return datos;
    }

    #endregion  IsInCache - Abre tablas que seran utilizadas frecuentemente

    #region Retorna el Path relativo
    /// <summary>
    /// Este método returna el path relativo 
    /// </summary>
    /// <param name="request">Objeto System.Web.HttpRequest </param>
    /// <returns>El Path relativo</returns>
    /// <example>Util.GetRelativePath( Request )</example>
    public static string GetRelativePath(HttpRequest request)
    {

        string sPath = String.Empty;

        try
        {
            if (request.ApplicationPath != "/") { sPath = request.ApplicationPath; }

        }
        catch (Exception err)
        {
            throw err;
        }

        return sPath;

    }
    #endregion

    #region validoIP
    public static bool ValidoIP(string IP)
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

    #region CleanInput - Limpia todos los caracteres No-alfanumericos
    public static string CleanInput(string strIn)
    {
        //Reemplaza Caracteres no Alfa Por Blancos. 
        Regex _patron = new Regex("[^A-Za-z0-9]");
        return _patron.Replace(strIn, "");
    }
    #endregion

    #region Convertir a Double - Respetando separador de decimales
    /* #region Convertir a Double - Respetando separador de decimales
    public static double ConvertToDouble(string Valor)
    {
        string Separador = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        return double.Parse(Valor.Replace(",", Separador).Replace(".", Separador));
    }*/
    /*Recupero_Simulador*/
    #region Convertir a Double - Respetando separador de decimales
    public static double ConvertToDouble(string Valor)
    {
        return double.Parse(RemplazaPuntoXComa(Valor));

    }
    public static string RemplazaPuntoXComa(string Valor)
    {
        string Separador = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

        return Valor.Replace(",", Separador).Replace(".", Separador);

    }
    #endregion

    #region Diferencia entre 2 (dos) fechas
    public static string DateDiff(string FechaDesde, string FechaHasta)
    {

        try
        {
            TimeSpan Dif = DateTime.Parse(FechaHasta).Subtract(DateTime.Parse(FechaDesde));
            return Dif.Days.ToString();
        }
        catch (Exception)
        {
            return "";
        }

    }


    #endregion

    #region Valida Fecha

    public static bool EsFecha(string valor)
    {
        DateTime unFechavalida;
        bool esValido = false;
        if (DateTime.TryParse(valor, out unFechavalida))
        {
            String Patron = "^\\d{2}/\\d{2}/\\d{4}$";
            Regex ExpresionRegular = new Regex(Patron);
            esValido = ExpresionRegular.IsMatch(valor);
        }
        return esValido;

        //DateTime unFechavalida;
        //return DateTime.TryParse(Valor, out unFechavalida);
    }


    //public static bool esFechaValida(string fecha)
    //{
    //    DateTime unFechavalida;
    //    return DateTime.TryParse(Fecha, out unFechavalida);

    //    //if (Fecha.Length != 10)
    //    //    return false;

    //    //string dia = Fecha.Trim().Substring(0, 2);
    //    //string mes = Fecha.Trim().Substring(3, 2);
    //    //string ano = Fecha.Trim().Substring(6, 4);

    //    //if ((dia.Length == 0) || (dia.Length != 2) || (mes.Length == 0) || (mes.Length != 2) || (ano.Length == 0) || (ano.Length != 4))
    //    //    return false;
    //    //else if ((!esNumerico(dia)) || (!esNumerico(mes)) || (!esNumerico(ano)))
    //    //    return false;

    //    //else if ((int.Parse(mes) > 12) || (int.Parse(mes) < 1))
    //    //    return false;

    //    //else if (int.Parse(dia) < 1)
    //    //    return false;
    //    //else if ((int.Parse(mes) == 1) || (int.Parse(mes) == 3) || (int.Parse(mes) == 5) || (int.Parse(mes) == 7) || (int.Parse(mes) == 8) || (int.Parse(mes) == 10) || (int.Parse(mes) == 12))
    //    //{
    //    //    if (int.Parse(dia) > 31)
    //    //        return false;
    //    //}
    //    //else if ((int.Parse(mes) == 4) || (int.Parse(mes) == 6) || (int.Parse(mes) == 9) || (int.Parse(mes) == 11))
    //    //{
    //    //    if (int.Parse(dia) > 30)
    //    //        return false;
    //    //}
    //    //else
    //    //{
    //    //    int anio = int.Parse(ano);
    //    //    if (((anio % 4) == 0) && (((anio % 100) != 0) || (anio % 400) == 0))
    //    //    {
    //    //        if (int.Parse(dia) > 29)
    //    //            return false;
    //    //        else if (int.Parse(dia) > 28)
    //    //            return false;
    //    //    }
    //    //}
    //    //return true;
    //}

    #endregion Valida Fecha

    #region Valida Ingreso de Numeros

    public static bool esNumerico(string Valor)
    {
        bool ValidoDatos = false;

        Regex numeros = new Regex(@"^\d{1,}$");

        if (Valor.Length != 0)
        {
            ValidoDatos = numeros.IsMatch(Valor);
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
    public static bool EsNumerioConDecimales(string Valor)
    {
        bool ValidoDatos = false;

        Regex numeros = new Regex(@"^\d{1,}\.\d{2}$|^\d{1,}$");

        if (Valor.Length != 0)
        {
            ValidoDatos = numeros.IsMatch(Valor);
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

    public static bool ValidaMail(string Valor)
    {
        bool ValidoDatos = false;

        Regex numeros = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})");

        if (Valor.Length != 0)
        {
            ValidoDatos = numeros.IsMatch(Valor);
        }
        return ValidoDatos;
    }
    #endregion

    #region Validacion de CUIL CUIT
    
    public static bool ValidoCuil(string cuil)
    {
        string FACTORES = "54327654321";
        double dblSuma = 0;
        bool resul = false;

        if (cuil == null)
            return false;

        cuil = cuil.Replace("-", string.Empty);
        if (!(cuil.Substring(0, 1).ToString() != "3" && cuil.Substring(0, 1).ToString() != "2"))
        {
            for (int i = 0; i < 11; i++)
                dblSuma = dblSuma + int.Parse(cuil.Substring(i, 1).ToString()) * int.Parse(FACTORES.Substring(i, 1).ToString());
        }

        resul = Modulo(dblSuma) == 0;
        return resul;
    }
    private static double Modulo(double numero)
    {
        double resul = Math.IEEERemainder(numero, 11);
        return resul;
    }

    public static bool ValidoCuit(string cuit)
    {
        if (cuit == null)
            return false;
        //Quito los guiones, el cuit resultante debe tener 11 caracteres.  
        cuit = cuit.Replace("-", string.Empty);
        if (cuit.Length != 11)
            return false;
        else
        {
            int calculado = CalcularDigitoCuit(cuit);
            int digito = int.Parse(cuit.Substring(10));
            return calculado == digito;
        }
    }
    public static int CalcularDigitoCuit(string cuit)
    {
        int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        char[] nums = cuit.ToCharArray();
        int total = 0;
        for (int i = 0; i < mult.Length; i++)
        {
            total += int.Parse(nums[i].ToString()) * mult[i];
        }
        var resto = total % 11;
        return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
    }

    #endregion

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

    public static string FormateoExpediente(string Nro, bool PonerGiones)
    {
        string sNro = Nro.Replace("-", "");

        if (!PonerGiones)
        {
            return sNro;
        }
        else
        {
            if (sNro.Length == 23)
            {
                sNro = sNro.Substring(0, 3) + "-" + sNro.Substring(3, 2).ToString() + "-" + sNro.Substring(5, 8).ToString() +
                        "-" + sNro.Substring(13, 1).ToString() + "-" + sNro.Substring(14, 3).ToString() + "-" + sNro.Substring(17).ToString();
            }
        }
        return sNro;

    }

    #region Beneficio
    /// <summary>
    /// Formatea un Numero de CUIL 12-12345678-1
    /// </summary>
    /// <param name="Numero">el Numero de Expdiente a formatear</param>
    /// <param name="PonerGiones">true para ponerle los giones</param>
    /// <returns>Número de Expediente formateado.</returns>
    public static string FormateoBeneficio(string Numero, bool PonerGiones)
    {
        string beneficio = Numero.Replace("-", "");

        if (!PonerGiones)
        {
            return beneficio;
        }
        else
        {
            if (beneficio.Length == 11)
            {
                beneficio = beneficio.Substring(0, 2).ToString() + "-" + beneficio.Substring(2, 1).ToString() +
                        "-" + beneficio.Substring(3, 7).ToString() + "-" + beneficio.Substring(10, 1);
            }
            else
            {
                beneficio = "0" + beneficio.Substring(0, 1).ToString() + "-" + beneficio.Substring(1, 1).ToString() +
                        "-" + beneficio.Substring(2, 7).ToString() + "-" + beneficio.Substring(9, 1); ;
            }
        }
        return beneficio;
    }

    #endregion

    #region Fromateo CUIL
    /// <summary>
    /// Formatea un Numero de CUIL 12-12345678-1
    /// </summary>
    /// <param name="Numero">el Numero de Expdiente a formatear</param>
    /// <param name="PonerGiones">true para ponerle los giones</param>
    /// <returns>Número de Expediente formateado.</returns>
    public static string FormateoCUIL(string Numero, bool PonerGiones)
    {
        string sCUIL = Numero.Replace("-", "");

        if (!PonerGiones)
        {
            return sCUIL;
        }
        else
        {
            if (sCUIL.Length == 11)
            {
                sCUIL = sCUIL.Substring(0, 2).ToString() + "-" + sCUIL.Substring(2, 8).ToString() +
                        "-" + sCUIL.Substring(10, 1).ToString();
            }
        }
        return sCUIL;
    }
    #endregion

    #region Fromateo Telefono
    /// <summary>
    /// Formatea un Numero de CUIL 12-12345678-1
    /// </summary>
    /// <param name="Numero">el Numero de Expdiente a formatear</param>
    /// <param name="PonerGiones">true para ponerle los giones</param>
    /// <returns>Número de Expediente formateado.</returns>
    public static string FormateoTelefono(string TelediscadoPais, string Telediscado,string Numero, bool PonerGiones)
    {
        string sTelefono;

        if (!PonerGiones)
        {
            return TelediscadoPais + Telediscado + Numero;
        }
        else
        {
            if (string.IsNullOrEmpty(TelediscadoPais))
            {
                if(string.IsNullOrEmpty(Telediscado))
                    sTelefono= Numero;
                else
                    sTelefono = "(" + Telediscado + ") "+ Numero;
            }
            else
                sTelefono = "(" + TelediscadoPais + "-" + Telediscado + ") " + Numero;
        }
        return sTelefono;
    }
    #endregion

    #region formato para Error

    public static string FormatoError(string Error)
    {
        return "Errores detectados:<div style='margin-left:20px; margin-bottom:10px'> " + Error + "</div>";
    }

    #endregion

    /*public static void LLenarRadioButtonList(Object Radio, string ObjTabla, Object[] aParam, DataSet _datos)
    {

        RadioButtonList _Radio = (RadioButtonList)Radio;

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

                /*switch (byte.Parse(aParam[0].ToString()))
                {
                    case 1:
                        filtro = "TipoConcepto not in (1,6)";
                        break;
                    case 2:
                        filtro = "TipoConcepto in (1,6)";
                        break;
                    default:
                        break;
                }

                _datos.tables[0].DefaultView.RowFilter = filtro;
                if (_datos.tables[0].Rows.Count > 0)
                {
                    _Radio.DataSource = _datos.tables[0].DefaultView;
                    _Radio.DataTextField = "DescTipoConcepto";
                    _Radio.DataValueField = "TipoConcepto";
                }
                break;
        }
    }*/

    public static string RenderControl(Control ctrl)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        ctrl.RenderControl(hw);

        sb.Replace("ContenedorBotones", "neverDisplay").Replace("Botones", "neverDisplay");

        return sb.ToString();
    }

    public static string RenderFiltros(List<FiltroDeSeleccion> filtros)
    {
        StringWriter sw = new StringWriter();
        sw.Write("<table><tr>");
        sw.Write("<td colspan='9' align=\"center\" style=\"font-weight:bold; font-size:15px; color:black; border:1px solid black;\" bgcolor=\"white\">");
        sw.Write(" Filtros De Seleccion</td>");
        sw.Write("</tr></table>");
        sw.Write("<table><tr>");
        foreach (FiltroDeSeleccion filtro in filtros)
        {
            sw.Write("<td {0}>{1}</td>", "style=\"background-color:#D3D3D3 ; font-size: 10px;\"", filtro.NombreFiltro);
        }
        sw.Write("</tr><tr>");
        foreach (FiltroDeSeleccion filtro in filtros)
        {
            sw.Write("<td>{0}</td>", filtro.ValorFiltro.ToString());
        }
        sw.Write("</tr></table>");
        return sw.ToString();
    }

    public static string RenderNovedadesTotales(List<WSNovedad.NovedadTotal> novedades)
    {
        StringWriter sw = new StringWriter();
        foreach (WSNovedad.NovedadTotal novedadTotal in novedades)
        {
            sw.Write("<table><tr><td align=\"center\">{0}</td></tr></table>", novedadTotal.Descripcion);
            sw.Write("<table><tr><td align=\"center\">Concepto</td><td align=\"center\">1Cuota</td><td align=\"center\">Cuotas12</td><td align=\"center\">Cuotas24</td><td align=\"center\">Cuotas36</td><td align=\"center\">Cuotas40</td><td align=\"center\">Cuotas48</td><td align=\"center\">Cuotas60</td><td align=\"center\">Total</td></tr>");

            if (novedadTotal.ContenedoresDeCuotas != null)
            {
                foreach (WSNovedad.ContenedorDeCuotas contenedorCuotas in novedadTotal.ContenedoresDeCuotas)
                {
                    sw.Write("<tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td><td align=\"center\">{6}</td><td align=\"center\">{7}</td><td align=\"center\">{8}</td></tr>", contenedorCuotas.concepto, contenedorCuotas.cuotas1, contenedorCuotas.cuotas12, contenedorCuotas.cuotas24, contenedorCuotas.cuotas36, contenedorCuotas.cuotas40, contenedorCuotas.cuotas48, contenedorCuotas.cuotas60, contenedorCuotas.total);
                }
            }
            sw.Write("<tr><td align=\"center\">Total</td><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td><td align=\"center\">{6}</td><td align=\"center\">{7}</td></tr>", novedadTotal.Total1Cuotas, novedadTotal.Total12Cuotas, novedadTotal.Total24Cuotas,novedadTotal.Total36Cuotas, novedadTotal.Total40Cuotas, novedadTotal.Total48Cuotas, novedadTotal.Total60Cuotas, novedadTotal.Total1Cuotas + novedadTotal.Total12Cuotas + novedadTotal.Total24Cuotas + novedadTotal.Total36Cuotas + novedadTotal.Total40Cuotas + novedadTotal.Total48Cuotas + novedadTotal.Total60Cuotas);
            sw.Write("<tr></tr>");
            sw.Write("</table>");
        }
        return sw.ToString();
    }

    public static Int16 calcularEdad(DateTime fechaNacimiento, DateTime aFecha)
    {
        Int16 _edad;
        DateTime hoy = aFecha;

        _edad = Convert.ToInt16(hoy.Year - fechaNacimiento.Year);
        if (hoy.Month < fechaNacimiento.Month || (hoy.Month == fechaNacimiento.Month && hoy.Day < fechaNacimiento.Day)) _edad--;
        return _edad;
    }

    public static void calcularEdad(DateTime fechaNacimiento, DateTime aFecha, out int anios, out int meses, out int dias)
    {       
        TimeSpan ts = aFecha - fechaNacimiento;
        DateTime d = DateTime.MinValue + ts;
        dias = d.Day - 1;
        meses = d.Month - 1;
        anios = d.Year - 1;
    }

    #endregion

}
