using MatchBetting.Data;
using MatchBetting.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MatchBetting.Service
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _context;

        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void LogInfo(string userId, string message)
        {
            Log(userId, message, "INFO");
        }

        private void Log(string userId, string message, string level, string payload = null)
        {
            var logLevel = level;
            var log = new Log()
            {
                UserId = userId,
                TimeStamp = DateTime.Now,
                Message = message,
                Level = logLevel,
                Payload = payload
            };

            try
            {
                _context.Logs.Add(log);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                // Fail in silence   
            }
        }
    }

    public interface ILogService
    {
        void LogInfo(string userId, string message);
    }
}
