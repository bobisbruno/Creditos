using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class DocumentoDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecuperoDao).Name);


        /// <summary>
        /// metodo encargado de listar los tipos de documento, si se parametriza con null lista todos
        /// </summary>
        /// <param name="id">representa el id</param>
        /// <returns></returns>
        public List<Documento> EjecutarDocumento_T(Nullable<int> id)
        {

            SqlCommand command = null;
            SqlDataReader reader = null;
            var documentos = new List<Documento>();

            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "Documento_T";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    documentos.Add(parseToDocumento(reader));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw;
            }
            finally
            {
                AdministradorDeConexion.CerrarConexion();
            }
            return documentos;
        
        }

        private Documento parseToDocumento(SqlDataReader reader)
        {
            return new Documento(
                (int)reader["idDoc"],
                reader["Descripcion"] == null ? string.Empty : (string)reader["Descripcion"] ,
                reader["CODALFA"] == null ? string.Empty : (string)reader["CODALFA"]
                 );
        }
    }
}
