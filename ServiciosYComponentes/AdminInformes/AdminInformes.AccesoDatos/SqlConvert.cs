using System;


namespace AdminInformes.AccesoDatos
{
    public class SqlConvert
    {
        public static T Convert<T>(object obj)
        {
            Type tipoDato = typeof(T);
            object respuesta = obj;
            if (tipoDato == typeof(int))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = 0;
                }
                else
                {
                    respuesta = int.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(int?))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = (int?)null;
                }
                else
                {
                    respuesta = int.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(short))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = 0;
                }
                else
                {
                    respuesta = short.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(short?))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = (short?)null;
                }
                else
                {
                    respuesta = short.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(long))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = 0;
                }
                else
                {
                    respuesta = long.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(long?))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = (long?)null;
                }
                else
                {
                    respuesta = long.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(char))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = char.Parse("");
                }
                else
                {
                    respuesta = char.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(char?))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = (char?)null;
                }
                else
                {
                    respuesta = char.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(bool))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = false;
                }
                else
                {
                    respuesta = bool.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(bool?))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = (bool?)null;
                }
                else
                {
                    respuesta = bool.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(string))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = obj == System.DBNull.Value ? null : "";
                }
                else
                {
                    respuesta = obj.ToString();
                }
            }
            else if (tipoDato == typeof(DateTime))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = new DateTime();
                }
                else
                {
                    respuesta = DateTime.Parse(obj.ToString());
                }
            }
            else if (tipoDato == typeof(DateTime?))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    respuesta = (DateTime?)null;
                }
                else
                {
                    respuesta = DateTime.Parse(obj.ToString());
                }
            }
            return (T)respuesta;
        }
    }
}