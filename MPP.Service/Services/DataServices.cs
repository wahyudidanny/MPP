
using MPP.Service.Models;
using MPP.Service.Interface;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using Dapper;

namespace MPP.Service.Services
{
    public class DataServices : IDataService
    {
        private readonly string connectionString1;

        public DataServices(IOptions<ConnectionStrings> connectionStrings)
        {
            connectionString1 = connectionStrings.Value.DbConnectionString;
        }

        public IEnumerable<T_MsBusinessUnit> GetDataBusinessUnit()
        {
            List<T_MsBusinessUnit> result = new List<T_MsBusinessUnit>();

            using (var connection = new SqlConnection(connectionString1))
            {
                string query = @"
                    select Company,Location,Description, RegionCode, case when KodeGroup = 'FRG' then 'FR' else KodeGroup end KodeGroup  from T_MsBusinessUnit where Active = 1 and Location like '2%' 
                    and Company+Location not in ('A1022','A1121','A1422','A1423','A2521','0122')";

                var data = connection.Query<T_MsBusinessUnit>(query);
                if (data.Count() > 0)
                {
                    result.AddRange(data);
                }
 
            }
            return result;

        }

    }
}