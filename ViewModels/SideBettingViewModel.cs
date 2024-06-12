using MatchBetting.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MatchBetting.ViewModels
{
    public class SideBettingViewModel(Models.SideBet sideBetting)
    {
        public int Id { get; set; } = sideBetting.Id;

        public string WinnerTeam { get; set; } = sideBetting.WinnerTeam;

        public string Toppscorer { get; set; } = sideBetting.Toppscorer;

        public string MostCards { get; set; } = sideBetting.MostCards;

        public string UserId { get; set; } = sideBetting.UserId;
    }
    public class SideBettingMinViewModel
    {

        public string WinnerTeam { get; set; }

        public string Toppscorer { get; set; }

        public string MostCards { get; set; }
    }
}
