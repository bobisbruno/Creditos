using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
     [Serializable]
    public class Caratulacion
    {
         private long idNovedad;
         private string beneficio; 
         private string cuil;
         private string documento;
         private string tipodocumento; 
         private string nombre;
         private TipoConcepto unTipoConcepto;
         private double importeTotal;
         private Byte cantidadCuotas;
         private DateTime fechaNovedad;
         private DateTime fechaPresentacion;
         private string entidad;
         private string cbu;
         private ConceptoLiquidacion unConceptoLiquidacion;
         private string nroComprobante;
         private string idEstadoReg;
         private string idItem;
         private string descripcionItem;
         private string modoPago;
         private string estaCartulado;
         private string nroExpediente;
         private DateTime? fechaRecepcion;
         private DateTime? fechaAlta;
         private string idEstadoExpediente;
         private string idEstado;
         private string descEstadoNov;
         private string ingresoDocumentacion;
         private string observacion;
         private string usuarioAlta;
         private string oficinaAlta;
         private string ipAlta;
         private DateTime? fechaProceso;
         private string usuario;
         private string oficina;
         private string ip;
         private DateTime? fechaUltActualizacion;


  #region Get/Set
        
         public long IdNovedad
        {
            get { return this.idNovedad; }
            set { this.idNovedad = value; }
        }

         public string Beneficio
        {
            get { return this.beneficio; }
            set { this.beneficio = value; }
        }

         public string Cuil
        {
            get { return this.cuil; }
            set { this.cuil = value; }
        }

         public string Documento
        {
            get { return this.documento; }
            set { this.documento = value; }
        }
      
         public string TipoDocumento
         {
             get { return this.tipodocumento; }
             set { this.tipodocumento = value; }
         }

         public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

         public TipoConcepto UnTipoConcepto
        {
            get{ return unTipoConcepto;}
            set { unTipoConcepto = value; }
        }

         public double ImporteTotal
        {
            get{ return importeTotal;}
            set { importeTotal = value; }
        }

         public Byte CantidadCuotas
        {
            get{ return cantidadCuotas;}
            set { cantidadCuotas = value; }
        }
        
         public DateTime FechaNovedad
        {
            get { return this.fechaNovedad; }
            set { this.fechaNovedad = value; }
        }

         public DateTime FechaPresentacion
        {
            get { return this.fechaPresentacion; }
            set { this.fechaPresentacion = value; }
        }
        
         public string Entidad
         {
             get { return this.entidad; }
             set { this.entidad = value; }
         }

         public string CBU
         {
             get { return this.cbu; }
             set { this.cbu = value; }
         }

         public ConceptoLiquidacion UnConceptoLiquidacion
         {
             get { return unConceptoLiquidacion; }
             set { unConceptoLiquidacion = value; }
         }
         public string NroComprobante
         {
             get { return this.nroComprobante; }
             set { this.nroComprobante = value; }
         }
         public string IdEstadoReg
         {
             get { return this.idEstadoReg; }
             set { this.idEstadoReg = value; }
         }
         public string IdItem
         {
             get { return this.idItem; }
             set { this.idItem = value; }
         }
         public string DescripcionItem
         {
             get { return this.descripcionItem; }
             set { this.descripcionItem = value; }
         }
         public string ModoPago
         {
             get { return this.modoPago; }
             set { this.modoPago = value; }
         }
         public string EstaCartulado
         {
             get { return this.estaCartulado; }
             set { this.estaCartulado = value; }
         }
         public string NroExpediente {
             get { return this.nroExpediente; }
             set { this.nroExpediente = value; }
         }
         public DateTime? FechaRecepcion
         {
             get { return this.fechaRecepcion; }
             set { this.fechaRecepcion = value; }
         }
         public DateTime? FechaAlta
         {
             get { return this.fechaAlta; }
             set { this.fechaAlta = value; }
         }
         public string IdEstadoExpediente
         {
             get { return this.idEstadoExpediente; }
             set { this.idEstadoExpediente = value; }
         }
         public string IdEstado
         {
             get { return this.idEstado; }
             set { this.idEstado = value; }
         }
         public string DescEstadoNov
         {
             get { return this.descEstadoNov; }
             set { this.descEstadoNov = value; }
         }
         public string IngresoDocumentacion
         {
             get { return this.ingresoDocumentacion; }
             set { this.ingresoDocumentacion = value; }
         }
         public string Observacion
         {
             get { return this.observacion; }
             set { this.observacion = value; }
         }
         public string UsuarioAlta
         {
             get { return this.usuarioAlta; }
             set { this.usuarioAlta = value; }
         }
         public string OficinaAlta
         {
             get { return this.oficinaAlta; }
             set { this.oficinaAlta = value; }
         }
         public string IpAlta
         {
             get { return this.ipAlta; }
             set { this.ipAlta = value; }
         }
         public DateTime? FechaProceso
         {
             get { return this.fechaProceso; }
             set { this.fechaProceso = value; }
         }
         public string Usuario
         {
             get { return this.usuario; }
             set { this.usuario = value; }
         }
         public string Oficina
         {
             get { return this.oficina; }
             set { this.oficina = value; }
         }
         public string Ip
         {
             get { return this.ip; }
             set { this.ip = value; }
         }
         public DateTime? FechaUltActualizacion
         {
             get { return this.fechaUltActualizacion; }
             set { this.fechaUltActualizacion = value; }
         }
        
#endregion

    
    public Caratulacion() 
        {
            IdNovedad = 0;
            beneficio = string.Empty; 
            cuil = string.Empty;
            documento = string.Empty;
            tipodocumento = string.Empty; 
            nombre = string.Empty;
            unTipoConcepto = new TipoConcepto();
            importeTotal = 0;
            cantidadCuotas= 0;
            fechaNovedad = new DateTime();
            fechaPresentacion = new DateTime();
            entidad= string.Empty;
            cbu = string.Empty;
            unConceptoLiquidacion = new ConceptoLiquidacion();
            nroComprobante= string.Empty;
            idEstadoReg= string.Empty;
            idItem= string.Empty;
            descripcionItem= string.Empty;
            modoPago= string.Empty;
            estaCartulado= string.Empty;
            nroExpediente = string.Empty;
            fechaRecepcion= new DateTime();
            fechaAlta=new DateTime();
            idEstadoExpediente = string.Empty;
            idEstado = string.Empty;
            ingresoDocumentacion= string.Empty;
            observacion= string.Empty;
            usuarioAlta= string.Empty;
            oficinaAlta= string.Empty;
            ipAlta= string.Empty;
            fechaProceso= new DateTime();
            usuario = string.Empty;
            oficina= string.Empty; 
            ip= string.Empty;
            fechaUltActualizacion= new DateTime();
          }
     }

}
