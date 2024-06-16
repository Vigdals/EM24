﻿using MatchBetting.Models;

namespace MatchBetting.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeScore90 { get; set; }
        public int? AwayScore90 { get; set; }
        public string Result { get; set; }
        public DateTime Timestamp { get; set; }

        public MatchViewModel(Match match)
        {
            Id = match.Id;
            MatchId = match.MatchId;
            HomeTeam = Euro2024Teams.HentForkortelse(match.HomeTeam);
            AwayTeam = Euro2024Teams.HentForkortelse(match.AwayTeam);
            HomeScore90 = match.HomeScore90;
            AwayScore90 = match.AwayScore90;
            Result = match.Result;
            Timestamp = match.Timestamp;
        }

        
    }
}
