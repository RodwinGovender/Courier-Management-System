using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Messenger_Kings.Models
{
    public class ClientSignature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Signature ID")]
        public int Signaturee_ID { get; set; }

        [DisplayName("Sign Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Sign_Date { get; set; }


        [UIHint("SignaturePad")]
        [DisplayName("      ")]
        public byte[] MySignature { get; set; }

        [DisplayName("Signed By")]
        public string SignedBy { get; set; }

        [DisplayName("Recipient Type")]
        public string Recipient_Type { get; set; }

        public int Order_ID { get; set; }
        public virtual Order Order { get; set; }
        public string Driver_ID { get; set; }
        public virtual Driver Driver { get; set; }

    }
}