using libraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace libraryManagementSystem.Utils
{
    /// <summary>
    /// Central ADO.NET helper. Every Repository goes through this class to talk to
    /// SQL Server — this is the ONLY place connection strings and SqlConnection/
    /// SqlCommand objects are created, which keeps connection handling and
    /// exception handling consistent everywhere and avoids duplicated ADO.NET
    /// boilerplate across the 4 repositories.
    ///
    /// All methods use parameterized commands (SqlParameter[]) — callers must
    /// NEVER concatenate user input into SQL text, which is how this class
    /// prevents SQL injection across the whole app.
    /// </summary>
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
                    // Wrap in a generic exception so callers/UI never need to know
                    // this layer uses SqlClient specifically — keeps Repository/Service
                    // layers decoupled from the ADO.NET provider.
                    throw new ApplicationException("A database error occurred while executing the query.", ex);
                }
            }
        }

        /// <summary>
        /// Executes an INSERT/UPDATE/DELETE (stored procedure or raw query) and
        /// returns the number of rows affected. Used by Insert()/Update()/Delete().
        /// </summary>
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

        /// <summary>
        /// Executes an INSERT and returns the newly generated identity value
        /// (e.g. the new BookId). Stored procedures should end with
        /// "SELECT SCOPE_IDENTITY();" for this to work.
        /// </summary>
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

        /// <summary>
        /// Executes a query that returns a single scalar value (e.g. COUNT(*) for
        /// Dashboard cards or pagination totals).
        /// </summary>
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

        /// <summary>
        /// Runs multiple commands inside a single transaction. Used by BorrowService/
        /// ReturnService, where a borrow/return must update BOTH BorrowRecords AND
        /// Books.AvailableQuantity atomically — if either fails, both roll back so
        /// stock counts never drift out of sync with actual transactions.
        /// </summary>
        /// <param name="actions">
        /// A list of actions, each given an open SqlConnection + SqlTransaction to
        /// execute its command against.
        /// </param>
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