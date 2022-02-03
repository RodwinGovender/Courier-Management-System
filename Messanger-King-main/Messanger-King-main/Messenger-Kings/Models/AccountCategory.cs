using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger_Kings.Models
{
    public class AccountCategory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Account Category ID")]
        public int AccountCat_ID { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        [DisplayName("Account Type")]
        public string Account_Type { get; set; }

        public virtual List<Bank> Banks { get; set; }

    }
}