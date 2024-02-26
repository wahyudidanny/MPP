
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MPP.API.Entities;
using Microsoft.Extensions.Options;
using System.Data;

namespace MPP.API.Repository
{
    public class MPPRepository
    {
        private readonly string? _connectionString;
        private readonly string? _connectionString2;
        private readonly string? _connectionString3;
        private readonly string? _connectionString4;
        private readonly string? _connectionString5;
        private readonly string? _connectionString6;
        private readonly string? _connectionString7;

        public MPPRepository(IOptions<ConnectionStrings> connectionStrings)
        {

            _connectionString = connectionStrings.Value.DbConnectionString;
            _connectionString2 = connectionStrings.Value.DbConnectionString2;
            _connectionString3 = connectionStrings.Value.DbConnectionString3;
            _connectionString4 = connectionStrings.Value.DbConnectionString4;
            _connectionString5 = connectionStrings.Value.DbConnectionString5;
            _connectionString6 = connectionStrings.Value.DbConnectionString6;
            _connectionString7 = connectionStrings.Value.DbConnectionString7;

        }

        public string getConnectionString(string kodeRegion)
        {
            string connectionStrings = string.Empty;

            if (kodeRegion == "PKU")
            {
                connectionStrings = _connectionString;

            }
            // else if (kodeRegion == "PKU")
            // {
            //     connectionStrings = _connectionString;

            // }else if (kodeRegion == "PKU")
            // {
            //     connectionStrings = _connectionString;

            // }else if (kodeRegion == "PKU")
            // {
            //     connectionStrings = _connectionString;

            // }else if (kodeRegion == "PKU")
            // {
            //     connectionStrings = _connectionString;

            // } else if (kodeRegion == "PKU")
            // {
            //     connectionStrings = _connectionString;

            // }  else if (kodeRegion == "PKU")
            // {
            //     connectionStrings = _connectionString;
            // }   


            return connectionStrings;
        }

        public async Task<IEnumerable<T_MsMPPDetail?>> getAllDataDetailApprovalMPP(string company, string location, string kodeRegion, string tahun, string bulan)
        {

            List<T_MsMPPDetail> result = new List<T_MsMPPDetail>();
            string connectionStrings = getConnectionString(kodeRegion);

            using (var connection = new SqlConnection(connectionStrings))
            {

                try
                {

                    await connection.OpenAsync();
                    using (var command = new SqlCommand("Get_Data_ApprovalMPP_Email_Detail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Company", company);
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@Year", tahun);
                        command.Parameters.AddWithValue("@Month", bulan);
                        command.CommandTimeout = 180;

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                var item = new T_MsMPPDetail
                                {
                                    noPengajuan = reader.IsDBNull(reader.GetOrdinal("noPengajuan")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("noPengajuan")),
                                    company = reader.IsDBNull(reader.GetOrdinal("company")) ? null : reader.GetString(reader.GetOrdinal("company")),
                                    location = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location")),
                                    TahunPriode = reader.IsDBNull(reader.GetOrdinal("TahunPriode")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TahunPriode")),
                                    BulanPriodeTo = reader.IsDBNull(reader.GetOrdinal("BulanPriodeTo")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("BulanPriodeTo")),
                                    BulanPriodeName = reader.IsDBNull(reader.GetOrdinal("BulanPriodeName")) ? null : reader.GetString(reader.GetOrdinal("BulanPriodeName")),
                                    state = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                                    FinalState = reader.IsDBNull(reader.GetOrdinal("FinalState")) ? null : reader.GetString(reader.GetOrdinal("FinalState")),
                                };

                                result.Add(item);
                            }
                        }

                    }

                    return result;

                }
                catch (Exception ex)
                {

                    if (connection.State != ConnectionState.Closed)
                        connection.Close();

                    return result;
                }

            }

        }


        public async Task<IEnumerable<T_MsMPP?>> getAllDataApprovalMPP(string company, string location, string kodeRegion, string tahun, string bulan)
        {

            List<T_MsMPP> result = new List<T_MsMPP>();
            string connectionStrings = string.Empty;

            if (kodeRegion == "PKU")
            {
                connectionStrings = _connectionString;
            }

            using (var connection = new SqlConnection(connectionStrings))
            {

                try
                {

                    await connection.OpenAsync();
                    using (var command = new SqlCommand("Get_Data_ApprovalMPP_Email", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Company", company);
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@Year", tahun);
                        command.Parameters.AddWithValue("@Month", bulan);
                        command.CommandTimeout = 180;

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                var item = new T_MsMPP
                                {

                                    noPengajuan = reader.IsDBNull(reader.GetOrdinal("noPengajuan")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("noPengajuan")),
                                    BusinessUnit = reader.IsDBNull(reader.GetOrdinal("BusinessUnit")) ? null : reader.GetString(reader.GetOrdinal("BusinessUnit")),
                                    totalCOO = reader.IsDBNull(reader.GetOrdinal("totalCOO")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("totalCOO")),
                                    totalBdgt = reader.IsDBNull(reader.GetOrdinal("totalBdgt")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("totalBdgt")),
                                    totalAktual = reader.IsDBNull(reader.GetOrdinal("totalAktual")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("totalAktual")),
                                    varianceBudget = reader.IsDBNull(reader.GetOrdinal("varianceBudget")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("varianceBudget")),
                                    varianceAktual = reader.IsDBNull(reader.GetOrdinal("varianceAktual")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("varianceAktual")),
                                    MasaBerlakuTo = reader.IsDBNull(reader.GetOrdinal("MasaBerlakuTo")) ? null : reader.GetString(reader.GetOrdinal("MasaBerlakuTo")),
                                    emailLevel = reader.IsDBNull(reader.GetOrdinal("emailLevel")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("emailLevel")),
                                    state = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                                    FinalState = reader.IsDBNull(reader.GetOrdinal("FinalState")) ? null : reader.GetString(reader.GetOrdinal("FinalState")),
                                    catatan = reader.IsDBNull(reader.GetOrdinal("catatan")) ? null : reader.GetString(reader.GetOrdinal("catatan")),
                                    masaBerlakuInt = reader.IsDBNull(reader.GetOrdinal("masaBerlakuInt")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("masaBerlakuInt")),
                                };

                                result.Add(item);
                            }
                        }

                    }

                    return result;

                }
                catch (Exception ex)
                {

                    if (connection.State != ConnectionState.Closed)
                        connection.Close();

                    return result;
                }

            }
        }
    }
}