using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace APICore.Services
{
   public interface IDapper : IDisposable
    {
        T Get<T>(string query, DynamicParameters param, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string query, DynamicParameters param, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string query, DynamicParameters param, CommandType commandType = CommandType.StoredProcedure);
        T GeneralCrud<T>(string query, DynamicParameters param, CommandType commandType = CommandType.StoredProcedure);
    }
}
