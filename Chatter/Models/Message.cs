using System.ComponentModel.DataAnnotations;

namespace Chatter.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Text { get; set; }
        public DateTime When { get; set; } = DateTime.Now;

        public string? UserId { get; set; }
        public virtual AppUser? Sender { get; set; }
    }
}
