using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Messenger_Kings.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Status ID")]
        public int Status_ID { get; set; }

        [DisplayName("Storage Status")]
        public string Storage_Status { get; set; }

        public virtual List<Waybill> Waybills { get; set; }


    }
}