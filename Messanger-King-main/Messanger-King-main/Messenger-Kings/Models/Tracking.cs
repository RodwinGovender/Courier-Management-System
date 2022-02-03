using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Messenger_Kings.Models
{
    public class Tracking
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Tracking number")]
        public string Track_ID { get; set; }

        [DisplayName("Tracking Message")]
        public string Track_Message { get; set; }
        
        [DisplayName("Order Status")]
        public string Order_Status { get; set; }
        
        [DisplayName("Out for delivery")]
        public string Out_for_delivery { get; set; }
        
        [DisplayName("Pickup Status")]
        public string Pickup_Status { get; set; }
        
        [DisplayName("Delivery Status")]
        public string Delivery_Status { get; set; }



        public int Order_ID { get; set; }
        public virtual Order Order { get; set; }

        
        

    }
}