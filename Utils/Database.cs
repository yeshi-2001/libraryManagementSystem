using libraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace libraryManagementSystem.Utils
{

    public static class Database
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;

        /// <summary>
        /// Executes a SELECT (stored procedure or raw query) and returns the
        /// result as a DataTable. Used by GetAll()/GetById()/Search() in every repository.
        /// </summary>
        public static DataTable ExecuteQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
                catch (SqlException ex)
                {

                    throw new ApplicationException("A database error occurred while executing the query.", ex);
                }
            }
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ApplicationException("A database error occurred while executing the command.", ex);
                }
            }
        }

        public static int ExecuteInsertAndGetId(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);
                }
                catch (SqlException ex)
                {
                    throw new ApplicationException("A database error occurred while inserting the record.", ex);
                }
            }
        }

        public static object ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    connection.Open();
                    return command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    throw new ApplicationException("A database error occurred while executing the query.", ex);
                }
            }
        }

       
        public static void ExecuteTransaction(List<Action<SqlConnection, SqlTransaction>> actions)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var action in actions)
                        {
                            action(connection, transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("The transaction failed and was rolled back.", ex);
                    }
                }
            }
        }

        /// <summary>Convenience wrapper for building SqlParameters — keeps calling code compact.</summary>
        public static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }
    }
}