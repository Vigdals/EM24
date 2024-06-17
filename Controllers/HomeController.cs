﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using MatchBetting.NifsModels;
using MatchBetting.Utils;
using MatchBetting.ViewModels;
using System.Security.Claims;
using MatchBetting.Data;
using MatchBetting.Models;
using MatchBetting.Service;
using static MatchBetting.NifsModels.MatchModel;
using Result = MatchBetting.NifsModels.Result;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MatchBetting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;

        public HomeController(ApplicationDbContext context, ILogService logservice)
        {
            _context = context;
            _logService = logservice;

        }

        [Authorize]
        public IActionResult Index()
        {
            var tournamentID = "59";

            //Henter ut info om heile turneringa. Kvar gruppe har ein ID som treng eit api kall for å henta alle kampar
            var TournamentViewModelList = GetTournamentInfo(tournamentID);

            //Oppretta tom liste av kampar
            var matchViewModelList = new List<NifsKampViewModel>();

            //Går igjennom kvar gruppe og hentar ut alle kampar
            foreach (var tournamentViewModel in TournamentViewModelList)
            {
                var matchModels = GetKampInfo("https://api.nifs.no/stages/" + tournamentViewModel.id + "/matches/");

                //adding info just to display to the view
                foreach (var match in matchModels.Result)
                {
                    matchViewModelList.Add(new NifsKampViewModel(match, tournamentViewModel));

                    AddOrUpdateMatchInDatabase(match);

                    Debug.WriteLine(tournamentViewModel.gruppenamn + match.homeTeam.name + " + " + match.awayTeam.name);
                }
                //return View(matchViewModelList);
            }

            try
            {
                _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View(matchViewModelList);
        }
        [Authorize]
        public IActionResult Rules()
        {
            return View();
        }
        [Authorize]
        public IActionResult LeaderBoard()
        {
            var currentMatches = GetMatchesWithinTimeRange();

            var users = _context.Set<IdentityUser>().ToList()
                .Select(user => new LeaderBoardByUserViewModel
                {
                    UserId = user.Id,
                    UserName = Euro2024Users.HentBrukernavn(user.Id),
                    Score = CalculatePoints(user.Id),
                    CurrentBettings = GetCurrentUserCurrentBettings(user.Id, currentMatches)
                }).ToList();

            ViewBag.CurrentMatches = currentMatches;

            return View(users);
        }
        [Authorize]
        public IActionResult Historikk()
        {
            var currentMatches = GetAllMatchesUpToTimeRange();

            var users = _context.Set<IdentityUser>().ToList()
                .Select(user => new LeaderBoardByUserViewModel
                {
                    UserId = user.Id,
                    UserName = Euro2024Users.HentBrukernavn(user.Id),
                    Score = CalculatePoints(user.Id),
                    CurrentBettings = GetCurrentUserCurrentBettings(user.Id, currentMatches)
                }).ToList();

            ViewBag.CurrentMatches = currentMatches;

            return View(users);
        }

        private List<MatchViewModel> GetAllMatchesUpToTimeRange()
        {
            var now = GetServerDateTimeNow();

            // Override for test
            //now = DateTime.Now.Date.AddHours(-5);


            var matches = _context.Matches
                .Where(m => now >= m.Timestamp.AddHours(-2))
                .OrderBy(o => o.Timestamp)
                .ToList();

            return matches.Select(m => new MatchViewModel(m)).ToList();
        }

        public List<MatchViewModel> GetMatchesWithinTimeRange()
        {
            var now = GetServerDateTimeNow();

            // Override for test
            //now = DateTime.Now.Date.AddHours(-5);

            var matches = _context.Matches
                .Where(m => now >= m.Timestamp.AddHours(-2) && now <= m.Timestamp.Date.AddDays(1))
                .OrderBy(o => o.Timestamp)
                .ToList();

            return matches.Select(m => new MatchViewModel(m)).ToList();
        }


        private List<MatchBettingViewModel> GetCurrentUserCurrentBettings(string userId, List<MatchViewModel> currentMatches)
        {
            List<MatchBettingViewModel> matchBettings = new List<MatchBettingViewModel>();

            foreach (var match in currentMatches)
            {
                var betting = _context.MatchBettings.FirstOrDefault(m => m.UserId == userId && m.MatchId == match.MatchId);
                if (betting != null) matchBettings.Add(new MatchBettingViewModel(betting));
                else matchBettings.Add(new MatchBettingViewModel(match, userId) { });
            }

            return matchBettings;

            //return currentMatches.Select(m => new MatchBettingViewModel(_context.MatchBettings.FirstOrDefault(mb => mb.UserId == userId && mb.MatchId == m.MatchId))).ToList();
        }


        private int CalculatePoints(string userId)
        {
            var now = GetServerDateTimeNow();
            var score = 0;
            var bets = _context.MatchBettings.Where(mb => mb.UserId == userId).ToList();
            var matchesWithResults = _context.Matches.Where(m => m.Result != string.Empty && now >= m.Timestamp).ToList();

            foreach (var match in matchesWithResults)
            {
                var bettingOnActualMatch = bets.FirstOrDefault(b => b.MatchId == match.MatchId);
                if (bettingOnActualMatch != null)
                {
                    score += bettingOnActualMatch.Result == match.Result ? 1 : 0;
                }
            }

            return score;
        }

        private DateTime GetServerDateTimeNow()
        {
            var osloTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var utcNow = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, osloTimeZone);
        }


        private async void AddOrUpdateMatchInDatabase(NifsKampModel match)
        {
            try
            {
                //Check if a match exists
                var dbMatch = _context.Matches.FirstOrDefault(m => m.MatchId == match.id);

                if (dbMatch == null)
                {
                    dbMatch = new Match()
                    {
                        AwayTeam = match.awayTeam.name,
                        HomeTeam = match.homeTeam.name,
                        MatchId = match.id,
                        Timestamp = match.timestamp,
                        HomeScore90 = match.result.homeScore90,
                        AwayScore90 = match.result.awayScore90,
                        Result = GetResultFullTime(match.result)
                    };

                    _context.Matches.Add(dbMatch);
                }
                else
                {
                    dbMatch.AwayTeam = match.awayTeam.name;
                    dbMatch.HomeTeam = match.homeTeam.name;
                    dbMatch.Timestamp = match.timestamp;
                    dbMatch.HomeScore90 = match.result.homeScore90;
                    dbMatch.AwayScore90 = match.result.awayScore90;
                    dbMatch.Result = GetResultFullTime(match.result);

                    _context.Matches.Update(dbMatch);
                }

            }
            catch (Exception ex)
            {
                // Log the exception if needed
            }
        }

        private string GetResultFullTime(Result matchResult)
        {
            if (matchResult.homeScore90 > matchResult.awayScore90)
            {
                return "H";
            }
            else if (matchResult.homeScore90 < matchResult.awayScore90)
            {
                return "B";
            }
            else
            {
                return "U";
            }
        }

        public List<TournamentViewModel> GetTournamentInfo(string tournamentID)
        {
            var tournamentModels = GetGruppeInfo("https://api.nifs.no/tournaments/" + tournamentID + "/stages/");

            var TournamentViewModelList = new List<TournamentViewModel>();

            //adding info just to display to the view
            foreach (var gruppe in tournamentModels.Result)
            {
                if (gruppe.yearStart == 2024)
                {
                    TournamentViewModelList.Add(new TournamentViewModel(gruppe));
                }
            }
            return TournamentViewModelList;
        }

        public async Task<List<TournamentModel.Root>> GetGruppeInfo(string apiEndpoint)
        {
            var jsonResult = await ApiCall.DoApiCallAsync(apiEndpoint);

            //The most sexy oneliner in the world!
            //Takes the jsonResult, deserializes it and adds it to my model. Crazy easy
            var tournamentModels = JsonSerializer.Deserialize<List<TournamentModel.Root>>(jsonResult);

            return tournamentModels;
        }

        public async Task<List<NifsKampModel>> GetKampInfo(string apiEndpoint)
        {
            // string apiUrl = "https://api.nifs.no/stages/690256/matches/";
            var jsonResult = await ApiCall.DoApiCallAsync(apiEndpoint);

            //The most sexy oneliner in the world!
            //Takes the jsonResult, deserializes it and adds it to my model. Crazy easy
            var matchModel = JsonSerializer.Deserialize<List<NifsKampModel>>(jsonResult);

            return matchModel;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Added by BTA
        /// 

        [HttpPost]
        public async Task<IActionResult> UpdateStorage(int matchId, string result)
        {
            var now = DateTime.Now;

            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {

                // Get the match object from DB
                var match = _context.Matches.FirstOrDefault(m => m.MatchId == matchId);

                // Check if the match timespan is valid
                if (match == null || now.AddHours(2) > match.Timestamp) throw new Exception("Cannot bet on this match when starting time is less than two hours from now");

                //Check if a bet has been made on actual match
                var dbMatchBetting = _context.MatchBettings.FirstOrDefault(m => m.UserId == userId && m.MatchId == matchId);

                // The user has clicked on an checked checkbox to remove bet
                if (dbMatchBetting != null) await RemoveStorage(dbMatchBetting.MatchId);

                // Create a new MatchBetting entity
                var matchBetting = new Models.MatchBetting
                {
                    MatchId = matchId,
                    Result = result,
                    UserId = userId
                };

                // Add the MatchBetting entity to the context and save changes
                _context.MatchBettings.Add(matchBetting);
                await _context.SaveChangesAsync();

                var message = $"Successfully stored match id {matchId} with result {result} for user {userId}";
                _logService.LogInfo(userId, message);
                return Json(new { Success = true, Message = message });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                var message = $"Failed to store match id {matchId} with result {result}. Error: {ex.Message}";
                _logService.LogInfo(userId, message);

                return Json(new { Success = false, Message = message });
            }
        }

        [HttpPost]

        public async Task<IActionResult> UpdateSideBets(SideBettingMinViewModel sideBet)
        {
            if (sideBet.MostCards == null) sideBet.MostCards = string.Empty;

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var sidebet = _context.SideBettings.FirstOrDefault(m => m.UserId == userId);
                if (sidebet == null)
                {
                    sidebet = new SideBet()
                    {
                        Toppscorer = sideBet.Toppscorer,
                        WinnerTeam = sideBet.WinnerTeam,
                        MostCards = sideBet.MostCards,
                        UserId = userId
                    };
                    _context.SideBettings.Add(sidebet);
                }
                else
                {
                    sidebet.Toppscorer = sideBet.Toppscorer;
                    sidebet.WinnerTeam = sideBet.WinnerTeam;
                    sidebet.MostCards = sideBet.MostCards;
                    sidebet.UserId = userId;
                    _context.SideBettings.Update(sidebet);
                }

                await _context.SaveChangesAsync();
                return Json(new { Success = true, Message = $"Successfully stored sidebettings for user {userId}" });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { Success = false, Message = $"Failed to store sidebettings. Error: {ex.Message}" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemoveStorage(int matchId)
        {
            try
            {
                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Get the betting

                var matchBetting = _context.MatchBettings.FirstOrDefault(m => m.UserId == userId && m.MatchId == matchId);

                // Remove the MatchBetting entity to the context and save changes
                _context.MatchBettings.Remove(matchBetting);
                await _context.SaveChangesAsync();

                return Json(new { Success = true, Message = $"Successfully removed match id {matchId} for user {userId}" });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { Success = false, Message = $"Failed to remove match id {matchId}. Error: {ex.Message}" });
            }
        }

        public IActionResult GetCurrentUserBettings()
        {
            try
            {
                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create a new MatchBetting entity
                var bettings = _context.MatchBettings.Where(m => m.UserId == userId).Select(mb => new MatchBettingViewModel(mb));

                return Json(new { Success = true, Bettings = bettings });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { Success = false, Message = $"Failed to get bettings. Error: {ex.Message}" });
            }
        }
        public IActionResult GetCurrentUserSideBettings()
        {
            try
            {
                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create a new MatchBetting entity

                var sideBettings = _context.SideBettings.FirstOrDefault(m => m.UserId == userId);
                if (sideBettings == null)
                {
                    sideBettings = new SideBet();
                    sideBettings.UserId = userId;
                }
                var sideBet = new SideBettingViewModel(sideBettings);

                return Json(new { Success = true, SideBettings = sideBet });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { Success = false, Message = $"Failed to get sidebettings. Error: {ex.Message}" });
            }
        }
    }
}
