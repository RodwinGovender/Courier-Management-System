using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Messenger_Kings.Models
{
    public class Quote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Quote ID")]
        public int Quote_ID { get; set; }

        [DisplayName("Quote Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Quote_Date { get; set; }

        [Required(ErrorMessage = "Pickup address is required")]
        [DisplayName("Pickup address")]
        public string Quote_PickupAddress { get; set; }

        [Required(ErrorMessage = "Delivery address is required")]
        [DisplayName("Delivery address")]
        public string Quote_DeliveryAddress { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Out of range")]
        [DisplayName("Distance (km)")]
        public decimal Quote_Distance { get; set; }

        [Required(ErrorMessage = "Item description is required")]
        [DisplayName("Item description")]
        [DataType(DataType.MultilineText)]
        public string Quote_Description { get; set; }

        
        [DisplayName("Total cost"), DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Quote_Cost { get; set; }

        [Range(1,100, ErrorMessage = "Out of range 1-100")]
        [Required(ErrorMessage = "Item quantity is required")]
        [DisplayName("Item quantity")]
        public int Item_Quantity { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Out of range")]
        [DisplayName("Item length (cm)")]
        public decimal Quote_length { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Out of range")]
        [Required(ErrorMessage = "Item height is required")]
        [DisplayName("Item height (cm)")]
        public decimal Quote_Height { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Out of range")]
        [Required(ErrorMessage = "Item width is required")]
        [DisplayName("Item width (cm)")]
        public decimal Quote_Width { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Out of range")]
        [Required(ErrorMessage = "Item weight is required")]
        [DisplayName("Item weight (kg)")]
        public decimal Quote_Weight { get; set; }
    

        [DisplayName("Client type")]
        public int ClientCat_ID { get; set; }
        public virtual ClientCategory ClientCategory { get; set; }



    }
}
