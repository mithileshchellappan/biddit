using System.ComponentModel.DataAnnotations;

namespace BidditApi.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }
        public int ArtId { get; set; }
        public int MinBid { get; set; }
        public int MaxBid { get; set; }
        public DateTime BidExpiry { get; set; }

    }
}
