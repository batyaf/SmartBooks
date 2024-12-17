using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace QBCustomer.Models
{
    public class CustomerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore] 
        public int Id { get; set; } // Database primary key

        public int UsrId { get; set; }

        [JsonIgnore]
        [ForeignKey("UsrId")]
        public SBUser User { get; set; }

        [JsonProperty("Id")] // Maps to "Id" in JSON
        public string QuickBooksId { get; set; }

        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string FullyQualifiedName { get; set; }
        public string CompanyName { get; set; }
        public bool? BillWithParent { get; set; }
        public bool? Job { get; set; }
        public bool? Active { get; set; }
        public bool? Taxable { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Balance { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BalanceWithJobs { get; set; }

        public string PreferredDeliveryMethod { get; set; }
        public string PrintOnCheckName { get; set; }
        public string SyncToken { get; set; }

        public int? BillAddrId { get; set; }
        public QbAddress BillAddr { get; set; }

        public int? PrimaryPhoneId { get; set; }
        public QbContactInfo PrimaryPhone { get; set; }

        public int? PrimaryEmailAddrId { get; set; }
        public QbContactInfo PrimaryEmailAddr { get; set; }

        public DateTime? CreateTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
    }

    public class QbContactInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FreeFormNumber { get; set; }
        public string? Address { get; set; }
    }

    public class QbAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public string? Line1 { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? CountrySubDivisionCode { get; set; }
        public string? Lat { get; set; }
        public string? Long { get; set; }

        [JsonProperty("Id")] // Maps to "Id" in JSON
        public string? QuickBooksAddressId { get; set; }
    }
}
