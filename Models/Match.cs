using MatchBetting.NifsModels;
using System.ComponentModel.DataAnnotations;

namespace MatchBetting.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public int MatchId { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string Result { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
