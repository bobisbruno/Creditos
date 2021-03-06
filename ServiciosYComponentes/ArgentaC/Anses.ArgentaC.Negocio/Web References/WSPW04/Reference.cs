﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Anses.ArgentaC.Negocio.WSPW04 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WS_PW04Soap", Namespace="http://adp.anses.gov.ar")]
    public partial class WS_PW04 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ObtenerRelacionesxCuilOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WS_PW04() {
            this.Url = global::Anses.ArgentaC.Negocio.Properties.Settings.Default.Anses_ArgentaC_Negocio_WSPW04_WS_PW04;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ObtenerRelacionesxCuilCompletedEventHandler ObtenerRelacionesxCuilCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://adp.anses.gov.ar/ObtenerRelacionesxCuil", RequestNamespace="http://adp.anses.gov.ar", ResponseNamespace="http://adp.anses.gov.ar", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ListaPw04 ObtenerRelacionesxCuil(string cuil, short nro_pagina_entrada) {
            object[] results = this.Invoke("ObtenerRelacionesxCuil", new object[] {
                        cuil,
                        nro_pagina_entrada});
            return ((ListaPw04)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerRelacionesxCuilAsync(string cuil, short nro_pagina_entrada) {
            this.ObtenerRelacionesxCuilAsync(cuil, nro_pagina_entrada, null);
        }
        
        /// <remarks/>
        public void ObtenerRelacionesxCuilAsync(string cuil, short nro_pagina_entrada, object userState) {
            if ((this.ObtenerRelacionesxCuilOperationCompleted == null)) {
                this.ObtenerRelacionesxCuilOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerRelacionesxCuilOperationCompleted);
            }
            this.InvokeAsync("ObtenerRelacionesxCuil", new object[] {
                        cuil,
                        nro_pagina_entrada}, this.ObtenerRelacionesxCuilOperationCompleted, userState);
        }
        
        private void OnObtenerRelacionesxCuilOperationCompleted(object arg) {
            if ((this.ObtenerRelacionesxCuilCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerRelacionesxCuilCompleted(this, new ObtenerRelacionesxCuilCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://adp.anses.gov.ar")]
    public partial class ListaPw04 {
        
        private short cod_retornoField;
        
        private string desc_mensajeField;
        
        private string cod_errorField;
        
        private string cod_gravedadField;
        
        private short tot_ocurField;
        
        private short cant_reg_parcialField;
        
        private DatosPw04[] listaField;
        
        /// <remarks/>
        public short cod_retorno {
            get {
                return this.cod_retornoField;
            }
            set {
                this.cod_retornoField = value;
            }
        }
        
        /// <remarks/>
        public string desc_mensaje {
            get {
                return this.desc_mensajeField;
            }
            set {
                this.desc_mensajeField = value;
            }
        }
        
        /// <remarks/>
        public string cod_error {
            get {
                return this.cod_errorField;
            }
            set {
                this.cod_errorField = value;
            }
        }
        
        /// <remarks/>
        public string cod_gravedad {
            get {
                return this.cod_gravedadField;
            }
            set {
                this.cod_gravedadField = value;
            }
        }
        
        /// <remarks/>
        public short tot_ocur {
            get {
                return this.tot_ocurField;
            }
            set {
                this.tot_ocurField = value;
            }
        }
        
        /// <remarks/>
        public short cant_reg_parcial {
            get {
                return this.cant_reg_parcialField;
            }
            set {
                this.cant_reg_parcialField = value;
            }
        }
        
        /// <remarks/>
        public DatosPw04[] Lista {
            get {
                return this.listaField;
            }
            set {
                this.listaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://adp.anses.gov.ar")]
    public partial class DatosPw04 {
        
        private string cuil_relaField;
        
        private short c_relacionField;
        
        private string da_relacionField;
        
        private string f_desdeField;
        
        private string f_hastaField;
        
        private string f_vig_hastaField;
        
        private short c_docrespalField;
        
        private string tipo_docrespalField;
        
        private string leg_unicoField;
        
        private string ape_nomField;
        
        private string baseField;
        
        private short c_est_grconField;
        
        private int c_unico_progenField;
        
        private string d_unico_progenField;
        
        private string da_unico_progenField;
        
        private string d_est_grconField;
        
        private System.DateTime f_naciField;
        
        /// <remarks/>
        public string cuil_rela {
            get {
                return this.cuil_relaField;
            }
            set {
                this.cuil_relaField = value;
            }
        }
        
        /// <remarks/>
        public short c_relacion {
            get {
                return this.c_relacionField;
            }
            set {
                this.c_relacionField = value;
            }
        }
        
        /// <remarks/>
        public string da_relacion {
            get {
                return this.da_relacionField;
            }
            set {
                this.da_relacionField = value;
            }
        }
        
        /// <remarks/>
        public string f_desde {
            get {
                return this.f_desdeField;
            }
            set {
                this.f_desdeField = value;
            }
        }
        
        /// <remarks/>
        public string f_hasta {
            get {
                return this.f_hastaField;
            }
            set {
                this.f_hastaField = value;
            }
        }
        
        /// <remarks/>
        public string f_vig_hasta {
            get {
                return this.f_vig_hastaField;
            }
            set {
                this.f_vig_hastaField = value;
            }
        }
        
        /// <remarks/>
        public short c_docrespal {
            get {
                return this.c_docrespalField;
            }
            set {
                this.c_docrespalField = value;
            }
        }
        
        /// <remarks/>
        public string tipo_docrespal {
            get {
                return this.tipo_docrespalField;
            }
            set {
                this.tipo_docrespalField = value;
            }
        }
        
        /// <remarks/>
        public string leg_unico {
            get {
                return this.leg_unicoField;
            }
            set {
                this.leg_unicoField = value;
            }
        }
        
        /// <remarks/>
        public string ape_nom {
            get {
                return this.ape_nomField;
            }
            set {
                this.ape_nomField = value;
            }
        }
        
        /// <remarks/>
        public string Base {
            get {
                return this.baseField;
            }
            set {
                this.baseField = value;
            }
        }
        
        /// <remarks/>
        public short c_est_grcon {
            get {
                return this.c_est_grconField;
            }
            set {
                this.c_est_grconField = value;
            }
        }
        
        /// <remarks/>
        public int c_unico_progen {
            get {
                return this.c_unico_progenField;
            }
            set {
                this.c_unico_progenField = value;
            }
        }
        
        /// <remarks/>
        public string d_unico_progen {
            get {
                return this.d_unico_progenField;
            }
            set {
                this.d_unico_progenField = value;
            }
        }
        
        /// <remarks/>
        public string da_unico_progen {
            get {
                return this.da_unico_progenField;
            }
            set {
                this.da_unico_progenField = value;
            }
        }
        
        /// <remarks/>
        public string d_est_grcon {
            get {
                return this.d_est_grconField;
            }
            set {
                this.d_est_grconField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime f_naci {
            get {
                return this.f_naciField;
            }
            set {
                this.f_naciField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    public delegate void ObtenerRelacionesxCuilCompletedEventHandler(object sender, ObtenerRelacionesxCuilCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerRelacionesxCuilCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerRelacionesxCuilCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ListaPw04 Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ListaPw04)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591