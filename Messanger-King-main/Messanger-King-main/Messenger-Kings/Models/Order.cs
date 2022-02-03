using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Entity;
using System.Web.Mvc;
using System;
using Messenger_Kings.Models;
using System.Collections.Generic;

namespace Messenger_Kings.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Order ID")]
        public int Order_ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Order_DateTime { get; set; }

    
        public int Book_ID { get; set; }
        public virtual Booking Bookings { get; set; }

        public string Driver_ID { get; set; }
        public virtual Driver Driver { get; set; }

        public virtual List<ClientSignature> ClientSignatures { get; set; }

        public virtual List<Waybill> Waybills { get; set; }

        public virtual List<Tracking> Trackings { get; set; }

    }
}