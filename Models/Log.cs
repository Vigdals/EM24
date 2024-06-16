using System.ComponentModel.DataAnnotations;

namespace MatchBetting.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Payload { get; set; }
        public System.DateTime TimeStamp { get; set; }
    }
}
