using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Messenger_Kings.Models
{
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Contract ID")]
        public int Co_ID { get; set; }

        public decimal Co_Amount { get; set; }

        public string Co_Status { get; set; }



        public int Book_ID { get; set; }
        public virtual Booking Bookings { get; set; }


    }
}