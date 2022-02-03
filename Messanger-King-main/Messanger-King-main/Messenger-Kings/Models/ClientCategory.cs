using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger_Kings.Models
{
    public class ClientCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Client Category ID")]
        public int ClientCat_ID { get; set; }

        [Required(ErrorMessage = "Category type is required")]
        [DisplayName("Client Type")]
        public string ClientCat_Type { get; set; }

        public virtual List<Rate> Rates { get; set; }
        public virtual List<Client> Clients { get; set; }


    }
}