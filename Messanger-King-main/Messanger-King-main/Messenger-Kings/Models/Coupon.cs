using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Messenger_Kings.Models
{
    public class Coupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Coupon ID")]
        public int Coupon_ID { get; set; }

        [Display(Name = "Coupon Code")]
        public string Coupon_Code { get; set; }

        [Display(Name = "Coupon Status")]
        public string Coupon_Status { get; set; }

        [Display(Name = "Coupon Value")]
        public decimal Coupon_Value { get; set; }


        public string Client_ID { get; set; }
        public virtual Client Client { get; set; }
    }
}