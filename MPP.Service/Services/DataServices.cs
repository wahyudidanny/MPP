
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
        private readonly string connectionString2;
        private readonly string connectionString3;
        private readonly string connectionString4;
        private readonly string connectionString5;
        private readonly string connectionString6;
        private readonly string connectionString7;

        public DataServices(IOptions<ConnectionStrings> connectionStrings)
        {
            connectionString1 = connectionStrings.Value.DbConnectionString;
            connectionString2 = connectionStrings.Value.DbConnectionString2;
            connectionString3 = connectionStrings.Value.DbConnectionString3;
            connectionString4 = connectionStrings.Value.DbConnectionString4;
            connectionString5 = connectionStrings.Value.DbConnectionString5;
            connectionString6 = connectionStrings.Value.DbConnectionString6;
            connectionString7 = connectionStrings.Value.DbConnectionString7;
        }

        public IEnumerable<T_MsBusinessUnit> GetDataBusinessUnit()
        {
            List<T_MsBusinessUnit> result = new List<T_MsBusinessUnit>();

            // using (var connection = new SqlConnection(connectionString1))
            // {
            //     var data = connection.Query<T_MsBusinessUnit>("select Company,Location,Description,'PTK' RegionCode, 'FR' KodeGroup from T_MsBusinessUnit where Active = 1 and Location like '2%' and RegionCode = 'PTK' and KodeGroup = 'FR'");
            //     if (data.Count() > 0)
            //     {
            //         result.AddRange(data);
            //     }
            // }

            // using (var connection = new SqlConnection(connectionString2))
            // {
            //     var data = connection.Query<T_MsBusinessUnit>("select Company,Location,Description,'PTK' RegionCode, 'KAS' KodeGroup from T_MsBusinessUnit where Active = 1 and Location like '2%' and RegionCode = 'PTK' and KodeGroup = 'KAS'");
            //     if (data.Count() > 0)
            //     {
            //         result.AddRange(data);
            //     }
            // }

            // using (var connection = new SqlConnection(connectionString3))
            // {
            //     string query = @"
            //         select Company,Location,Description, RegionCode, KodeGroup from T_MsBusinessUnit where Active = 1 and Location like '2%' and RegionCode = 'NNK' and KodeGroup = 'FAP'
            //         and Company+Location not in ('C0126','C0127')";
            //     var data = connection.Query<T_MsBusinessUnit>(query);
            //     if (data.Count() > 0)
            //     {
            //         result.AddRange(data);
            //     }
            // }

            // using (var connection = new SqlConnection(connectionString4))
            // {
            //     string query = @"
            //         select Company,Location,Description, RegionCode, KodeGroup from T_MsBusinessUnit where Active = 1 and Location like '2%' and RegionCode = 'BPN' and KodeGroup = 'FR'
            //         and Company+Location not in ('C1221','C1321','A1422')";
            //     var data = connection.Query<T_MsBusinessUnit>(query);
            //     if (data.Count() > 0)
            //     {
            //         result.AddRange(data);
            //     }
            // }

            // using (var connection = new SqlConnection(connectionString5))
            // {
            //     string query = @"
            //         select Company,Location,Description, RegionCode, KodeGroup from T_MsBusinessUnit where Active = 1 and Location like '2%' and RegionCode = 'BPN' and KodeGroup = 'FAP'
            //         and Company+Location not in ('C1921','C1922')";
            //     var data = connection.Query<T_MsBusinessUnit>(query);
            //     if (data.Count() > 0)
            //     {
            //         result.AddRange(data);
            //     }
            // }

            // using (var connection = new SqlConnection(connectionString6))
            // {
            //     string query = @"
            //         select Company,Location,Description, RegionCode, KodeGroup from T_MsBusinessUnit where Active = 1 and Location like '2%' and RegionCode = 'BPN' and KodeGroup = 'KAS'
            //         and Company+Location not in ('C2821')";
            //     var data = connection.Query<T_MsBusinessUnit>(query);
            //     if (data.Count() > 0)
            //     {
            //         result.AddRange(data);
            //     }
            // }

            using (var connection = new SqlConnection(connectionString7))
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