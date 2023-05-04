using System.ComponentModel.DataAnnotations;

namespace BidditApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public  string UserName { get; set; } = null!;
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public int WalletBalance { get; set; }

    }
}
