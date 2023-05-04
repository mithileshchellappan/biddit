using System.ComponentModel.DataAnnotations;

namespace BidditApi.Models
{
    public class UserBids
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BidId { get; set; }
        public int BidAmount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
