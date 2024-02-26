using Microsoft.EntityFrameworkCore;

namespace MPP.API.Entities
{
     [Keyless]
    public class T_MsMPPDetail
    {
        public int? noPengajuan { get; set; }
        public string? company { get; set; }
        public string? location { get; set; }
        public int? TahunPriode { get; set; }
        public int? BulanPriodeTo { get; set; }
        public string? BulanPriodeName { get; set; }
        public string? state { get; set; }
        public string? FinalState { get; set; }

    }
}

