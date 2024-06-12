using Microsoft.AspNetCore.Authorization;
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
using static MatchBetting.NifsModels.MatchModel;
using Result = MatchBetting.NifsModels.Result;

namespace MatchBetting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
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
                        Result = GetResultFullTime(match.result)
                    };

                    _context.Matches.Add(dbMatch);
                }
                else
                {
                    dbMatch.AwayTeam = match.awayTeam.name;
                    dbMatch.HomeTeam = match.homeTeam.name;
                    dbMatch.Timestamp = match.timestamp;
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

            try
            {
                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

                return Json(new { Success = true, Message = $"Successfully stored match id {matchId} with result {result} for user {userId}" });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { Success = false, Message = $"Failed to store match id {matchId} with result {result}. Error: {ex.Message}" });
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
    }
}
