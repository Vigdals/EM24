using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchBetting.Models
{
    public class MatchBetting
    {

        [Key]
        public int Id { get; set; }

        public int MatchId { get; set; }

        public string Result { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public virtual ApplicationUser User { get; set; }
    }
}
