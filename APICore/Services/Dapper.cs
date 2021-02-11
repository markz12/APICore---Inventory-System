using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICore.Services;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace APICore.Services
{
    public class Dapper: IDapper
    {
        private string ConnectionString = "default";
        private readonly IConfiguration _config;
        public Dapper(IConfiguration config)
        {
            _config = config;
        }

        public int Execute(string query,DynamicParameters param, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public T Get<T>(string query, DynamicParameters param, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
                return db.Query<T>(query, param, commandType: commandType).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<T> GetAll<T>(string query,DynamicParameters param, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
            return db.Query<T>(query, param, commandType: commandType).ToList();
        }

        public T GeneralCrud<T>(string query, DynamicParameters param, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed) db.Open();
                using var transaction = db.BeginTransaction();
                try
                {
                     db.Execute(query, param, commandType: commandType, transaction: transaction);
                    result = param.Get<T>("return");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open) db.Close();
            }
            return result;
        }
    }
}
