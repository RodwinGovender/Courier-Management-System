using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger_Kings.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Document ID")]
        public int Documents_ID { get; set; }

        [DisplayName("ID Document")]
        public byte[] Documents { get; set; }

        public string Client_ID { get; set; }
        public virtual Client Client { get; set; }
        public byte[] Document_Residence { get; internal set; }


        //public string Driver_IDNo { get; set; }
        //public virtual Driver Driver { get; set; }


    }
}