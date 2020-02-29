using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using log4net;

/// <summary>
/// Summary description for ExtensionMethods
/// </summary>
public static class ExtensionMethods
{

    private static readonly ILog log = LogManager.GetLogger(typeof(ExtensionMethods).Name);


    public static void AddToFront<T>(this List<T> list,  Predicate<T> filter)
    {
        try
        {
            var item = list.Find(filter);
            list.Remove(item);
            list.Insert(0, item);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, "AddToFront", ex.Source, ex.Message));
            throw;
        } 
        
    }

}