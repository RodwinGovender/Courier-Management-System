using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger_Kings.Models
{
    public class Waybill
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Waybill ID")]
        public int Waybill_ID { get; set; }


        [DisplayName("Date Checked Out")]
        public string Date_Packages_CheckedOut { get; set; }

        [DisplayName("Date Checked In")]
        public string Date_Packages_CheckedIN { get; set; }

        [DisplayName("Order ID")]
        public int Order_ID { get; set; }
        public virtual Order Order { get; set; }

        [DisplayName("Status ID")]
        public int Status_ID { get; set; }
        public virtual Status Status { get; set; }

        [DisplayName("Tracking ID")]
        public string Track_ID { get; set; }
        public virtual Tracking Tracking { get; set; }

    }
}