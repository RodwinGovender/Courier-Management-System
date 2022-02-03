using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Messenger_Kings.Models
{
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Application ID")]
        public int App_ID { get; set; }

        public string App_AdminComment { get; set; }

        public byte [] IDCard { get; set; }

        public byte[] BankStatement { get; set; }


        public string App_Status { get; set; }



    }
}