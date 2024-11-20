using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Flippy_Backend.Entities
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public int Score { get; set; } = 0;

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public int TriviaQuestions { get; set; }

        public int AnsweredTriviaQuestions { get; set; }




    }
}
