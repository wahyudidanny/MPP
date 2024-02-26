
using Microsoft.EntityFrameworkCore;
using MPP.API.Entities;

namespace MPP.API.Repository
{
    public class MPPRepository
    {
		private readonly MPPDBContext _context;
		public MPPRepository(MPPDBContext context)
		{
			_context = context;
		}


          public async Task<IEnumerable<T_MsMPPDetail?>> getAllDataDetailApprovalMPP(string company, string location,string tahun, string bulan)
        {

            List<T_MsMPPDetail> result = new List<T_MsMPPDetail>();

            try
            {
                _context.Database.SetCommandTimeout(300);

                result = await _context.Set<T_MsMPPDetail>().FromSqlRaw("EXECUTE Get_Data_ApprovalMPP_Email_Detail '" + company + "','"+ location+"','" + tahun +"','" + bulan + "'").ToListAsync();

                return result;

            }
            catch(Exception ex)
            {
                return result;
            }

        }


        public async Task<IEnumerable<T_MsMPP?>> getAllDataApprovalMPP(string company, string location,string tahun, string bulan)
        {

            List<T_MsMPP> result = new List<T_MsMPP>();

            try
            {
                _context.Database.SetCommandTimeout(300);

                result = await _context.Set<T_MsMPP>().FromSqlRaw("EXECUTE Get_Data_ApprovalMPP_Email '" + company + "','"+ location+"','" + tahun +"','" + bulan + "'").ToListAsync();

                return result;

            }
            catch(Exception ex)
            {
                return result;
            }

        }

    }
}