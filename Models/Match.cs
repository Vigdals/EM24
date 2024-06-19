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
        public int? HomeScore90 { get; set; }
        public int? AwayScore90 { get; set; }
        public string Result { get; set; }
        public DateTime Timestamp { get; set; }
        public int MatchStatusId { get; set; }
        public string MatchStatus { get; set; }
        public string HomeTeamLogoUrl { get; set; }
        public string AwayTeamLogoUrl { get;set; }
    }
}
