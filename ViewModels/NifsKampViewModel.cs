﻿using MatchBetting.NifsModels;

namespace MatchBetting.ViewModels
{
    public class NifsKampViewModel
    {
        public string homeTeam { get; set; }
        public string awayTeam { get; set; }
        public string result { get; set; }
        public string stadium { get; set; }

        public int round { get; set; }
        public DateTime date { get; set; }
        public string gruppe { get; set; }
        public int id { get; set; }

        public NifsKampViewModel(NifsKampModel match, TournamentViewModel model)
        {
            this.homeTeam = match.homeTeam.name;
            this.awayTeam = match.awayTeam.name;
            this.result = match.result.homeScore90 + " - " + match.result.awayScore90;
            this.stadium = match.stadium.name;
            round = match.round;
            date = match.timestamp;
            gruppe = model.gruppenamn;
            id = match.id;
        }

        public NifsKampViewModel(NifsKampModel match)
        {
            this.homeTeam = match.homeTeam.name;
            this.awayTeam = match.awayTeam.name;
            this.result = match.result.homeScore90 + " - " + match.result.awayScore90;
            this.stadium = match.stadium.name;
            round = match.round;
            date = match.timestamp;
            id = match.id;
        }
    }
    
}