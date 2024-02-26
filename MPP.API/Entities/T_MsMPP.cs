using Microsoft.EntityFrameworkCore;

namespace MPP.API.Entities
{
    
    [Keyless]
    public class T_MsMPP
    {
        public int? noPengajuan { get; set; }
        public string? BusinessUnit { get; set; }
        public int? totalCOO { get; set; }
        public int? totalBdgt { get; set; }
        public int? totalAktual { get; set; }
        public int? varianceBudget { get; set; }
        public int? varianceAktual { get; set; }
        public string? MasaBerlakuTo { get; set; }
        public int? emailLevel { get; set; }
        public string? state { get; set; }
        public string? FinalState { get; set; }
        public string? catatan { get; set; }
        public int? masaBerlakuInt { get; set; }

    }
}

