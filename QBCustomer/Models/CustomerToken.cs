using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCustomer.Models
{
    public class CustomerToken
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UsrId { get; set; }

        [ForeignKey("UsrId")]
        public SBUser User { get; set; }

        public string? RealmId { get; set; }
        public string? Token { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiresAt { get; set; }

        public DateTime CreationTokenExpiresAt { get; private set; } = DateTime.UtcNow;
    }
}
