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

namespace Anses.ArgentaC.Negocio.CertificadosWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1099.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CertificadosWSSoap", Namespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez")]
    public partial class CertificadosWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback obtenerCertificadosCudxCuilOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CertificadosWS() {
            this.Url = global::Anses.ArgentaC.Negocio.Properties.Settings.Default.Anses_ArgentaC_Negocio_CertificadosWS_CertificadosWS;
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
        public event obtenerCertificadosCudxCuilCompletedEventHandler obtenerCertificadosCudxCuilCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez/obtenerCertificadosCudxCuil", RequestNamespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez", ResponseNamespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Certificados obtenerCertificadosCudxCuil(string cuil) {
            object[] results = this.Invoke("obtenerCertificadosCudxCuil", new object[] {
                        cuil});
            return ((Certificados)(results[0]));
        }
        
        /// <remarks/>
        public void obtenerCertificadosCudxCuilAsync(string cuil) {
            this.obtenerCertificadosCudxCuilAsync(cuil, null);
        }
        
        /// <remarks/>
        public void obtenerCertificadosCudxCuilAsync(string cuil, object userState) {
            if ((this.obtenerCertificadosCudxCuilOperationCompleted == null)) {
                this.obtenerCertificadosCudxCuilOperationCompleted = new System.Threading.SendOrPostCallback(this.OnobtenerCertificadosCudxCuilOperationCompleted);
            }
            this.InvokeAsync("obtenerCertificadosCudxCuil", new object[] {
                        cuil}, this.obtenerCertificadosCudxCuilOperationCompleted, userState);
        }
        
        private void OnobtenerCertificadosCudxCuilOperationCompleted(object arg) {
            if ((this.obtenerCertificadosCudxCuilCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.obtenerCertificadosCudxCuilCompleted(this, new obtenerCertificadosCudxCuilCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1099.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez")]
    public partial class Certificados {
        
        private Certificado[] certificadoField;
        
        private Errores errorField;
        
        /// <remarks/>
        public Certificado[] certificado {
            get {
                return this.certificadoField;
            }
            set {
                this.certificadoField = value;
            }
        }
        
        /// <remarks/>
        public Errores error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1099.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez")]
    public partial class Certificado {
        
        private long cuilField;
        
        private string tipoCertificadoField;
        
        private string origenCertificadoField;
        
        private string permanenteField;
        
        private string idCudField;
        
        private System.DateTime fechaInicioDanioField;
        
        private System.DateTime fechaEmisionCudField;
        
        private System.DateTime fechaVtoCudField;
        
        private System.DateTime fechaAutorizacionField;
        
        private string mAsigFamiliarField;
        
        private string mFamACargoField;
        
        private System.DateTime fechaBajaField;
        
        private CodigoBajaSNR bajaSNRField;
        
        private Jurisdiccion jurisdiccionField;
        
        /// <remarks/>
        public long cuil {
            get {
                return this.cuilField;
            }
            set {
                this.cuilField = value;
            }
        }
        
        /// <remarks/>
        public string tipoCertificado {
            get {
                return this.tipoCertificadoField;
            }
            set {
                this.tipoCertificadoField = value;
            }
        }
        
        /// <remarks/>
        public string origenCertificado {
            get {
                return this.origenCertificadoField;
            }
            set {
                this.origenCertificadoField = value;
            }
        }
        
        /// <remarks/>
        public string permanente {
            get {
                return this.permanenteField;
            }
            set {
                this.permanenteField = value;
            }
        }
        
        /// <remarks/>
        public string idCud {
            get {
                return this.idCudField;
            }
            set {
                this.idCudField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime fechaInicioDanio {
            get {
                return this.fechaInicioDanioField;
            }
            set {
                this.fechaInicioDanioField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime fechaEmisionCud {
            get {
                return this.fechaEmisionCudField;
            }
            set {
                this.fechaEmisionCudField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime fechaVtoCud {
            get {
                return this.fechaVtoCudField;
            }
            set {
                this.fechaVtoCudField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime fechaAutorizacion {
            get {
                return this.fechaAutorizacionField;
            }
            set {
                this.fechaAutorizacionField = value;
            }
        }
        
        /// <remarks/>
        public string mAsigFamiliar {
            get {
                return this.mAsigFamiliarField;
            }
            set {
                this.mAsigFamiliarField = value;
            }
        }
        
        /// <remarks/>
        public string mFamACargo {
            get {
                return this.mFamACargoField;
            }
            set {
                this.mFamACargoField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime fechaBaja {
            get {
                return this.fechaBajaField;
            }
            set {
                this.fechaBajaField = value;
            }
        }
        
        /// <remarks/>
        public CodigoBajaSNR bajaSNR {
            get {
                return this.bajaSNRField;
            }
            set {
                this.bajaSNRField = value;
            }
        }
        
        /// <remarks/>
        public Jurisdiccion jurisdiccion {
            get {
                return this.jurisdiccionField;
            }
            set {
                this.jurisdiccionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1099.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez")]
    public partial class CodigoBajaSNR {
        
        private short codigoField;
        
        private string nombreField;
        
        /// <remarks/>
        public short Codigo {
            get {
                return this.codigoField;
            }
            set {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        public string Nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1099.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez")]
    public partial class Errores {
        
        private string codigoField;
        
        private string mensajeField;
        
        /// <remarks/>
        public string codigo {
            get {
                return this.codigoField;
            }
            set {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1099.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Ar.Gov.Anses.Prissa.Carpeta.Invalidez")]
    public partial class Jurisdiccion {
        
        private short codigoField;
        
        private string nombreField;
        
        private bool esCudField;
        
        /// <remarks/>
        public short Codigo {
            get {
                return this.codigoField;
            }
            set {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        public string Nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
            }
        }
        
        /// <remarks/>
        public bool EsCud {
            get {
                return this.esCudField;
            }
            set {
                this.esCudField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1099.0")]
    public delegate void obtenerCertificadosCudxCuilCompletedEventHandler(object sender, obtenerCertificadosCudxCuilCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1099.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class obtenerCertificadosCudxCuilCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal obtenerCertificadosCudxCuilCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Certificados Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Certificados)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591