using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QBCustomer.Models
{
    public class SBUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }

        //public SBUser(string id)
        //{
        //    UserId = id;
        //}
    }
}
