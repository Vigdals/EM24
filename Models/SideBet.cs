using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MatchBetting.Models
{
    public class SideBet
    {
        [Key]
        public int Id { get; set; }

        public string Toppscorer { get; set; }

        public string MostCards { get; set; }

        public string WinnerTeam { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public virtual ApplicationUser User { get; set; }
    }
}
