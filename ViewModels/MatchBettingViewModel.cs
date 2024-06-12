using MatchBetting.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MatchBetting.ViewModels
{
    public class MatchBettingViewModel(Models.MatchBetting matchBetting)
    {
        public int Id { get; set; } = matchBetting.Id;

        public int MatchId { get; set; } = matchBetting.MatchId;

        public string Result { get; set; } = matchBetting.Result;

        public string UserId { get; set; } = matchBetting.UserId;
    }
}
