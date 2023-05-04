using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace BidditApi.Models
{
    public class Art
    {
        [Key]
        public int ArtId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ArtURL { get; set; }
        [ForeignKey("Bid")]
        public int BidId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPromoted { get; set; }

    }
}
