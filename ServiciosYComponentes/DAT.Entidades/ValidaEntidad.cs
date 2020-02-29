using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class ValidaEntidad<T>
    {
        public string ValidateRuleSetOutString()
        {
            string validacionConfig = ConfigurationManager.AppSettings["validacionConfig"].ToString();
            string defaultRuleSet = ConfigurationManager.AppSettings["defaultRuleSet"].ToString();
            FileConfigurationSource source = new FileConfigurationSource(validacionConfig);
            Validator oValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(T), defaultRuleSet, source);

            ValidationResults oValidationResults = oValidator.Validate(this);
            string strError = string.Empty;

            if (!oValidationResults.IsValid)
            {
                foreach (ValidationResult result in oValidationResults)
                {
                    strError += result.Message + " ";
                }
            }

            return strError;
        }

        public List<Error> ValidateRuleSet()
        {
            string validacionConfig = ConfigurationManager.AppSettings["validacionConfig"].ToString();
            string defaultRuleSet = ConfigurationManager.AppSettings["defaultRuleSet"].ToString();
            FileConfigurationSource source = new FileConfigurationSource(validacionConfig);
            Validator oValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(T), defaultRuleSet, source);

            ValidationResults oValidationResults = oValidator.Validate(this);
            List<Error> listError = new List<Error>();

            if (!oValidationResults.IsValid)
            {
                foreach (ValidationResult result in oValidationResults)
                {
                    Error oError = new Error();
                    oError.Codigo = result.Key;
                    oError.Clase = typeof(T).Name;
                    oError.Descripcion = result.Message;
                    oError.DescripcionConcatenada += result.Message + " ";
                    listError.Add(oError);
                }
                //"Incosistencia en Objeto Notificacion - Error/es : ";
            }

            return listError;
        }

        public List<Error> ValidateRuleSet<O>(string ruleSet)
        {
            string validacionConfig = ConfigurationManager.AppSettings["validacionConfig"].ToString();
            IConfigurationSource source = new FileConfigurationSource(validacionConfig);

            Validator oValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(O), ruleSet, source);
            ValidationResults oValidationResults = oValidator.Validate(this);
            List<Error> listError = new List<Error>();

            if (!oValidationResults.IsValid)
            {
                foreach (ValidationResult result in oValidationResults)
                {
                    Error oError = new Error();
                    oError.Codigo = result.Key;
                    oError.Clase = this.GetType().Name;
                    oError.Descripcion = result.Message;
                    oError.DescripcionConcatenada += result.Message + " ";

                    listError.Add(oError);
                }
                //"Incosistencia en Objeto Notificacion - Error/es : ";
            }

            return listError;
        }

        public List<Error> ValidateRuleSet(object obj, string ruleSet)
        {
            string validacionConfig = ConfigurationManager.AppSettings["validacionConfig"].ToString();
            IConfigurationSource source = new FileConfigurationSource(validacionConfig);
            
            Validator oValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(T), ruleSet, source);
            ValidationResults oValidationResults = oValidator.Validate(obj);
            List<Error> listError = new List<Error>();

            if (!oValidationResults.IsValid)
            {
                foreach (ValidationResult result in oValidationResults)
                {
                    Error oError = new Error();
                    oError.Codigo = result.Key;
                    oError.Clase = this.GetType().Name;
                    oError.Descripcion = result.Message;
                    oError.DescripcionConcatenada += result.Message + " ";

                    listError.Add(oError);
                }
                //"Incosistencia en Objeto Notificacion - Error/es : ";
            }

            return listError;
        }

        #region ValidateRuleSet - Comentado

        //public List<Error> ValidateRuleSet(string ruleSet, params object[] objParametros)
        //{
        //    string validacionConfig = ConfigurationManager.AppSettings["validacionConfig"].ToString();
        //    FileConfigurationSource source = new FileConfigurationSource(validacionConfig);

        //    int nroProperty = 1;            
        //    DataTable dt1 = new DataTable();
        //    DataTable dt2 = new DataTable();

        //    foreach (var item in objParametros)
        //    {                
        //        dt1.Columns.Add("Propiedad"+nroProperty , item.GetType());
        //        dt1.Rows.Add(nroProperty, item, "Propiedad" + nroProperty);                
        //        nroProperty ++;
        //        dt2.Columns.Add("Propiedad" + nroProperty, item.GetType());
        //        dt2.Rows.Add(nroProperty, item, "Propiedad" + nroProperty);

        //        //IQueryable outerTable = dt1.AsEnumerable().AsQueryable(); 
        //        //System.Collections.IEnumerable innerTable = dt2.AsEnumerable();                                                
        //    }

        //    //var query = objParametros.AsEnumerable().Join(objParametros.GetEnumerator(),
        //    //                           "new(get_Item(0) as FundId, get_Item(1) as Date)",
        //    //                           "new(get_Item(0) as FundId, get_Item(1) as Date)",
        //    //                           "new(outer.get_Item(0) as FundId, outer.get_Item(2) as CodeA, inner.get_Item(2) as CodeB)");
        //    //var obj = from obj1 in dt1
        //    //          join obj2 in dt2 on o  
        //    //          select new {dt1.Columns[0].ColumnName};

        //    //var obj = from p in objParametros
        //    //          select new { objParametros }.objParametros;


        //    Validator oValidator = ValidationFactory.CreateValidatorFromConfiguration(typeof(T), ruleSet, source);
        //    ValidationResults oValidationResults = oValidator.Validate(this);
        //    List<Error> listError = new List<Error>();

        //    if (!oValidationResults.IsValid)
        //    {
        //        foreach (ValidationResult result in oValidationResults)
        //        {
        //            Error oError = new Error();
        //            oError.Codigo = result.Key;
        //            oError.Clase = typeof(T).Name;
        //            oError.Descripcion = result.Message;
        //            oError.DescripcionConcatenada += result.Message + " ";

        //            listError.Add(oError);
        //        }
        //        //"Incosistencia en Objeto Notificacion - Error/es : ";
        //    }

        //    return listError;
        //}
        #endregion
    }
}
