using MatchBetting.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MatchBetting.ViewModels
{
    public class MatchBettingViewModel
    {

        public MatchBettingViewModel()
        {
        }

        public MatchBettingViewModel(MatchViewModel match, string userId)
        {
            Id = match.Id;
            MatchId = match.MatchId;
            Result = string.Empty;
            UserId = userId;
        }

        public MatchBettingViewModel(Models.MatchBetting matchBetting)
        {
            Id = matchBetting.Id;
            MatchId = matchBetting.MatchId;
            Result = matchBetting.Result;
            UserId = matchBetting.UserId;
        }

        public int Id { get; set; }
        public int MatchId { get; set; }
        public string Result { get; set; }
        public string UserId { get; set; }
    }
}
