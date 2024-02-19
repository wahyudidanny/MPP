using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPP.API.Entities
{
    public class T_MsMPP
    {
        public Int64? id { get; set; }
        public string? BusinessUnit { get; set; }
        public int? totalBdgt { get; set; }
        public int? totalAktual { get; set; }
        public int? varianceOver { get; set; }
        public string? MasaBerlaku { get; set; }
        public int? EmailLevel { get; set; }
    }
}

